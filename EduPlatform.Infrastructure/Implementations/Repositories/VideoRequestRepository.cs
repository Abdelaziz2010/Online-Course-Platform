
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Domain.Entities;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class VideoRequestRepository : IVideoRequestRepository
    {
        private readonly EduPlatformDbContext _context;

        public VideoRequestRepository(EduPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<VideoRequest>> GetAllAsync()
        {
            return await _context.VideoRequests
                .Include(vr => vr.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<VideoRequest?> GetByIdAsync(int id)
        {
            return await _context.VideoRequests
                .Include(vr => vr.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(vr => vr.VideoRequestId == id);
        }

        public async Task<IReadOnlyList<VideoRequest>> GetByUserIdAsync(int userId)
        {
            return await _context.VideoRequests
                .Include(vr => vr.User)
                .AsNoTracking()
                .Where(vr => vr.UserId == userId)
                .ToListAsync();
        }

        public async Task<VideoRequest> AddAsync(VideoRequest videoRequest)
        {
            _context.VideoRequests.Add(videoRequest);
            await _context.SaveChangesAsync();
            return videoRequest;
        }

        public async Task DeleteAsync(VideoRequest videoRequest)
        {
            _context.VideoRequests.Remove(videoRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<VideoRequest> UpdateAsync(VideoRequest videoRequest)
        {
           _context.VideoRequests.Update(videoRequest);
            await _context.SaveChangesAsync();
            return videoRequest;
        }
    }
}
