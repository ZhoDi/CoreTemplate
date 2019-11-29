using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CommonUtils
{
    public static class JsonUtil
    {
        #region 转换
        /// <summary>
        /// 类序列化为json
        /// </summary>
        public static string Serialize(object obj, bool min = false)
        {
            if (obj == null)
                return null;

            if (min)
                return JsonConvert.SerializeObject(obj);
            return JToken.FromObject(obj).ToString();
        }

        /// <summary>
        /// 将实体转换为Json
        /// </summary>
        public static string ToJson(this object obj, bool min = false)
        {
            return Serialize(obj, min);
        }

        /// <summary>
        /// 类序列化为json再转为uft8.bytes
        /// </summary>
        public static byte[] ToJsonBytes(this object obj)
        {
            return obj.ToJson(true).ToBytes();
        }

        /// <summary>
        /// json反序列化为类
        /// </summary>
        public static T Deserialize<T>(string json, T defaultValue = default(T))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return defaultValue;
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                LogUtil.Log(string.Format("json deserialize text:\r\n{0}\r\njson deserialize error:\r\n{1}\r\njson deserialize trace:\r\n{2}", json, ex.Message, new StackTrace(true)));
                return defaultValue;
            }
        }

        /// <summary>
        /// json反序列化为类
        /// </summary>
        public static T DeserializeFromFile<T>(string path, T defaultValue = default(T))
        {
            string json = FileUtil.Read(path);
            return Deserialize(json, defaultValue);
        }

        /// <summary>
        /// json反序列化为类
        /// </summary>
        public static T DeserializeFromBytes<T>(byte[] bytes, T defaultValue = default(T))
        {
            string json = bytes.ToText();
            return Deserialize<T>(json, defaultValue);
        }

        /// <summary>
        /// 获取JObject
        /// </summary>
        public static JObject ParseJObject(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return null;
                return JObject.Parse(json);
            }
            catch (Exception ex)
            {
                LogUtil.Log(string.Format("json parse text:\r\n{0}\r\njson parse error:\r\n{1}\r\njson parse trace:\r\n{2}", json, ex.Message, new StackTrace(true)));
                return null;
            }
        }

        /// <summary>
        /// 获取JObject
        /// </summary>
        public static JObject ParseJObjectFromFile(string path)
        {
            return ParseJObject(FileUtil.Read(path));
        }

        /// <summary>
        /// 获取JObject
        /// </summary>
        public static JObject ToJObject(this object obj)
        {
            return JObject.FromObject(obj);
        }

        /// <summary>
        /// 获取JArray
        /// </summary>
        public static JArray ParseJArray(string json)
        {
            try
            {
                return JArray.Parse(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("JsonUtil.ParseJArray Error:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取JArray
        /// </summary>
        public static JArray ParseJArrayFromFile(string path)
        {
            return ParseJArray(FileUtil.Read(path));
        }

        /// <summary>
        /// 获取JArray
        /// </summary>
        public static JArray ToJArray(this object obj)
        {
            return JArray.FromObject(obj);
        }

        /// <summary>
        /// 获取JToken
        /// </summary>
        public static JToken ParseJToken(string json)
        {
            try
            {
                return JToken.Parse(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("JsonUtil.ParseJToken Error:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取JToken
        /// </summary>
        public static JToken ParseJTokenFromFile(string path)
        {
            return JToken.Parse(FileUtil.Read(path));
        }

        /// <summary>
        /// 获取JToken
        /// </summary>
        public static JToken ToJToken(this object obj)
        {
            return JToken.FromObject(obj);
        }
        #endregion

        #region 设值取值
        /// <summary>
        /// 设值
        /// </summary>
        public static void Put(this JToken jObject, string key, object value)
        {
            jObject[key] = JToken.FromObject(value);
        }

        /// <summary>
        /// 设值
        /// </summary>
        public static void Set(this JToken jObject, string key, object value)
        {
            jObject.Put(key, value);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static T Get<T>(this JToken jObject, string key, T defaultValue = default(T), bool log = true)
        {
            if (jObject[key] != null)
                return jObject.Value<T>(key);
            if (log)
                LogUtil.Log(string.Format("JToken尝试获取不存在的Key：{0}\r\n{1}", key, new StackTrace(true)));
            return defaultValue;
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static T GetValue<T>(this JToken jObject, string key, T defaultValue = default(T), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static string GetString(this JToken jObject, string key, string defaultValue = default(string), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static string[] GetLines(this JToken jObject, string key, string[] defaultValue = default(string[]), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static bool GetBool(this JToken jObject, string key, bool defaultValue = default(bool), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static DateTime GetTime(this JToken jObject, string key, DateTime defaultValue = default(DateTime), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static int GetInt(this JToken jObject, string key, int defaultValue = default(int), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static float GetFloat(this JToken jObject, string key, float defaultValue = default(float), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 取值
        /// </summary>
        public static object GetObject(this JToken jObject, string key, object defaultValue = default(object), bool log = true)
        {
            return jObject.Get(key, defaultValue, log);
        }

        /// <summary>
        /// 获取长度
        /// </summary>
        public static int Size(this JToken jObject)
        {
            return ((JContainer)jObject).Count;
        }

        /// <summary>
        /// 获取字符串内容
        /// </summary>
        public static string ToNormalString(this JToken jObject)
        {
            return jObject.ToObject<string>();
        }
        #endregion

        /// <summary>
        /// 用于有部分相同结构的类的深复制
        /// </summary>
        public static T Copy<T>(object obj)
        {
            return Deserialize<T>(obj.ToJson(true));
        }
    }
}
