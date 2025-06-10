using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        /// <summary>
        /// Get a review by its ID
        /// </summary>
        [HttpGet("Get-Review-By-Id/{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReviewDTO>> GetReviewByIdAsync(int reviewId)
        {
            try 
            {
                var review = await _reviewService.GetReviewByIdAsync(reviewId);

                if (review == null)
                {
                    return NotFound($"Review with ID {reviewId} not found.");
                }
                return Ok(review);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Get all reviews for a specific course
        /// </summary>
        [HttpGet("Get-Reviews-By-Course/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ReviewDTO>>> GetReviewsByCourseAsync(int courseId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByCourseIdAsync(courseId);
                
                return Ok(reviews);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Get all reviews for a specific user
        /// </summary>
        [HttpGet("Get-Reviews-By-User/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<ReviewDTO>>> GetReviewsByUserAsync(int userId)
        {
            try
            {
                var reviews = await _reviewService.GetUserReviewsAsync(userId);
               
                return Ok(reviews);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Create a new review
        /// </summary>
        [HttpPost("Create-Review")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateReviewAsync([FromBody] CreateReviewDTO createReviewDto)
        {
            try
            {
                await _reviewService.AddReviewAsync(createReviewDto);

                // Return created at action with the new resource URL
                return Created();
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is ValidationException)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Update an existing review
        /// </summary>
        [HttpPut("Update-Review/{reviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateReviewAsync([FromRoute] int reviewId, [FromBody] ReviewDTO reviewDto)
        {
            if (reviewId != reviewDto.ReviewId)
            {
                return BadRequest("Review ID mismatch");
            }

            try
            {
                await _reviewService.UpdateReviewAsync(reviewDto);

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is ValidationException)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Delete a review
        /// </summary>
        [HttpDelete("Delete-Review/{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteReviewAsync(int reviewId)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(reviewId);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
