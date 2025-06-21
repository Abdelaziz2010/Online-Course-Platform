using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using EduPlatform.Presentation.Common;
using EduPlatform.Presentation.Helpers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.RateLimiting;

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


            // register Rate Limiting in services container 
            #region Rate Limiting Configurations

            services.AddRateLimiter(options =>
            {
                // Policy for read-only endpoints (GET)
                options.AddPolicy("ReadOnlyPolicy", context =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 20,                     // 20 requests total
                            Window = TimeSpan.FromMinutes(1),     // in a 1 minute window
                            SegmentsPerWindow = 4,                // divide 1 minute into 4 parts (15 seconds each)
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0                        // reject immediately if limit is exceeded, no frozen requests
                        }));

                // Policy for write endpoints (POST/PUT/DELETE)
                options.AddPolicy("WritePolicy", context =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 5,                     // 5 requests total
                            Window = TimeSpan.FromMinutes(1),    // in a 1 minute window
                            SegmentsPerWindow = 4,               // divide 1 minute into 4 parts (15 seconds each)
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                // Global limiter as a fallback for untagged endpoints.
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        key => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 50,                    // 50 requests total
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 4,               // 4 segments of 15 seconds each
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0
                        }));

                // Custom response on rate limit rejection
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync("{\"error\": \"You are being rate limited. Please try again later.\"}",token);
                };
            });

            #endregion


            services.AddSwaggerGen();

            services.ConfigureOptions<ConfigureSwaggerOptions>();    // to generate Swagger docs for each API version

            // This Service Registration ensures version descriptions are available to Swagger.
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            // register api versioning in services container 

            #region API Versioning Configurations

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);    // Set default API version
                options.AssumeDefaultVersionWhenUnspecified = true;    // Use default version if not specified
                options.ReportApiVersions = true;     // Report API versions in response headers
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new UrlSegmentApiVersionReader()
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // Format for versioned API groups
                options.SubstituteApiVersionInUrl = true; // Substitute version in URL
            });

            #endregion

            return services;
        }
    }
}
