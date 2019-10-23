using Autofac;
using CoreTemplate.Domain.IRepositories;
using CoreTemplate.EntityFrameworkCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreTemplate
{
    public class AutofacModuleRegister : Autofac.Module
    {
        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            //注册Application.Services中的对象,Services中的类要以Services结尾，否则注册失败

            builder.RegisterAssemblyTypes(Assembly.Load("CoreTemplate.Application")).Where(a => a.Name.EndsWith("Services")).AsImplementedInterfaces();

            //注册仓储泛型
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
        }

    }
}
