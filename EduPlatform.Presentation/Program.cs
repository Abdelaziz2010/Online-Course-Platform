using Ecom.Presentation.Middlewares;
using EduPlatform.Application.Extensions;
using EduPlatform.Infrastructure.Extensions;

namespace EduPlatform.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {

            #region Services Configuration
            
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
          
            builder.Services.AddOpenApi();
            
            builder.Services.AddEndpointsApiExplorer();
            
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApplicationServices();

            #endregion

            #region Middlewares
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
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
