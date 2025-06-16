
namespace EduPlatform.Application.DTOs.User
{
    public record UserDTO
    {
        public int UserId { get; set; }

        public string DisplayName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string AdObjId { get; set; } = null!;
        
        public string? ProfilePictureUrl { get; set; }
        
        public string? Bio { get; set; }
        
        public required List<UserRoleDTO> UserRoles { get; set; }  
    }
}
