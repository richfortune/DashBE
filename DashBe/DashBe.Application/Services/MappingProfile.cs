using AutoMapper;
using DashBe.Application.DTOs;
using DashBe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CoinDeskPrice, CoinDeskPriceDTO>()
                .ForMember(dest => dest.Currencies, opt => opt.MapFrom(src => 
                new List<CurrencyInfoDTO>
                    {
                        MapCurrency(src.Bpi.USD),
                        MapCurrency(src.Bpi.EUR),
                        MapCurrency(src.Bpi.GBP)
                }));
        }

        private CurrencyInfoDTO MapCurrency(CurrencyInfo currency)
        {
            return new CurrencyInfoDTO
            {
                Code = currency.Code,
                Symbol = currency.Symbol,
                Rate = currency.Rate,
                Description = currency.Description
            };
        }


    }
}
