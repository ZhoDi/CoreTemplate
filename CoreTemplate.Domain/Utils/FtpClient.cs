using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// ftp链接
    /// </summary>
    public class FtpClient
    {
        /// <summary>
        /// 基础url
        /// </summary>
        private string mBaseUrl { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        private string mUserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        private string mPassword { get; set; }

        /// <summary>
        /// 初始化
        /// </summary> 
        public FtpClient(string url, string user = null, string pwd = null)
        {
            mBaseUrl = url;
            mUserName = user;
            mPassword = pwd;
        }

        /// <summary>
        /// 文件列表
        /// </summary>
        public string[] FileNames(string path = null, Encoding encoding = null)
        {
            return FtpUtil.FileNames(UrlUtil.Combine(mBaseUrl, path), mUserName, mPassword, encoding);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        public bool Exists(string path = null)
        {
            return FtpUtil.Exists(UrlUtil.Combine(mBaseUrl, path), mUserName, mPassword);
        }


        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        public void CreateDir(string path = null)
        {
            FtpUtil.MakeDirectory(UrlUtil.Combine(mBaseUrl, path), mUserName, mPassword);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public string Download(string path, string savePath)
        {
            return FtpUtil.Download(UrlUtil.Combine(mBaseUrl, path), savePath, mUserName, mPassword);
        }

        /// <summary>
        /// 下载文本
        /// </summary>
        public string DownloadText(string path = null)
        {
            return FtpUtil.DownloadText(UrlUtil.Combine(mBaseUrl, path), mUserName, mPassword);
        }

        /// <summary>
        /// 下载数据流
        /// </summary>
        public Stream DownloadStream(string path = null)
        {
            return FtpUtil.DownloadStream(UrlUtil.Combine(mBaseUrl, path), mUserName, mPassword);
        }

        /// <summary>
        /// 上传
        /// </summary>
        public string Upload(string path, string file)
        {
            return FtpUtil.Upload(UrlUtil.Combine(mBaseUrl, path), file, mUserName, mPassword);
        }

        /// <summary>
        /// 上传数据流
        /// </summary>
        public string Upload(string path, Stream stream)
        {
            return FtpUtil.Upload(UrlUtil.Combine(mBaseUrl, path), stream, mUserName, mPassword);
        }

        /// <summary>
        /// 上传字节流
        /// </summary>
        public string Upload(string path, byte[] bytes)
        {
            return FtpUtil.Upload(UrlUtil.Combine(mBaseUrl, path), bytes, mUserName, mPassword);
        }

        /// <summary>
        /// 上传文本
        /// </summary>
        public string UploadText(string path, string text)
        {
            return FtpUtil.UploadText(UrlUtil.Combine(mBaseUrl, path), text, mUserName, mPassword);
        }

        /// <summary>
        /// 附加文本
        /// </summary>
        public string AppendText(string path, string text)
        {
            return FtpUtil.AppendText(UrlUtil.Combine(mBaseUrl, path), text, mUserName, mPassword);
        }

        /// <summary>
        /// 附加文本行
        /// </summary>
        public string AppendLine(string path, string text)
        {
            return FtpUtil.AppendLine(UrlUtil.Combine(mBaseUrl, path), text, mUserName, mPassword);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public string Delete(string path = null)
        {
            return FtpUtil.Delete(UrlUtil.Combine(mBaseUrl, path), mUserName, mPassword);
        }

        /// <summary>
        /// 重命名
        /// </summary>
        public string Rename(string path, string name)
        {
            return FtpUtil.Rename(UrlUtil.Combine(mBaseUrl, path), name, mUserName, mPassword);
        }
    }
}
