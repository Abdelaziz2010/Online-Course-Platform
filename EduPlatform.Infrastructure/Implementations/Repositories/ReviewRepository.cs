using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Domain.Entities;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly EduPlatformDbContext _context;

        public ReviewRepository(EduPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _context.Reviews
                .Include(r => r.Course)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task<IReadOnlyList<Review>> GetReviewsByCourseIdAsync(int courseId)
        {
            return await _context.Reviews
                .Include (r => r.User)
                .AsNoTracking()
                .Where( r => r.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Review>> GetUserReviewsAsync(int userId)
        {
            return await _context.Reviews
                .Include (r => r.Course)
                .AsNoTracking()
                .Where ( r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);

            if (review != null) 
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
