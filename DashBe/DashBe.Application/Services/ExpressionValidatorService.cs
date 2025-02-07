using DashBe.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Services
{
    public class ExpressionValidatorService : IExpressionValidatorService
    {

        public bool IsBalanced(string input)
        {
            if(input == null) return false;

            int iBalance = 0;

            foreach (char item in input)
            {
                if(item == '(')
                {
                    iBalance++; 
                }
                else if(item == ')') 
                {
                    iBalance--;
                    if(iBalance < 0) { return false; }
                }
            }

            return iBalance == 0;
        }
    }
}
