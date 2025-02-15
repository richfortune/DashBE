using AutoMapper;
using DashBe.Application.DTOs;
using DashBe.Application.Interfaces;
using DashBe.Domain.Models;
using Microsoft.Extensions.Logging;
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
        private readonly IMapper _mapper;
        private readonly ILogger<CryptoService> _logger;

        public CryptoService(HttpClient httpClient, IMapper mapper, ILogger<CryptoService> logger)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CoinDeskPriceDTO> GetCurrentPriceAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("bpi/currentprice.json");
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Errore API: {response.StatusCode} - {response.ReasonPhrase}");
                    throw new HttpRequestException($"Errore nella richiesta API: {response.StatusCode}");

                }

                var json = await response.Content.ReadAsStringAsync();
                var coinDeskPrice = JsonSerializer.Deserialize<CoinDeskPrice>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (coinDeskPrice == null)
                {
                    _logger.LogError("La risposta dell'API di CoinDesk è nulla.");
                    throw new NullReferenceException("La risposta dell'API di CoinDesk è vuota.");
                }

                return _mapper.Map<CoinDeskPriceDTO>(coinDeskPrice);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Errore nella richiesta API: {ex.Message}");
                throw new Exception("Servizio CoinDesk non disponibile. Riprova più tardi.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore generico: {ex.Message}");
                throw;
            }
        }
     
    }
}
