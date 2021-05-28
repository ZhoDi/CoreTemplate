using Autofac;
using CoreTemplate.EntityFrameworkCore;
using CoreTemplate.EntityFrameworkCore.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Reflection;
using CoreTemplate.Domain.Shared.Attribute;
using CoreTemplate.Domain.Shared.Helper;
using CoreTemplate.Domain.Shared.MemoryCache;
using CoreTemplate.Filters;
using CoreTemplate.Middlewares;
using CoreTemplate.ServiceExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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
            services.AddSingleton(new Appsettings(Configuration));
            services.AddScoped<MemoryCacheAttribute>();
            services.AddScoped<ICaching, MemoryCaching > ();

            services.AddCorsSetup();
            services.AddAuthorizationSetup();
            services.AddSwaggerSetup();
            services.AddAutoMapperSetup();

            services.AddMemoryCache();
            services.AddDbContext<TempDbContext>(options => options.UseMySql(Configuration.GetValue<string>("ConnStr")));

            #region 自带DI注入容器示例
            //自带DI容器注入示例
            //注册仓储泛型
            //services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            //services.AddScoped<IUserServices, UserServices>();
            #endregion

            //1.开启允许同步IO
            //2.关闭自带模型验证,在
            services.Configure<KestrelServerOptions>(o => o.AllowSynchronousIO = true)
                //.Configure<IISServerOptions>(o => o.AllowSynchronousIO = true)
                .Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);

            services.AddControllers(o =>
                {
                    // 全局异常过滤
                    o.Filters.Add(typeof(GlobalExceptionFilter));
                    o.Filters.Add(typeof(GlobalRequestFilter));
                })
                .AddNewtonsoftJson(options =>
                {
                    ////忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    ////使用默认Model
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    ////使用小驼峰
                    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    ////设置时间格式
                    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                });

        }


        // 添加Autofac DI服务工厂
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModuleRegister());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TempDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwaggerMiddleware(() => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("CoreTemplate.wwwroot.swagger.ui.index.html"));

            //跨域
            app.UseCors(Appsettings.App("Startup", "Cors", "PolicyName"));
            // 跳转https
            //app.UseHttpsRedirection();
            // 使用静态文件
            app.UseStaticFiles();
            // 使用cookie
            app.UseCookiePolicy();
            // 返回错误码
            app.UseStatusCodePages();
            // Routing
            app.UseRouting();
            //认证
            app.UseAuthentication();
            //授权
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //初始化数据
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
            SeedData.SeedDb(dbContext);
        }
    }
}