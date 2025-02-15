using DashBe.Application.Interfaces;
using DashBe.Domain.Models;
using DashBe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task AssignRoleAsync(Guid userId, int roleId)
        {
            _context.RoleUser.Add(new RoleUser { UserId = userId, RoleId = roleId });
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _context.Users.Include(u => u.RoleUsers).ThenInclude(ru => ru.Role).FirstOrDefaultAsync(u => u.Id == id);

        
        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.Users.Include(u => u.RoleUsers).ThenInclude(ru => ru.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class =>
                await _context.Set<T>().AnyAsync(predicate); 

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        }

        public async Task<bool> UserRoleExistsAsync(Guid userId, int roleId)
        {
            return await _context.RoleUser.AnyAsync(ru => ru.UserId == userId && ru.RoleId == roleId);
        }
    }
}
