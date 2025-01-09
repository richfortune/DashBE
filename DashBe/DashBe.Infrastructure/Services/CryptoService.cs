using DashBe.Application.Interfaces;
using DashBe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DashBe.Infrastructure.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly HttpClient _httpClient;

        public CryptoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CoinDeskPrice> GetCurrentPriceAsync()
        {
            var response = await _httpClient.GetAsync("bpi/currentprice.json");
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Errore API: {response.StatusCode}");
            }


            var json = await response.Content.ReadAsStringAsync();
            var coinDeskPrice = JsonSerializer.Deserialize<CoinDeskPrice>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return coinDeskPrice;

        }
    }
}
