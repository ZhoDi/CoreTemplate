using Castle.DynamicProxy;
using CoreTemplate.Application.Application.Redis;
using CoreTemplate.Application.TemplateAttribute;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemplate.Application.AOP.Memory
{
    public class RedisCacheAop: IInterceptor
    {
        private readonly IRedisCacheManager _cache;
        public RedisCacheAop(IRedisCacheManager cache)
        {
            _cache = cache;
        }

        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;
            if (qCachingAttribute != null)
            {
                //获取自定义缓存键，这个和Memory内存缓存是一样的，不细说
                var cacheKey = CustomCacheKey(invocation);
                //核心1：注意这里和之前不同，是获取的string值，之前是object
                var cacheValue = _cache.GetValue(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    var type = invocation.Method.ReturnType;
                    var resultTypes = type.GenericTypeArguments;
                    if (type.FullName == "System.Void")
                    {
                        return;
                    }
                    object response;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        //核心2：返回异步对象Task<T>
                        if (resultTypes.Length > 0)
                        {
                            var resultType = resultTypes.FirstOrDefault();
                            // 核心3，直接序列化成 dynamic 类型，之前我一直纠结特定的实体
                            dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, resultType);
                            response = Task.FromResult(temp);

                        }
                        else
                        {
                            //Task 无返回方法 指定时间内不允许重新运行
                            response = Task.Yield();
                        }
                    }
                    else
                    {
                        // 核心4，要进行 ChangeType
                        response = System.Convert.ChangeType(_cache.Get<object>(cacheKey), type);
                    }

                    invocation.ReturnValue = response;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();

                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    object response;

                    //Type type = invocation.ReturnValue?.GetType();
                    var type = invocation.Method.ReturnType;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    response ??= string.Empty;
                    // 核心5：将获取到指定的response 和特性的缓存时间，进行set操作
                    _cache.Set(cacheKey, response, TimeSpan.FromSeconds(qCachingAttribute.AbsoluteExpiration));
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
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
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            return arg is DateTime time ? time.ToString("yyyyMMddHHms") : "";
        }
    }
}
