using EduPlatform.Domain.Entities;
namespace EduPlatform.Application.Interfaces.Repositories
{
    // IRepository for SessionDetail
    public interface ISessionDetailRepository
    {
        Task<SessionDetail?> GetByIdAsync(int id);
        Task<IEnumerable<SessionDetail>> GetByCourseIdAsync(int courseId);
        Task AddAsync(SessionDetail sessionDetail);
        Task<bool> UpdateAsync(SessionDetail sessionDetail);
        Task DeleteAsync(SessionDetail sessionDetail);
    }
}
