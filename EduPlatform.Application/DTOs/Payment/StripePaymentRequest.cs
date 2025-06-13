
namespace EduPlatform.Application.DTOs.Payment
{
    public record StripePaymentRequest
    {
        public long Amount { get; init; }
        public string Currency { get; init; } = "usd";
        public int CourseId { get; init; }
        public int UserId { get; init; }
        public string? Description { get; init; }
    }
}
