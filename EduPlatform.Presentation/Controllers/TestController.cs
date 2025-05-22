using EduPlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduPlatform.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly EduPlatformDbContext _context;

        public TestController(EduPlatformDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllCourses")]
        public IActionResult GetCourses()
        {
            var courses = _context.Courses.ToList();
            return Ok(courses);
        }


        [HttpGet("GetAllPayments")]
        public IActionResult GetPayments()
        {
            var payments = _context.Payments.ToList();
            return Ok(payments);
        }


        [HttpGet("GetAllEnrollments")]
        public IActionResult GetEnrollments()
        {
            var enrollments = _context.Enrollments.ToList();
            return Ok(enrollments);
        }


        [HttpGet("GetAllCategories")]
        public IActionResult GetCategories()
        {
            var categories = _context.CourseCategories.ToList();
            return Ok(categories);
        }


        [HttpGet("GetAllInstructors")]
        public IActionResult GetInstructors()
        {
            var instructors = _context.Instructors.ToList();
            return Ok(instructors);
        }


        [HttpGet("GetAllUsers")]
        public IActionResult GetUsers()
        {
            var users = _context.UserProfiles.ToList();
            return Ok(users);
        }


        [HttpGet("GetAllReviews")]
        public IActionResult GetReviews()
        {
            var reviews = _context.Reviews.ToList();
            return Ok(reviews);
        }


        [HttpGet("GetAllRoles")]
        public IActionResult GetRoles()
        {
            var roles = _context.Roles.ToList();
            return Ok(roles);
        }


        [HttpGet("GetAllSmartApps")]
        public IActionResult GetSmartApps()
        {
            var smartApps = _context.SmartApps.ToList();
            return Ok(smartApps);
        }
        
        
        [HttpGet("GetAllVideoRequests")]
        public IActionResult GetVideoRequests()
        {
            var videoRequests = _context.VideoRequests.ToList();
            return Ok(videoRequests);
        }
        
        
        [HttpGet("GetAllSessionDetails")]
        public IActionResult GetSessionDetails()
        {
            var sessionDetails = _context.SessionDetails.ToList();
            return Ok(sessionDetails);
        }
        
        
        [HttpGet("GetAllUserRoles")]
        public IActionResult GetUserRoles()
        {
            var userRoles = _context.UserRoles.ToList();
            return Ok(userRoles);
        }

    }
}
