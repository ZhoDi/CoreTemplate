using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using CoreTemplate.Application.Application.AutoMapper;

namespace CoreTemplate.Application.AutoMapper
{
    /// <summary>
    /// AutoMapper配置文件
    /// </summary>
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrganizationProfile());
            });
        }
    }
}
