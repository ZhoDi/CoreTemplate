using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace CommonUtils
{
    /// <summary>
    /// 配置文件
    /// sub代表分支
    /// 相对路径在发布到服务、IIS时会出错，所以此处使用绝对路径
    /// </summary>
    public static class ConfigUtil
    {
        #region 存储位置

        /// <summary>
        /// Config路径
        /// </summary>
        public static string Floder(string sub = null)
        {
            if (string.IsNullOrWhiteSpace(sub) || sub == "undefined")
                return PathUtil.Get("Config");
            else
                return PathUtil.Get("Config/sub/" + sub);
        }

        /// <summary>
        /// Config路径
        /// </summary>
        public static string Path(string file, string sub = null)
        {
            return PathUtil.Combine(Floder(sub), file);
        }

        /// <summary>
        /// Config路径
        /// </summary>
        public static string[] Files(string sub = null)
        {
            DirectoryInfo dir = new DirectoryInfo(Floder(sub));
            return dir.GetFiles().Select(m => m.Name).ToArray();
        }

        #endregion

        #region 配置信息

        /// <summary>
        /// 配置信息
        /// </summary>
        public static void Save(string fileName, string text, string sub = null)
        {
            var path = Path(fileName, sub);
            FileUtil.Save(path, text);
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public static string Text(string fileName, string sub = null)
        {
            var path = Path(fileName, sub);
            return FileUtil.Read(path);
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public static string[] Lines(string fileName, string sub = null)
        {
            var path = Path(fileName, sub);
            return FileUtil.ReadLines(path);
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public static JObject Json(string fileName, string sub = null)
        {
            return JsonUtil.ParseJObject(Text(fileName, sub));
        }

        #endregion

        #region 默认配置

        /// <summary>
        /// 默认配置文件
        /// </summary>
        private const string mDefaultFile = "settings.json";

        /// <summary>
        /// 默认配置文件
        /// </summary>
        public static string DefaultPath { get; } = Path(mDefaultFile);

        /// <summary>
        /// 默认配置
        /// </summary>
        public static JObject Default { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        static ConfigUtil()
        {
            var config = Json(mDefaultFile);
            if (config == null)
                config = new JObject();
            Default = config;
        }

        /// <summary>
        /// 提醒配置可抛出此报错
        /// </summary>
        public static Exception Exception { get; } = new Exception("配置缺失！！！\r\n" + DefaultPath);

        /// <summary>
        /// 赋值
        /// </summary>
        public static void Set(string key, object value)
        {
            Default.Set(key, value);
            Save(mDefaultFile, Default.ToJson());
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static T Get<T>(string key, T defaultValue = default(T))
        {
            return Default.Get(key, defaultValue);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            return Default.Get(key, defaultValue);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static string GetString(string key, string defaultValue = default(string))
        {
            return Default.Get(key, defaultValue);
        }

        #endregion
    }
}
