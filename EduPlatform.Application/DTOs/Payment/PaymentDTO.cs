
namespace EduPlatform.Application.DTOs.Payment
{
    public record PaymentDTO
    {
        public int PaymentId { get; set; }

        public int EnrollmentId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public string PaymentMethod { get; set; } = null!;

        public string PaymentStatus { get; set; } = null!;
    }
}
