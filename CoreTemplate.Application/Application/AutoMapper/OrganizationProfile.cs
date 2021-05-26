using AutoMapper;
using CoreTemplate.Application.Model.User.Dto;
using CoreTemplate.Domain.Entities;

namespace CoreTemplate.Application.Application.AutoMapper
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
