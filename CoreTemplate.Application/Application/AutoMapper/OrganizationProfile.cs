using AutoMapper;
using CoreTemplate.Application.Dto;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Domain;
using CoreTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemplate.Application.AutoMapper
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
