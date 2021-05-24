using System;
using System.Threading.Tasks;
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
        public IActionResult GetToken([FromBody]AuthenticateModel model)
        {
            string jwtStr = string.Empty;
            var userRole = _userService.GetUserRoleNameStr(model.UserName, model.Password);
            var userInfo = _userService.GetUserInfoByName(model.UserName);

            if (!string.IsNullOrEmpty(userRole))
            {
                TokenModel tokenModel = new TokenModel { Uid = userInfo.Id, Role = userRole };
                jwtStr = JwtHelper.IssueJwt(tokenModel);
            }
            else
            {
                return BadRequest(new
                {
                    res= new AuthenticateResultModel()
                });
            }

            var res = new AuthenticateResultModel
            {
                AccessToken = jwtStr,
                ExpireInSeconds = (int)TimeSpan.FromDays(1).TotalSeconds,
                UserId = userInfo.Id
            };

            return Ok(new
            {
                res
            });
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser([FromBody]UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _userService.RegisterUser(userRegisterDto);
            return Ok();
        }

        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {
            var result = _userService.GetAllList();
            return Ok(new { res = result });
        }
        /// <summary>
        /// 根据姓名获取用户信息(配置缓存)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("GetUserByName")]
        public IActionResult GetUserById(string name)
        {
            var result =  _userService.GetUserInfoByName(name);
            return Ok(new { res = result });
        }
    }
}