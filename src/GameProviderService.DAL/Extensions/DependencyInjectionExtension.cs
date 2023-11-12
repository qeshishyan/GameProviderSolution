using GameProviderService.DAL.Context;
using GameProviderService.DAL.IRepository;
using GameProviderService.DAL.Repository;
using GameProviderService.Repository.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameProviderService.DAL.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GameProviderDbContext>();
            services.AddScoped<IGameProviderRepository, GameProviderRepository>();
            services.Configure<ContextOptions>(options =>
            {
                options.ConnectionString = configuration.GetConnectionString("GameProviderPgDB")!;
            });
        }
    }
}
