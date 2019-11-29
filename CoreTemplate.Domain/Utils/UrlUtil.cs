using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace CommonUtils
{
    public static class UrlUtil
    {
        /// <summary>
        /// UrlEncode
        /// </summary>
        public static string Encode(string text)
        {
            return HttpUtility.UrlEncode(text);
        }

        /// <summary>
        /// UrlDecode
        /// </summary>
        public static string Decode(string text)
        {
            return HttpUtility.UrlDecode(text);
        }

        /// <summary>
        /// URL拼接
        /// </summary>
        public static string Combine(string left, params string[] rights)
        {
            StringBuilder sb = new StringBuilder(left);
            foreach (string right in rights)
            {
                if (string.IsNullOrEmpty(right))
                    continue;

                if (sb[sb.Length - 1] == '/' && right[0] == '/')
                {
                    sb.Append(right.Substring(1));
                    continue;
                }

                if (sb[sb.Length - 1] == '/' || right[0] == '/')
                {
                    sb.Append(right);
                    continue;
                }

                sb.Append('/');
                sb.Append(right);
            }
            return sb.ToString();
        }

        /// <summary>
        /// URL父级
        /// </summary>
        public static string Parent(string url)
        {
            return url.Substring(0, url.LastIndexOf('/') + 1);
        }

        /// <summary>
        /// URL根路径
        /// </summary>
        public static string Root(string url)
        {
            Uri uri = new Uri(url);
            return string.Format("{0}://{1}/", uri.Scheme, uri.Authority);
        }

        /// <summary>
        /// 替换头部
        /// </summary>
        public static string ReplaceHead(string url, string head)
        {
            Uri uri1 = new Uri(url);
            Uri uri2 = new Uri(head);
            return string.Format("{0}://{1}{2}", uri2.Scheme, uri2.Authority, uri1.PathAndQuery);
        }
    }
}
