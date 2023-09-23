using CrashGameService.DAL.Context;
using CrashGameService.DAL.IRepository;
using CrashGameService.DAL.Repository;
using CrashGameService.Repository.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrashGameService.DAL.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<CrashDbContext>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.Configure<ContextOptions>(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("CrashGamePgDB")!;
            });
        }
    }
}
