using DashBe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Models
{
    public sealed class Role : Entity<int>
    {
        
        public string Name { get; private set; }

        public ICollection<RoleUser> RoleUsers { get; private set; } = new List<RoleUser>();

        public Role(int id, string name) : base(id)
        {
            Name = name;
        }

    }
}
