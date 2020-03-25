using AutoMapper;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Application.IServices;
using CoreTemplate.Domain.Entities;
using CoreTemplate.Domain.IRepositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Application.Services
{
    public class UserServices : BaseServices<User, UserDto, int>, IUserServices
    {
        private readonly IRepository<User, int> _UserRepository;
        private readonly IRepository<UserRole, int> _UserRoleRepository;
        private readonly IRepository<Role, int> _RoleRepository;

        public UserServices(IRepository<User, int> UserRepository, IRepository<UserRole, int> UserRoleRepository, IRepository<Role, int> RoleRepository, IMapper Mapper) : base(UserRepository, Mapper)
        {
            _UserRepository = UserRepository;
            _UserRoleRepository = UserRoleRepository;
            _RoleRepository = RoleRepository;
        }

        public User GetUserInfoByName(string name)
        {
            var user = _UserRepository.FirstOrDefault(p => p.Name == name);
            return user;
        }

        public string GetUserRoleNameStr(string name, string pwd)
        {
            var user = _UserRepository.FirstOrDefault(p => p.Name == name && p.PassWord == pwd);
            if (user == null)
            {
                return "";
            }
            var userRole = _UserRoleRepository.GetAllList(p => p.UserId == user.Id);
            StringBuilder strB = new StringBuilder();
            foreach (var item in userRole)
            {
                var role = _RoleRepository.FirstOrDefault(p => p.Id == item.RoleId);
                if (role != null)
                {
                    strB.Append(role.Name + ',');
                }
            }
            //结尾有","在解析时注意
            return strB.ToString();
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userRegisterDto"></param>
        public void RegisterUser(UserRegisterDto userRegisterDto)
        {
            var userInfo = _Mapper.Map<User>(userRegisterDto);

            userInfo.CreateDate = DateTimeOffset.Now.ToUnixTimeSeconds();


            var user = _UserRepository.Insert(userInfo);
            var role = _RoleRepository.FirstOrDefault(p => p.Name == "User");

            UserRole userRole = new UserRole();

            userRole.RoleId = role.Id;
            userRole.UserId = user.Id;

            _UserRoleRepository.Insert(userRole);
        }
    }
}
