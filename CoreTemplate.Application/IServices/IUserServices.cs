using CoreTemplate.Application.Dto.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Application.IServices
{
    public interface IUserServices
    {
        string GetUserRoleNameStr(string name, string pwd);

        void RegisterUser(UserRegisterDto userRegisterDto);
    }
}
