using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreTemplate.Filters
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class ExceptionAttribute: IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {

		}
    }
}
