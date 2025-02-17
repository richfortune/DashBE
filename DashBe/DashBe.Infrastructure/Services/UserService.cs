using DashBe.Application.DTOs;
using DashBe.Application.Interfaces;
using DashBe.Domain.Common;
using DashBe.Domain.Models;
using DashBe.Infrastructure.Services.Message;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEmailService _emailService;
       

        public UserService(IUserRepository userRepository, IApplicationDbContext dbContext, IPasswordHasher<User> passwordHasher, IEmailService emailService)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task AssignRoleAsync(Guid userId, int roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                await _dbContext.LogAsync($"Assegnazione ruolo fallita: Utente {userId} non trovato.", "Warning");
                throw new InvalidOperationException("Utente non trovato.");
            }

            if (!await _userRepository.ExistsAsync<Role>(r => r.Id == roleId))
            {
                await _dbContext.LogAsync($"Assegnazione ruolo fallita: Ruolo {roleId} non trovato.", "Warning");
                throw new InvalidOperationException("Ruolo non trovato.");
            }

            if (await _userRepository.UserRoleExistsAsync(userId, roleId))
            {
                await _dbContext.LogAsync($"Assegnazione ruolo fallita: L'utente {userId} ha già il ruolo {roleId}.", "Warning");
                throw new InvalidOperationException("L'utente ha già questo ruolo.");
            }

            await _userRepository.AssignRoleAsync(userId, roleId);
            await _dbContext.LogAsync($"Ruolo {roleId} assegnato con successo all'utente {user.Username}.", "Information");
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            {
                await _dbContext.LogAsync($"Autenticazione fallita per l'utente {username}.", "Warning");
                return null;
            }

            //await _dbContext.LogAsync($"Utente {username} autenticato con successo.", "Information");

            return user;
        }

        public async Task RegisterAsync(string username, string email, string password)
        {
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username già in uso.");
            }

            if (await _userRepository.ExistsAsync<User>(u => u.Email == email))
            {
                throw new InvalidOperationException("Email già registrata.");
            }

            var userId = Guid.NewGuid();
            var hashedPassword = _passwordHasher.HashPassword(null, password);
            var user = new User(userId, username, email, hashedPassword)
            {
                IsEmailConfirmed = false, // Aggiungi un flag per la conferma email
                EmailConfirmationToken = Guid.NewGuid().ToString() // Generiamo un token di conferma
            };

            var defaultRole = await _userRepository.GetRoleByIdAsync(Constants.DefaultUserRoleId);
            if (defaultRole != null)
            {
                user.AssignRole(defaultRole);
            }

            await _userRepository.AddAsync(user);

            // Costruire il link di conferma
            string confirmationLink = $"https://localhost:7150/api/users/confirm-email?token={user.EmailConfirmationToken}&email={user.Email}";

            // Inviare l'email di conferma
            string subject = "Conferma la tua email";
            string body = $"Ciao {username},<br><br>Per confermare la tua registrazione, clicca sul link:<br><a href='{confirmationLink}'>Conferma Email</a>";

            await _emailService.SendEmailAsync(email, subject, body);

        }

        public async Task<User?> ConfirmEmailAsync(string token, string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || user.EmailConfirmationToken != token)
            {
                return null;
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = string.Empty; // Rimuoviamo il token usato
            await _userRepository.UpdateAsync(user);

            return user;
        }


        //public async Task RegisterAsync(string username, string email, string password)
        //{
        //    var existingUser = await _userRepository.GetByUsernameAsync(username);
        //    if (existingUser != null)
        //    {
        //        await _dbContext.LogAsync($"Registrazione fallita: username {username} già in uso.", "Warning");
        //        throw new InvalidOperationException("Username già in uso.");
        //    }

        //    if (await _userRepository.ExistsAsync<User>(u => u.Email == email))
        //    {
        //        await _dbContext.LogAsync($"Registrazione fallita: email {email} già registrata.", "Warning");
        //        throw new InvalidOperationException("Email già registrata.");
        //    }

        //    var userId = Guid.NewGuid();
        //    var hashedPassword = _passwordHasher.HashPassword(null, password);
        //    var user = new User(userId, username, email, hashedPassword);

        //    var defaultRole = await _userRepository.GetRoleByIdAsync(Constants.DefaultUserRoleId);
        //    if (defaultRole != null)
        //    {
        //        user.AssignRole(defaultRole);
        //    }

        //    await _userRepository.AddAsync(user);
        //    await _dbContext.LogAsync($"Utente {username} ({email}) registrato con successo.", "Information");
        //}

        public async Task<UserDTO?> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdWithRolesAsync(userId);

            if (user == null)
            {
                await _dbContext.LogAsync($"Ricerca utente fallita: ID {userId} non trovato.", "Warning");
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Roles = user.RoleUsers.Select(ru => ru.Role.Name).ToList()
            };
        }
    }
}
