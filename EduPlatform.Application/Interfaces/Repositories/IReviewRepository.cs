

using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<Review?> GetReviewByIdAsync(int reviewId);
        Task<IReadOnlyList<Review>> GetReviewsByCourseIdAsync(int courseId);
        Task<IReadOnlyList<Review>> GetUserReviewsAsync(int userId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int reviewId);
    }
}
