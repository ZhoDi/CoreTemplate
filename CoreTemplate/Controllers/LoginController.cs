using System;
using System.Threading.Tasks;
using CoreTemplate.Application.Dto.Base;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Application.Helper;
using CoreTemplate.Application.IServices;
using CoreTemplate.Domain.APIModel.User;
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
        public BaseResponse<AuthenticateResultModel> GetToken([FromBody]AuthenticateModel model)
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
                return new BaseResponse<AuthenticateResultModel>(new AuthenticateResultModel());
            }

            var res = new AuthenticateResultModel
            {
                AccessToken = jwtStr,
                ExpireInSeconds = JwtHelper.SerializeJwt(jwtStr).Expiration,
                UserId = userInfo.Id
            };

            return new BaseResponse<AuthenticateResultModel>(res);
        }

        [HttpPost("RegisterUser")]
        public BaseResponse RegisterUser([FromBody]UserRegisterDto userRegisterDto)
        {
            return _userService.RegisterUser(userRegisterDto);
        }
    }
}