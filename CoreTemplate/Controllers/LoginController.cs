using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Application.Helper;
using CoreTemplate.Application.IServices;
using CoreTemplate.AuthHelp;
using CoreTemplate.Domain.APIModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplate.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserServices _UserService;

        public LoginController(IUserServices UserService)
        {
            _UserService = UserService;
        }

        [HttpPost("Authenticate")]
        public IActionResult GetToken([FromBody]AuthenticateModel model)
        {
            string jwtStr = string.Empty;
            var userRole = _UserService.GetUserRoleNameStr(model.UserName, model.Password);
            var userInfo = _UserService.GetUserInfoByName(model.UserName);

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
            _UserService.RegisterUser(userRegisterDto);
            return Ok();
        }

        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {
            var result = _UserService.GetAllList();
            return Ok(new { res = result });
        }
    }
}