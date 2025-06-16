
namespace EduPlatform.Application.DTOs.User
{
    public record UserRoleDTO
    {
        public int UserRoleId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public int UserId { get; set; }
    }
}
