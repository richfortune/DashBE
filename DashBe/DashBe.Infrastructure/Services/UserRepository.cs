using DashBe.Application.Interfaces;
using DashBe.Domain.Models;
using DashBe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IApplicationDbContext _dbContext;
        

        public UserRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            _dbContext.Insert(user);
            try
            {
                await _dbContext.SaveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task AssignRoleAsync(Guid userId, int roleId)
        {
            _dbContext.Insert(new RoleUser { UserId = userId, RoleId = roleId });
            await _dbContext.SaveAsync();

        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _dbContext.FirstOrDefaultAsync<User>(u => u.Id == id);

        public async Task<User?> GetByIdWithRolesAsync(Guid id)
        {
            return await _dbContext.GetData<User>()
                .Include(u => u.RoleUsers)
                .ThenInclude(ru => ru.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username) 
        {
            return await _dbContext.GetData<User>(true)
                .Include(u => u.RoleUsers)
                .ThenInclude(ru => ru.Role)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.GetData<User>(true)
                .Include(u => u.RoleUsers)
                .ThenInclude(ru => ru.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class =>
                //await _context.Set<T>().AnyAsync(predicate); 
                await _dbContext.AnyAsync(predicate);

        public async Task UpdateAsync(User user)
        {
            _dbContext.Update(user);
            await _dbContext.SaveAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            //user.RoleUsers.ToList().ForEach(role => _dbContext.Delete(role));
            _dbContext.DeleteRange(user.RoleUsers);
            _dbContext.Delete(user);

            await _dbContext.SaveAsync();
         }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _dbContext.FirstOrDefaultAsync<Role>(r => r.Id == roleId);
        }

        public async Task<bool> UserRoleExistsAsync(Guid userId, int roleId)
        {
            return await _dbContext.AnyAsync<RoleUser>(ru => ru.UserId == userId && ru.RoleId == roleId);
        }
    }
}
