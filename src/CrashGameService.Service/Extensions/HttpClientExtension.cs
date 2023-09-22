using CrashGameService.Service.HttpClients;
using Microsoft.Extensions.DependencyInjection;

namespace CrashGameService.Service.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddClients(this IServiceCollection services)
        {
            services.AddHttpClient<IGameLogicPhytonClient, GameLogicPhytonClient>(client =>
            {
                //docker HOST: game-logic-service
                client.BaseAddress = new Uri("http://game-logic-service:5000");
            });
        }
    }
}
