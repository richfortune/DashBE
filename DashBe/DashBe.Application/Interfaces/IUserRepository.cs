using DashBe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Interfaces
{
    public  interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> UserRoleExistsAsync(Guid userId, int roleId);
        Task AssignRoleAsync(Guid userId, int roleId);
        Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        Task<Role?> GetRoleByIdAsync(int roleId);

    }
}
