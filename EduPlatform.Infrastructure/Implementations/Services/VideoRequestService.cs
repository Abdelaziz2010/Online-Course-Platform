using AutoMapper;
using EduPlatform.Application.DTOs.VideoRequest;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class VideoRequestService : IVideoRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VideoRequestService> _logger;

        public VideoRequestService(IUnitOfWork unitOfWork, IMapper mapper, 
            IConfiguration configuration, ILogger<VideoRequestService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IReadOnlyList<VideoRequestDTO>> GetAllVideoRequestsAsync()
        {
            var videoRequests = await _unitOfWork.VideoRequestRepository.GetAllAsync();

            if (!videoRequests.Any())  // Check if the list is empty, if so return an empty list.
            {
                return new List<VideoRequestDTO>();
            }

            return _mapper.Map<IReadOnlyList<VideoRequestDTO>>(videoRequests); // Execute mapping from the source object to a new destination object.
        }

        public async Task<VideoRequestDTO?> GetVideoRequestByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid video request ID.", nameof(id));
            }

            var videoRequest = await _unitOfWork.VideoRequestRepository.GetByIdAsync(id);

            return videoRequest == null ? null : _mapper.Map<VideoRequestDTO>(videoRequest);
        }

        public async Task<IReadOnlyList<VideoRequestDTO>> GetVideoRequestsByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID.", nameof(userId));
            }

            var videoRequests = await _unitOfWork.VideoRequestRepository.GetByUserIdAsync(userId);
            
            if (!videoRequests.Any())
            {
                return new List<VideoRequestDTO>();
            }

            return _mapper.Map<IReadOnlyList<VideoRequestDTO>>(videoRequests);
        }

        public async Task<VideoRequestDTO> CreateVideoRequestAsync(VideoRequestDTO videoRequestDto)
        {
            if (videoRequestDto is null)
            {
                throw new ArgumentNullException(nameof(videoRequestDto), "Video request cannot be null.");
            }

            var videoRequest = _mapper.Map<VideoRequest>(videoRequestDto);

            var createdVideoRequest = await _unitOfWork.VideoRequestRepository.AddAsync(videoRequest); // pass to repo layer.

            return _mapper.Map<VideoRequestDTO>(createdVideoRequest);
        }

        public async Task<VideoRequestDTO> UpdateVideoRequestAsync(int id, VideoRequestDTO videoRequestDto)
        {
            if (videoRequestDto is null)
            {
                throw new ArgumentNullException(nameof(videoRequestDto), "Video request cannot be null.");
            }

            if (id <= 0)
            {
                throw new ArgumentException("Invalid video request ID.", nameof(id));
            }

            var existingVideoRequest = await _unitOfWork.VideoRequestRepository.GetByIdAsync(id);
            
            if (existingVideoRequest is null)
            {
                throw new KeyNotFoundException($"Video request with ID {id} not found.");
            }

            videoRequestDto.UserId = existingVideoRequest.UserId;  // set it to user's id itself if not it becomes admin's request

            _mapper.Map(videoRequestDto, existingVideoRequest);    // Execute mapping from source object to the existing destination object.

            var updatedVideoRequest = await _unitOfWork.VideoRequestRepository.UpdateAsync(existingVideoRequest);

            return _mapper.Map<VideoRequestDTO>(updatedVideoRequest);
        }

        public async Task DeleteVideoRequestAsync(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("Invalid video request ID.", nameof(id));
            }

            var existingVideoRequest = await _unitOfWork.VideoRequestRepository.GetByIdAsync(id);
            
            if (existingVideoRequest is null)
            {
                throw new KeyNotFoundException($"Video request with ID {id} not found.");
            }

            await _unitOfWork.VideoRequestRepository.DeleteAsync(existingVideoRequest);
        }
    }
}
