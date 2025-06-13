using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int paymentId);
        Task<Payment?> GetByStripePaymentIntentIdAsync(string paymentIntentId);
        Task<IReadOnlyList<Payment>> GetByEnrollmentIdAsync(int enrollmentId);
        Task<Payment> AddAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(int paymentId);
    }
}
