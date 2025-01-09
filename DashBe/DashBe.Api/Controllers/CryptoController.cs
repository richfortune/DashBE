using DashBe.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DashBe.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoService _cryptoService;

        public CryptoController(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet("currentprice")]
        public async Task<IActionResult> GetCurrentPrice()
        {
            var result = await _cryptoService.GetCurrentPriceAsync();
            return Ok(result);
        }
    }
}
