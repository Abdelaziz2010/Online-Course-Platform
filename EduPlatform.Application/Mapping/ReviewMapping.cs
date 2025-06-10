using AutoMapper;
using EduPlatform.Application.DTOs.Course;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            CreateMap<Review, ReviewDTO>()
                .ForMember(dest => dest.UserName, op => op.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            CreateMap<ReviewDTO, Review>()
                .ForMember(dest => dest.User, op => op.Ignore())
                .ForMember(dest => dest.Course, op => op.Ignore());
        }
    }
}
