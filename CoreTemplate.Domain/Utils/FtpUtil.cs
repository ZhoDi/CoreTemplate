using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// ftp工具
    /// </summary>
    public class FtpUtil
    {
        /// <summary>
        /// 文件列表(包含目录列表)
        /// </summary> 
        public static string[] FileNames(string url, string user = null, string pwd = null, Encoding encoding = null)
        {
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.GetResponseStream().GetString(encoding).GetLines();
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary> 
        public static bool Exists(string url, string user = null, string pwd = null)
        {
            try
            {
                return FileNames(url, user, pwd).Length > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ftp ListDirectory error:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        public static void MakeDirectory(string url, string user = null, string pwd = null)
        {
            if (url == UrlUtil.Root(url))
                return;

            if (Exists(url, user, pwd))
                return;

            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                request.GetResponse();
            }
            catch (Exception ex)
            {
                //主要是忽略文件夹已存在的异常
                //获取文件列表判断文件是否存在是不可取的，因为权限可以是只写
                Console.WriteLine("ftp MakeDirectory error:" + ex.Message);
            }
        }

        /// <summary>
        /// 创建文件的文件夹
        /// </summary>
        public static void MakeFileDirectory(string url, string user = null, string pwd = null)
        {
            MakeDirectory(UrlUtil.Parent(url), user, pwd);
        }

        /// <summary>
        /// 上传
        /// </summary> 
        public static string Upload(string url, string path, string user = null, string pwd = null)
        {
            MakeFileDirectory(url, user, pwd);
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.GetRequestStream().LoadFile(path, true);
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 上传内存流
        /// </summary> 
        public static string Upload(string url, Stream stream, string user = null, string pwd = null)
        {
            MakeFileDirectory(url, user, pwd);
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.GetRequestStream().Load(stream, true);
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 上传二进制
        /// </summary> 
        public static string Upload(string url, byte[] bytes, string user = null, string pwd = null)
        {
            MakeFileDirectory(url, user, pwd);
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.GetRequestStream().Load(bytes, true);
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 上传文本
        /// </summary> 
        public static string UploadText(string url, string text, string user = null, string pwd = null)
        {
            MakeFileDirectory(url, user, pwd);
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.GetRequestStream().Load(text, Encoding.UTF8, true);
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 附加文本，支持创建
        /// </summary> 
        public static string AppendText(string url, string text, string user = null, string pwd = null)
        {
            MakeFileDirectory(url, user, pwd);
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.AppendFile;
            request.GetRequestStream().Load(text, Encoding.UTF8, true);
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 附加文本行
        /// </summary> 
        public static string AppendLine(string url, string text, string user = null, string pwd = null)
        {
            return AppendText(url, text + "\r\n", user, pwd);
        }

        /// <summary>
        /// 判连接可用
        /// </summary> 
        public static bool CanConect(string url, string user = null, string pwd = null)
        {
            try
            {
                FileNames(UrlUtil.Root(url), user, pwd);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ftp ListDirectory error:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 下载
        /// </summary> 
        public static string Download(string url, string path, string user = null, string pwd = null)
        {
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            response.GetResponseStream().CreateFile(path);
            return response.StatusDescription;
        }

        /// <summary>
        /// 读取文本
        /// </summary> 
        public static string DownloadText(string url, string user = null, string pwd = null)
        {
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.GetResponseStream().GetString();
        }

        /// <summary>
        /// 读取数据流
        /// </summary> 
        public static Stream DownloadStream(string url, string user = null, string pwd = null)
        {
            try
            {
                FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
                if (!string.IsNullOrEmpty(user))
                    request.Credentials = new NetworkCredential(user, pwd);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                return response.GetResponseStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DownloadStream Error at Url::" + url);
                throw ex;
            }
        }

        /// <summary>
        /// 删除
        /// </summary> 
        public static string Delete(string url, string user = null, string pwd = null)
        {
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 删除文件夹
        /// </summary> 
        public static string DeleteDirectory(string url, string user = null, string pwd = null)
        {
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 重命名，移动
        /// </summary> 
        public static string Rename(string url, string name, string user = null, string pwd = null)
        {
            FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
            if (!string.IsNullOrEmpty(user))
                request.Credentials = new NetworkCredential(user, pwd);
            request.Method = WebRequestMethods.Ftp.Rename;
            //这个地方支持跨文件夹，写相对路径即可，前提是文件夹存在
            //可以算出新文件夹，并尝试创建，太麻烦了不写了
            request.RenameTo = name;
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            return response.StatusDescription;
        }

        /// <summary>
        /// 测试
        /// </summary> 
        public static JObject Test(string url, string user = null, string pwd = null)
        {
            JObject result = new JObject();
            result.Add("Url", url);
            result.Add("User", user);
            result.Add("Pwd", pwd);
            try
            {
                FtpWebRequest request = WebRequest.Create(url) as FtpWebRequest;
                if (!string.IsNullOrEmpty(user))
                    request.Credentials = new NetworkCredential(user, pwd);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                result.Add("RequestMethod", request.Method);
                FtpWebResponse response = request.GetResponse() as FtpWebResponse;
                result.Add("ResponseStatusCode", (int)response.StatusCode);
                result.Add("ResponseStatusCodeString", response.StatusCode.ToString());
                result.Add("ResponseStatusDescription", response.StatusDescription);
                result.Add("ResponseText", response.GetResponseStream().GetString());
            }
            catch (Exception ex)
            {
                result.Add("Error", ex.Message);
            }
            return result;
        }
    }
}
