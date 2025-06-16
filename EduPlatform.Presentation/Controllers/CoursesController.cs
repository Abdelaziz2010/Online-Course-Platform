using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Infrastructure.Implementations.Services;
using EduPlatform.Presentation.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IAzureBlobStorageService _blobStorageService;

        public CoursesController(ICourseService courseService, IAzureBlobStorageService blobStorageService)
        {
            _courseService = courseService;
            _blobStorageService = blobStorageService;
        }

        // These 3 get methods are publicly available from our UI, no need to authenticate!

        // GET: api/Courses/Get-All-Courses
        [HttpGet("Get-All-Courses")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CourseDTO>>> GetAllCoursesAsync()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            if (!courses.Any())
            {
                return NotFound("No courses found.");
            }
            return Ok(courses);
        }


        // GET: api/Courses/Get-All-Courses-By-Category/{categoryId}
        [HttpGet("Get-All-Courses-By-Category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CourseDTO>>> GetAllCoursesByCategoryAsync([FromRoute] int categoryId)
        {
            var courses = await _courseService.GetAllCoursesAsync(categoryId);
            if (!courses.Any())
            {
                return NotFound($"No courses found for category ID {categoryId}.");
            }
            return Ok(courses);
        }

         
        // GET: api/Courses/Get-Course-Details-By-Id/{courseId}
        [HttpGet("Get-Course-Details-By-Id/{courseId}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseDetailDTO>> GetCourseDetailsByIdAsync([FromRoute] int courseId)
        {
            var course = await _courseService.GetCourseDetailsByIdAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost("Create-Course")]
        [Authorize]
        [AdminRole]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Write")]
        public async Task<IActionResult> CreateCourseAsync([FromBody] CourseDetailDTO courseDetailDTO)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _courseService.AddCourseAsync(courseDetailDTO);

            return Ok(courseDetailDTO);
        }

        [HttpPut("Update-Course/{id}")]
        [Authorize]
        [AdminRole]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Write")]
        public async Task<IActionResult> UpdateCourseAsync(int id, [FromBody] CourseDetailDTO courseDetailDTO)
        {
            if (id != courseDetailDTO.CourseId)
            {
                return BadRequest("Course ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _courseService.UpdateCourseAsync(courseDetailDTO);

            return NoContent();
        }

        [HttpPost("Upload-Thumbnail/{courseId}")]
        //[Authorize]
        //[AdminRole]
        //[RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Write")]
        public async Task<IActionResult> UploadThumbnail(int courseId, IFormFile file)
        {
            string thumbnailUrl = null;

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var course = await _courseService.GetCourseDetailsByIdAsync(courseId);
            
            if (course == null)
                return NotFound("Course not found");


            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                var fileExtension = Path.GetExtension(file.FileName);

                // Upload the byte array to Azure Blob Storage
                thumbnailUrl = await _blobStorageService.UploadAsync(
                    stream.ToArray(),
                    $"{courseId}_{course.Title.Trim().Replace(' ', '_')}{fileExtension}",
                    "course-images");
            }

            // Update the thumbnail URL in the database
            var success = await _courseService.UpdateCourseThumbnail(thumbnailUrl, courseId);
            
            if (!success)
            {
                return StatusCode(500, "Failed to update course thumbnail");
            }

            return Ok(new { Message = "Thumbnail updated successfully", thumbnailUrl });
        }

        [HttpDelete("Delete-Course/{id}")]
        [Authorize]
        [AdminRole]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Write")]
        public async Task<IActionResult> DeleteCourseAsync(int id)
        {
            await _courseService.DeleteCourseAsync(id);

            return NoContent();
        }

        [HttpGet("Get-All-Instructors")]
        [RequiredScope(RequiredScopesConfigurationKey = "AzureADB2C:Scopes:Read")]
        public async Task<ActionResult<List<InstructorDTO>>> GetAllInstructorsAsync()
        {
            var instructors = await _courseService.GetAllInstructorsAsync();

            return Ok(instructors);
        }
    }
}
