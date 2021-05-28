﻿using System;
using Microsoft.Extensions.Caching.Memory;

namespace CoreTemplate.Domain.Shared.MemoryCache
{
    public class MemoryCaching : ICaching
    {
        /// <summary>
        /// 实例化缓存接口ICaching
        /// </summary>
        private readonly IMemoryCache _cache;
        //还是通过构造函数的方法，获取
        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue,int catchTime)
        {
            _cache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(catchTime));
        }
    }
}