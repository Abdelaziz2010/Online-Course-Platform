using EduPlatform.Application.DTOs.Course;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task<IReadOnlyList<CourseDTO>> GetAllCoursesAsync(int? categoryId = null);
        Task<CourseDetailDTO> GetCourseDetailsByIdAsync(int courseId);
        Task<Course> GetCourseByIdAsync(int courseId);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int courseId);
        void RemoveSessionDetail(SessionDetail sessionDetail);
        Task<IReadOnlyList<Instructor>> GetAllInstructorsAsync();
        Task<bool> UpdateCourseThumbnail(string courseThumbnailUrl, int courseId);
    }
}
