using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class SessionDetailService : ISessionDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SessionDetailDTO?> GetSessionDetailByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid session ID", nameof(id));
            }

            var session = await _unitOfWork.SessionDetailRepository.GetByIdAsync(id);

            if (session == null)
            {
                return null; // or throw an exception if preferred
            }

            return new SessionDetailDTO
            {
                SessionId = session.SessionId,
                CourseId = session.CourseId,
                Title = session.Title,
                Description = session.Description,
                VideoUrl = session.VideoUrl,
                VideoOrder = session.VideoOrder
            };
        }

        public async Task UpdateVideoUrlAsync(int sessionId, string videoUrl)
        {
            if (sessionId <= 0)
            {
                throw new ArgumentException("Invalid session ID", nameof(sessionId));
            }

            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                throw new ArgumentException("Video URL cannot be null or empty", nameof(videoUrl));
            }

            var session = await _unitOfWork.SessionDetailRepository.GetByIdAsync(sessionId);
            
            if (session == null)
            {
                throw new KeyNotFoundException($"Session with ID {sessionId} not found.");
            }

            session.VideoUrl = videoUrl;

            await _unitOfWork.SessionDetailRepository.UpdateAsync(session);
        }
    }
}
