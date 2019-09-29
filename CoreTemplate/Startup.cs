using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreTemplate.AuthHelper;
using CoreTemplate.Domain.IRepositories;
using CoreTemplate.EntityFrameworkCore;
using CoreTemplate.EntityFrameworkCore.Repositories;
using CoreTemplate.EntityFrameworkCore.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace CoreTemplate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    ////忽略循环引用
                    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    ////使用默认Model返回数据格式
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    ////使用默认Json序列化格式(小驼峰)
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    ////设置时间格式
                    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                });
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                //这里的v1,对应下面注册json路径中的v1
                c.SwaggerDoc("v1", new Info
                {
                    Title = "CoreTemplate API文档",
                    Version = "v0.1.0",
                    Description = "个人DotNetCore框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "阿迪",
                        Email = "910824572@qq.com",
                        Url = "https://www.zhaodi.top/"
                    },
                    License = new License
                    {
                        Name = "个人许可证",
                        Url = "http://www.zhaodi.top/"
                    }
                });
                //获取注释xml文件,没有它也行,但是接口没注释
                //获取程序根目录
                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "CoreTemplate.xml");
                c.IncludeXmlComments(xmlPath, true);//true,显示控制器注释

                //Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "CoreTemplate", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("CoreTemplate", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
            #endregion

            #region 认证
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireRole("User").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrUser", policy => policy.RequireRole("Admin,User").Build());
            })
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //options.Audience = Configuration["Authentication:Audience"];
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Query["AccessToken"];
                        return Task.CompletedTask;
                    }
                };

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //设置令牌值
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:SecurityKey"])),
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],

                    //开启验证
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    //允许的服务器时间偏移量,不设置默认五分钟
                     ClockSkew = TimeSpan.FromSeconds(10),
                };
            });
            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy("CorsLocal",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(
                            Configuration["CorsLocal"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        )
                    );
            });
            #endregion

            #region MySql数据库
            var connection = this.Configuration.GetValue<string>("ConnStr");
            services.AddDbContext<TempDbContext>(options => options.UseMySql(connection));
            services.BuildServiceProvider().GetService<TempDbContext>().Database.Migrate();
            //注册仓储泛型
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreTemplate API v1");
                    //http://localhost:<port>/应用根显示swagger
                    c.RoutePrefix = string.Empty;
                });
                #endregion
            }
            app.UseCors("CorsLocal");

            //认证
            app.UseAuthentication();

            //个人认证
            //app.UseMiddleware<TokenAuthMiddleware>();

            app.UseStaticFiles();

            app.UseMvc();


            ServiceLocator.Instance = app.ApplicationServices;

            SeedData.SeedDb();
        }
    }
}
