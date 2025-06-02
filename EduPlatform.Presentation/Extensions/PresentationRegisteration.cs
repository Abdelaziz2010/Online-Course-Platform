using EduPlatform.Presentation.Common;

namespace EduPlatform.Presentation.Extensions
{
    public static class PresentationRegisteration
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            // Register the services
            services.AddScoped<IUserClaims, UserClaims>();

            return services;
        }
    }
}
