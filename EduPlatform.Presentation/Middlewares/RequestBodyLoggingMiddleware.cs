using Microsoft.ApplicationInsights.DataContracts;
using Serilog;
using System.Text;
using System.Text.Json;

namespace EduPlatform.Presentation.Middlewares
{
    // This middleware logs the request body(Post/Put) to Application Insights and Serilog sink.
    // Application Insights (for telemetry & monitoring)
    // Serilog (for structured logging)
    public class RequestBodyLoggingMiddleware
    {
        
        private readonly RequestDelegate _next;
        
        public RequestBodyLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            // Ensure the request body can be read multiple times
            context.Request.EnableBuffering();
            // Only if we are dealing with POST or PUT, GET and others shouldn't have a body
            if (context.Request.Body.CanRead && (method == HttpMethods.Post || method == HttpMethods.Put))
            {
                // Leave stream open so next middleware can read it
                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 512, 
                    leaveOpen: true);
               
                // Read the request body
                var requestBody = await reader.ReadToEndAsync();

                // Sanitize the request body to remove sensitive information
                string sanitizedRequestBody = SanitizeRequestBody(requestBody); 

                // Reset stream position, so next middleware can read it from the beginning.
                context.Request.Body.Position = 0;
                
                // Write request body to Application Insights
                var requestTelemetry = context.Features.Get<RequestTelemetry>();
                requestTelemetry?.Properties.Add("RequestBody", sanitizedRequestBody);
                
                // Log the request body to Serilog sinks
                Log.Information("Request:" + sanitizedRequestBody);
            }
            // Call next middleware in the pipeline
            await _next(context);
        }

        // Sanitize the request body before logging it to remove sensitive data like "password".
        private string SanitizeRequestBody(string requestBody)
        {
            try
            {
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(requestBody);
               
                if (dictionary == null)
                {
                    return requestBody;
                }

                // List of keys to remove or mask
                var sensitiveKeys = new[] { "password", "confirmPassword", "creditCard" };

                foreach (var key in sensitiveKeys)
                {
                    if (dictionary.ContainsKey(key))
                    {
                        dictionary[key] = "***REDACTED***";
                    }
                }

                return JsonSerializer.Serialize(dictionary);
            }
            catch
            {
                // If it's not valid JSON, just return the original
                return requestBody;
            }
        }
    }
}
