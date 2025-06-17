using EduPlatform.Application.DTOs.Course;
namespace EduPlatform.Application.Interfaces.Services
{
    public interface ISessionDetailService
    {
        Task<SessionDetailDTO?> GetSessionDetailByIdAsync(int id);
        Task UpdateVideoUrlAsync(int sessionId, string videoUrl);
    }
}
