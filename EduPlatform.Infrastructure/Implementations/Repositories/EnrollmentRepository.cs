using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Domain.Entities;
using EduPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduPlatform.Infrastructure.Implementations.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly EduPlatformDbContext _context;
        public EnrollmentRepository(EduPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<Enrollment?> GetEnrollmentByIdAsync(int enrollmentId)
        {
            return await _context.Enrollments
                .Include(e => e.Payments)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId);
        }

        public async Task<IReadOnlyList<Enrollment>> GetEnrollmentsByUserIdAsync(int userId)
        {
            return await _context.Enrollments
                .Include(e => e.Payments)
                .Include(e => e.Course)
                .AsNoTracking()
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Enrollment>> GetEnrollmentsByCourseIdAsync(int courseId)
        {
            return await _context.Enrollments
               .Include(e => e.Payments)
               .AsNoTracking()
               .Where(e => e.CourseId == courseId)
               .ToListAsync();
        }

        public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }
    }
}
