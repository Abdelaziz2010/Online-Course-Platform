using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Domain.Entities;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EduPlatformDbContext _context;

        public PaymentRepository(EduPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Enrollment)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }

        public async Task<Payment?> GetByStripePaymentIntentIdAsync(string paymentIntentId)
        {
            return await _context.Payments
                .Include(p => p.Enrollment)
                .FirstOrDefaultAsync(p => p.StripePaymentIntentId == paymentIntentId);
        }

        public async Task<IReadOnlyList<Payment>> GetByEnrollmentIdAsync(int enrollmentId)
        {
            return await _context.Payments
                .Include(p => p.Enrollment)
                .Where(p => p.EnrollmentId == enrollmentId)
                .ToListAsync();
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                return false;
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


