

using EduPlatform.Application.DTOs.Course;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<List<CourseDTO>> GetAllCoursesAsync(int? categoryId = null);

        // get the details of a specific course by its ID.
        Task<CourseDetailDTO> GetCourseDetailsByIdAsync(int courseId); 
    }
}
