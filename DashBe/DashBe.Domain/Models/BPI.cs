using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Models
{
    public class BPI
    {
        public CurrencyInfo USD { get; set; }
        public CurrencyInfo GBP { get; set; }
        public CurrencyInfo EUR { get; set; }
    }
}
