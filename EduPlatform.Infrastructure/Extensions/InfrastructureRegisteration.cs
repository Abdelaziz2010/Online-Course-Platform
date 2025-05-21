using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduPlatform.Infrastructure.Extensions
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //register DbContext
            //services.AddDbContext<AppDbContext>(option =>
            //{
            //    option.UseSqlServer(configuration.GetConnectionString("EduPlatformDB"));
            //});

            return services;
        }
    }
}
