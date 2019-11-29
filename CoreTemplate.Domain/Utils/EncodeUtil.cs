using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public static class EncodeUtil
    {
        /// <summary>
        /// UrlEncode
        /// </summary>
        public static string UrlEncode(this string url)
        {
            return UrlUtil.Encode(url);
        }

        /// <summary>
        /// UrlDecode
        /// </summary>
        public static string UrlDecode(this string url)
        {
            return UrlUtil.Decode(url);
        }

        /// <summary>
        /// base64编码
        /// </summary>
        public static string Base64Encode(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// base64编码
        /// </summary>
        public static string Base64Encode(this string text, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return encoding.GetBytes(text).Base64Encode();
        }

        /// <summary>
        /// base64解码
        /// </summary>
        public static byte[] Base64Decode(this string base64)
        {
            try
            {
                return Convert.FromBase64String(base64);
            }
            catch (Exception ex)
            {
                return ex.Message.ToBytes();
            }
        }

        /// <summary>
        /// base64解码
        /// </summary>
        public static string Base64DecodeString(this string base64, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return encoding.GetString(base64.Base64Decode());
        }

        /// <summary>
        /// Md5Encode
        /// </summary>
        public static string Md5Encode(this string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Md5Decode
        /// </summary>
        public static string Md5Decode(this string md5)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// AesEncode
        /// </summary>
        public static string AesEncode(this string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// AesDecode
        /// </summary>
        public static string AesDecode(this string aes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// DesEncode
        /// </summary>
        public static string DesEncode(this string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// DesDecode
        /// </summary>
        public static string DesDecode(this string des)
        {
            throw new NotImplementedException();
        }
    }
}
