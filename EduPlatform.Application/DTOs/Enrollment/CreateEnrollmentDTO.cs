
namespace EduPlatform.Application.DTOs.Enrollment
{
    public record CreateEnrollmentDTO
    {
        public int CourseId { get; set; }
        
        public int UserId { get; set; }
    }
}
