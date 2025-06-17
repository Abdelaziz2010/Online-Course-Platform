using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Domain.Entities;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class SessionDetailRepository : ISessionDetailRepository
    {
        private readonly EduPlatformDbContext _context;

        public SessionDetailRepository(EduPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<SessionDetail?> GetByIdAsync(int id)
        {
            return await _context.SessionDetails.FindAsync(id);
        }

        public async Task<IEnumerable<SessionDetail>> GetByCourseIdAsync(int courseId)
        {
            return await _context.SessionDetails
                .Where(s => s.CourseId == courseId)
                .ToListAsync();
        }

        public async Task UpdateAsync(SessionDetail sessionDetail)
        {
            _context.SessionDetails.Update(sessionDetail);
            await _context.SaveChangesAsync();
        }
    }
}
