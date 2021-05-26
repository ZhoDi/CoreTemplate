using CoreTemplate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreTemplate.Application.Model.Base;
using CoreTemplate.Application.Model.User.Dto;

namespace CoreTemplate.Application.IServices
{
    public interface IUserServices: IBaseServices<User,UserDto, int>
    {
        string GetUserRoleNameStr(string name, string pwd);

        User GetUserInfoByLoginId(string loginId);

        BaseResponse RegisterUser(UserRegisterDto userRegisterDto);
    }
}
