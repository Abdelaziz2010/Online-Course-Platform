using Ecom.Presentation.Middlewares;
using EduPlatform.Application.Extensions;
using EduPlatform.Infrastructure.Extensions;
using EduPlatform.Presentation.Middlewares;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Templates;

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
                "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
            .WriteTo.ApplicationInsights(
              services.GetRequiredService<TelemetryConfiguration>(),TelemetryConverter.Traces));

            Log.Information("Starting the EduPlatform API.....");


            builder.Services.AddControllers();
          
            builder.Services.AddOpenApi();
            
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureServices(configuration);

            builder.Services.AddApplicationServices();

            #endregion


            #region Middlewares
            
            var app = builder.Build();

            // enable custom middlewares

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

            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run(); 

            #endregion

        }
    }
}
