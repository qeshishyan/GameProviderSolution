using CrashGameService.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CrashGameService.Service.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
        }
    }
}
