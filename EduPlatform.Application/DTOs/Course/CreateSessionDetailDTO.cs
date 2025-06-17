
namespace EduPlatform.Application.DTOs.Course
{
    public record CreateSessionDetailDTO
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int VideoOrder { get; set; }
    }
}
