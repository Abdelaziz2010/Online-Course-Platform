
namespace EduPlatform.Application.DTOs.User
{
    public record RoleDTO
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;
    }
}
