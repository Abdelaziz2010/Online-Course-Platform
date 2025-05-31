using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }


        // GET: api/Courses/Get-All-Courses/{categoryId}
        [HttpGet("Get-All-Courses")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CourseDTO>>> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }


        // GET: api/Courses/Get-All-Courses-By-Category/{categoryId}
        [HttpGet("Get-All-Courses-By-Category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CourseDTO>>> GetAllCoursesByCategory(int categoryId)
        {
            var courses = await _courseService.GetAllCoursesAsync(categoryId);
            return Ok(courses);
        }

         
        // GET: api/Courses/Get-Course-Details-By-Id/{courseId}
        [HttpGet("Get-Course-Details-By-Id/{courseId}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseDetailDTO>> GetCourseDetailsById(int courseId)
        {
            var course = await _courseService.GetCourseDetailsByIdAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }
    }
}
