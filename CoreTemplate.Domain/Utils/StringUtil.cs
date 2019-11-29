using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;

namespace CommonUtils
{
    /// <summary>
    /// 字符串工具
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// GUID
        /// </summary>
        public static string Guid(string tail = null)
        {
            return System.Guid.NewGuid() + tail;
        }

        /// <summary>
        /// 判断起始字符
        /// </summary>
        public static bool StartWith(this string text, params char[] chars)
        {
            foreach (char ch in chars)
                if (text[0] == ch)
                    return true;
            return false;
        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        public static string Substring(this string text, string keyword, bool include = true)
        {
            var length = text.Length;
            var index = text.IndexOf(keyword);
            if (!include)
                index += keyword.Length;
            return text.Substring(index, length - index);

        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        public static string Substring(this string text, int start, string keyword)
        {
            var length = text.Length;
            var index = text.IndexOf(keyword);
            return text.Substring(start, index);
        }

        /// <summary>
        /// 数组连接
        /// </summary> 
        public static string ToText(this IEnumerable<string> lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines)
                sb.AppendLine(line);
            return sb.ToString();
        }

        /// <summary>
        /// 格式化Url,http检查:如果有http返回原值,否者添加http://
        /// </summary>
        public static string FormatUrl(string web)
        {
            web = web.Trim();
            if (web.Length < 4)
            { return "http://" + web; }
            if (web.Substring(0, 4) == "http")
            { return web; }
            return "http://" + web;
        }

        /// <summary>
        /// 拼接
        /// </summary>
        public static string Join<T>(this IEnumerable<T> values, object separator)
        {
            return string.Join(separator.ToString(), values);
        }

        /// <summary>
        /// key数量
        /// </summary>
        public static int CountOf(this string text, string key)
        {
            return (text.Length - text.Replace(key, "").Length) / key.Length;
        }

        /// <summary>
        /// ASCII枚举
        /// </summary>
        public enum Ascii
        {
            _0 = 48,
            _9 = 57,
            A = 65,
            Z = 90,
            a = 97,
            z = 122,
        }

        /// <summary>
        /// 只取数字
        /// </summary>
        public static string GetDigits(this string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in text)
                if (ch >= (int)Ascii._0 && ch <= (int)Ascii._9)
                    sb.Append(ch);
            return sb.ToString();
        }

        /// <summary>
        /// 只取字母数字
        /// </summary>
        public static string GetLettersAndDigits(this string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in text)
            {
                if (ch >= (int)Ascii._0 && ch <= (int)Ascii._9)
                    sb.Append(ch);

                if (ch >= (int)Ascii.a && ch <= (int)Ascii.z)
                    sb.Append(ch);

                if (ch >= (int)Ascii.A && ch <= (int)Ascii.Z)
                    sb.Append(ch);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 移除char
        /// </summary>
        public static string RemoveChar(this string text, params char[] removes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in text)
                if (!removes.Contains(ch))
                    sb.Append(ch);
            return sb.ToString();
        }

        /// <summary>
        /// 移除字符串
        /// </summary>
        public static string Remove(this string text, params string[] removes)
        {
            foreach (string remove in removes)
                text = text.Replace(remove, null);
            return text;
        }

        /// <summary>
        /// 分隔，忽略空值
        /// </summary>
        public static string[] SplitNoEmpty(this string value, params char[] separators)
        {
            return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 分隔，忽略空值
        /// </summary>
        public static string[] SplitNoEmpty(this string value, params string[] separators)
        {
            return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 多字符串判空
        /// </summary>
        public static bool IsNullOrEmpty(params string[] strs)
        {
            foreach (string str in strs)
                if (string.IsNullOrEmpty(str))
                    return true;
            return false;
        }

        /// <summary>
        /// 多字符串判空
        /// </summary>
        public static bool IsNullOrWhiteSpace(params string[] strs)
        {
            foreach (string str in strs)
                if (string.IsNullOrWhiteSpace(str))
                    return true;
            return false;
        }

        /// <summary>
        /// 包含关键字
        /// </summary>
        public static bool Contains(this string str, string[] keys)
        {
            foreach (string key in keys)
                if (str.Contains(key))
                    return true;
            return false;
        }

        /// <summary>
        /// 文本行分隔符
        /// </summary>
        private static string[] mLineSeparator = new string[] { "\r\n", "\n", "\r" };

        /// <summary>
        /// 获取文本行
        /// </summary>
        public static string[] GetLines(this string text, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            return text.Split(mLineSeparator, splitOptions);
        }

        /// <summary>
        /// 空格拆分符
        /// </summary>
        private static char[] mSpaceSeparator = new char[] { ' ', '\t' };

        /// <summary>
        /// 空格拆分
        /// </summary>
        public static string[] SplitBySpace(this string text)
        {
            return text.Split(mSpaceSeparator, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
