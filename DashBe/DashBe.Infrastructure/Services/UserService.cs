using DashBe.Application.Interfaces;
using DashBe.Domain.Models;
using DashBe.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(IUserRepository userRepository, AppDbContext context, IPasswordHasher<User> passwordHasher) 
        {
            _userRepository = userRepository;
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task AssignRoleAsync(Guid userId, int roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if(user == null){throw new InvalidOperationException("Utente non trovato");}

            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
                throw new InvalidOperationException("Ruolo non trovato.");


            var userRole = new RoleUser { UserId = userId, RoleId = roleId };
            await _context.RoleUser.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
                return null;
            return user;
        }

        public async Task RegisterAsync(string username, string email, string password)
        {
            // Controlla se l'utente esiste già
            var existingUser = await _userRepository.GetByUsernameAsync(username);
            if (existingUser != null)
                throw new InvalidOperationException("Username già in uso.");

            var existingEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingEmail != null)
                throw new InvalidOperationException("Email già registrata.");

            // Genera un ID unico per il nuovo utente
            var userId = Guid.NewGuid();

            // Hash della password
            var hashedPassword = _passwordHasher.HashPassword(null, password);

            // Crea il nuovo utente
            var user = new User(userId, username, email, hashedPassword);

            // Assegna automaticamente il ruolo "User" al nuovo utente
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (defaultRole != null)
            {
                user.AssignRole(defaultRole);
            }

            // Salva l'utente nel database
            await _userRepository.AddAsync(user);
        }

        
        public async Task<User?> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            // Qui possiamo aggiungere logiche extra in futuro (esempio: logging, validazioni)
            if (user == null)
            {
                // Log di errore (esempio)
                Console.WriteLine($"User {userId} not found.");
            }

            return user;
        }

    }
}
