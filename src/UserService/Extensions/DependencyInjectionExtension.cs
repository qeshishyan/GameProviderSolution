using UserService.Repositories;
using UserService.Services;

namespace UserService.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, Services.UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
