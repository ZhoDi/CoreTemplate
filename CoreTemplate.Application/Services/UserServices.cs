using AutoMapper;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Application.IServices;
using CoreTemplate.Application.TemplateAttribute;
using CoreTemplate.Domain.Entities;
using CoreTemplate.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<List<User>> GetProcedureUserById(int id)
        {
            var result = await _userRepository.GetFromSql($@"CALL userbyid({0})").ToListAsync();
            return result;
        }

        [Caching]
        public User GetUserInfoByName(string name)
        {
            var user = _userRepository.FirstOrDefault(p => p.Name == name);
            return user;
        }

        public string GetUserRoleNameStr(string name, string pwd)
        {
            var user = _userRepository.FirstOrDefault(p => p.Name == name && p.PassWord == pwd);
            if (user == null)
            {
                return "";
            }
            var userRole = _userRoleRepository.GetAllList(p => p.UserId == user.Id);
            StringBuilder strB = new StringBuilder();
            foreach (var item in userRole)
            {
                var role = _roleRepository.FirstOrDefault(p => p.Id == item.RoleId);
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
            var userInfo = Mapper.Map<User>(userRegisterDto);

            userInfo.CreateDate = DateTimeOffset.Now.ToUnixTimeSeconds();


            var user = _userRepository.Insert(userInfo);
            var role = _roleRepository.FirstOrDefault(p => p.Name == "User");

            UserRole userRole = new UserRole {RoleId = role.Id, UserId = user.Id};


            _userRoleRepository.Insert(userRole);
        }

        public async Task<List<User>> TestSQLInjection(string name)
        {
            //这种字符串插值方式会导致SQL注入
            //var result = await _UserRepository.GetFromSql("SELECT * FROM USERS WHERE Name='" + name + "';").ToListAsync();

            //选择$进行字符串检查就不会导致SQL注入了
            var result = await _userRepository.GetFromSql($@"SELECT * FROM USERS WHERE Name='{name}';").ToListAsync();
            return result;
        }
    }
}
