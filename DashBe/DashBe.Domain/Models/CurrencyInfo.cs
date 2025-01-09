using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Models
{
    public class CurrencyInfo
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        public float RateFloat { get; set; }
    }
}
