using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class MapKeyString : Dictionary<string, string>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public MapKeyString()
        {
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public MapKeyString(string key, string value)
        {
            Add(key, value);
        }

        /// <summary>
        /// 初始化 xxx:xxx;xxxx
        /// </summary>
        public MapKeyString(string[] lines)
        {
            int splitIndex;
            foreach (var line in lines)
            {
                splitIndex = line.IndexOf(':');
                Add(line.Substring(0, splitIndex), line.Substring(splitIndex + 1));
            }
        }
    }
}
