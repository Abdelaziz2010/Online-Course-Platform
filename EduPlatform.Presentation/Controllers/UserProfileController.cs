using EduPlatform.Application.DTOs.User;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Ensure only authenticated users can access these endpoints
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IAzureBlobStorageService _blobStorageService;

        public UserProfileController(IUserProfileService userProfileService, IAzureBlobStorageService blobStorageService)
        {
            _userProfileService = userProfileService;
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Get user profile information
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>User profile information</returns>
        /// <response code="200">Returns the user profile information</response>
        /// <response code="404">If the user is not found</response>
        [HttpGet("Get-User-Info/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
        /// <param name="updatePictureDto">The picture update data</param>
        /// <returns>Success status</returns>
        /// <response code="200">Profile picture updated successfully</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the user is not found</response>
        [HttpPut("Update-Profile-Picture")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateUserPictureDTO updatePictureDto)
        {
            try
            {
                if (updatePictureDto.Picture is null)
                {
                    return BadRequest("No picture file was provided.");
                }

                using (var stream = new MemoryStream())
                {
                    await updatePictureDto.Picture.CopyToAsync(stream);
                   
                    var fileExtension = Path.GetExtension(updatePictureDto.Picture.FileName);
                    
                    // Upload to blob storage
                    var pictureUrl = await _blobStorageService.UploadAsync(
                        stream.ToArray(),
                        $"{updatePictureDto.UserId}_profile_picture{fileExtension}"
                    );

                    // Update the profile picture URL in the database
                    var result = await _userProfileService.UpdateUserProfilePicture(updatePictureDto.UserId, pictureUrl);
                    
                    if (!result)
                    {
                        return NotFound($"User with ID {updatePictureDto.UserId} not found.");
                    }

                    return Ok(new { Message = "Picture updated successfully.", pictureUrl }); 
                }
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
        /// <param name="updateBioDto">The bio update data</param>
        /// <returns>Success status</returns>
        /// <response code="200">Bio updated successfully</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the user is not found or is not an instructor</response>
        [HttpPut("Update-Profile-Bio")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProfileBio([FromBody] UpdateUserBioDTO updateBioDto)
        {
            try 
            {
                var result = await _userProfileService.UpdateUserBio(updateBioDto.UserId, updateBioDto.Bio ?? string.Empty);
                
                if (!result)
                {
                    return NotFound($"User with ID {updateBioDto.UserId} not found or is not an instructor.");
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