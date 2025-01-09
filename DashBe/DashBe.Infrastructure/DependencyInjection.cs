using DashBe.Application.Interfaces;
using DashBe.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DashBe.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDi(this IServiceCollection services) 
        {
            services.AddHttpClient<ICryptoService, CryptoService>(option =>
            {
                option.BaseAddress = new Uri("https://api.coindesk.com/v1/");
            });

            return services;
        }
    }
}
