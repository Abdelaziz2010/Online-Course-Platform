using AutoMapper;
using EduPlatform.Application.DTOs.Course;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            // Map Review to ReviewDTO
            CreateMap<Review, ReviewDTO>()
                .ForMember(dest => dest.UserName, op => op.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            // Map ReviewDTO to Review
            CreateMap<ReviewDTO, Review>()
                .ForMember(dest => dest.User, op => op.Ignore())
                .ForMember(dest => dest.Course, op => op.Ignore());

            // Map CreateReviewDTO to Review
            CreateMap<CreateReviewDTO, Review>()
                .ForMember(dest => dest.ReviewId, op => op.Ignore()) // Ignore ReviewId for creation
                .ForMember(dest => dest.User, op => op.Ignore()) 
                .ForMember(dest => dest.Course, op => op.Ignore()); 
        }
    }
}
