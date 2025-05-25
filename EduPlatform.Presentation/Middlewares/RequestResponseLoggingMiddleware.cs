using Serilog;

namespace EduPlatform.Presentation.Middlewares
{
    // This middleware logs request and response details to Serilog sink (console).
    public class RequestResponseLoggingMiddleware
    {

        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request details
            Log.Information($"Request: {context.Request.Method} {context.Request.Path}");

            // Copy the original response body stream
            var originalBodyStream = context.Response.Body;

            // We create a MemoryStream to temporarily hold the response.
            using (var responseBody = new MemoryStream())
            {
                // Set the response body stream to the memory stream.
                context.Response.Body = responseBody;

                // Continue processing the request with the next middleware in the pipeline.
                await _next(context);

                // Log the response details
                var response = await FormatResponse(context.Response);
                Log.Information($"Response: {response}");

                // Copy the captured response to the original response body stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);   // reset the position of the stream to the beginning
            //response.Body.Position = 0;
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{response.StatusCode}: {text}";
        }
    }
}
