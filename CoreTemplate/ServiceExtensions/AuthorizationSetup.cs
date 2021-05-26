using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using CoreTemplate.Application.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CoreTemplate.ServiceExtensions
{
    /// <summary>
    /// 授权服务
    /// </summary>
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {

            if (services == null) throw new ArgumentNullException(nameof(services));

            #region 令牌
            var issuer = Appsettings.App("Authentication", "JwtBearer", "Issuer");
            var audience = Appsettings.App("Authentication", "JwtBearer", "Audience");
            var tokenValidationParameters = new TokenValidationParameters
            {
                // 秘钥
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Appsettings.App("Authentication", "JwtBearer", "SecurityKey"))),
                // 发行人
                ValidateIssuer = true,
                ValidIssuer = issuer,
                // 订阅人
                ValidateAudience = true,
                ValidAudience = audience,
                // 令牌过期
                ValidateLifetime = true,
                RequireSignedTokens = true,
                RequireExpirationTime = true,

                // 时间偏移量,不设置默认五分钟
                ClockSkew = TimeSpan.Zero
            };
            #endregion

            #region 1.授权

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("User", policy => policy.RequireRole("User").Build());
                    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                    options.AddPolicy("AdminOrUser", policy => policy.RequireRole("Admin", "User").Build());
                });

            #endregion

            #region 2.认证

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = QueryStringTokenResolver,

                    OnChallenge = context =>
                    {
                        context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                        if (jwtToken.Issuer != issuer)
                        {
                            context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                        }

                        if (jwtToken.Audiences.FirstOrDefault() != audience)
                        {
                            context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                        }
                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            #endregion


            //四种授权
            // 1、简单授权只需要在对应的上面加入对应的Role。例：[Authorize(Roles = "Admin,User")]
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("User", policy => policy.RequireRole("User").Build());
            //    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            //    options.AddPolicy("AdminOrUser", policy => policy.RequireRole("Admin", "User").Build());
            //});

            // 2、不用再写多个。例：[Authorize(Policy = "AdminOrUser")]
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("User", policy => policy.RequireRole("User").Build());
            //    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            //    options.AddPolicy("AdminOrUser", policy => policy.RequireRole("Admin", "User").Build());
            //});

            // 3、自定义复杂的策略授权
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(Permissions.Name,
            //             policy => policy.Requirements.Add(permissionRequirement));
            //});

            // 4、基于Scope策略授权
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Scope_BlogModule_Policy", builder =>
            //    {
            //        //客户端Scope中包含blog.core.api.BlogModule才能访问
            //        // 同时引用nuget包：IdentityServer4.AccessTokenValidation
            //        builder.RequireScope("blog.core.api.BlogModule");
            //    });
            //});
        }

        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/signalr"))
            {
                return Task.CompletedTask;
            }

            var authToken = context.HttpContext.Request.Query["auth_token"];
            if (!authToken.IsNullOrEmpty())
            {
                context.Token = authToken;
            }
            return Task.CompletedTask;
        }
    }
}
