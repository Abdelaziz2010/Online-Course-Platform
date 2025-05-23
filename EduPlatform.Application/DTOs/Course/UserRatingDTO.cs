namespace EduPlatform.Application.DTOs.Course
{
    public record UserRatingDTO
    {
        public int CourseId { get; set; }
        public decimal AverageRating { get; set; } // this will hold the average rating from N number of users rating/review.
        public int TotalRating { get; set; }       // this will hold the total number of ratings/reviews for a course.
    }
}
