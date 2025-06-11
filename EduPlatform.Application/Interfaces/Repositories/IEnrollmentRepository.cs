using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment?> GetEnrollmentByIdAsync(int enrollmentId); 
        Task<IReadOnlyList<Enrollment>> GetEnrollmentsByUserIdAsync(int userId);
        Task<IReadOnlyList<Enrollment>> GetEnrollmentsByCourseIdAsync(int courseId);
        Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment);
        Task<Enrollment> UpdateEnrollmentAsync(Enrollment enrollment);
    }
}
