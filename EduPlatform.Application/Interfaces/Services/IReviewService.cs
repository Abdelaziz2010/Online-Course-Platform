using EduPlatform.Application.DTOs.Course;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ReviewDTO?> GetReviewByIdAsync(int reviewId); 
        Task<IReadOnlyList<ReviewDTO>> GetReviewsByCourseIdAsync(int courseId);
        Task<IReadOnlyList<ReviewDTO>> GetUserReviewsAsync(int userId);
        Task AddReviewAsync(CreateReviewDTO review);
        Task UpdateReviewAsync(ReviewDTO review);
        Task DeleteReviewAsync(int reviewId);
    }
}
