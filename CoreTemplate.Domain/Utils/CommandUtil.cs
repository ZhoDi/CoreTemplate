using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 命令行
    /// </summary>
    public static class CommandUtil
    {
        /// <summary>
        /// 命令
        /// </summary>
        public static void CreateRun(string namespace_)
        {
            var cmd = string.Format("dotnet {0}.dll", namespace_);
            FileUtil.Save("run.cmd", cmd, Encodings.UTF8NoBom);
        }

        /// <summary>
        /// 命令
        /// </summary>
        public static void Command(string cmd)
        {
            var info = new ProcessStartInfo(cmd);
            info.RedirectStandardOutput = true;
            var process = Process.Start(info);
            var msg = process.StandardOutput.ReadToEnd();
            process.StandardOutput.Close();
            Console.WriteLine(msg);
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        public static void Open(string path)
        {
            string cmd = string.Format("start \"\" \"{0}\"", path);
            //Console.WriteLine(cmd);
            Command(cmd);
        }
    }
}
