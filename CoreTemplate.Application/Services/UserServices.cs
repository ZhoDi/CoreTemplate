using AutoMapper;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Application.IServices;
using CoreTemplate.Application.TemplateAttribute;
using CoreTemplate.Domain.Entities;
using CoreTemplate.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate.Application.Services
{
    public class UserServices : BaseServices<User, UserDto, int>, IUserServices
    {
        private readonly IRepository<User, int> _userRepository;
        private readonly IRepository<UserRole, int> _userRoleRepository;
        private readonly IRepository<Role, int> _roleRepository;

        public UserServices(IRepository<User, int> userRepository, IRepository<UserRole, int> userRoleRepository, IRepository<Role, int> roleRepository,
            IMapper Mapper) : base(userRepository, Mapper)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        [Caching]
        public User GetUserInfoByName(string name)
        {
            var user = _userRepository.FirstOrDefault(p => p.Name == name);
            return user;
        }

        public string GetUserRoleNameStr(string loginId, string pwd)
        {
            var user = _userRepository.FirstOrDefault(p => p.LoginId == loginId && p.Password == pwd);
            if (user == null)
            {
                return "";
            }
            var userRole = _userRoleRepository.GetAllList(p => p.UserId == user.Id);
            var strB = new StringBuilder();
            foreach (var role in userRole.Select(item => _roleRepository.FirstOrDefault(p => p.Id == item.RoleId)).Where(role => role != null))
            {
                strB.Append(role.Name + ',');
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
            var userInfo = Mapper.Map<User>(userRegisterDto);

            userInfo.CreateTime = DateTimeOffset.Now.ToUnixTimeSeconds();


            var user = _userRepository.Insert(userInfo);
            var role = _roleRepository.FirstOrDefault(p => p.Name == "User");

            var userRole = new UserRole {RoleId = role.Id, UserId = user.Id};


            _userRoleRepository.Insert(userRole);
        }
    }
}
