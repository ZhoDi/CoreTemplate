using Castle.DynamicProxy;
using CoreTemplate.Application.TemplateAttribute;
using System;
using System.Linq;

namespace CoreTemplate.Application.AOP.Memory
{
    public class MemoryCacheAOP : IInterceptor
    {
        /// <summary>
        /// 注入构造接口
        /// </summary>
        private ICaching _cache;
        public MemoryCacheAOP(ICaching cache)
        {
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(CachingAttribute)) as CachingAttribute;
            //只有那些指定的才可以被缓存，需要验证
            if (qCachingAttribute != null)
            {
                //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
                //根据key获取相应的缓存值
                var cacheValue = _cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue,qCachingAttribute.AbsoluteExpiration);
                }
            }
            else
            {
                invocation.Proceed();
            }
        }

        //自定义缓存键
        private string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).ToList();

            string key = $"{typeName}:{methodName}:";
            key += string.Join(":", methodArguments);
            return key;
        }
        //object 转 string
        private string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}
