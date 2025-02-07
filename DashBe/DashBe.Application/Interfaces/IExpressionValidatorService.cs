using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Interfaces
{
    public interface IExpressionValidatorService
    {
        public bool IsBalanced(string input);
    }
}
