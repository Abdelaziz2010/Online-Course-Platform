
using Microsoft.Extensions.DependencyInjection;

namespace EduPlatform.Application.Extensions
{
    public static class ApplicationRegisteration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register the AutoMapper service
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            return services;
        }
    }
}
