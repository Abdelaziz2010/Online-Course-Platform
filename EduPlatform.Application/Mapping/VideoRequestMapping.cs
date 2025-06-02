
using AutoMapper;
using EduPlatform.Application.DTOs.VideoRequest;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class VideoRequestMapping : Profile
    {
        public VideoRequestMapping()
        {
            CreateMap<VideoRequest, VideoRequestDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));

            CreateMap<VideoRequestDTO, VideoRequest>()  // // We don't map User here since it's handled separately
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
