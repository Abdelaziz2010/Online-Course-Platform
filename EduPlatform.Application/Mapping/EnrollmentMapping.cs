using AutoMapper;
using EduPlatform.Application.DTOs.Enrollment;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class EnrollmentMapping : Profile
    {
        public EnrollmentMapping()
        {
            // Mapping from EnrollmentDTO to Enrollment
            CreateMap<EnrollmentDTO, Enrollment>();

            // Mapping from Enrollment to EnrollmentDTO
            CreateMap<Enrollment, EnrollmentDTO>()
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.PaymentDTO, opt => opt.MapFrom(src => src.Payments.FirstOrDefault()));

            // Mapping from CreateEnrollmentDTO to Enrollment
            CreateMap<CreateEnrollmentDTO, Enrollment>()
                .ForMember(dest => dest.EnrollmentId, opt => opt.Ignore()) // Ignore auto-generated ID
                .ForMember(dest => dest.Course, opt => opt.Ignore())       // Course navigation property
                .ForMember(dest => dest.User, opt => opt.Ignore())         // User navigation property
                .ForMember(dest => dest.Payments, opt => opt.Ignore());    // Payments collection

            // Mapping from UpdateEnrollmentDTO to Enrollment
            CreateMap<UpdateEnrollmentDTO, Enrollment>();


        }
    }
}
