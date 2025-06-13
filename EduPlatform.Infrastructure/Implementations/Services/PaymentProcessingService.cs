using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class PaymentProcessingService : IPaymentProcessingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentProcessingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandleStripeWebhookEventAsync(string paymentIntentId, string eventType)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var payment = await _unitOfWork.PaymentRepository.GetByStripePaymentIntentIdAsync(paymentIntentId);
                if (payment == null) return false;

                var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(payment.EnrollmentId);
                if (enrollment == null) return false;

                switch (eventType)
                {
                    case "payment_intent.succeeded":
                        payment.PaymentStatus = "Completed";
                        enrollment.PaymentStatus = "Completed";
                        break;
                    case "payment_intent.payment_failed":
                        payment.PaymentStatus = "Failed";
                        enrollment.PaymentStatus = "Failed";
                        break;
                    default:
                        return false;
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}