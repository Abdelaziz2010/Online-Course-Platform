using AutoMapper;
using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReviewDTO?> GetReviewByIdAsync(int reviewId)
        {
            if (reviewId <= 0)
            {
                throw new ArgumentException("Invalid review ID", nameof(reviewId));
            }

            var review = await _unitOfWork.ReviewRepository.GetReviewByIdAsync(reviewId);
            
            return review != null ? _mapper.Map<ReviewDTO>(review) : null;
        }

        public async Task<IReadOnlyList<ReviewDTO>> GetReviewsByCourseIdAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("Invalid course ID", nameof(courseId));
            }

            var reviews = await _unitOfWork.ReviewRepository.GetReviewsByCourseIdAsync(courseId);
            
            if (!reviews.Any())
            {
                return new List<ReviewDTO>();
            }
            
            return _mapper.Map<IReadOnlyList<ReviewDTO>>(reviews);
        }

        public async Task<IReadOnlyList<ReviewDTO>> GetUserReviewsAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            var reviews = await _unitOfWork.ReviewRepository.GetUserReviewsAsync(userId);
            
            if (!reviews.Any())
            {
                return new List<ReviewDTO>();
            }
            
            return _mapper.Map<IReadOnlyList<ReviewDTO>>(reviews);
        }

        public async Task AddReviewAsync(ReviewDTO reviewDto)
        {
            ValidateReview(reviewDto);

            var review = _mapper.Map<Review>(reviewDto);

            review.ReviewDate = DateTime.UtcNow;

            await _unitOfWork.ReviewRepository.AddReviewAsync(review);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(ReviewDTO reviewDto)
        {
            ValidateReview(reviewDto);

            var existingReview = await _unitOfWork.ReviewRepository.GetReviewByIdAsync(reviewDto.ReviewId);
           
            if (existingReview == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewDto.ReviewId} not found.");
            }

            _mapper.Map(reviewDto, existingReview);
            
            await _unitOfWork.ReviewRepository.UpdateReviewAsync(existingReview);
            
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            if (reviewId <= 0)
            {
                throw new ArgumentException("Invalid review ID", nameof(reviewId));
            }

            var review = await _unitOfWork.ReviewRepository.GetReviewByIdAsync(reviewId);
            
            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewId} not found.");
            }

            await _unitOfWork.ReviewRepository.DeleteReviewAsync(reviewId);
            
            await _unitOfWork.SaveChangesAsync();
        }

        private void ValidateReview(ReviewDTO review)
        {
            ArgumentNullException.ThrowIfNull(review);

            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new ValidationException("Rating must be between 1 and 5");
            }

            if (review.CourseId <= 0)
            {
                throw new ValidationException("Invalid course ID");
            }

            if (review.UserId <= 0)
            {
                throw new ValidationException("Invalid user ID");
            }

            if (string.IsNullOrWhiteSpace(review.UserName))
            {
                throw new ValidationException("User name is required");
            }

            if (review.Comments != null && review.Comments.Length > 2000)
            {
                throw new ValidationException("Comments cannot exceed 2000 characters");
            }
        }
    }
}
