using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class MapKeyObject : Dictionary<string, object>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public MapKeyObject()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public MapKeyObject(string key, object value)
        {
            this.Add(key, value);
        }
    }
}
