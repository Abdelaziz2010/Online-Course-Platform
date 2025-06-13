
using EduPlatform.Application.DTOs.Payment;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface IStripeService
    {
        Task<StripePaymentResponse> CreatePaymentIntentAsync(StripePaymentRequest request);
        Task<StripePaymentResponse> ConfirmPaymentAsync(string paymentIntentId);
        Task HandleWebhookAsync(string json, string stripeSignature);
    }
}