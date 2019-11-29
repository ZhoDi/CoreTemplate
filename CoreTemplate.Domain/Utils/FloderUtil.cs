using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 文件夹操作
    /// </summary>
    public static class FloderUtil
    {
        /// <summary>
        /// 打开文件夹
        /// </summary>
        public static void Open(string path)
        {
            Process.Start(path);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        public static void Create(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 获取文件夹信息
        /// </summary>
        public static DirectoryInfo Info(string path)
        {
            return new DirectoryInfo(path);
        }

        /// <summary>
        /// 文件夹中的文件最后修改时间
        /// </summary>
        public static DateTime LastFileWriteTime(this DirectoryInfo floder)
        {
            DateTime time = floder.LastWriteTime;
            foreach (var file in floder.GetFiles())
                if (file.LastWriteTime > time)
                    time = file.LastWriteTime;
            foreach (var subFloder in floder.GetDirectories())
            {
                var subTime = subFloder.LastFileWriteTime();
                if (subTime > time)
                    time = subTime;
            }
            return time;
        }

        /// <summary>
        /// 覆盖文件夹
        /// </summary>
        public static void MoveTo(string src, string dest)
        {
            Directory.Move(src, dest);
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        public static void CopyTo(this DirectoryInfo src, DirectoryInfo dest)
        {
            dest.Create();
            foreach (var file in src.GetFiles())
                file.CopyTo(Path.Combine(dest.FullName, file.Name), true);
            foreach (var floder in src.GetDirectories())
                floder.CopyTo(new DirectoryInfo(Path.Combine(dest.FullName, floder.Name)));
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        public static void CopyTo(string src, string dest)
        {
            CopyTo(new DirectoryInfo(src), new DirectoryInfo(dest));
        }
    }
}
