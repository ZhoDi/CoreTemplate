using System;

namespace CoreTemplate.Domain.Shared.Attribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute:System.Attribute
    {
        //缓存绝对过期时间
        public int AbsoluteExpiration { get; set; } = 30;
    }
}
