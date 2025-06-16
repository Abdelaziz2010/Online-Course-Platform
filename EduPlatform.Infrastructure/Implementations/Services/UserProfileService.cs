using EduPlatform.Application.DTOs.User;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDTO?> GetUserInfoAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }

            var userProfile = await _unitOfWork.UserProfileRepository.GetUserInfoAsync(userId);

            if (userProfile == null)
            {
                return null;
            }

            // Store the first instructor (if any) to avoid redundant calls
            var instructor = userProfile.Instructors.FirstOrDefault();

            // Map UserProfile to UserDTO
            var userDto = new UserDTO
            {
                UserId = userProfile.UserId,
                DisplayName = userProfile.DisplayName,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                ProfilePictureUrl = userProfile.ProfilePictureUrl,
                UserRoles = userProfile.UserRoles.Select(role => new UserRoleDTO
                {
                    UserRoleId = role.UserRoleId,
                    RoleId = role.RoleId,
                    RoleName = role.Role?.RoleName ?? "Unknown Role", // Handle null Role,
                    UserId = role.UserId
                }).ToList(),
                Bio = instructor?.Bio ?? string.Empty // Set Bio only if the user is an instructor
            };

            return userDto;
        }
        public async Task<bool> UpdateUserProfilePicture(int userId, string pictureUrl)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(pictureUrl))
            {
                throw new ArgumentException("Picture URL cannot be null or empty.", nameof(pictureUrl));
            }

            await _unitOfWork.UserProfileRepository.UpdateUserProfilePicture(userId, pictureUrl);

            return true;
        }
        public async Task<bool> UpdateUserBio(int userId, string bio)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(bio))
            {
                throw new ArgumentException("Bio cannot be null or empty.", nameof(bio));
            }

            return await _unitOfWork.UserProfileRepository.UpdateUserBio(userId, bio);
        }
    }
}
