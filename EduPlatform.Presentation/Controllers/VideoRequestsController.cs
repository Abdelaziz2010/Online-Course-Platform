using Asp.Versioning;
using EduPlatform.Application.DTOs.VideoRequest;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Identity.Web.Resource;
using System.Collections.Generic;

namespace EduPlatform.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class VideoRequestsController : ControllerBase
    {
        // This controller is responsible for handling video request related operations.
        private readonly IVideoRequestService _videoRequestService;
        private readonly IUserClaims _userClaims; 
        public VideoRequestsController(IVideoRequestService videoRequestService, IUserClaims userClaims)
        {
            _videoRequestService = videoRequestService;
            _userClaims = userClaims;
        }



        [EnableRateLimiting("ReadOnlyPolicy")]
        [HttpGet("Get-All-Video-Requests")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Read")]
        // This endpoint retrieves all video requests. Admins can see all requests, while regular users can only see their own.
        public async Task<ActionResult<IReadOnlyList<VideoRequestDTO>>> GetAllAsync()
        {
            IReadOnlyList<VideoRequestDTO> videoRequests;

            var userRoles = _userClaims.GetUserRoles();

            if(userRoles.Contains("Admin"))
            {
                videoRequests = await _videoRequestService.GetAllVideoRequestsAsync();
            }
            else
            {
                var userId = _userClaims.GetUserId();
                videoRequests = await _videoRequestService.GetVideoRequestsByUserIdAsync(userId);
            }

            return Ok(videoRequests);
        }

        [HttpGet("Get-Video-Request-By-Id/{id}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Read")]
        public async Task<ActionResult<VideoRequestDTO?>> GetByIdAsync(int id)
        {
            var videoRequest = await _videoRequestService.GetVideoRequestByIdAsync(id);

            if (videoRequest == null)
            {
                return NotFound($"Video request with ID {id} not found.");
            }

            return Ok(videoRequest);
        }

        [HttpGet("Get-Video-Requests-By-User-Id/{userId}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Read")]
        public async Task<ActionResult<IReadOnlyList<VideoRequestDTO>>> GetByUserIdAsync(int userId)
        {
            var videoRequests = await _videoRequestService.GetVideoRequestsByUserIdAsync(userId);

            if (!videoRequests.Any())
            {
                return NotFound($"No video requests found for user ID {userId}.");
            }

            return Ok(videoRequests);
        }

        [EnableRateLimiting("WritePolicy")]
        [HttpPost("Create-Video-Request")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Write")]
        public async Task<ActionResult<VideoRequestDTO>> CreateAsync([FromBody] VideoRequestDTO videoRequest)
        {
            var createdVideoRequest = await _videoRequestService.CreateVideoRequestAsync(videoRequest);
            // 201 Created status code indicates that a new resource has been created successfully.
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdVideoRequest.VideoRequestId }, createdVideoRequest);
        }

        [HttpPut("Update-Video-Request/{id}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Write")]
        public async Task<ActionResult<VideoRequestDTO>> UpdateAsync(int id, [FromBody] VideoRequestDTO videoRequest)
        {
            if (id != videoRequest.VideoRequestId)
            {
                return BadRequest("Video request ID mismatch.");
            }

            try
            {
                var updatedVideoRequest = await _videoRequestService.UpdateVideoRequestAsync(id, videoRequest);

                return Ok(updatedVideoRequest);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Video request with ID {id} not found.");
            }
        }

        [HttpDelete("Delete-Video-Request/{id}")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Write")] 
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _videoRequestService.DeleteVideoRequestAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Video request with ID {id} not found.");
            }
        }
    }
}
