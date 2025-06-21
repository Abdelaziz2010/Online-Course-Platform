using EduPlatform.Presentation.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting; // Add this using

namespace EduPlatform.Presentation.Extensions
{
    public static class PresentationRegisteration
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the services
            services.AddScoped<IUserClaims, UserClaims>();

            // register the health checks in services container 
            #region Health Check Configurations

            services.AddHealthChecks()
                      .AddSqlServer(
                          connectionString: configuration.GetConnectionString("PlatformDB"),
                          healthQuery: "SELECT 1;", // Query to check database health.
                          name: "sqlserver",
                          failureStatus: HealthStatus.Degraded, // Degraded health status if the check fails.
                          tags: new[] { "db", "sql" })
                      .AddCheck("Memory", new ManagedMemoryHealthCheck(1024 * 1024 * 1024)); // A custom health check for managed memory. 

            #endregion


            // Add Rate Limiting globally
            #region Rate Limiting Configurations

            services.AddRateLimiter(options =>
            {
                // Policy for read-only endpoints (GET)
                options.AddPolicy("ReadOnlyPolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20, // 20 requests
                            Window = TimeSpan.FromMinutes(1), // per minute
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        }));

                // Policy for write endpoints (POST/PUT/DELETE)
                options.AddPolicy("WritePolicy", context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5, // 5 requests
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 2
                        }));

                // Optional: Set a sensible global fallback
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 50,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 3
                        }));

                // Custom response on rate limit rejection
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(
                        "{\"error\": \"You are being rate limited. Please try again later.\"}",
                        token);
                };
            });
            // •	Keep the global limiter as a fallback for untagged endpoints.
            #endregion

            return services;
        }
    }
}
