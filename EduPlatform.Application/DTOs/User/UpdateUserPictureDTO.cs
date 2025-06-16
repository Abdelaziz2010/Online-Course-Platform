using Microsoft.AspNetCore.Http;

namespace EduPlatform.Application.DTOs.User
{
    public record UpdateUserPictureDTO
    {
        public required int UserId { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
