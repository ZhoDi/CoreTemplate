using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using CoreTemplate.Domain.Shared.Helper;

namespace CoreTemplate.Application.Application.Redis
{
    public class RedisCacheManager:IRedisCacheManager
    {
        private readonly string _redisConnectionString;
        public volatile ConnectionMultiplexer RedisConnection;
        private static readonly object RedisConnectionLock = new object();
        public RedisCacheManager()
        {
            string redisConfiguration = Appsettings.App("AOP", "RedisCatchAOP", "ConnectionString");//获取连接字符串

            if (string.IsNullOrWhiteSpace(redisConfiguration))
            {
                throw new ArgumentException("redis config is empty", nameof(redisConfiguration));
            }
            this._redisConnectionString = redisConfiguration;
            this.RedisConnection = GetRedisConnection();
        }

        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this.RedisConnection == null || !this.RedisConnection.IsConnected)
            {
                //加锁，防止异步编程中，出现单例无效的问题
                lock (RedisConnectionLock)
                {
                    //释放redis连接
                    RedisConnection?.Dispose();
                    try
                    {
                        this.RedisConnection = ConnectionMultiplexer.Connect(_redisConnectionString);
                    }
                    catch (Exception)
                    {

                        throw new Exception("Redis服务未启用，请开启该服务");
                    }
                }
            }

            return this.RedisConnection;
        }

        public void Clear()
        {
            foreach (var endPoint in this.GetRedisConnection().GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    RedisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }

        public bool Get(string key)
        {
            return RedisConnection.GetDatabase().KeyExists(key);
        }

        public string GetValue(string key)
        {
            return RedisConnection.GetDatabase().StringGet(key);
        }

        public TEntity Get<TEntity>(string key)
        {
            var value = RedisConnection.GetDatabase().StringGet(key);
            return value.HasValue ? SerializeHelper.Deserialize<TEntity>(value) : default(TEntity);
        }

        public void Remove(string key)
        {
            RedisConnection.GetDatabase().KeyDelete(key);
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                RedisConnection.GetDatabase().StringSet(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }

        public bool SetValue(string key, byte[] value)
        {
            return RedisConnection.GetDatabase().StringSet(key, value, TimeSpan.FromSeconds(120));
        }
    }
}
