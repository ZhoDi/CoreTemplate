using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreTemplate.AuthHelp
{
    public class JwtHelper
    {
        /// <summary>
        /// 根据传进来的TokenModel生成taokn字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        public static string IssueJwt(TokenModel tokenModel)
        {
            //通过ConfigurationBuilder获取json配置
            var config = new ConfigurationBuilder()
               //将配置文件的数据加载到内存中
               .AddInMemoryCollection()
               //指定配置文件所在的目录
               .SetBasePath(Directory.GetCurrentDirectory())
               //指定加载的配置文件
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               //编译成对象
               .Build();
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
                new Claim(JwtRegisteredClaimNames.Iss,config["Authentication:JwtBearer:Issuer"]),

                //jwt接收者
                new Claim(JwtRegisteredClaimNames.Aud,config["Authentication:JwtBearer:Audience"])
            };

            //一个用户多个角色,StringSplitOptions.RemoveEmptyEntries用以去除最后一个空数据(示例数据  Admin,User,  最后有,)
            claims.AddRange(tokenModel.Role.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => new Claim(ClaimTypes.Role, s)));

            //秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:JwtBearer:SecurityKey"]));

            //加密
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                //上面生命的"身份单元集合"相当于身份证上的姓名,性别...等基本信息
                claims: claims,

                //凭证
                signingCredentials: creds);

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
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            object role = new object();
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tm = new TokenModel
            {
                Uid = long.Parse(jwtToken.Id),
                Role = role.ToString()
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
        /// 项目名称
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 令牌类型
        /// </summary>
        public string TokenType { get; set; }
    }
}
