using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileUtil
    {
        #region 属性
        /// <summary>
        /// 获取文件名
        /// </summary>
        public static string GetName(string path)
        {
            path = path.Replace('\\', '/');
            return path.Substring(path.LastIndexOf('/') + 1);
        }

        /// <summary>
        /// 扩展名
        /// </summary>
        public static string Extension(string name)
        {
            return Path.GetExtension(name);
        }

        /// <summary>
        /// 大小
        /// </summary>
        public static long Length(string path)
        {
            return new FileInfo(path).Length;
        }

        /// <summary>
        /// 大小
        /// </summary>
        public static string Size(string path)
        {
            return Size(Length(path));
        }

        /// <summary>
        /// 大小
        /// </summary>
        public static string Size(long length)
        {
            if (length < 1024)
                return length + "B";

            decimal size = (decimal)length / 1024;
            if (size < 1024)
                return size.ToString("0.00") + "KB";

            size /= 1024;
            if (size < 1024)
                return size.ToString("0.00") + "MB";

            size /= 1024;
            if (size < 1024)
                return size.ToString("0.00") + "GB";

            size /= 1024;
            return size.ToString("0.00") + "TB";
        }
        #endregion

        #region 行为

        /// <summary>
        /// 复制
        /// </summary>
        public static void Copy(string src, string dest)
        {
            File.Copy(src, dest, true);
        }

        /// <summary>
        /// 获取Base64编码
        /// </summary>
        public static string GetBase64(string path)
        {
            return Convert.ToBase64String(GetBytes(path));
        }

        /// <summary>
        /// 获取同目录文件
        /// </summary>
        public static string GetBrother(string path, string name)
        {
            return PathUtil.Combine(GetFloder(path), name);
        }

        /// <summary>
        /// 获取文件夹
        /// </summary>
        public static string GetFloder(string path)
        {
            return new FileInfo(path).DirectoryName;
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        public static void Start(string path)
        {
            Process.Start(path);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        public static void Open(string path)
        {
            Start(path);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        public static void Run(string path)
        {
            Start(path);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        public static void Exe(string path)
        {
            Start(path);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        public static void Excute(string path)
        {
            Start(path);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 文件删除
        /// </summary> 
        public static void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        public static void CreateFloder(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        public static void Create(string path, Stream stream)
        {
            try
            {
                new FileInfo(path).Directory.Create();
                File.Create(path).Load(stream, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save(string path, Stream stream)
        {
            Create(path, stream);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        /// <summary>
        /// 文件流
        /// </summary>
        public static Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        /// <summary>
        /// 文件流
        /// </summary>
        public static Stream GetStream(string path)
        {
            return OpenRead(path);
        }

        /// <summary>
        /// 文件流
        /// </summary>
        public static byte[] GetBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        #endregion

        #region 文本

        /// <summary>
        /// 创建文本文件,csv使用utf8bom编码,cmd使用utf8nobom编码
        /// </summary>
        public static void Write(string path, string text, Encoding encoding = null)
        {
            try
            {
                new FileInfo(path).Directory.Create();

                if (encoding == null)
                    encoding = Encoding.UTF8;
                File.WriteAllText(path, text, encoding);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save(string path, string text, Encoding encoding = null)
        {
            Write(path, text, encoding);
        }

        /// <summary>
        /// 创建文本文件
        /// </summary>
        public static void Write(string path, IEnumerable<string> lines, Encoding encoding = null, bool endLine = true, string crlf = "\r\n")
        {
            try
            {
                new FileInfo(path).Directory.Create();

                if (encoding == null)
                    encoding = Encoding.UTF8;

                if (endLine)
                    File.WriteAllLines(path, lines, encoding);
                else
                    File.WriteAllText(path, lines.Join(crlf), encoding);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 创建文本文件
        /// </summary>
        public static void Save(string path, IEnumerable<string> lines, Encoding encoding = null, bool endLine = true, string crlf = "\r\n")
        {
            Write(path, lines, encoding, endLine, crlf);
        }

        /// <summary>
        /// 追加/创建文本文件
        /// </summary>
        public static void Append(string path, string text, Encoding encoding = null)
        {
            try
            {
                new FileInfo(path).Directory.Create();

                if (encoding == null)
                    encoding = Encoding.UTF8;
                File.AppendAllText(path, text, encoding);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 追加/创建文本文件
        /// </summary>
        public static void AppendLine(string path, string line, Encoding encoding = null)
        {
            try
            {
                new FileInfo(path).Directory.Create();

                if (encoding == null)
                    encoding = Encoding.UTF8;
                File.AppendAllText(path, line + "\r\n", encoding);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 追加/创建文本文件
        /// </summary>
        public static void AppendLines(string path, IEnumerable<string> lines, Encoding encoding = null)
        {
            try
            {
                new FileInfo(path).Directory.Create();

                if (encoding == null)
                    encoding = Encoding.UTF8;
                File.AppendAllLines(path, lines, encoding);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        public static string Read(string path, Encoding encoding = null)
        {
            try
            {
                if (encoding == null)
                    encoding = Encoding.UTF8;
                return File.ReadAllText(path, encoding);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        public static string GetText(string path, Encoding encoding = null)
        {
            return Read(path, encoding);
        }

        /// <summary>
        /// 读取文本行
        /// </summary>
        public static string[] ReadLines(string path, Encoding encoding = null)
        {
            try
            {
                if (encoding == null)
                    encoding = Encoding.UTF8;
                return File.ReadAllLines(path, encoding);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 读取文本行
        /// </summary>
        public static string[] GetLines(string path, Encoding encoding = null)
        {
            return ReadLines(path, encoding);
        }

        #endregion
    }
}
