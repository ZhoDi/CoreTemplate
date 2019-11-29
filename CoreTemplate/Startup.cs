using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CoreTemplate.Config;
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
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate
{
    public class Startup
    {
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
                    ////使用默认Model返回数据格式
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    ////使用默认Json序列化格式(小驼峰)
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


                #region 添加权限验证
                //Token绑定到ConfigureServices
                var security = new Dictionary<string, IEnumerable<string>> { { "CoreTemplate", new string[] { } }, };

                c.AddSecurityRequirement(security);

                c.AddSecurityDefinition("CoreTemplate", new ApiKeyScheme
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
                options.AddPolicy("CorsLocal",
                    build => build.AllowAnyOrigin()
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

            #region MySql
            var connection = this.Configuration.GetValue<string>("ConnStr");
            services.AddDbContext<TempDbContext>(options => options.UseMySql(connection));
            services.BuildServiceProvider().GetService<TempDbContext>().Database.Migrate();

            //注册仓储泛型
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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreTemplate API v1");
                    //http://localhost:<port>/应用根显示swagger
                    c.RoutePrefix = string.Empty;
                });
                #endregion
            }
            app.UseStaticFiles();

            app.UseCors("CorsLocal");
            //官方认证
            app.UseAuthentication();

            //个人认证
            //app.UseMiddleware<TokenAuthMiddleware>();


            app.UseMvc();

            //初始化数据
            SeedData.SeedDb(Container);
        }

        public IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            ////注册示例
            //builder.RegisterType<UserServices>().As<IUserServices>();
            //新模块组件注册
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            Container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(Container);
        }
    }
}