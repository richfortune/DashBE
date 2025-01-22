using DashBe.Application.DTOs;
using DashBe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Services
{
    public static class MappingHelper
    {
        public static CoinDeskPriceDTO toDTO(CoinDeskPrice coinDeskPrice)
        {
            return new CoinDeskPriceDTO
            {
                UpdatedISO = coinDeskPrice.Time.UpdatedISO,
                ChartName = coinDeskPrice.ChartName,
                Currencies = new List<CurrencyInfoDTO>
                {
                    new CurrencyInfoDTO
                    {
                        Code = coinDeskPrice.Bpi.USD.Code,
                        Symbol = coinDeskPrice.Bpi.USD.Symbol,
                        Rate = coinDeskPrice.Bpi.USD.Rate,
                        Description = coinDeskPrice.Bpi.USD.Description
                    },
                }
            };
        }


    }
}
