using DashBe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string username, string email, IEnumerable<string> roles);
    }
}

