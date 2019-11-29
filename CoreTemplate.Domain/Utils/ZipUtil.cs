using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// zip工具
    /// TODO:多个文件或stream，生成压缩包
    /// </summary>
    public static class ZipUtil
    {
        /// <summary>
        /// 获取压缩包中的文件
        /// </summary>
        public static ReadOnlyCollection<ZipArchiveEntry> GetFiles(string path, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            var zipFile = ZipFile.Open(path, ZipArchiveMode.Read, encoding);
            return zipFile.Entries;
        }

        /// <summary>
        /// 获取压缩包中的文件
        /// </summary>
        public static ReadOnlyCollection<ZipArchiveEntry> GetFiles(Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            ZipArchive zipFile = new ZipArchive(stream, ZipArchiveMode.Read, false, encoding);
            return zipFile.Entries;
        }
    }
}
