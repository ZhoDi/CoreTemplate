using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CoreTemplate.Domain.Shared.Helper
{
    public class JwtHelper
    {
        /// <summary>
        /// 根据传进来的TokenModel生成token字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModel tokenModel)
        {
            var time = DateTime.Now;
            var claims = new List<Claim>
            {
                //JWT ID的唯一标识
                new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),

                //Issued At，JWT颁发的时间，用于验证过期
                new Claim(JwtRegisteredClaimNames.Iat,new DateTimeOffset(time).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),

                //过期时间
                new Claim(JwtRegisteredClaimNames.Exp,new DateTimeOffset(time).AddDays(1).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),

                //jwt签发者
                new Claim(JwtRegisteredClaimNames.Iss,Appsettings.App("Authentication:JwtBearer:Issuer")),

                //jwt接收者
                new Claim(JwtRegisteredClaimNames.Aud,Appsettings.App("Authentication:JwtBearer:Audience"))
            };

            //一个用户多个角色,StringSplitOptions.RemoveEmptyEntries用以去除最后一个空数据(示例数据  Admin,User,  最后有,)
            claims.AddRange(tokenModel.Role.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => new Claim(ClaimTypes.Role, s)));

            //秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Appsettings.App("Authentication:JwtBearer:SecurityKey")));
            //加密
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                //上面生命的"身份单元集合"相当于身份证上的姓名,性别...等基本信息
                claims: claims,
                signingCredentials: cred);

            //生成JWT字符串
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModel SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            jwtToken.Payload.TryGetValue(ClaimTypes.Role, out var role);
            jwtToken.Payload.TryGetValue(JwtRegisteredClaimNames.Exp, out var exp);
            var tm = new TokenModel
            {
                Uid = long.Parse(jwtToken.Id),
                Role = role != null ? role.ToString() : "",
                Expiration = exp != null ? Convert.ToInt64(exp) : 0
            };
            return tm;
        }
    }

    /// <summary>
    /// 令牌类
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long Expiration { get; set; }
    }
}
