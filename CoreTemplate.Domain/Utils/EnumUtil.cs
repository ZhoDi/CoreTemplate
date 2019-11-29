using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public static class EnumUtil
    {
        /// <summary>
        /// 通过枚举名或ID称获取枚举,区分大小写
        /// </summary>
        public static T Parse<T>(object obj)
        {
            return (T)Enum.Parse(typeof(T), obj.ToString());
        }

        /// <summary>
        /// 通过枚举名或ID称获取枚举,区分大小写
        /// </summary>
        public static T GetEnum<T>(object obj)
        {
            return Parse<T>(obj);
        }

        /// <summary>
        /// 获取枚举的枚举数组,ID顺序
        /// </summary>
        public static T[] GetElements<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// 获取枚举的数值数组,顺序
        /// </summary>
        public static int[] GetIds<T>()
        {
            return (int[])Enum.GetValues(typeof(T));
        }

        /// <summary>
        /// 获取枚举的字符串数组,ID顺序
        /// </summary>
        public static string[] GetNames<T>()
        {
            return Enum.GetNames(typeof(T));
        }

        /// <summary>
        /// 获取键值对,ID顺序,正数在前,负数在后
        /// </summary> 
        public static Dictionary<int, string> GetMap<T>()
        {
            Dictionary<int, string> map = new Dictionary<int, string>();
            int[] ids = GetIds<T>();
            string[] names = GetNames<T>();
            if (ids.Length != names.Length)
                return map;
            for (int index = 0; index < ids.Length; index++)
            {
                map.Add(ids[index], names[index]);
            }
            return map;
        }

        /// <summary>
        /// 获取键值对,ID倒序
        /// </summary> 
        public static Dictionary<int, string> GetMapDesc<T>()
        {
            Dictionary<int, string> map = new Dictionary<int, string>();
            int[] ids = GetIds<T>();
            string[] names = GetNames<T>();
            if (ids.Length != names.Length)
                return map;
            for (int index = ids.Length; index > 0; index--)
            {
                map.Add(ids[index - 1], names[index - 1]);
            }
            return map;
        }
    }
}
