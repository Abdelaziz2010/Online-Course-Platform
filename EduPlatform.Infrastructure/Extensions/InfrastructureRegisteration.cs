using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EduPlatform.Infrastructure.Extensions
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //register DbContext
            services.AddDbContextPool<EduPlatformDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("PlatformDB"),
                    providerOptions => providerOptions.EnableRetryOnFailure());
            });



            return services;
        }
    }
}
