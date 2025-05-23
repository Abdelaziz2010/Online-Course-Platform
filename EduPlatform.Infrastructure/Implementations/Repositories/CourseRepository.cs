using EduPlatform.Application.DTOs.Category;
using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly EduPlatformDbContext _context;
        public CourseRepository(EduPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseDTO>> GetAllCoursesAsync(int? categoryId = null)
        {
            var courses = _context.Courses
                .Include(c => c.Category)  //pull the category
                .AsQueryable();

            // filter by category if provided
            if (categoryId.HasValue)
            {
                courses = courses.Where(c => c.CategoryId == categoryId.Value);
            }

            var courseList = await courses.Select(c => new CourseDTO
            {
                CourseId = c.CourseId,
                Title = c.Title,
                Description = c.Description,
                Price = c.Price,
                CourseType = c.CourseType,
                SeatsAvailable = c.SeatsAvailable,
                Duration = c.Duration,
                CategoryId = c.CategoryId,
                InstructorId = c.InstructorId,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = new CategoryDTO
                {
                    CategoryId = c.Category.CategoryId,
                    CategoryName = c.Category.CategoryName,
                    Description = c.Category.Description
                },
                UserRatingDTO = new UserRatingDTO
                {
                    CourseId = c.CourseId,
                    AverageRating = c.Reviews.Any() ? Convert.ToDecimal(c.Reviews.Average(r => r.Rating)) : 0,
                    TotalRating = c.Reviews.Count()
                }
            }).ToListAsync();

            return courseList;
        }

        public async Task<CourseDetailDTO> GetCourseDetailsByIdAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Category)             //pull the category
                .Include(c => c.Reviews)              //pull the reviews
                .Include(c => c.SessionDetails)       //pull the session details
                .Where(c => c.CourseId == courseId)
                .Select(c => new CourseDetailDTO
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    Description = c.Description,
                    Price = c.Price,
                    CourseType = c.CourseType,
                    SeatsAvailable = c.SeatsAvailable,
                    Duration = c.Duration,
                    CategoryId = c.CategoryId,
                    InstructorId = c.InstructorId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Category = new CategoryDTO
                    {
                        CategoryId = c.Category.CategoryId,
                        CategoryName = c.Category.CategoryName,
                        Description = c.Category.Description
                    },
                    Reviews = c.Reviews.Select(r => new ReviewDTO
                    {
                        ReviewId = r.ReviewId,
                        CourseId = r.CourseId,
                        UserId = r.UserId,
                        UserName = r.User.DisplayName,
                        Rating = r.Rating,
                        Comments = r.Comments,
                        ReviewDate = r.ReviewDate,
                    }).OrderByDescending(o => o.Rating).Take(10).ToList(),
                    SessionDetails = c.SessionDetails.Select(s => new SessionDetailDTO
                    {
                        SessionId = s.SessionId,
                        CourseId = s.CourseId,
                        Title = s.Title,
                        Description = s.Description,
                        VideoUrl = s.VideoUrl,
                        VideoOrder = s.VideoOrder
                    }).OrderBy(s => s.VideoOrder).ToList(),
                    UserRatingDTO = new UserRatingDTO
                    {
                        CourseId = c.CourseId,
                        AverageRating = c.Reviews.Any() ? Convert.ToDecimal(c.Reviews.Average(r => r.Rating)) : 0,
                        TotalRating = c.Reviews.Count()
                    },
                }).FirstOrDefaultAsync();

            return course;
        }
    }
}
