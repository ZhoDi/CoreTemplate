using Castle.DynamicProxy;
using System;
using System.IO;
using System.Linq;

namespace CoreTemplate.Application.Aop.Log
{
    public class LogAop : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var dataIntercept = $"{DateTime.Now:yyyyMMddHHmmss} " +
                $"执行方法：---{invocation.Method.Name}---" +
                $"方法参数：---{string.Join("，", invocation.Arguments.Select(p => (p ?? "无参").ToString()).ToArray())}---\r\n";
            try
            {
                //在被拦截的方法执行完毕，再执行当前方法
                invocation.Proceed();
            }
            catch (Exception e)
            {
                dataIntercept += ($"出现异常：{e.Message + e.InnerException}");
            }

            dataIntercept += ($"执行完毕，返回结果：{invocation.ReturnValue}");

            #region 输出到当前项目日志
            var path = Directory.GetCurrentDirectory() + @"\Log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = path + $@"\InterceptLog-{DateTime.Now:yyyyMMddHHmmss}.log";

            StreamWriter sw = File.AppendText(fileName);
            sw.WriteLine(dataIntercept);
            sw.Close();
            #endregion
        }
    }
}