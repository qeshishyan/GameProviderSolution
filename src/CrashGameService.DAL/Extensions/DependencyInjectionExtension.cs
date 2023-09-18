using CrashGameService.DAL.IRepository;
using CrashGameService.DAL.Repository;
using CrashGameService.Repository.Context;
using Microsoft.Extensions.DependencyInjection;

namespace CrashGameService.DAL.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<CrashDbContext>();
            services.AddSingleton<IGameRepository, GameRepository>();
        }
    }
}
