using AutoMapper;
using EduPlatform.Application.DTOs;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CourseCategory, CategoryDTO>().ReverseMap();
        }
    }
}
