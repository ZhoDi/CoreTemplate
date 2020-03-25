using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using CoreTemplate.AOP.Log;
using CoreTemplate.AOP.Memory;
using CoreTemplate.Application.Application;
using CoreTemplate.Domain.APIModel.User;
using CoreTemplate.Domain.IRepositories;
using CoreTemplate.EntityFrameworkCore.Repositories;
using CoreTemplate.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreTemplate.Startup
{
    public class AutofacModuleRegister : Autofac.Module
    {

        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            ////注册示例
            //builder.RegisterType<UserServices>().As<IUserServices>();
            //日志AOP
            builder.RegisterType<TemplateLogAOP>();
            //缓存AOP
            builder.RegisterType<TemplateCacheAOP>();

            //注册仓储泛型
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();

            //注册AutoMapper
            builder.RegisterType<Mapper>().As<IMapper>().SingleInstance();

            builder.RegisterType<MemoryCaching>().As<ICaching>().InstancePerLifetimeScope();

            //builder.RegisterType<AutoMapperObjectMapper>().As<Application.Application.IObjectMapper>().InstancePerDependency();
            //builder.Register<IMapper>(ctx => new Mapper(ctx.Resolve<IConfigurationProvider>(), ctx.Resolve)).InstancePerDependency();


            var aopTypeList = new List<Type>();
            if (Convert.ToBoolean(Appsettings.app(new string[] { "AOP", "LogAOP", "Enabled" })))
            {
                aopTypeList.Add(typeof(TemplateLogAOP));
            }
            if (Convert.ToBoolean(Appsettings.app(new string[] { "AOP", "MemoryCaching", "Enabled" })))
            {
                aopTypeList.Add(typeof(TemplateLogAOP));
            }
            //注册Application.Services中的对象,Services中的类要以Services结尾，否则注册失败
            var dataAccess = Assembly.Load("CoreTemplate.Application");
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(a => a.Name.EndsWith("Services"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy; 关闭AOP只需要注释这两行
                .InterceptedBy(aopTypeList.ToArray());//拦截器注入
        }

    }
}
