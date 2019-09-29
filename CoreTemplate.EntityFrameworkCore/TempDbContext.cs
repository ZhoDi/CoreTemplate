using CoreTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.EntityFrameworkCore
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }

    public class TempDbContext : DbContext
    {
        public TempDbContext(DbContextOptions<TempDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }


        //public void RunSaveChanges()
        //{
        //    SaveChanges();
        //}

        //public void RunSaveChangesAsync()
        //{
        //    SaveChangesAsync();
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //配置查询过滤器
            builder.Entity<User>().Property<bool>("IsDeleted");
            builder.Entity<Role>().Property<bool>("IsDeleted");
        }
    }
}
