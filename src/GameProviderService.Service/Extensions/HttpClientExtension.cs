using GameProviderService.Service.HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameProviderService.Service.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IGameProviderClient, GameProviderClient>((client) =>
            {
                //docker HOST: game-logic-service
                client.BaseAddress = new Uri(configuration["GameLogicApiUrl"]!);
            });
        }
    }
}
