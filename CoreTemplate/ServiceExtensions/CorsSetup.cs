using System;
using CoreTemplate.Application.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTemplate.ServiceExtensions
{
    /// <summary>
    /// Cors 配置
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(options =>
            {
                if (Convert.ToBoolean((Appsettings.App("Startup", "Cors", "EnableAllIp"))))
                {
                    //允许任意跨域请求
                    options.AddPolicy(Appsettings.App("Startup", "Cors", "PolicyName"),
                        build => build
                            .SetIsOriginAllowed((host) => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetPreflightMaxAge(TimeSpan.FromHours(1))//预检请求过期时间，用于减少OPTIONS请求;
                        );
                }
                else
                {
                    //指定IP跨域
                    options.AddPolicy(Appsettings.App("Startup", "Cors", "PolicyName"),
                        build => build
                            .WithOrigins(
                                Appsettings.App("Startup", "Cors", "CorsOrigins")
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetPreflightMaxAge(TimeSpan.FromHours(1)));//预检请求过期时间，用于减少OPTIONS请求;
                }
            });

        }
    }
}
