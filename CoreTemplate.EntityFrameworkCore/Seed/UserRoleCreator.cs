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

            var users = dbContext.Users.ToList();

            if (users.Any()) return;
            var roleForAdmin = dbContext.Roles.Add(new Role()
            {
                Name = ConstName.Admin, 
                IsDeleted = false, 
                CreateTime = DateTime.Now,
                CreateUser = 1,
                CreateUserName = ConstName.Admin
            }).Entity;

            var roleForUser = dbContext.Roles.Add(new Role()
            {
                Name = ConstName.User,
                IsDeleted = false,
                CreateTime = DateTime.Now,
                CreateUser = 1,
                CreateUserName = ConstName.Admin,
            }).Entity;

            var adminUserForHost = dbContext.Users.IgnoreQueryFilters().FirstOrDefault(u => u.Name == ConstName.Admin);
            if (adminUserForHost == null)
            {
                var user = new User
                {
                    LoginId = ConstName.Admin,
                    Name = ConstName.Admin,
                    Email = "910824572@qq.com",
                    Mobile = "15737652771",
                    Birthday = DateTime.Now,
                    IsDeleted = false,
                    CreateUser = 1,
                    CreateUserName = ConstName.Admin,
                    CreateTime = DateTime.Now,
                    Avatar = "",
                    Password = "123456",
                    Gender = 1
                };

                adminUserForHost = dbContext.Users.Add(user).Entity;
                dbContext.SaveChanges();

                //// 分配管理员角色
                dbContext.UserRoles.Add(new UserRole()
                {
                    UserId = adminUserForHost.Id, 
                    RoleId = roleForAdmin.Id,
                    CreateUser = 1,
                    CreateUserName = ConstName.Admin,
                    CreateTime = DateTime.Now,
                });
                dbContext.UserRoles.Add(new UserRole()
                {
                    UserId = adminUserForHost.Id, 
                    RoleId = roleForUser.Id,
                    CreateUser = 1,
                    CreateUserName = ConstName.Admin,
                    CreateTime = DateTime.Now,
                });
            }
            dbContext.SaveChanges();
        }
    }
}
