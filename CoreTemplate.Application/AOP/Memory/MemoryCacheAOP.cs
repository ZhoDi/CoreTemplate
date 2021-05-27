using Castle.DynamicProxy;
using System;
using System.Linq;
using CoreTemplate.Domain.Shared.Attribute;
using CoreTemplate.Domain.Shared.MemoryCache;

namespace CoreTemplate.Application.Aop.Memory
{
    public class MemoryCacheAop : IInterceptor
    {
        /// <summary>
        /// 注入构造接口
        /// </summary>
        private readonly ICaching _cache;
        public MemoryCacheAop(ICaching cache)
        {
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //只有那些指定的才可以被缓存，需要验证
            if (method.GetCustomAttributes(true).FirstOrDefault(p => p.GetType() == typeof(CachingAttribute)) is CachingAttribute qCachingAttribute)
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

            var key = $"{typeName}:{methodName}:";
            key += string.Join(":", methodArguments);
            return key;
        }
        //object 转 string
        private static string GetArgumentValue(object arg)
        {
            switch (arg)
            {
                case int _:
                case long _:
                case string _:
                    return arg.ToString();
                case DateTime time:
                    return time.ToString("yyyyMMddHHms");
                default:
                    return "";
            }
        }
    }
}
