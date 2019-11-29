using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 数据收集器
    /// </summary>
    public class MapKeyList<TKey, TValue> : Dictionary<TKey, List<TValue>>
    {
        /// <summary>
        /// 收集
        /// </summary>
        public void Collect(TKey key, TValue item)
        {
            if (ContainsKey(key))
                this[key].Add(item);
            else
                this[key] = new List<TValue>() { item };
        }
    }
}
