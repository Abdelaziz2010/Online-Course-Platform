﻿using EduPlatform.Application.Interfaces.Repositories;
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

        public async Task<Enrollment?> GetByIdAsync(int enrollmentId)
        {
            return await _context.Enrollments
                .Include(e => e.Payments)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentId); // Returns null if not found
        }

        public async Task<Enrollment?> GetByCourseAndUserIdAsync(int courseId, int userId)
        {
            return await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.UserId == userId); // Returns null if not found
        }

        public async Task<IReadOnlyList<Enrollment>> GetByUserIdAsync(int userId)
        {
            return await _context.Enrollments
                .Include(e => e.Payments)
                .Include(e => e.Course)
                .AsNoTracking()
                .Where(e => e.UserId == userId)
                .ToListAsync();    // Returns an empty list if no enrollments found
        }

        public async Task<IReadOnlyList<Enrollment>> GetByCourseIdAsync(int courseId)
        {
            return await _context.Enrollments
               .Include(e => e.Payments)
               .AsNoTracking()
               .Where(e => e.CourseId == courseId)
               .ToListAsync();    // Returns an empty list if no enrollments found
        }

        public async Task<Enrollment> AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<Enrollment> UpdateAsync(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<bool> DeleteAsync(int enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollment is null)
            {
                return false; // Record not found
            }

            // delete the related payments first if necessary (Cascade delete)
            var payments = await _context.Payments.Where(p => p.EnrollmentId == enrollmentId).ToListAsync();
            
            if (payments.Any())
            {
                _context.Payments.RemoveRange(payments);
                await _context.SaveChangesAsync(); // Ensure payments are deleted first
            }
            
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true; // Deletion successful
        }
    }
}
