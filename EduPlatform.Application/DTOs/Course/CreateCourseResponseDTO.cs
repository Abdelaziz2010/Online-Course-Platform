
namespace EduPlatform.Application.DTOs.Course
{
    public record CreateCourseResponseDTO
    {
        public int CourseId { get; set; }
        public List<int> SessionIds { get; set; } = new();
    }
}
