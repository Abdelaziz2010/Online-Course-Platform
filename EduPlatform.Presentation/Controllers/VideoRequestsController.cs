using EduPlatform.Application.DTOs.VideoRequest;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoRequestsController : ControllerBase
    {
        // This controller is responsible for handling video request related operations.
        private readonly IVideoRequestService _videoRequestService;
        public VideoRequestsController(IVideoRequestService videoRequestService)
        {
            _videoRequestService = videoRequestService;
        }
        
        [HttpGet("Get-All-Video-Requests")]
        public async Task<ActionResult<IReadOnlyList<VideoRequestDTO>>> GetAllAsync()
        {
            var videoRequests = await _videoRequestService.GetAllVideoRequestsAsync();

            if (!videoRequests.Any())
            {
                return NotFound("No video requests found.");
            }

            return Ok(videoRequests);
        }

        [HttpGet("Get-Video-Request-By-Id/{id}")]
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
        public async Task<ActionResult<IReadOnlyList<VideoRequestDTO>>> GetByUserIdAsync(int userId)
        {
            var videoRequests = await _videoRequestService.GetVideoRequestsByUserIdAsync(userId);

            if (!videoRequests.Any())
            {
                return NotFound($"No video requests found for user ID {userId}.");
            }

            return Ok(videoRequests);
        }

        [HttpPost("Create-Video-Request")]
        public async Task<ActionResult<VideoRequestDTO>> CreateAsync([FromBody] VideoRequestDTO videoRequest)
        {
            var createdVideoRequest = await _videoRequestService.CreateVideoRequestAsync(videoRequest);
            // 201 Created status code indicates that a new resource has been created successfully.
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdVideoRequest.VideoRequestId }, createdVideoRequest);
        }

        [HttpPut("Update-Video-Request/{id}")]
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
