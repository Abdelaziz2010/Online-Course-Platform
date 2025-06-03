using EduPlatform.Presentation.Middlewares;
using EduPlatform.Application.Extensions;
using EduPlatform.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Serilog.Templates;
using EduPlatform.Presentation.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace EduPlatform.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Configure Serilog with the settings, used to log during startup (before the app is built)
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

            try
            {
                #region Services Configuration

                var builder = WebApplication.CreateBuilder(args);

                var configuration = builder.Configuration;


                // Add services to the container.

                // Register Application Insights telemetry for monitoring and logging.
                builder.Services.AddApplicationInsightsTelemetry();


                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .WriteTo.Console(new ExpressionTemplate(
                    // Include trace and span ids when present.
                    "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}")));

                Log.Information("Starting the EduPlatform API.....");


                #region Azure AD B2C configuration

                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddMicrosoftIdentityWebApi(options =>
                  {
                      configuration.Bind("AzureADB2C", options);

                      options.Events = new JwtBearerEvents
                      {
                          OnTokenValidated = context =>
                          {
                              var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                              // Access the scope claim (scp) directly
                              var scopeClaim = context.Principal?.Claims.FirstOrDefault(c => c.Type == "scp")?.Value;

                              if (scopeClaim != null)
                              {
                                  logger.LogInformation("Scope found in token: {Scope}", scopeClaim);
                              }
                              else
                              {
                                  logger.LogWarning("Scope claim not found in token.");
                              }

                              return Task.CompletedTask;
                          },
                          OnAuthenticationFailed = context =>
                          {
                              var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                              logger.LogError("Authentication failed: {Message}", context.Exception.Message);
                              return Task.CompletedTask;
                          },
                          OnChallenge = context =>
                          {
                              var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                              logger.LogError("Challenge error: {ErrorDescription}", context.ErrorDescription);
                              return Task.CompletedTask;
                          }
                      };
                  }, options => { configuration.Bind("AzureADB2C", options); });

                // The following flag can be used to get more descriptive errors in development environments

                IdentityModelEventSource.ShowPII = true;

                #endregion Azure AD B2C configuration


                builder.Services.AddControllers();

                builder.Services.AddOpenApi();

                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerGen();

                builder.Services.AddPresentationServices(configuration);

                builder.Services.AddInfrastructureServices(configuration);

                builder.Services.AddApplicationServices();

                #endregion


                #region Middlewares

                var app = builder.Build();


                // Global exception handling middleware
                app.UseMiddleware<GlobalExceptionMiddleware>();

                // Middlewares for logging request and response details.
                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                app.UseMiddleware<RequestBodyLoggingMiddleware>();
                app.UseMiddleware<ResponseBodyLoggingMiddleware>();


                // Configure swagger middleware.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                #region  Azure AD B2C

                app.UseAuthentication();

                app.UseAuthorization();

                #endregion  Azure AD B2C

                app.UseHealthChecks("/health");

                // check Readiness of the application
                app.UseHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = check => check.Name == "self" || check.Name == "sqlserver"
                });

                // check Liveness of the application
                app.UseHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = check => check.Name == "self"
                });


                app.UseMiddleware<SecurityHeadersMiddleware>();

                app.MapControllers();

                app.Run();

                #endregion
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
