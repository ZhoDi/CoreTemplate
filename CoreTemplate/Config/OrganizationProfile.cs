using AutoMapper;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemplate.Config
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile()
        {
            CreateMap<UserRegisterDto, User>();
        }
    }
}
