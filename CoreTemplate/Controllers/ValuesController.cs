using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplate.Application.Model.Base;
using CoreTemplate.Application.Model.Test.Param;
using CoreTemplate.Domain.Shared.Enum;
using CoreTemplate.Domain.Shared.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreTemplate.Controllers
{
    /// <summary>
    /// Values控制器
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BaseResponse<string> HttpGet()
        {
            throw new BaseException("错误",DetailedStatus.DataAlreadyExists);
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseResponse<string> HttpPost(ValueParam param)
        {
            return new BaseResponse<string>("post");
        }
    }
}
