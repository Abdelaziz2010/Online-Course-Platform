
namespace EduPlatform.Application.DTOs.Enrollment
{
    public record UpdateEnrollmentDTO
    {
        public int EnrollmentId { get; set; }

        public int CourseId { get; set; }

        public int UserId { get; set; }

        public string PaymentStatus { get; set; } = null!;
    }
}
