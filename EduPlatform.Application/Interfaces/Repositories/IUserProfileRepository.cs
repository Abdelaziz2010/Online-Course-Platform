
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetUserInfoAsync(int userId);
        Task<bool> UpdateUserProfilePicture(int userId, string pictureUrl);
        Task<bool> UpdateUserBio(int userId, string bio);
    }
}
