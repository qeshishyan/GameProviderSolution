using CrashGameService.Services;

namespace CrashGameService.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IGameService, GameService>((value) => new GameService("providerId_4578d687ad65", new CancellationTokenSource()));
        }
    }
}
