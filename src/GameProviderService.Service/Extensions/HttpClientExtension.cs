using GameProviderService.Service.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace GameProviderService.Service.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddClients(this IServiceCollection services)
        {
            services.AddHttpClient<IGameProviderClient, GameProviderClient>((client) =>
            {
                //docker HOST: game-logic-service
                //client.BaseAddress = new Uri(configuration["GameLogicApiUrl"]!);
            });
        }
    }
}
