using DashBe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Models
{
    public sealed class User : Entity<Guid>
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public ICollection<RoleUser> RoleUsers { get; private set; } = new List<RoleUser>();

        public User(Guid id, string username, string email, string passwordHash) : base(id)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }

        public void AssignRole(Role role)
        {
            if (!RoleUsers.Any(ru => ru.RoleId == role.Id))
            {
                RoleUsers.Add(new RoleUser { UserId = this.Id, RoleId = role.Id, Role = role });
            }
        }
    }
}
