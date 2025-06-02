
using EduPlatform.Application.DTOs.VideoRequest;

namespace EduPlatform.Application.Interfaces.Services
{
    public interface IVideoRequestService
    {
        Task<VideoRequestDTO?> GetVideoRequestByIdAsync(int id);
        Task<IReadOnlyList<VideoRequestDTO>> GetAllVideoRequestsAsync();
        Task<IReadOnlyList<VideoRequestDTO>> GetVideoRequestsByUserIdAsync(int userId);
        Task<VideoRequestDTO> CreateVideoRequestAsync(VideoRequestDTO videoRequestDto);
        Task<VideoRequestDTO> UpdateVideoRequestAsync(int id, VideoRequestDTO videoRequestDto);
        Task DeleteVideoRequestAsync(int id);
    }
}
