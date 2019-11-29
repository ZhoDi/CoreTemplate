using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 缓存工具
    /// </summary>
    public static class CacheUtil
    {
        #region 短存储

        /// <summary>
        /// 缓存
        /// </summary>
        private static MapKeyObject mMemoryMap { get; } = new MapKeyObject();

        /// <summary>
        /// 存储到缓存中
        /// </summary>
        public static void Set(string key, object value)
        {
            mMemoryMap.Set(key, value);
        }

        /// <summary>
        /// 存储到缓存中
        /// </summary>
        public static void Save(string key, object value)
        {
            Set(key, value);
        }

        /// <summary>
        /// 从缓存中获取
        /// </summary>
        /// <returns></returns>
        public static TValue Get<TValue>(string key, TValue defaultValue = default(TValue))
        {
            if (mMemoryMap.ContainsKey(key))
                return (TValue)mMemoryMap.Get(key);
            return defaultValue;
        }

        /// <summary>
        /// 首次访问
        /// </summary>
        public static bool FirstAccess(string key)
        {
            if (mMemoryMap.ContainsKey(key))
                return false;

            mMemoryMap.Set(key, true);
            return true;
        }

        #endregion

        #region 长存储

        /// <summary>
        /// 缓存
        /// </summary>
        private static MapKeyObject mFileMap { get; } = new MapKeyObject();

        /// <summary>
        /// 存储路径
        /// </summary>
        private static string GetSavePath(string key)
        {
            return PathUtil.Get("temp").Combine(key + ".json");
        }

        /// <summary>
        /// 存储到文件中
        /// </summary>
        public static void SetWithFile(string key, object value)
        {
            mFileMap.Set(key, value);
            FileUtil.Save(GetSavePath(key), value.ToJson(true));
        }

        /// <summary>
        /// 存储到文件中
        /// </summary>
        public static void SaveWithFile(string key, object value)
        {
            SetWithFile(key, value);
        }

        /// <summary>
        /// 从文件中读取
        /// </summary>
        public static TValue GetFromFile<TValue>(string key, TValue defaultValue = default(TValue))
        {
            if (mFileMap.ContainsKey(key))
                return (TValue)mFileMap.Get(key);
            var path = GetSavePath(key);
            if (FileUtil.Exists(path))
                return JsonUtil.DeserializeFromFile<TValue>(path);
            return defaultValue;
        }

        #endregion
    }
}
