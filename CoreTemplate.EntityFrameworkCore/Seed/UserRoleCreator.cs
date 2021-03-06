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
        public void Create(TempDbContext dbContext)
        {

            var adminRoleForHost = dbContext.Roles.ToList();
            var roleForAdmin = new Role();
            var roleForUser = new Role();

            if (!adminRoleForHost.Any())
            {
                roleForAdmin = dbContext.Roles.Add(new Role() { Name = ConstName.Admin, IsDeleted = false, CreateDate = DateTimeOffset.Now.ToUnixTimeSeconds() }).Entity;

                roleForUser = dbContext.Roles.Add(new Role() { Name = ConstName.User, IsDeleted = false, CreateDate = DateTimeOffset.Now.ToUnixTimeSeconds() }).Entity;

                var adminUserForHost = dbContext.Users.IgnoreQueryFilters().FirstOrDefault(u => u.Name == ConstName.Admin);
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

                    adminUserForHost = dbContext.Users.Add(user).Entity;
                    dbContext.SaveChanges();


                    //// 分配管理员角色
                    dbContext.UserRoles.Add(new UserRole() { UserId = adminUserForHost.Id, RoleId = roleForAdmin.Id });
                    dbContext.UserRoles.Add(new UserRole() { UserId = adminUserForHost.Id, RoleId = roleForUser.Id });
                }
                dbContext.SaveChanges();
            }
        }
    }
}
