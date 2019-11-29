using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 时间相关工具
    /// </summary>
    public static class TimeUtil
    {
        /// <summary>
        /// 网络时间,失败返回 DateTime.Now
        /// </summary>
        private static DateTime mNet
        {
            get
            {
                try
                {
                    string res = HttpUtil.GetString("http://cgi.im.qq.com/cgi-bin/cgi_svrtime", null, false);
                    if (DateTime.TryParse(res, out DateTime time))
                        return time;
                    Console.WriteLine("网络时间获取失败:" + res);
                    return DateTime.Now;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("网络时间获取失败:" + ex.Message);
                    return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 时间差
        /// </summary>
        private static long mNetDiff = (mNet - DateTime.Now).Ticks;

        /// <summary>
        /// 网络时间
        /// </summary>
        public static DateTime Net
        {
            get
            {
                return DateTime.Now.AddTicks(mNetDiff);
            }
        }

        /// <summary>
        /// 时间戳起点
        /// </summary>
        private static DateTime mTimestampStart = DateTime.Parse("1970-01-01 00:00:00") + TimeZoneInfo.Local.BaseUtcOffset;

        /// <summary>
        /// 从时间戳获取时间
        /// </summary> 
        public static DateTime FromStamp(long stamp)
        {
            while (stamp < 1000000000000)
                stamp *= 10;
            return mTimestampStart.AddMilliseconds(stamp % 10000000000000);
        }

        /// <summary>
        /// 时间戳 10位 秒级
        /// </summary> 
        public static int Stamp(this DateTime time)
        {
            return (int)(time - mTimestampStart).TotalSeconds;
        }

        /// <summary>
        /// 时间戳 13位 毫秒级
        /// </summary> 
        public static long LongStamp(this DateTime time)
        {
            return (long)(time - mTimestampStart).TotalMilliseconds;
        }

        /// <summary>
        /// 日期时间戳 10位 秒级
        /// </summary>
        public static int DateStamp(this DateTime time)
        {
            return time.Date.Stamp();
        }

        /// <summary>
        /// 获取时间
        /// </summary> 
        public static DateTime FromTicks(long ticks)
        {
            return new DateTime(ticks);
        }

        /// <summary>
        /// 获取时间
        /// </summary> 
        public static DateTime Parse(object time)
        {
            try
            {
                return DateTime.Parse(time.ToString());
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 获取时间
        /// </summary> 
        public static DateTime ToTime(this object time)
        {
            return Parse(time);
        }

        /// <summary>
        /// 非标准时间字符串转为时间,标准时间字符串直接使用Parse或ToDateTime
        /// </summary>
        public static DateTime ToDateTimeByFormat(string str, string format = "yyyyMMddHHmmssfff")
        {
            if (str.Length < format.Length)
                format = format.Substring(0, str.Length);
            if (str.Length > format.Length)
                str = str.Substring(0, format.Length);
            return DateTime.ParseExact(str, format, null);
        }

        /// <summary>
        /// 时间拆分字符串
        /// </summary>
        public static string YearString(this DateTime time)
        {
            return time.ToString("yyyy");
        }

        /// <summary>
        /// 时间拆分字符串
        /// </summary>
        public static string MonthString(this DateTime time)
        {
            return time.ToString("MM");
        }

        /// <summary>
        /// 获取星期信息
        /// </summary>
        public static string WeekString(this DateTime time)
        {
            return time.ToString("dddd");
        }

        /// <summary>
        /// 时间拆分字符串
        /// </summary>
        public static string DayString(this DateTime time)
        {
            return time.ToString("dd");
        }

        /// <summary>
        /// 时间拆分字符串
        /// </summary>
        public static string HourString(this DateTime time)
        {
            return time.ToString("HH");
        }

        /// <summary>
        /// 时间拆分字符串
        /// </summary>
        public static string MinuteString(this DateTime time)
        {
            return time.ToString("mm");
        }

        /// <summary>
        /// 时间拆分字符串
        /// </summary>
        public static string SecondString(this DateTime time)
        {
            return time.ToString("ss");
        }

        /// <summary>
        /// 时间拆分字符串
        /// </summary>
        public static string DateString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 精确到毫秒
        /// </summary>
        public static string FullString(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 时间字符串
        /// </summary>
        public static string Info(this DateTime time, string format = "yyyy-MM-ddTHH:mm:ss")
        {
            return time.ToString(format);
        }

        /// <summary>
        /// 带小数的小时
        /// </summary>
        public static float FloatHour(this DateTime time)
        {
            return time.Hour + (float)time.Minute / 60;
        }

        /// <summary>
        /// 不计日期 只计时间
        /// </summary>
        public static DateTime Time(this DateTime time)
        {
            return new DateTime().Add(time - time.Date);
        }

        /// <summary>
        /// 时间计算的中间状态
        /// </summary>
        public static TimeSpan Span(this DateTime time)
        {
            return time - new DateTime();
        }

        /// <summary>
        /// 时间累加
        /// </summary>
        public static DateTime Add(this DateTime time1, DateTime time2)
        {
            return time1.AddTicks(time2.Ticks); ;
        }

        /// <summary>
        /// 时间累加,仅仅时间
        /// </summary>
        public static DateTime AddTime(this DateTime time1, DateTime time2)
        {
            return time1.AddTicks(time2.Time().Ticks); ;
        }

        /// <summary>
        /// 返回当前的分钟序列，一天有60*24个序列
        /// </summary>
        public static int MinuteIndex(this DateTime time)
        {
            return (int)time.Time().Span().TotalMinutes;
        }
    }
}