using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CoreTemplate.AOP;
using CoreTemplate.Config;
using CoreTemplate.Domain.APIModel.User;
//using AutoMapper;
using CoreTemplate.EntityFrameworkCore;
using CoreTemplate.EntityFrameworkCore.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static IContainer Container { get; set; }
        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    ////忽略循环引用
                    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    ////使用默认Model
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    ////使用小驼峰
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    ////设置时间格式
                    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                });

            #region 其他配置
            //配置文件在哪个程序集中，本文是在Startup对应的程序集中
            services.AddAutoMapper(typeof(Startup));
            services.AddMemoryCache();
            #endregion

            #region Swagger
            services.AddSwaggerGen(options =>
            {
                //这里的v1,对应下面注册json路径中的v1
                options.SwaggerDoc("v1", new Info
                {
                    Title = "CoreTemplate API文档",
                    Version = "v0.1.0",
                    Description = "个人DotNetCore框架说明文档"
                });

                //获取注释xml文件,没有它也行,但是接口没注释
                //获取程序根目录
                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "CoreTemplate.xml");
                //true,显示控制器注释
                options.IncludeXmlComments(xmlPath, true);

                #region 添加权限验证
                //配合下方自定义UI使用
                options.AddSecurityDefinition("CoreTemplate", new ApiKeyScheme
                {
                    Description = "JWT授权直接在下框中输入Bearer {token},空格不可省略",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });
            #endregion

            #region 授权
            AuthConfigurer.Configure(services, Configuration);
            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy(
                    _defaultCorsPolicyName,
                    build => build.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromHours(1))//预检请求过期时间，用于减少OPTIONS请求
                    .WithOrigins(
                            Configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        )
                    );
            });
            #endregion

            #region MySql
            var connection = this.Configuration.GetValue<string>("ConnStr");
            services.AddDbContext<TempDbContext>(options => options.UseMySql(connection));
            services.BuildServiceProvider().GetService<TempDbContext>().Database.Migrate();

            //自带DI容器 注册仓储泛型
            //services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            //services.AddScoped<IUserServices, UserServices>();
            #endregion

            return RegisterAutofac(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreTemplate API v1");
                    var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                    //自定义UI
                    options.IndexStream = () =>
                    {
                        return new FileStream(basePath + "/wwwroot/swagger/ui/index.html", FileMode.Open);
                    };
                });

                #endregion
            }
            //静态文件
            app.UseStaticFiles();

            //跨域
            app.UseCors(_defaultCorsPolicyName);
            //官方认证
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        public IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            //新模块组件注册
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            Container = builder.Build();

            //初始化数据
            SeedData.SeedDb(Container);

            //第三方IOC接管 core内置DI容器
            return new AutofacServiceProvider(Container);
        }
    }
}