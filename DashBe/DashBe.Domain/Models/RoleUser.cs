using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Models
{
    public sealed class RoleUser
    {
        public int Id { get; set; }  // 🔥 Nuova chiave primaria

        public Guid UserId { get; set; }
        public User User { get; set; }

       
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
