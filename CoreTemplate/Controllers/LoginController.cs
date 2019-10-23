using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            //var userRole = "Admin,User";
            if (userRole != null)
            {
                TokenModel tokenModel = new TokenModel { Uid = 1, Role = userRole };
                jwtStr = JwtHelper.IssueJwt(tokenModel);
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return Ok(new
            {
                token = jwtStr
            });
        }

    }
}