using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;

namespace CoreTemplate.Application.Extension
{
    /// <summary>
    /// Swagger 服务
    /// </summary>
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {

            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(options =>
            {
                //这里的v1,对应下面注册json路径中的v1
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CoreTemplate API文档",
                    Version = "v0.1.0",
                    Description = "个人DotNetCore框架说明文档"
                });

                //获取注释xml文件,没有它也行,但是接口没注释
                //获取程序根目录
                var basePath = AppContext.BaseDirectory;
                //true,显示控制器注释
                options.IncludeXmlComments(Path.Combine(basePath, "CoreTemplate.xml"), true);

                // 开启加权小锁
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权直接在下框中输入Bearer {token},空格不可省略",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }
    }
}
