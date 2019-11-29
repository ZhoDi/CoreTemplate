using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 类扩展
    /// </summary>
    public static class Extension
    {
        #region Array

        /// <summary>
        /// 数组分割
        /// </summary> 
        public static List<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, int size)
        {
            List<IEnumerable<TSource>> set = new List<IEnumerable<TSource>>();
            while (source.Count() > size)
            {
                set.Add(source.Take(size));
                source = source.Skip(size);
            }
            set.Add(source);
            return set;
        }

        /// <summary>
        /// 分页
        /// </summary>
        public static IEnumerable<T> Paged<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return source.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
        }

        /// <summary>
        /// 数组取值
        /// </summary>  
        public static T Get<T>(this T[] items, int index, T defaultValue = default(T))
        {
            if (index < items.Length)
                return items[index];
            else
                return defaultValue;

        }

        #endregion

        #region Dictionary key不可为空，value可以为空

        /// <summary>
        /// 判空取值
        /// </summary>
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> map, TKey key, TValue defaultValue = default(TValue), bool log = true)
        {
            if (key != null && map.ContainsKey(key))
                return map[key];
            if (log)
                LogUtil.Log(string.Format("Dictionary尝试获取不存在的Key：{0}\r\n{1}", key, new StackTrace(true)));
            return defaultValue;
        }

        /// <summary>
        /// 赋值,默认覆盖，可选保留
        /// </summary>
        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> map, TKey key, TValue value, bool replace = true)
        {
            if (key == null)
                return;

            if (!replace && map.ContainsKey(key))
                return;

            map[key] = value;
        }

        /// <summary>
        /// 根据Value删除项
        /// </summary>
        public static void RemoveValue<TKey, TValue>(this Dictionary<TKey, TValue> map, TValue value)
        {
            if (map.ContainsValue(value))
            {
                List<TKey> keys = new List<TKey>();
                foreach (var pair in map)
                    if (pair.Value.Equals(value))
                        keys.Add(pair.Key);
                foreach (TKey key in keys)
                    map.Remove(key);
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public static Dictionary<TKey, TValue> SortByKey<TKey, TValue>(this Dictionary<TKey, TValue> map, TKey[] keyOrders = null)
        {
            TKey[] newKeys;
            if (keyOrders == null)
                newKeys = map.Keys.OrderBy(m => m).ToArray();
            else
                newKeys = map.Keys.OrderBy(m => DigitUtil.Order(m, keyOrders)).ToArray();

            var newMap = new Dictionary<TKey, TValue>();
            foreach (TKey key in newKeys)
                newMap.Add(key, map[key]);

            return newMap;
        }

        #endregion;

        #region DataTable
        /// <summary>
        /// 类封装类型名
        /// </summary>
        private static string PackName(this Type type, bool compatible)
        {
            string name = type.Name;
            switch (name)
            {
                case "String":
                    name = "string";
                    break;

                case "Int32":
                    name = "int";
                    break;

                case "Int16":
                    name = "short";
                    break;
            }
            if (compatible && type.IsValueType)
                name += '?';
            return name;
        }

        /// <summary>
        /// 获取DataTable隐含的类结构
        /// </summary>
        public static string GetClassCode(this DataTable table, bool compatible = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region ---GeneratedByTable---");
            foreach (DataColumn column in table.Columns)
                sb.AppendLine(string.Format("public {0} {1} ", column.DataType.PackName(compatible), column.ColumnName) + "{ get; set; }");
            sb.Append("#endregion");
            return sb.ToString();
        }

        /// <summary>
        /// 删除标题中的_,引用类型不用return,return是为了兼容连写
        /// </summary>
        public static DataTable TitleRemove_(this DataTable table)
        {
            for (int index = 0; index < table.Columns.Count; index++)
                table.Columns[index].ColumnName = table.Columns[index].ColumnName.RemoveChar('_');
            return table;
        }

        /// <summary>
        /// datatable转为对象数组
        /// </summary>
        public static T[] ToArray<T>(this DataTable table)
        {
            //用反射机制可以实现的，这里偷懒了，直接用newtonsoft的json转一下
            return JsonUtil.Deserialize<T[]>(table.ToJson());
        }
        #endregion

        #region DataRow
        /// <summary>
        /// 获取object
        /// </summary>
        public static object Get(this DataRow dr, string columnName)
        {
            try
            {
                return dr[columnName];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取String
        /// </summary>
        public static string GetString(this DataRow dr, string columnName)
        {
            return dr.Get(columnName).ToString();
        }

        /// <summary>
        /// 获取Int
        /// </summary>
        public static int GetInt(this DataRow dr, string columnName)
        {
            return dr.Get(columnName).ToInt();
        }

        /// <summary>
        /// 获取Long
        /// </summary>
        public static long GetLong(this DataRow dr, string columnName)
        {
            return dr.Get(columnName).ToLong();
        }

        /// <summary>
        /// 获取Bool
        /// </summary>
        public static bool GetBool(this DataRow dr, string columnName)
        {
            return dr.Get(columnName).ToBool();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        public static DateTime GetTime(this DataRow dr, string columnName)
        {
            return TimeUtil.Parse(dr.Get(columnName));
        }

        /// <summary>
        /// 获取二进制
        /// </summary>
        public static byte[] GetBytes(this DataRow dr, string columnName)
        {
            return dr.Get(columnName) as byte[];
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        public static T GetEnum<T>(this DataRow dr, string columnName)
        {
            return dr.Get(columnName).ToEnum<T>();
        }
        #endregion

        #region IDataReader
        /// <summary>
        /// 获取object
        /// </summary>
        public static object Get(this IDataReader dr, string columnName)
        {
            try
            {
                return dr[columnName];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取String
        /// </summary>
        public static string GetString(this IDataReader dr, string columnName)
        {
            return dr.Get(columnName).ToString();
        }

        /// <summary>
        /// 获取Int
        /// </summary>
        public static int GetInt(this IDataReader dr, string columnName)
        {
            return dr.Get(columnName).ToInt();
        }

        /// <summary>
        /// 获取Long
        /// </summary>
        public static long GetLong(this IDataReader dr, string columnName)
        {
            return dr.Get(columnName).ToLong();
        }

        /// <summary>
        /// 获取Bool
        /// </summary>
        public static bool GetBool(this IDataReader dr, string columnName)
        {
            return dr.Get(columnName).ToBool();
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        public static DateTime GetTime(this IDataReader dr, string columnName)
        {
            return dr.Get(columnName).ToTime();
        }

        /// <summary>
        /// 获取枚举
        /// </summary>
        public static T GetEnum<T>(this IDataReader dr, string columnName)
        {
            return dr.Get(columnName).ToEnum<T>();
        }
        #endregion

        #region WebRequest WebResponse
        /// <summary>
        /// WebRequest添加文件
        /// </summary>
        /// <param name="request"></param>
        /// <param name="path"></param>
        public static void AddFile(this WebRequest request, string path)
        {
            Stream requestStream = request.GetRequestStream();
            var fileStream = File.OpenRead(path);
            fileStream.CopyTo(requestStream);
            fileStream.Close();
            requestStream.Close();
        }

        /// <summary>
        /// WebRequest添加文本
        /// </summary>
        /// <param name="request"></param>
        /// <param name="path"></param>
        public static void AddText(this WebRequest request, string text, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            Stream requestStream = request.GetRequestStream();
            StreamWriter sw = new StreamWriter(requestStream, encoding);
            sw.Write(text);
            sw.Close();
            requestStream.Close();
        }

        /// <summary>
        /// WebRequest添加二进制
        /// </summary>
        public static void AddBytes(this WebRequest request, byte[] bytes)
        {
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
        }

        /// <summary>
        /// WebRequest添加内存流
        /// </summary>
        public static void AddStream(this WebRequest request, Stream stream)
        {
            Stream requestStream = request.GetRequestStream();
            stream.CopyTo(requestStream);
            stream.Close();
            requestStream.Close();
        }

        /// <summary>
        /// WebResponse获取文件
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static void SaveFile(this WebResponse response, string path)
        {
            var stream = response.GetResponseStream();
            var fileStream = File.Create(path);
            stream.CopyTo(fileStream);
            fileStream.Close();
            stream.Close();
        }

        /// <summary>
        /// WebResponse获取文本
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetText(this WebResponse response, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            var stream = response.GetResponseStream();
            StreamReader sd = new StreamReader(stream, encoding);
            string res = sd.ReadToEnd();
            sd.Close();
            stream.Close();
            return res;
        }
        #endregion
    }
}
