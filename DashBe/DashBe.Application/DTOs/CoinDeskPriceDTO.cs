using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.DTOs
{
    public class CoinDeskPriceDTO
    {
        public DateTime UpdatedISO { get; set; }
        public string ChartName { get; set; }
        public List<CurrencyInfoDTO> Currencies { get; set; }
    }
}
