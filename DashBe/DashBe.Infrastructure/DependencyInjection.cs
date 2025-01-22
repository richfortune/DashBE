using DashBe.Application.Interfaces;
using DashBe.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DashBe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDi(this IServiceCollection services, IConfiguration configuration) 
        {
            var baseAddresses = configuration["CoinDeskApi:BaseAddress"];
            if(string.IsNullOrEmpty(baseAddresses)) 
            {
                throw new ArgumentException("Errore nella lettura della configurazione di BaseAddress");
            }


            services.AddHttpClient<ICryptoService, CryptoService>(option =>
            {
                option.BaseAddress = new Uri(baseAddresses);
            });

            return services;
        }
    }
}
