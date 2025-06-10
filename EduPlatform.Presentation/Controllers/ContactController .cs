using EduPlatform.Application.DTOs;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ContactController : ControllerBase
    {
        private readonly IEmailNotification _emailNotification;
       
        public ContactController(IEmailNotification emailNotification)
        {
            _emailNotification = emailNotification;
        }

        [HttpPost("Send-Message")]
        public async Task<IActionResult> SendMessageAsync([FromBody] ContactMessageDTO contactMessage)
        { 
            if (contactMessage == null)
            {
                return BadRequest("Contact message cannot be null.");
            }

            await _emailNotification.SendEmailForContactUs(contactMessage);

            return Ok(new { message = "Message sent successfully!", model = contactMessage });
        }
    }
}
