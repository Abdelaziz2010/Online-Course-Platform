using Microsoft.ApplicationInsights.DataContracts;
using Serilog;

namespace EduPlatform.Presentation.Middlewares
{
    // the middleware that logs the response body to Application Insights
    public class ResponseBodyLoggingMiddleware
    {

        private readonly RequestDelegate _next;
        
        public ResponseBodyLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            try
            {
                // Swap out stream with one that is buffered and suports seeking
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;
                // hand over to the next middleware and wait for the call to return
                await _next(context);
                // Read response body from memory stream
                memoryStream.Position = 0;
                var reader = new StreamReader(memoryStream);
                var responseBody = await reader.ReadToEndAsync();
                // Copy body back to so its available to the user agent
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBodyStream);
                // Write response body to App Insights
                var requestTelemetry = context.Features.Get<RequestTelemetry>();
                requestTelemetry?.Properties.Add("ResponseBody", responseBody);
                // Log the response body
                Log.Information("Response:" + responseBody);
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }
}
