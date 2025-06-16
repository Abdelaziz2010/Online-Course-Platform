using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Domain.Entities;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly EduPlatformDbContext _context;
        public UserProfileRepository(EduPlatformDbContext context)
        {
            _context = context;
        }
        public async Task<UserProfile?> GetUserInfoAsync(int userId)
        {
            var user = await _context.UserProfiles
                .Include(u => u.Instructors)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role) // Include the Role navigation property
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (user == null)
            {
                return null;
            }

            return user;

        }

        public async Task<bool> UpdateUserBio(int userId, string bio)
        {
            var user = await _context.Instructors.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user != null)
            {
                user.Bio = bio;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateUserProfilePicture(int userId, string pictureUrl)
        {
            var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserId == userId);
            
            if (user != null)
            {
                user.ProfilePictureUrl = pictureUrl;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
