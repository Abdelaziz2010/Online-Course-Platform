
using EduPlatform.Application.DTOs.Payment;

namespace EduPlatform.Application.DTOs.Enrollment
{
    public record EnrollmentDTO
    {
        public int EnrollmentId { get; set; }
       
        public int CourseId { get; set; }
        
        public string? CourseTitle { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public DateTime EnrollmentDate { get; set; }
        
        public string PaymentStatus { get; set; } = null!;
        
        public PaymentDTO PaymentDTO { get; set; }
    }
}
