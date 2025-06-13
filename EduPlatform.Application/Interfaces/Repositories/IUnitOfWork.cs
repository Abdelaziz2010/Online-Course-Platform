
using Microsoft.EntityFrameworkCore.Storage;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IVideoRequestRepository VideoRequestRepository { get; }
        public IReviewRepository ReviewRepository { get; }
        public IEnrollmentRepository EnrollmentRepository { get; }
        public IPaymentRepository PaymentRepository { get; }
        Task SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
