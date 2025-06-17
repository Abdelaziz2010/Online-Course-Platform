namespace EduPlatform.Application.DTOs.Course
{
    // For creating new course	
    public record CreateCourseDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CourseType { get; set; } = null!;
        public int? SeatsAvailable { get; set; }
        public decimal Duration { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<CreateSessionDetailDTO> SessionDetails { get; set; } = new();

    }
}
