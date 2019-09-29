﻿using CoreTemplate.Domain.Entities;
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
        public void Create()
        {
            using (var scope = ServiceLocator.Instance.CreateScope())
            {
                var _context = scope.ServiceProvider.GetService<TempDbContext>();

                var adminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.Name == ConstName.Admin);
                if (adminRoleForHost == null)
                {
                    adminRoleForHost = _context.Roles.Add(new Role() { Name = ConstName.Admin, IsDeleted = false, CreateDate = DateTimeOffset.Now.ToUnixTimeSeconds() }).Entity;
                    _context.SaveChanges();
                }

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
                        Gender = 1,
                        Number = "1011150",
                    };

                    //user.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(user, "admin");
                    //user.SetNormalizedNames();

                    adminUserForHost = _context.Users.Add(user).Entity;
                    _context.SaveChanges();

                    //// 分配管理员角色
                    //_context.UserRoles.Add(new UserRole(adminUserForHost.Id, adminRoleForHost.Id));
                    _context.SaveChanges();

                    _context.SaveChanges();
                }


            }
        }
    }
}
