using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class MapKeyValue<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public MapKeyValue()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public MapKeyValue(TKey key, TValue value)
        {
            this.Add(key, value);
        }
    }
}
