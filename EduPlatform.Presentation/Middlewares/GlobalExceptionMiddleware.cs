using EduPlatform.Presentation.Helpers;
using Serilog;
using System.Net;
using System.Text.Json;

namespace EduPlatform.Presentation.Middlewares
{
    // this middleware is used to handle global exceptions in the applicationd
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        public GlobalExceptionMiddleware(RequestDelegate next, IHostEnvironment environment)
        {
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception using Serilog 
                Log.Error(ex, "Unhandled exception occurred.");

                // Set the response status code and content type
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // Return a generic error response
                var response = _environment.IsDevelopment() ?
                   new ProblemDetail((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                 : new ProblemDetail((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
