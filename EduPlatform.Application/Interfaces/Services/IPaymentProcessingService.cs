
namespace EduPlatform.Application.Interfaces.Services
{
    public interface IPaymentProcessingService
    {
        Task<bool> HandleStripeWebhookEventAsync(string paymentIntentId, string eventType);
    }
}