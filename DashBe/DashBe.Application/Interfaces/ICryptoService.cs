using DashBe.Domain.Models;

namespace DashBe.Application.Interfaces
{
    public interface ICryptoService
    {
        Task<CoinDeskPrice> GetCurrentPriceAsync();
    }
}
