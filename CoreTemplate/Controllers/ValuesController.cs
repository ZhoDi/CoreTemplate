using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplate.Application.IServices;
using CoreTemplate.Application.Model.Base;
using CoreTemplate.Application.Model.Test.Param;
using CoreTemplate.Domain.Shared.Attribute;
using CoreTemplate.Domain.Shared.Enum;
using CoreTemplate.Domain.Shared.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTemplate.Controllers
{
    /// <summary>
    /// Values控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValuesServices _valuesServices;
        public ValuesController(IValuesServices valuesServices)
        {
            _valuesServices = valuesServices;
        }
        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public BaseResponse<string> HttpGet()
        {
            throw new BaseException("错误",DetailedStatus.DataAlreadyExists);
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BaseResponse<int> HttpPost(ValueParam param)
        {
            return new BaseResponse<int>(_valuesServices.Get(param));
        }
    }
}
