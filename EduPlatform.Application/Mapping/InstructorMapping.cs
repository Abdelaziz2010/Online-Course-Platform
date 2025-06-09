using AutoMapper;
using EduPlatform.Application.DTOs.Course;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class InstructorMapping : Profile
    {
        public InstructorMapping() 
        {
            CreateMap<Instructor, InstructorDTO>();
            CreateMap<InstructorDTO, Instructor>();
        }

    }
}
