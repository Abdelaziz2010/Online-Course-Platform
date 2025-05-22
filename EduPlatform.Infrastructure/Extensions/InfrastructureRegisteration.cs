using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Infrastructure.Data;
using EduPlatform.Infrastructure.Implementations.Repositories;
using EduPlatform.Infrastructure.Implementations.Services;
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


            // register IUnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //register services
            services.AddScoped<ICategoryService, CategoryService>();


            return services;
        }
    }
}
