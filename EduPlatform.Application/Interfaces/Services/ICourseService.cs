using EduPlatform.Application.DTOs.Course;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface ICourseService
    {
        Task<IReadOnlyList<CourseDTO>> GetAllCoursesAsync(int? categoryId = null);
        Task<CourseDetailDTO> GetCourseDetailsByIdAsync(int courseId);
        Task AddCourseAsync(CourseDetailDTO courseDTO); 
        Task UpdateCourseAsync(CourseDetailDTO courseDTO);
        Task DeleteCourseAsync(int courseId);
        Task<IReadOnlyList<InstructorDTO>> GetAllInstructorsAsync();
        Task<bool> UpdateCourseThumbnail(string courseThumbnailUrl, int courseId);
    }
}
