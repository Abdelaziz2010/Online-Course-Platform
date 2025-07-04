﻿using Asp.Versioning;
using EduPlatform.Application.DTOs;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EduPlatform.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class ContactController : ControllerBase
    {
        private readonly IEmailNotification _emailNotification;
       
        public ContactController(IEmailNotification emailNotification)
        {
            _emailNotification = emailNotification;
        }

        [EnableRateLimiting("WritePolicy")]
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
