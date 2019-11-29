using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CommonUtils
{
    public class WeatherUtil
    {
        /// <summary>
        /// 获取天气信息
        /// </summary>
        /// <param name="cityPinyin">城市汉语拼音</param>
        public static Weather Weather(string cityPinyin = "shanghai")
        {
            string url = string.Format("http://weather.sina.com.cn/{0}", cityPinyin);
            try
            {
                HtmlDoc html = HtmlDoc.Load(HttpUtil.GetString(url, null, false));

                var core0 = html.GetElementByHead("div", "<div class=\"slider_ct\">");
                var city = core0.GetElementByHead("h4", "<h4 class=\"slider_ct_name\" id = \"slider_ct_name\" >").Value();
                var time = core0.GetElementByHead("p", "<p class=\"slider_ct_date\">").Value() + " " + core0.GetElementByHead("div", "<div class=\"slider_ct_time png24\"").Value();

                var core1 = html.GetElementByHead("div", "<div class=\"slider_states\">");
                var temp = core1.GetElementByHead("div", "<div class=\"slider_degree\">").Value().Remove("&#8451;").ToInt();
                var detail = core1.GetElementByHead("p", "<p class=\"slider_detail\">").Value();
                var values = detail.SplitNoEmpty(" ", "&nbsp;", "|", "\n");

                var core2 = html.GetElementByHead("div", "<div class=\"blk_fc_c0_i\" >");
                var tempRange = core2.GetElementByHead("p", "<p class=\"wt_fc_c0_i_temp\">").Value().SplitNoEmpty("°C", " ", "/");
                var tempLow = tempRange[0].ToInt();
                var tempHigh = tempRange[1].ToInt();
                var air = core2.GetElementByHead("li", "<li class=\"l\">").Value() + " " + core2.GetElementByHead("li", "<li class=\"r\">").Value();

                var weather = new Weather()
                {
                    City = city,
                    Time = time,
                    Air = air,
                    Desc = values[0],
                    Humidity = values[2].GetDigits().ToInt(),
                    Temp = temp,
                    TempLow = tempLow,
                    TempHigh = tempHigh,
                    Wind = values[1]
                };
                weather.SortTemp();
                return weather;
            }
            catch (Exception ex)
            {
                Console.WriteLine("天气信息获取失败!");
                Console.WriteLine("Api:" + url);
                Console.WriteLine("Error:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取天气信息,缺少空气信息
        /// </summary>
        public static Weather WeatherShanghai()
        {
            string url = "http://flash.weather.com.cn/wmaps/xml/shanghai.xml";
            try
            {
                var text = HttpUtil.GetString(url);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(text);
                var element = xml.GetElementsByTagName("city")[2];
                Weather weather = new Weather()
                {
                    City = element.Attributes["cityname"].Value,
                    Desc = element.Attributes["stateDetailed"].Value,
                    TempHigh = element.Attributes["tem1"].Value.ToInt(),
                    TempLow = element.Attributes["tem2"].Value.ToInt(),
                    Temp = element.Attributes["temNow"].Value.ToInt(),
                    Wind = element.Attributes["windDir"].Value + " " + element.Attributes["windPower"].Value,
                    Humidity = element.Attributes["humidity"].Value.RemoveChar('%').ToInt(),
                    Time = element.Attributes["time"].Value,
                    Air = "27优"
                };
                weather.SortTemp();
                return weather;
            }
            catch (Exception ex)
            {
                Console.WriteLine("天气信息获取失败!");
                Console.WriteLine("Api:" + url);
                Console.WriteLine("Error:" + ex.Message);
                return null;
            }
        }
    }
}
