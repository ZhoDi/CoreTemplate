using CoreTemplate.Application.Dto.User;
using CoreTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate.Application.IServices
{
    public interface IUserServices: IBaseServices<User,UserDto, int>
    {
        string GetUserRoleNameStr(string name, string pwd);

        User GetUserInfoByName(string name);

        void RegisterUser(UserRegisterDto userRegisterDto);

        Task<List<User>> GetProcedureUserById(int id);
    }
}
