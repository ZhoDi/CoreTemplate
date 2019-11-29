using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonUtils
{
    /// <summary>
    /// 数据转换逻辑
    /// </summary>
    public static class ConvertUtil
    {
        /// <summary>
        /// 转换为.5格式
        /// </summary>
        public static double ToDotFive(this double hour)
        {
            if ((int)(hour * 10) / 5 % 2 == 0)
                return (int)hour;
            else
                return (int)hour + 0.5f;
        }

        /// <summary>
        /// 转换为枚举
        /// </summary>
        public static T ToEnum<T>(this object obj)
        {
            return EnumUtil.Parse<T>(obj);
        }

        /// <summary>
        /// 将datatable转换为XML
        /// </summary>
        public static string ToXml(DataTable dt, string dataName = "Data")
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
            xml += "\r\n<List>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                xml += "\r\n\t<" + dataName + ">";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    xml += "\r\n\t\t<" + dt.Columns[j].ColumnName + ">" + dt.Rows[i][j] + "</" + dt.Columns[j].ColumnName + ">";
                }
                xml += "\r\n\t</" + dataName + ">";
            }
            xml += "\r\n</List>";
            return xml;
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public static string ToString(object obj)
        {
            try
            {
                return obj.ToString();
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public static string ToHanzi(this int digit)
        {
            return digit.ToString()
                .Replace("0", "零")
                .Replace("1", "一")
                .Replace("2", "二")
                .Replace("3", "三")
                .Replace("4", "四")
                .Replace("5", "五")
                .Replace("6", "六")
                .Replace("7", "七")
                .Replace("8", "八")
                .Replace("9", "九");
        }

        /// <summary>
        /// 转换为布尔值
        /// </summary>
        public static bool ToBool(this object obj)
        {
            try
            {
                string str = obj.ToString();
                if (str == "0")
                    return false;
                if (str == "1")
                    return true;
                return Convert.ToBoolean(obj);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换为ToDouble
        /// </summary>
        public static double ToDouble(object obj)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为Decimal
        /// </summary>
        public static decimal ToDecimal(object obj)
        {
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return Convert.ToDecimal("0.00");
            }
        }

        /// <summary>
        /// 转换为float
        /// </summary>
        public static float ToFloat(object obj)
        {
            try
            {
                float f = float.Parse(obj.ToString());
                f = float.Parse(f.ToString("0.00"));
                return f;
            }
            catch
            {
                return float.Parse("0.00");
            }
        }

        /// <summary>
        /// 转换为2位小数字符串
        /// </summary>
        public static string ToFloatString(object obj)
        {
            float f = ToFloat(obj);
            return f.ToString("0.00");
        }

        /// <summary>
        /// 转换为Guid
        /// </summary>
        public static Guid ToGuid(object obj)
        {
            try
            {
                return new Guid(obj.ToString());
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// 转换为16进制
        /// </summary>
        public static string To16Int(int value)
        {
            return "0x" + value.ToString("X2");
        }

        /// <summary>
        /// 16转换为10进制
        /// </summary>
        public static int From16Int(string value)
        {
            return Convert.ToInt32(value, 16);
        }

        /// <summary>
        /// 转换为Int32
        /// </summary>
        public static int ToInt(this object obj, int defaultValue = 0)
        {
            try
            {
                string str = obj.ToString();
                if (str.Length > 10)
                {
                    long l = ToLong(str);
                    if (l != 0)
                    {
                        LogUtil.Log(new Exception("长整形丢失"));
                        return -999999999;
                    }
                }
                return int.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 转换为long
        /// </summary>
        public static long ToLong(this object obj)
        {
            try
            {
                return long.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将布尔值转换为性别
        /// </summary>
        public static string ToSex(object obj)
        {
            bool b;
            try
            { b = Convert.ToBoolean(obj); }
            catch
            { return "未知"; }
            if (b)
            { return "男"; }
            else
            { return "女"; }
        }

        /// <summary>
        /// 将布尔值转换为启用状态
        /// </summary>
        public static string ToStartState(object obj)
        {
            bool b;
            try
            { b = Convert.ToBoolean(obj); }
            catch
            { return "未知"; }
            if (b)
            { return "已启用"; }
            else
            { return "已禁用"; }
        }

        /// <summary>
        /// 将布尔值转换为登录状态
        /// </summary>
        public static string ToLoginState(object obj)
        {
            bool b;
            try
            { b = Convert.ToBoolean(obj); }
            catch
            { return "未知"; }
            if (b)
            { return "在线"; }
            else
            { return "离线"; }
        }
    }
}
