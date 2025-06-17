
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EduPlatformDbContext _context;
        private ICategoryRepository _categoryRepository;
        private ICourseRepository _courseRepository;
        private IVideoRequestRepository _videoRequestRepository;
        private IReviewRepository _reviewRepository;
        private IEnrollmentRepository _enrollmentRepository;
        private IPaymentRepository _paymentRepository;
        private IUserProfileRepository _userProfileRepository;
        private ISessionDetailRepository _sessionDetailRepository;

        public UnitOfWork(EduPlatformDbContext context)
        {
            _context = context;
        }


        //lazy intialization
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
        public ICourseRepository CourseRepository => _courseRepository ??= new CourseRepository(_context);
        public IVideoRequestRepository VideoRequestRepository => _videoRequestRepository ??= new VideoRequestRepository(_context);
        public IReviewRepository ReviewRepository => _reviewRepository ??= new ReviewRepository(_context);
        public IEnrollmentRepository EnrollmentRepository => _enrollmentRepository ??= new EnrollmentRepository(_context);
        public IPaymentRepository PaymentRepository => _paymentRepository ??= new PaymentRepository(_context);
        public IUserProfileRepository UserProfileRepository => _userProfileRepository ??= new UserProfileRepository(_context);
        public ISessionDetailRepository SessionDetailRepository => _sessionDetailRepository ??= new SessionDetailRepository(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
