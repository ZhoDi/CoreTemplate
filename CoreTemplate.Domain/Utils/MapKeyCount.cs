using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 计数器
    /// </summary>
    public class MapKeyCount<TKey> : Dictionary<TKey, int>
    {
        /// <summary>
        /// 累加
        /// </summary>
        public void Sum(TKey key, int count = 1)
        {
            if (key == null)
            {
                Console.WriteLine("MapKeyCount.Sum received a null key.");
                return;
            }

            if (ContainsKey(key))
                this[key] += count;
            else
                Add(key, count);
        }

        /// <summary>
        /// 比例
        /// </summary>
        public float Rate(TKey key)
        {
            int total = 0;
            foreach (var keyValue in this)
                total += keyValue.Value;
            if (total == 0)
                return 1;
            return ((float)this.Get(key)) / total;
        }

        /// <summary>
        /// 排序 正序
        /// </summary>
        public MapKeyCount<TKey> SortByCount()
        {
            var map = new MapKeyCount<TKey>();
            foreach (var keyValue in this.OrderBy(m => m.Value))
                map.Add(keyValue.Key, keyValue.Value);
            return map;
        }

        /// <summary>
        /// 排序 倒序
        /// </summary>
        public MapKeyCount<TKey> SortByCountDesc()
        {
            var map = new MapKeyCount<TKey>();
            foreach (var keyValue in this.OrderByDescending(m => m.Value))
                map.Add(keyValue.Key, keyValue.Value);
            return map;
        }

        /// <summary>
        /// [{Key:@Key,Count:@Count}] 转为数组，与map区分
        /// </summary>
        public JArray Array()
        {
            JArray array = new JArray();
            foreach (var keyValue in this)
            {
                JObject item = new JObject();
                item.Add("Key", keyValue.Key.ToString());
                item.Add("Count", keyValue.Value);
                array.Add(item);
            }
            return array;
        }
    }
}
