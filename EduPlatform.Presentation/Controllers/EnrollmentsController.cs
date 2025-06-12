using EduPlatform.Application.DTOs.Enrollment;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        
        public EnrollmentsController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        /// <summary>
        /// Get enrollment by ID
        /// </summary>
        [HttpGet("Get-Enrollment-By-Id/{enrollmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnrollmentDTO>> GetEnrollmentByIdAsync(int enrollmentId)
        {
            try
            {
                var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);
               
                if (enrollment == null)
                {
                    return NotFound($"Enrollment with ID {enrollmentId} not found.");
                }
                
                return Ok(enrollment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all enrollments for a specific user
        /// </summary>
        [HttpGet("Get-User-Enrollments/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<EnrollmentDTO>>> GetUserEnrollmentsAsync(int userId)
        {
            try
            {
                var enrollments = await _enrollmentService.GetEnrollmentsByUserIdAsync(userId);
                
                return Ok(enrollments);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all enrollments for a specific course
        /// </summary>
        [HttpGet("Get-Course-Enrollments/{courseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<EnrollmentDTO>>> GetCourseEnrollmentsAsync(int courseId)
        {
            try
            {
                var enrollments = await _enrollmentService.GetEnrollmentsByCourseIdAsync(courseId);

                return Ok(enrollments);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new enrollment
        /// </summary>
        [HttpPost("Create-Enrollment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EnrollmentDTO>> CreateEnrollmentAsync(CreateEnrollmentDTO createEnrollmentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var enrollment = await _enrollmentService.CreateEnrollmentAsync(createEnrollmentDto);
                
                return CreatedAtAction(nameof(GetEnrollmentByIdAsync), new { enrollmentId = enrollment.EnrollmentId }, enrollment);
            }
            catch (InvalidOperationException ex) // This exception is thrown if the user is already enrolled in the course
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Update an existing enrollment
        /// </summary>
        [HttpPut("Update-Enrollment/{enrollmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnrollmentDTO>> UpdateEnrollmentAsync(int enrollmentId, EnrollmentDTO enrollmentDto)
        {
            if (enrollmentDto.EnrollmentId != enrollmentId)
            {
                return BadRequest("Enrollment ID mismatch.");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedEnrollment = await _enrollmentService.UpdateEnrollmentAsync(enrollmentDto);
                
                return Ok(updatedEnrollment);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an enrollment by ID
        /// </summary>
        [HttpDelete("Delete-Enrollment/{enrollmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEnrollmentAsync(int enrollmentId)
        {
            try
            {
                var result = await _enrollmentService.DeleteEnrollmentAsync(enrollmentId);
               
                if (!result)
                {
                    return NotFound($"Enrollment with ID {enrollmentId} not found.");
                }
               
                return Ok($"Enrollment with ID {enrollmentId} was successfully deleted.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
