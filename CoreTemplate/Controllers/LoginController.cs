using System;
using System.Threading.Tasks;
using CoreTemplate.Application.IServices;
using CoreTemplate.Application.Model.Base;
using CoreTemplate.Application.Model.User.Dto;
using CoreTemplate.Domain.Shared.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplate.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserServices _userService;

        public LoginController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpPost("Authenticate")]
        public BaseResponse<AuthenticateResultDto> GetToken([FromBody]AuthenticateDto model)
        {
            string jwtStr = string.Empty;
            var userRole = _userService.GetUserRoleNameStr(model.LoginId, model.Password);
            var userInfo = _userService.GetUserInfoByLoginId(model.LoginId);

            if (!string.IsNullOrEmpty(userRole))
            {
                TokenModel tokenModel = new TokenModel { Uid = userInfo.Id, Role = userRole };
                jwtStr = JwtHelper.IssueJwt(tokenModel);
            }
            else
            {
                return new BaseResponse<AuthenticateResultDto>(new AuthenticateResultDto());
            }

            var res = new AuthenticateResultDto
            {
                AccessToken = jwtStr,
                ExpireInSeconds = JwtHelper.SerializeJwt(jwtStr).Expiration,
                UserId = userInfo.Id
            };

            return new BaseResponse<AuthenticateResultDto>(res);
        }

        [HttpPost("RegisterUser")]
        public BaseResponse RegisterUser([FromBody]UserRegisterDto userRegisterDto)
        {
            return _userService.RegisterUser(userRegisterDto);
        }
    }
}