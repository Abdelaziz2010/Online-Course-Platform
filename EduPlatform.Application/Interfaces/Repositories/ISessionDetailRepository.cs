using EduPlatform.Domain.Entities;
namespace EduPlatform.Application.Interfaces.Repositories
{
    // Repository for SessionDetail
    public interface ISessionDetailRepository
    {
        Task<SessionDetail?> GetByIdAsync(int id);
        Task<IEnumerable<SessionDetail>> GetByCourseIdAsync(int courseId);
        Task UpdateAsync(SessionDetail sessionDetail); 
    }
}
