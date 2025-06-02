
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Interfaces.Repositories
{
    public interface IVideoRequestRepository
    {
        Task<IReadOnlyList<VideoRequest>> GetAllAsync();
        Task<VideoRequest?> GetByIdAsync(int id);
        Task<IReadOnlyList<VideoRequest>> GetByUserIdAsync(int userId);
        Task<VideoRequest> AddAsync(VideoRequest videoRequest);  
        Task<VideoRequest> UpdateAsync(VideoRequest videoRequest);
        Task DeleteAsync(VideoRequest videoRequest);
    }
}
