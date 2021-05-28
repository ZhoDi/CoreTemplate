﻿// <auto-generated />
using System;
using CoreTemplate.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreTemplate.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(TempDbContext))]
    partial class TempDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CoreTemplate.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CreateUser")
                        .HasColumnType("bigint");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(10)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("UpdateUser")
                        .HasColumnType("bigint");

                    b.Property<string>("UpdateUserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CoreTemplate.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CreateUser")
                        .HasColumnType("bigint");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Gender")
                        .HasColumnType("int(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LoginId")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Mobile")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("UpdateUser")
                        .HasColumnType("bigint");

                    b.Property<string>("UpdateUserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("IsDeleted", "LoginId", "Mobile");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CoreTemplate.Domain.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CreateUser")
                        .HasColumnType("bigint");

                    b.Property<string>("CreateUserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("UpdateUser")
                        .HasColumnType("bigint");

                    b.Property<string>("UpdateUserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
