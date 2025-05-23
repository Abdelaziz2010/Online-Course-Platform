
using AutoMapper;
using EduPlatform.Application.DTOs.Course;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork  _unitOfWork;
        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CourseDTO>> GetAllCoursesAsync(int? categoryId = null)
        {
            return await _unitOfWork.CourseRepository.GetAllCoursesAsync(categoryId);
        }

        public async Task<CourseDetailDTO> GetCourseDetailsByIdAsync(int courseId)
        {
            return await _unitOfWork.CourseRepository.GetCourseDetailsByIdAsync(courseId);
        }
    }
}
