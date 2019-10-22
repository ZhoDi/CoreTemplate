using CoreTemplate.Application.IServices;
using CoreTemplate.Domain.Entities;
using CoreTemplate.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IRepository<User, int> _UserRepository;
        private readonly IRepository<UserRole, int> _UserRoleRepository;
        private readonly IRepository<Role, int> _RoleRepository;

        public UserServices(IRepository<User, int> UserRepository, IRepository<UserRole, int> UserRoleRepository, IRepository<Role, int> RoleRepository)
        {
            _UserRepository = UserRepository;
            _UserRoleRepository = UserRoleRepository;
            _RoleRepository = RoleRepository;
        }

        public string GetUserRoleNameStr(string name, string pwd)
        {
           var user= _UserRepository.FirstOrDefault(p => p.Name == name && p.PassWord == pwd);
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
                    strB.Append("," + role.Name);
                }
            }
            return strB.ToString();
        }
    }
}
