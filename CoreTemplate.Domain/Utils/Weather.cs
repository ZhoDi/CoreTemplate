using System;

namespace CommonUtils
{
    /// <summary>
    /// 天气
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 天气
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public int Temp { get; set; }

        /// <summary>
        /// 最低温度
        /// </summary>
        public int TempLow { get; set; }

        /// <summary>
        /// 最高温度
        /// </summary>
        public int TempHigh { get; set; }

        /// <summary>
        /// 风力
        /// </summary>
        public string Wind { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// 空气质量
        /// </summary>
        public string Air { get; set; }

        /// <summary>
        /// 温度排序
        /// </summary>
        public void SortTemp()
        {
            if (TempLow > TempHigh)
            {
                TempHigh = TempLow + TempHigh;
                TempLow = TempHigh - TempLow;
                TempHigh = TempHigh - TempLow;
            }
        }
    }
}
