using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CommonUtils
{
    /// <summary>
    /// 比特流，字节流，内存流处理（内存流优先，字节流容易崩）
    /// </summary>
    public static class BinaryUtil
    {
        /// <summary>
        /// 判断值是否相等
        /// </summary>
        public static bool EqualValue(this byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
                return false;
            for (int index = 0; index < left.Length; index++)
                if (left[index] != right[index])
                    return false;
            return true;
        }

        /// <summary>
        /// 数组截取,非引用,需赋值
        /// </summary>
        public static byte[] Cut(this byte[] left, long length)
        {
            byte[] right = new byte[length];
            Array.Copy(left, right, length);
            return right;
        }

        /// <summary>
        /// 数组截取,非引用,需赋值
        /// </summary>
        public static byte[] CutAt(this byte[] left, long index)
        {
            byte[] right = new byte[left.Length - index];
            Array.Copy(left, index, right, 0, left.Length - index);
            return right;
        }

        /// <summary>
        /// 字节流拼接
        /// </summary>
        public static byte[] Append(this byte[] left, byte[] right)
        {
            var list = new List<byte>();
            list.AddRange(left);
            list.AddRange(right);
            return list.ToArray();
        }

        /// <summary>
        /// Seek，内存指针回到起点，用于指针在末尾内存流需要继续使用的情况
        /// </summary>
        public static void Seek(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// 状态信息
        /// </summary>
        public static void StatePrint(this Stream stream)
        {
            JObject state = new JObject();
            state.Add("stream.CanTimeout", stream.CanTimeout);
            state.Add("stream.CanRead", stream.CanRead);
            state.Add("stream.CanWrite", stream.CanWrite);
            state.Add("stream.CanSeek", stream.CanSeek);
            if (stream.CanSeek)
            {
                state.Add("stream.Position", stream.Position);
                state.Add("stream.Length", stream.Length);
            }
            Console.WriteLine(state);
        }

        /// <summary>
        /// 序列化 typeof(value) 必须声明[Serializable]
        /// </summary>
        public static Stream Serialize(object value)
        {
            MemoryStream memory = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memory, value);
            //不可省略，不可用Flush代替
            memory.Seek();
            return memory;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public static object Deserialize(Stream stream)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            var value = binaryFormatter.Deserialize(stream);
            stream.Close();
            return value;
        }

        #region 解析
        /// <summary>
        /// 字符串
        /// </summary>
        public static string GetString(this Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            StreamReader reader = new StreamReader(stream, encoding);
            string text = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return text;
        }

        /// <summary>
        /// 文本行
        /// </summary>
        public static List<string> GetLines(this Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            List<string> lines = new List<string>();
            StreamReader reader = new StreamReader(stream, encoding);
            while (!reader.EndOfStream)
                lines.Add(reader.ReadLine());
            reader.Close();
            stream.Close();
            return lines;
        }

        /// <summary>
        /// 字符串
        /// </summary>
        public static string GetString(this byte[] bytes, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        public static void CreateFile(this Stream stream, string path)
        {
            FileUtil.Create(path, stream);
        }
        #endregion

        #region 转换
        /// <summary>
        /// 将bytes转换成bytes字符串
        /// </summary>
        public static string Join(byte[] bytes)
        {
            return bytes.Join(',');
        }

        /// <summary>
        /// 将bytes字符串转换成bytes
        /// </summary>
        public static byte[] Split(string bytesString)
        {
            string[] byteStrings = bytesString.Split(',');
            byte[] bytes = new byte[byteStrings.Length];
            for (int index = 0; index < bytes.Length; index++)
            {
                int byteInt = Convert.ToInt32(byteStrings[index]);
                bytes[index] = (byte)byteInt;
            }
            return bytes;
        }

        /// <summary>
        /// 将string转成byte[]
        /// </summary>
        public static byte[] ToBytes(this string text, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            //Encoding.UTF8没有加Bom，这里判断Encodings.UTF8Bom强制加上了
            if (encoding == Encodings.UTF8Bom)
                return Encodings.UTF8BomBytes.Append(encoding.GetBytes(text));
            return encoding.GetBytes(text);
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public static string ToText(this byte[] bytes, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            //这里判断Encodings.UTF8Bom强制解除bom
            if (encoding == Encodings.UTF8Bom && bytes.Cut(3).EqualValue(Encodings.UTF8BomBytes))
                bytes = bytes.CutAt(3);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将string转成sbyte[]
        /// </summary>
        public static sbyte[] ToSbytes(this string text, Encoding encoding = null)
        {
            byte[] buffer = text.ToBytes(encoding);
            sbyte[] sbuffer = new sbyte[buffer.Length];
            for (int index = 0; index < buffer.Length; index++)
                sbuffer[index] = (sbyte)buffer[index];
            return sbuffer;
        }

        /// <summary>
        /// sbyte[]转成Text
        /// </summary>
        public static string ToText(this sbyte[] sbytes, Encoding encoding = null)
        {
            byte[] buffer = new byte[sbytes.Length];
            for (int index = 0; index < sbytes.Length; index++)
                buffer[index] = (byte)sbytes[index];
            if (encoding == null)
                encoding = Encoding.UTF8;
            string text = encoding.GetString(buffer);
            int length = text.IndexOf("\0");
            if (length > 0)
                text = text.Substring(0, length);
            return text;
        }

        /// <summary>
        /// 内存流
        /// </summary>
        public static Stream ToStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// 字节流
        /// </summary>
        public static byte[] ToBytes(this Stream stream)
        {
            MemoryStream memory = new MemoryStream();
            memory.Load(stream);
            var bytes = memory.ToArray();
            memory.Close();
            return bytes;
        }
        #endregion

        #region 内存流装载
        /// <summary>
        /// 装载字符串
        /// </summary>
        public static void Load(this Stream stream, string text, Encoding encoding = null, bool end = false)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            StreamWriter writer = new StreamWriter(stream, encoding);
            writer.Write(text);

            if (end)
                writer.Close();
            else
                //如果流继续使用，此处必须flush,因为writer不能close，无法触发内存输出
                writer.Flush();
        }

        /// <summary>
        /// 装载内存流
        /// </summary>
        public static void Load(this Stream stream, Stream aim, bool end = false)
        {
            aim.CopyTo(stream);
            aim.Close();
            if (end)
                stream.Close();
        }

        /// <summary>
        /// 装载内存流
        /// </summary>
        public static void Load(this Stream stream, byte[] bytes, bool end = false)
        {
            stream.Write(bytes, 0, bytes.Length);
            if (end)
                stream.Close();
        }

        /// <summary>
        /// 装载文件
        /// </summary>
        public static void LoadFile(this Stream stream, string path, bool end = false)
        {
            stream.Load(File.OpenRead(path), end);
        }
        #endregion

        #region 压缩/解压 GZipStream

        //GZipStream与初始化的Stream会关联Close,所以MemoryStream不用关，外部关闭GZipStream即可
        //GZipStream的CanSeek为False，不支持Position和Length

        /// <summary>
        /// 解压
        /// </summary>
        public static byte[] Unzip(this byte[] bytes)
        {
            return new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress).ToBytes();
        }

        /// <summary>
        /// 解压(此方法代替Unzip().ToStream(),少一步字节遍历)
        /// </summary>
        public static Stream UnzipStream(this byte[] bytes)
        {
            return new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress);
        }

        /// <summary>
        /// 解压
        /// </summary>
        public static Stream Unzip(this Stream stream)
        {
            return new GZipStream(stream, CompressionMode.Decompress);
        }

        /// <summary>
        /// 解压
        /// </summary>
        public static byte[] UnzipBytes(this Stream stream)
        {
            return new GZipStream(stream, CompressionMode.Decompress).ToBytes();
        }


        /// <summary>
        /// 获取解压后的UTF8编码的字符串
        /// </summary>
        public static string UnzipString(this byte[] bytes, Encoding encoding = null)
        {
            return new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress).GetString(encoding);
        }

        /// <summary>
        /// 获取解压后的UTF8编码的字符串
        /// </summary>
        public static string UnzipString(this Stream stream, Encoding encoding = null)
        {
            return new GZipStream(stream, CompressionMode.Decompress).GetString(encoding);
        }
        #endregion
    }
}
