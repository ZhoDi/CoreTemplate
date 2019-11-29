using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonUtils
{
    /// <summary>
    /// Html处理
    /// </summary>
    public static class HtmlUtil
    {
        /// <summary>
        /// 换行转为br
        /// </summary>
        public static string ToHtml(this string text)
        {
            string html = text.Replace(" ", "&nbsp;");
            html = html.Replace("\r\n", "<br />");
            html = html.Replace("\n", "<br />");
            return html;
        }

        /// <summary>
        /// 换行转为br
        /// </summary>
        public static string ToHtml(this string[] lines)
        {
            return lines.ToText().ToHtml();
        }

        /// <summary>
        /// 换行转为br
        /// </summary>
        public static string Html(this StringBuilder sb)
        {
            return sb.ToString().ToHtml();
        }

        /// <summary>
        /// 过滤html代码,保留文字字符
        /// </summary>
        public static string FilterTag(string html)
        {
            html = html.Trim();
            return string.IsNullOrEmpty(html) ? string.Empty : Regex.Replace(html, "<[^>]*>", string.Empty);
        }

        /// <summary>
        /// 获取用于显示的html代码
        /// </summary> 
        public static string GetDisplayXml(string html)
        {
            return html.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// UTF-8
        /// </summary>
        public static string CharsetMeta { get; } = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";

        /// <summary>
        /// ViewPort
        /// </summary>
        public static string ViewPortMeta { get; } = "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />";

        /// <summary>
        /// 网页重定位,尽量填写/绝对路径
        /// </summary>
        public static string RelocalMeta(string url)
        {
            return string.Format("<meta http-equiv=\"refresh\" content=\"0; url ={0}\">", url);
        }

        /// <summary>
        /// ICON
        /// </summary>
        public static string IconLink { get; } = "<link href=\"/favicon.ico\" mce_href=\"favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" />";

        /// <summary>
        /// 把字符串转换为UnicodeHtml
        /// </summary>
        public static string DocType(string innerBoday)
        {
            var html = "<!DOCTYPE html><html><head>{0}</head><body>{1}</body></html>";
            return string.Format(html, CharsetMeta, innerBoday);
        }

        /// <summary>
        /// 添加htmlA标签
        /// </summary> 
        public static string Href(string url, bool blank = true, string content = null)
        {
            url = StringUtil.FormatUrl(url);
            return string.Format("<a href=\"{0}\"{1}>{2}</a>", url, blank ? " target=\"_blank\"" : null, content);
        }

        /// <summary>
        /// 添加htmlA标签
        /// </summary> 
        public static string Href(string[] urls, bool blank = true)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string url in urls)
                sb.Append(string.Format("<a href=\"{0}\"{1}>{0}</a><hr />", url, blank ? " target=\"_blank\"" : null));
            return sb.ToString();
        }

        /// <summary>
        /// 邮件链接
        /// </summary>
        public static string MailHref(string mail, string content = null)
        {
            if (string.IsNullOrEmpty(content))
                content = mail;
            string html = "<a href=\"mailto:{0}\">{1}</a>";
            return string.Format(html, mail, content);
        }

        /// <summary>
        /// QQ链接
        /// </summary>
        public static string QQHref(string qq, string content = null)
        {
            if (string.IsNullOrEmpty(content))
                content = qq;
            string html = "<a href=\"tencent://message/?uin={0}\">{1}</a>";
            return string.Format(html, qq, content);
        }

        /// <summary>
        /// 网页背景音乐
        /// </summary>
        public static string Audio(string virPath = "/audio/bgm.mp3", bool display = false)
        {
            string html = "<audio autoplay=\"autoplay\" loop=\"loop\" src=\"{0}\"{1}>" +
            "Your browser does not support the audio element." +
            "</audio> ";
            return string.Format(html, virPath, display ? " controls=\"controls\"" : null);
        }

        /// <summary>
        /// 关闭网页
        /// </summary>
        public static string CloseScript { get; } = "<script>window.close();</script><button onclick=\"javascript:window.close();\">close</button>";
    }
}
