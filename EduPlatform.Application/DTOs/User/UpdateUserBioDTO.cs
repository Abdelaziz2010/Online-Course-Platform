
namespace EduPlatform.Application.DTOs.User
{
    public record UpdateUserBioDTO
    {
        public required int UserId { get; set; }
        public string? Bio { get; set; }
    }
}
