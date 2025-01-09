using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Domain.Models
{
    public class CoinDeskPrice
    {
        public TimeInfo Time { get; set; }
        public string? Disclaimer { get; set; }
        public string? ChartName { get; set; }
        public BPI Bpi { get; set; }
    }
}
