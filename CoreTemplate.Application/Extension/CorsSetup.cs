using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplate.Application.Helper;

namespace CoreTemplate.Application.Extension
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
                if (Appsettings.App("Startup", "Cors", "EnableAllIp").ObjToBool())
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
