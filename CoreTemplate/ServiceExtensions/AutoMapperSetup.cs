using CoreTemplate.Application.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTemplate.ServiceExtensions
{
    /// <summary>
    /// AutoMapper服务
    /// </summary>
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfig));
            AutoMapperConfig.RegisterMappings();
        }
    }
}
