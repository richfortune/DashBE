using DashBe.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DashBe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private readonly ILogger<CryptoController> _logger;

        public CryptoController(ICryptoService cryptoService, ILogger<CryptoController> logger)
        {
            _cryptoService = cryptoService;
            _logger = logger;
        }

        [HttpGet("currentprice")]
        public async Task<IActionResult> GetCurrentPrice()
        {
            try
            {
                var result = await _cryptoService.GetCurrentPriceAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nel controller: {ex.Message}");
                return StatusCode(503, new { Message = "Servizio CoinDesk non disponibile. Riprova più tardi." });
            }
            
        }
    }
}
