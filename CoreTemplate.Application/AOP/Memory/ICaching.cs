using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemplate.Application.AOP.Memory
{
    public interface ICaching
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue,int catchTime);
    }
}
