using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTemplate.Domain.Shared.Exception;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreTemplate.Filters
{
    /// <summary>
    /// 全局请求过滤器
    /// </summary>
    public class GlobalRequestFilter: IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ModelState.IsValid) return;
            //使用自定义参数绑定验证体系
            var modelState = context.ModelState.FirstOrDefault(f => f.Value.Errors.Any());
            var errorMsg = modelState.Value.Errors.First().ErrorMessage;
            throw new BaseException(errorMsg, Domain.Shared.Enum.DetailedStatus.ParamsError);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
