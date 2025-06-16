using AutoMapper;
using EduPlatform.Application.DTOs.User;
using EduPlatform.Domain.Entities;

namespace EduPlatform.Application.Mapping
{
    public class UserMapping  : Profile
    {
        public UserMapping()
        {
            CreateMap<UserProfile, UserDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<UserRole, UserRoleDTO>().ReverseMap();
        }
    }
}
