using EduPlatform.Application.DTOs.Course;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface ICourseService
    {
        Task<List<CourseDTO>> GetAllCoursesAsync(int? categoryId = null);
        Task<CourseDetailDTO> GetCourseDetailsByIdAsync(int courseId);
    }
}
