namespace EduPlatform.Application.DTOs.Category
{
    public record CategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
