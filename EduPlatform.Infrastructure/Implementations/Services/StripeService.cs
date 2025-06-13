using EduPlatform.Application.DTOs.Payment;
using EduPlatform.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentProcessingService _paymentProcessingService;
        private readonly string _secretKey;
        private readonly string _webhookSecret;

        public StripeService(
            IConfiguration configuration,
            IPaymentProcessingService paymentProcessingService)
        {
            _configuration = configuration;
            _paymentProcessingService = paymentProcessingService;
            _secretKey = _configuration["Stripe:SecretKey"] ?? throw new ArgumentNullException(nameof(_secretKey));
            _webhookSecret = _configuration["Stripe:WebhookSecret"] ?? throw new ArgumentNullException(nameof(_webhookSecret));
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<StripePaymentResponse> CreatePaymentIntentAsync(StripePaymentRequest request)
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = request.Currency,
                    PaymentMethodTypes = new List<string> { "card" },
                    Metadata = new Dictionary<string, string>
                    {
                        { "CourseId", request.CourseId.ToString() },
                        { "UserId", request.UserId.ToString() }
                    }
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                return new StripePaymentResponse
                {
                    PaymentIntentId = paymentIntent.Id,
                    ClientSecret = paymentIntent.ClientSecret,
                    Status = paymentIntent.Status
                };
            }
            catch (StripeException ex)
            {
                return new StripePaymentResponse
                {
                    Status = "failed",
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<StripePaymentResponse> ConfirmPaymentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);

                return new StripePaymentResponse
                {
                    PaymentIntentId = paymentIntent.Id,
                    ClientSecret = paymentIntent.ClientSecret,
                    Status = paymentIntent.Status
                };
            }
            catch (StripeException ex)
            {
                return new StripePaymentResponse
                {
                    Status = "failed",
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task HandleWebhookAsync(string json, string stripeSignature)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    stripeSignature,
                    _webhookSecret
                );

                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await HandleSuccessfulPayment(paymentIntent);
                }
                else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    await HandleFailedPayment(paymentIntent);
                }
            }
            catch (StripeException ex)
            {
                throw new ApplicationException("Failed to process Stripe webhook", ex);
            }
        }

        private async Task HandleSuccessfulPayment(PaymentIntent? paymentIntent)
        {
            if (paymentIntent is null)
            {
                return;
            }

            // Update payment and enrollment status using enrollment service
            // The service handles both updates in a single transaction
            await _paymentProcessingService.HandleStripeWebhookEventAsync(paymentIntent.Id, "payment_intent.succeeded");
        }

        private async Task HandleFailedPayment(PaymentIntent? paymentIntent)
        {
            if (paymentIntent is null)
            {
                return;
            }

            // Update payment and enrollment status using enrollment service
            // The service handles both updates in a single transaction
            await _paymentProcessingService.HandleStripeWebhookEventAsync(paymentIntent.Id, "payment_intent.payment_failed");
        }
    }
}

