using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommonUtils
{
    /// <summary>
    /// 日志工具
    /// sub代表分支
    /// 相对路径在发布到服务、IIS时会出错，所以此处使用绝对路径
    /// </summary>
    public class LogUtil
    {
        /// <summary>
        /// 未定义 代替null，因为用到了hasmap和javascript
        /// </summary>
        private const string mUndefined = "undefined";
        static ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// log路径
        /// </summary>
        private static string LogPath(string sub = mUndefined)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            if (string.IsNullOrWhiteSpace(sub) || sub == mUndefined)
                return PathUtil.Get(string.Format("log/log-{0}.log", date));
            else
                return PathUtil.Get(string.Format("log/sub/{0}/log-{1}.log", sub, date));
        }

        /// <summary>
        /// error路径
        /// </summary>
        private static string ErrorPath(string sub = mUndefined)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            if (string.IsNullOrWhiteSpace(sub) || sub == mUndefined)
                return PathUtil.Get(string.Format("error/error-{0}.log", date));
            else
                return PathUtil.Get(string.Format("error/sub/{0}/error-{1}.log", sub, date));
        }

        /// <summary>
        /// 格式化log
        /// </summary>
        private static string MsgFormat(object msg)
        {
            return string.Format("[{0}]\r\n{1}\r\n\r\n", DateTime.Now, msg);
        }

        /// <summary>
        /// 打log
        /// </summary>
        public static void Log(object msg, string sub = mUndefined)
        {
            Console.WriteLine(msg);
            lockSlim.EnterWriteLock();//打开写操作锁
            try
            {
                FileUtil.Append(LogPath(sub), MsgFormat(msg));
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// 异常信息缓存
        /// </summary>
        private static MapKeyString mErrorMessages = new MapKeyString();

        /// <summary>
        /// 记录异常
        /// </summary>
        public static void Log(Exception ex, string sub = mUndefined)
        {
            Console.WriteLine(ex.Message);

            string path = ErrorPath(sub);

            //去重操作
            if (ex.Message == mErrorMessages.Get("error-" + sub, null, false))
            {
                //不重复记录 节省资源
                Console.WriteLine(ex);
                return;
            }

            mErrorMessages.Set("error-" + sub, ex.Message);
            FileUtil.Append(path, MsgFormat(ex));
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        public static void Error(Exception ex, string sub = mUndefined)
        {
            Log(ex, sub);
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        public static void Error(string msg, Exception ex, string sub = mUndefined)
        {
            Log(new Exception(msg + ":" + ex.Message, ex), sub);
        }

        /// <summary>
        /// 记录异常
        /// </summary>
        public static void Log(string msg, Exception ex, string sub = mUndefined)
        {
            Error(msg, ex, sub);
        }

        /// <summary>
        /// 日志内容
        /// </summary>
        public static string GetLog(string sub = mUndefined)
        {
            return FileUtil.Read(LogPath(sub));
        }

        /// <summary>
        /// 异常内容
        /// </summary>
        public static string GetError(string sub = mUndefined)
        {
            return FileUtil.Read(ErrorPath(sub));
        }

        /// <summary>
        /// 清理日志和异常
        /// </summary> 
        public static void Clear(string sub = mUndefined)
        {
            FileUtil.Delete(LogPath(sub));
            FileUtil.Delete(ErrorPath(sub));
        }
    }
}
