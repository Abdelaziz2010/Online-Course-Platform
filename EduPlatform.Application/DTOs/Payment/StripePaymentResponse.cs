
namespace EduPlatform.Application.DTOs.Payment
{
    public record StripePaymentResponse
    {
        public string PaymentIntentId { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public string? ErrorMessage { get; init; }
    }
}
