using GameProviderService.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameProviderService.Service.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProviderService, ProviderService>();
            services.AddSingleton<IJwtService, JwtService>();
        }
    }
}
