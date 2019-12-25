using CoreTemplate.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate.Startup
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            //启用
            if (bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                //策略授权,加角色
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("User", policy => policy.RequireRole("User").Build());
                    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                    options.AddPolicy("AdminOrUser", policy => policy.RequireRole("Admin","User"));
                }).AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                }).AddJwtBearer("JwtBearer", options =>
                {
                    options.Audience = configuration["Authentication:JwtBearer:Audience"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 秘钥
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                        // 发行人
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                        // 订阅人
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                        // 令牌过期
                        ValidateLifetime = true,
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,

                        // 时间偏移量,不设置默认五分钟
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = QueryStringTokenResolver
                    };
                });
            }
        }
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/signalr"))
            {
                // We are just looking for signalr clients
                return Task.CompletedTask;
            }

            var qsAuthToken = context.HttpContext.Request.Query["enc_auth_token"].FirstOrDefault();
            if (qsAuthToken == null)
            {
                // Cookie value does not matches to querystring value
                return Task.CompletedTask;
            }

            context.Token = qsAuthToken;

            return Task.CompletedTask;
        }
    }
}
