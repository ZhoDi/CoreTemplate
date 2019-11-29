using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 数字相关工具
    /// </summary>
    public static class DigitUtil
    {
        /// <summary>
        /// 取最大值，可选参数：是否去重，序号
        /// </summary>
        /// <param name="noSame">是否去重</param>
        /// <param name="index">序号</param>
        public static int Max(this List<int> list, int index = 0, bool noSame = true)
        {
            if (noSame)
                list = list.Distinct().ToList();
            list = list.OrderByDescending(m => m).ToList();
            if (index >= list.Count)
                return 0;
            return list[index];
        }

        /// <summary>
        /// 转换为Int32
        /// </summary>
        public static float ToFloat(this object obj, float defaultValue = 0)
        {
            try
            {
                return float.Parse(obj.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 获取整形代表的布尔值
        /// </summary>
        public static bool ToBool(this int value)
        {
            return value > 0;
        }

        /// <summary>
        /// 字符串顺序
        /// </summary>
        public static int Order<Key>(Key str, Key[] orders)
        {
            for (int index = 0; index < orders.Length; index++)
                if (str.Equals(orders[index]))
                    return index;
            return orders.Length;
        }
    }
}
