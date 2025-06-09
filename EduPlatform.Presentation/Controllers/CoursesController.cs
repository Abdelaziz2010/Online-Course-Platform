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
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
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
