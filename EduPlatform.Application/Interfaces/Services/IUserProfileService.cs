
using EduPlatform.Application.DTOs.User;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface IUserProfileService
    {
        Task<UserDTO?> GetUserInfoAsync(int userId);
        Task<bool> UpdateUserProfilePicture(int userId, string pictureUrl);
        Task<bool> UpdateUserBio(int userId, string bio);
    }
}
