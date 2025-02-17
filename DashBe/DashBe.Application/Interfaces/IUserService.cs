using DashBe.Application.DTOs;
using DashBe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task RegisterAsync(string username, string email, string password);
        Task AssignRoleAsync(Guid userId, int roleId);

        Task<UserDTO?> GetByIdAsync(Guid userId);

        Task<User?> ConfirmEmailAsync(string token, string email);
    }
}
