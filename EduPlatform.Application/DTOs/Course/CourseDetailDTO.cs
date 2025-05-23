
namespace EduPlatform.Application.DTOs.Course
{
    public record CourseDetailDTO : CourseDTO
    {
        public List<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();

        public List<SessionDetailDTO> SessionDetails { get; set; } = new List<SessionDetailDTO>();
    }
}
