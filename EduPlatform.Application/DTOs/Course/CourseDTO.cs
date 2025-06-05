using EduPlatform.Application.DTOs.Category;

namespace EduPlatform.Application.DTOs.Course
{
    public record CourseDTO
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string CourseType { get; set; } = null!;

        public int? SeatsAvailable { get; set; }

        public decimal Duration { get; set; }

        public int CategoryId { get; set; }

        public int InstructorId { get; set; }

        public int InstructorUserId { get; set; }
        
        public string? Thumbnail { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual CategoryDTO? Category { get; set; } = null!; // I want to show the category for each course.

        public UserRatingDTO? UserRatingDTO { get; set; } = null!;  // I want to gather the average rating for each course.
    }
}
