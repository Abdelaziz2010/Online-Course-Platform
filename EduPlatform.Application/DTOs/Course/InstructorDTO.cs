
namespace EduPlatform.Application.DTOs.Course
{
    public record InstructorDTO
    {
        public int InstructorId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Bio { get; set; }

        public int UserId { get; set; }
    }
}
