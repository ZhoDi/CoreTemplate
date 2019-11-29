using Autofac;
using CoreTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreTemplate.EntityFrameworkCore.Seed
{
    public class UserRoleCreator
    {
        private static IContainer Container;
        public UserRoleCreator(IContainer container)
        {
            Container = container;
        }
        public void Create()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                var _context = scope.Resolve<TempDbContext>();

                var adminRoleForHost = _context.Roles.ToList();
                var roleForAdmin = new Role();
                var roleForUser = new Role();

                if (!adminRoleForHost.Any())
                {
                    roleForAdmin= _context.Roles.Add(new Role() { Name = ConstName.Admin, IsDeleted = false, CreateDate = DateTimeOffset.Now.ToUnixTimeSeconds() }).Entity;

                    roleForUser = _context.Roles.Add(new Role() { Name = ConstName.User, IsDeleted = false, CreateDate = DateTimeOffset.Now.ToUnixTimeSeconds() }).Entity;

                    var adminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.Name == ConstName.Admin);
                    if (adminUserForHost == null)
                    {
                        var user = new User
                        {
                            Name = ConstName.Admin,
                            Email = "910824572@qq.com",
                            Mobile = "15737652771",
                            IsDeleted = false,
                            Avatar = "",
                            PassWord = "123456",
                            Gender = 1,
                            Number = "1011150",
                        };

                        adminUserForHost = _context.Users.Add(user).Entity;
                        _context.SaveChanges();


                        //// 分配管理员角色
                        _context.UserRoles.Add(new UserRole() { UserId = adminUserForHost.Id, RoleId = roleForAdmin.Id });
                        _context.UserRoles.Add(new UserRole() { UserId = adminUserForHost.Id, RoleId = roleForUser.Id });
                    }
                    _context.SaveChanges();
                }


            }
        }
    }
}
