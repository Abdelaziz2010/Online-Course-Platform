using AutoMapper;
using EduPlatform.Application.DTOs.Enrollment;
using EduPlatform.Application.DTOs.Payment;
using EduPlatform.Application.Interfaces.Repositories;
using EduPlatform.Application.Interfaces.Services;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Infrastructure.Implementations.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStripeService _stripeService;
        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, IStripeService stripeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stripeService = stripeService;
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
            ArgumentNullException.ThrowIfNull(createEnrollmentDto);

            // Check for existing enrollment before creating a new one, to prevent duplicates enrollments
            var existingEnrollment = await _unitOfWork.EnrollmentRepository.GetByCourseAndUserIdAsync(
                createEnrollmentDto.CourseId,
                createEnrollmentDto.UserId);

            if (existingEnrollment != null)
            {
                throw new InvalidOperationException($"User {createEnrollmentDto.UserId} is already enrolled in course {createEnrollmentDto.CourseId}");
            }

            // Get course details for payment amount
            var course = await _unitOfWork.CourseRepository.GetCourseByIdAsync(createEnrollmentDto.CourseId);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {createEnrollmentDto.CourseId} not found.");
            }

            // Create enrollment with pending payment status
            var enrollment = _mapper.Map<Enrollment>(createEnrollmentDto);
            enrollment.EnrollmentDate = DateTime.UtcNow; 
            enrollment.PaymentStatus = "Pending";

            // Create Stripe payment intent
            var paymentRequest = new StripePaymentRequest
            {
                Amount = (long)(course.Price * 100),   // Convert to cents
                Currency = "usd",
                CourseId = course.CourseId,
                UserId = createEnrollmentDto.UserId,
                Description = $"Enrollment in course: {course.Title}"
            };

            var stripePayment = await _stripeService.CreatePaymentIntentAsync(paymentRequest); 

            if (string.IsNullOrEmpty(stripePayment.ClientSecret))
            {
                throw new InvalidOperationException("Failed to create payment intent with Stripe.");
            }

            // create a new payment record
            var payment = new Payment
            {
                Amount = course.Price,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "Pending",
                PaymentMethod = "Credit Card",
                StripePaymentIntentId = stripePayment.PaymentIntentId
            };

            enrollment.Payments.Add(payment);

            // Save enrollment and payment record with pending status
            var result = await _unitOfWork.EnrollmentRepository.AddAsync(enrollment);

            var createdEnrollmentWithDetails = await _unitOfWork.EnrollmentRepository.GetByIdAsync(result.EnrollmentId);

            var enrollmentDto = _mapper.Map<EnrollmentDTO>(createdEnrollmentWithDetails);

            // Add Stripe payment intent client secret to the response
            enrollmentDto.PaymentDTO.ClientSecret = stripePayment.ClientSecret;

            return enrollmentDto;
        }
        
        public async Task<EnrollmentDTO> UpdateEnrollmentAsync(UpdateEnrollmentDTO updateEnrollmentDTO)
        {
            ArgumentNullException.ThrowIfNull(updateEnrollmentDTO);

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
