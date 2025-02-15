using DashBe.Application.DTOs;
using DashBe.Application.Interfaces;
using DashBe.Domain.Common;
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
            if(user == null)
                throw new InvalidOperationException("Utente non trovato");
            
            if (!await _userRepository.ExistsAsync<Role>(r => r.Id.Equals(roleId)))
                throw new InvalidOperationException("Ruolo non trovato.");


            if (await _userRepository.UserRoleExistsAsync(userId, roleId))
                throw new InvalidOperationException("L'utente ha già questo ruolo.");

            await _userRepository.AssignRoleAsync(userId, roleId);
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

            if (await _userRepository.ExistsAsync<User>(u => u.Email == email))
                throw new InvalidOperationException("Email già registrata.");

            var userId = Guid.NewGuid();
            var hashedPassword = _passwordHasher.HashPassword(null, password);
            var user = new User(userId, username, email, hashedPassword);

            //var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            var defaultRole = await _userRepository.GetRoleByIdAsync(Constants.DefaultUserRoleId);
            if (defaultRole != null)
                user.AssignRole(defaultRole);
            

            await _userRepository.AddAsync(user);
            
        }

        
        public async Task<UserDTO?> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return null;

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
