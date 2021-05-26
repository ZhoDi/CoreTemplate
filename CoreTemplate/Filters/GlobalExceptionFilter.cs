using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplate.Application.Model.Base;
using CoreTemplate.Domain.Shared.Enum;
using CoreTemplate.Domain.Shared.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreTemplate.Filters
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {


        private readonly ILogger<GlobalExceptionFilter> _loggerHelper;


        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> loggerHelper)
        {
            _loggerHelper = loggerHelper;
        }

        public void OnException(ExceptionContext context)
        {
            var dto = new BaseResponse();

            switch (context.Exception)
            {
                case BaseException baseException:
                    dto.DetailedStatus = baseException.DetailedStatus;
                    dto.DetailedMessage = $"{context.Exception.Message}";
                    break;
                default:
                    dto.DetailedStatus = DetailedStatus.Fail;
                    dto.DetailedMessage = $"{context.Exception.Message}";
                    break;
            }

            _loggerHelper.LogError(WriteLog(dto.DetailedMessage, context.Exception));
            context.Result = new ContentResult() { Content = JsonConvert.SerializeObject(dto) };
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("\r\n【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }
    }
}
