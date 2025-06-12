
using EduPlatform.Application.DTOs.Enrollment;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int enrollmentId);
        Task<IReadOnlyList<EnrollmentDTO>> GetEnrollmentsByUserIdAsync(int userId);
        Task<IReadOnlyList<EnrollmentDTO>> GetEnrollmentsByCourseIdAsync(int courseId);
        Task<EnrollmentDTO> CreateEnrollmentAsync(CreateEnrollmentDTO createEnrollmentDto);
        Task<EnrollmentDTO> UpdateEnrollmentAsync(UpdateEnrollmentDTO updateEnrollmentDTO);
        Task<bool> DeleteEnrollmentAsync(int enrollmentId);
    }
}
