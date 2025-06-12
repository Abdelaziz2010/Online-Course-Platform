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

            // Check for existing enrollment before creating a new one, to prevent duplicates enrollments
            var existingEnrollment = await _unitOfWork.EnrollmentRepository.GetByCourseAndUserIdAsync(
                createEnrollmentDto.CourseId,
                createEnrollmentDto.UserId);

            if (existingEnrollment != null)
            {
                throw new InvalidOperationException($"User {createEnrollmentDto.UserId} is already enrolled in course {createEnrollmentDto.CourseId}");
            }

            var enrollment = _mapper.Map<Enrollment>(createEnrollmentDto);
            
            enrollment.EnrollmentDate = DateTime.UtcNow; // Set the enrollment date to now
            enrollment.PaymentStatus = "Completed";      // Temporary payment status

            // create a new payment record
            var payment = new Payment
            {
                EnrollmentId = enrollment.EnrollmentId,
                Amount = 0,                    // For Free 
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "Completed",   // Temporary status
                PaymentMethod = "Credit Card", // Example payment method, adjust as needed
            };

            enrollment.Payments.Add(payment);

            var result = await _unitOfWork.EnrollmentRepository.AddAsync(enrollment);

            var createdEnrollmentWithDetails = await _unitOfWork.EnrollmentRepository.GetByIdAsync(result.EnrollmentId);
             
            return _mapper.Map<EnrollmentDTO>(createdEnrollmentWithDetails);
        }
        
        public async Task<EnrollmentDTO> UpdateEnrollmentAsync(UpdateEnrollmentDTO updateEnrollmentDTO)
        {
            if (updateEnrollmentDTO == null)
            {
                throw new ArgumentNullException(nameof(updateEnrollmentDTO), "Enrollment cannot be null");
            }

            var existingEnrollment = await _unitOfWork.EnrollmentRepository.GetByIdAsync(updateEnrollmentDTO.EnrollmentId);
           
            if (existingEnrollment == null)
            {
                throw new KeyNotFoundException($"Enrollment with ID {updateEnrollmentDTO.EnrollmentId} not found.");
            }

            _mapper.Map(updateEnrollmentDTO, existingEnrollment);
            
            var result = await _unitOfWork.EnrollmentRepository.UpdateAsync(existingEnrollment);
            
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
