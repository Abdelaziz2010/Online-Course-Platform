using Asp.Versioning;
using EduPlatform.Presentation.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EduPlatform.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }


        [HttpGet()]
        public async Task Health()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            await HealthCheckResponseWriter.WriteJsonResponse(HttpContext, report);
        }

        [HttpGet("live")]
        public IActionResult Liveness()
        {
            return Ok(new
            {
                status = "Healthy",
                description = "Liveness check - the app is running"
            });
        }

        [HttpGet("ready")]
        public async Task Readiness()
        {
            var report = await _healthCheckService.CheckHealthAsync(h => h.Tags.Contains("ready"));
            
            await HealthCheckResponseWriter.WriteJsonResponse(HttpContext, report);
        }
    }
}