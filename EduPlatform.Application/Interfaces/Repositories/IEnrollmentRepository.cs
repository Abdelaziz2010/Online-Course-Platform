using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetByIdAsync(int enrollmentId); 
        Task<IReadOnlyList<Enrollment>> GetByUserIdAsync(int userId);
        Task<IReadOnlyList<Enrollment>> GetByCourseIdAsync(int courseId);
        Task<Enrollment> AddAsync(Enrollment enrollment);
        Task<Enrollment> UpdateAsync(Enrollment enrollment);
        Task<bool> DeleteAsync(int enrollmentId);
    }
}
