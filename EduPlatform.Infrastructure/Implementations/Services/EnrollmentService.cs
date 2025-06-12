using AutoMapper;
using EduPlatform.Application.DTOs.Enrollment;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EnrollmentDTO?> GetEnrollmentByIdAsync(int enrollmentId)
        {
            if (enrollmentId <= 0)
            {
                throw new ArgumentException("Invalid enrollment ID", nameof(enrollmentId));
            }

            var enrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(enrollmentId);
           
            return enrollment is null ? null : _mapper.Map<EnrollmentDTO>(enrollment);
        }
        
        public async Task<IReadOnlyList<EnrollmentDTO>> GetEnrollmentsByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user ID", nameof(userId));
            }

            var enrollments = await _unitOfWork.EnrollmentRepository.GetByUserIdAsync(userId);

            return _mapper.Map<IReadOnlyList<EnrollmentDTO>>(enrollments);
        }
        
        public async Task<IReadOnlyList<EnrollmentDTO>> GetEnrollmentsByCourseIdAsync(int courseId)
        {
            if (courseId <= 0)
            {
                throw new ArgumentException("Invalid course ID", nameof(courseId));
            }

            var enrollments = await _unitOfWork.EnrollmentRepository.GetByCourseIdAsync(courseId);
            
            return _mapper.Map<IReadOnlyList<EnrollmentDTO>>(enrollments);
        }
        
        public async Task<EnrollmentDTO> CreateEnrollmentAsync(CreateEnrollmentDTO createEnrollmentDto)
        {
            if (createEnrollmentDto == null)
            {
                throw new ArgumentNullException(nameof(createEnrollmentDto), "Enrollment cannot be null");
            }

            var enrollment = _mapper.Map<Enrollment>(createEnrollmentDto);
            
            var result = await _unitOfWork.EnrollmentRepository.AddAsync(enrollment);
                        
            return _mapper.Map<EnrollmentDTO>(result);
        }
        
        public async Task<EnrollmentDTO> UpdateEnrollmentAsync(EnrollmentDTO enrollmentDto)
        {
            if (enrollmentDto == null)
            {
                throw new ArgumentNullException(nameof(enrollmentDto), "Enrollment cannot be null");
            }

            var enrollment = _mapper.Map<Enrollment>(enrollmentDto);
            
            var result = await _unitOfWork.EnrollmentRepository.UpdateAsync(enrollment);
            
            return _mapper.Map<EnrollmentDTO>(result);
        }
        
        public async Task<bool> DeleteEnrollmentAsync(int enrollmentId)
        {
            if (enrollmentId <= 0)
            {
                throw new ArgumentException("Invalid enrollment ID", nameof(enrollmentId));
            }

            var result = await _unitOfWork.EnrollmentRepository.DeleteAsync(enrollmentId);

            return result;
        }
    }
}
