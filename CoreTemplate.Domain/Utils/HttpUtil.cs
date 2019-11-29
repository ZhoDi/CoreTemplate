using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// HTTP请求
    /// </summary>
    public static class HttpUtil
    {
        /// <summary>
        /// HttpPost用到的ContentType
        /// </summary>
        public static class ContentType
        {
            /// <summary>
            /// 文件
            /// </summary>
            public const string Stream = "application/octet-stream";

            /// <summary>
            /// UrlencodedForm
            /// </summary>
            public const string Form = "application/x-www-form-urlencoded";

            /// <summary>
            /// Json
            /// </summary>
            public const string Json = "application/json-patch+json";
        }

        /// <summary>
        /// 添加Headers
        /// </summary>
        private static void AddHeaders(this HttpClient client, MapKeyString headers)
        {
            if (headers != null)
                foreach (var header in headers)
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }

        /// <summary>
        /// GET
        /// </summary>
        private static HttpResponseMessage Get(string url, MapKeyString headers = null, bool log = true)
        {
            var client = new HttpClient();
            client.AddHeaders(headers);

            var message = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            try
            {
                message = client.GetAsync(url).Result;
            }
            catch (Exception ex)
            {
                if (log)
                    LogUtil.Log(url, ex);
            }

            Console.WriteLine(string.Format("HttpClient.Get.StatusCode:{0} ,Url:{1}", message.StatusCode, url));
            return message;
        }

        /// <summary>
        /// POST
        /// </summary>
        public static HttpResponseMessage Post(string url, string data, string contentType, Encoding encoding = null, MapKeyString headers = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            var client = new HttpClient();
            client.AddHeaders(headers);
            var content = new StringContent(data, encoding, contentType);

            var message = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            try
            {
                message = client.PostAsync(url, content).Result;
            }
            catch (Exception ex)
            {
                LogUtil.Log(url, ex);
            }

            Console.WriteLine(string.Format("HttpClient.Post.StatusCode:{0} ,Url:{1}", message.StatusCode, url));
            return message;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        public static string Upload(string url, string file, bool nameEncode = false, MapKeyString headers = null)
        {
            var client = new HttpClient();
            client.AddHeaders(headers);
            var stream = File.OpenRead(file);
            var streamContent = new StreamContent(stream);
            var multipartFormDataContent = new MultipartFormDataContent();
            var fileName = FileUtil.GetName(file);
            var formName = nameEncode ? UrlUtil.Encode(fileName) : fileName;
            multipartFormDataContent.Add(streamContent, "file", formName);

            var message = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            try
            {
                message = client.PostAsync(url, multipartFormDataContent).Result;
            }
            catch (Exception ex)
            {
                LogUtil.Log(url, ex);
            }

            Console.WriteLine(string.Format("HttpClient.Upload.StatusCode:{0} ,Url:{1}", message.StatusCode, url));
            stream.Close();
            if (message.StatusCode == HttpStatusCode.OK)
                return message.Content.ReadAsStringAsync().Result;
            return message.StatusCode.ToString();
        }

        /// <summary>
        /// GET
        /// </summary>
        public static string GetString(string url, MapKeyString headers = null, bool log = true)
        {
            var msg = Get(url, headers, log);
            if (msg.StatusCode == HttpStatusCode.OK)
                return msg.Content.ReadAsStringAsync().Result;
            return msg.StatusCode.ToString();
        }

        /// <summary>
        /// 文件内存流
        /// </summary>     
        public static Stream GetStream(string url, MapKeyString headers = null)
        {
            var msg = Get(url, headers);
            if (msg.StatusCode == HttpStatusCode.OK)
                return msg.Content.ReadAsStreamAsync().Result;
            return null;
        }

        /// <summary>
        /// 文件字节流
        /// </summary>
        public static byte[] GetBytes(string url, MapKeyString headers = null)
        {
            var msg = Get(url, headers);
            if (msg.StatusCode == HttpStatusCode.OK)
                return msg.Content.ReadAsByteArrayAsync().Result;
            return null;
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        public static bool DownloadFile(string url, string path, MapKeyString headers = null)
        {
            var msg = Get(url, headers);
            if (msg.StatusCode == HttpStatusCode.OK)
            {
                FileUtil.Save(path, msg.Content.ReadAsStreamAsync().Result);
                return true;
            }
            return false;
        }

        /// <summary>
        /// POST
        /// </summary>
        public static string PostForm(string url, string data, Encoding encoding = null, MapKeyString headers = null)
        {
            var msg = Post(url, data, ContentType.Form, encoding, headers);
            if (msg.StatusCode == HttpStatusCode.OK)
                return msg.Content.ReadAsStringAsync().Result;
            return msg.StatusCode.ToString();
        }

        /// <summary>
        /// POST
        /// </summary>
        public static Stream PostForm_(string url, string data, Encoding encoding = null, MapKeyString headers = null)
        {
            var msg = Post(url, data, ContentType.Form, encoding, headers);
            if (msg.StatusCode == HttpStatusCode.OK)
                return msg.Content.ReadAsStreamAsync().Result;
            return null;
        }

        /// <summary>
        /// POST
        /// </summary>
        public static string PostJson(string url, string json, Encoding encoding = null, MapKeyString headers = null)
        {
            var msg = Post(url, json, ContentType.Json, encoding, headers);
            if (msg.StatusCode == HttpStatusCode.OK)
                return msg.Content.ReadAsStringAsync().Result;
            return msg.StatusCode.ToString();
        }

        /// <summary>
        /// POST
        /// </summary>
        public static Stream PostJson_(string url, string json, Encoding encoding = null, MapKeyString headers = null)
        {
            var msg = Post(url, json, ContentType.Json, encoding, headers);
            if (msg.StatusCode == HttpStatusCode.OK)
                return msg.Content.ReadAsStreamAsync().Result;
            return null;
        }
    }
}
