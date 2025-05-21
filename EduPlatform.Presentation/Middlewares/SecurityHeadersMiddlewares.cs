namespace Ecom.Presentation.Middlewares
{
    public class SecurityHeadersMiddleware
    {
        readonly RequestDelegate _next;
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Add security headers to protect against common vulnerabilities and attacks
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1;mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";

            await _next(context);
        }
    }
}
