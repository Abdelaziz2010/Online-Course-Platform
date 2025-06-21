using Asp.Versioning;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class PaymentsController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        public PaymentsController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("WebHook")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeSignature = Request.Headers["Stripe-Signature"];

            try
            {
                await _stripeService.HandleWebhookAsync(json, stripeSignature);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
