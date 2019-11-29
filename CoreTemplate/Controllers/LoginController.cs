using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplate.Application.Dto.User;
using CoreTemplate.Application.IServices;
using CoreTemplate.AuthHelp;
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

        [HttpGet("GetToken{name}/{pass}")]
        public IActionResult GetToken(string name, string pass)
        {
            string jwtStr = string.Empty;
            var userRole = _UserService.GetUserRoleNameStr(name, pass);

            if (!string.IsNullOrEmpty(userRole))
            {
                TokenModel tokenModel = new TokenModel { Uid = 1, Role = userRole };
                jwtStr = JwtHelper.IssueJwt(tokenModel);
            }
            else
            {
                jwtStr = "用户名或密码有误!";
            }

            return Ok(new
            {
                token = jwtStr
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

    }
}