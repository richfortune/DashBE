using DashBe.Application.Interfaces;
using DashBe.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DashBe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;
        private readonly ILogger<CryptoController> _logger;
        private readonly AppDbContext _appDbContext;

        public CryptoController(ICryptoService cryptoService, ILogger<CryptoController> logger, AppDbContext appDbContext)
        {
            _cryptoService = cryptoService;
            _logger = logger;
            _appDbContext = appDbContext;
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
