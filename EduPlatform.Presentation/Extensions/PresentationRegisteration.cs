using EduPlatform.Presentation.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EduPlatform.Presentation.Extensions
{
    public static class PresentationRegisteration
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the services
            services.AddScoped<IUserClaims, UserClaims>();

            services.AddHealthChecks()
                   .AddSqlServer(
                       connectionString: configuration.GetConnectionString("PlatformDB"),
                       healthQuery: "SELECT 1;", // Query to check database health.
                       name: "sqlserver",
                       failureStatus: HealthStatus.Degraded, // Degraded health status if the check fails.
                       tags: new[] { "db", "sql" })
                   .AddCheck("Memory", new ManagedMemoryHealthCheck(1024 * 1024 * 1024)); // A custom health check for managed memory.

            return services;
        }
    }
}
