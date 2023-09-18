using CrashGameService.Services;

namespace CrashGameService.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IGameService, GameService>();
        }
    }
}
