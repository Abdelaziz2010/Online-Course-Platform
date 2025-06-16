using EduPlatform.Application.DTOs.User;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Ensure only authenticated users can access these endpoints
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        /// <summary>
        /// Get user profile information
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>User profile information</returns>
        /// <response code="200">Returns the user profile information</response>
        /// <response code="404">If the user is not found</response>
        [HttpGet("Get-User-Info/{userId}")]
        [ProducesResponseType(typeof(UserDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<UserDTO>> GetUserInfo(int userId)
        {
            try
            {
                var userInfo = await _userProfileService.GetUserInfoAsync(userId);
                
                if (userInfo == null)
                {
                    return NotFound($"User with ID {userId} not found.");
                }

                return Ok(userInfo);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Update user's profile picture
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="pictureUrl">The URL of the profile picture</param>
        /// <returns>Success status</returns>
        /// <response code="200">Profile picture updated successfully</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the user is not found</response>
        [HttpPut("Update-Profile-Picture/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProfilePicture(int userId, [FromBody] string pictureUrl)
        {
            try
            {
                var result = await _userProfileService.UpdateUserProfilePicture(userId, pictureUrl);
                
                if (!result)
                {
                    return NotFound($"User with ID {userId} not found.");
                }

                return Ok("Profile picture updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Update user's bio
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="bio">The new bio text</param>
        /// <returns>Success status</returns>
        /// <response code="200">Bio updated successfully</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the user is not found or is not an instructor</response>
        [HttpPut("Update-Profile-Bio/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateBio(int userId, [FromBody] string bio)
        {
            try
            {
                var result = await _userProfileService.UpdateUserBio(userId, bio);
                
                if (!result)
                {
                    return NotFound($"User with ID {userId} not found or is not an instructor.");
                }

                return Ok("Bio updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}