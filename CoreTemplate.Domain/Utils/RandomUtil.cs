using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 随机工具
    /// </summary>
    public class RandomUtil
    {
        /// <summary>
        /// 公用随机器
        /// </summary>
        public static Random Random { private set; get; }

        /// <summary>
        /// 初始化
        /// </summary>
        static RandomUtil()
        {
            Random = new Random();
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <returns></returns>
        public static int Intger(int max = 100)
        {
            return Random.Next(max);
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <returns></returns>
        public static int Intger(int min, int max)
        {
            return Random.Next(min, max);
        }

        /// <summary>
        /// 随机小数
        /// </summary>
        /// <returns></returns>
        public static float Float(int max = 100, int floatSize = 2)
        {
            int baseMax = (int)Math.Pow(10, floatSize);
            float value = (float)Random.Next(baseMax) / baseMax;
            return Random.Next(max) + value;
        }

        /// <summary>
        /// 字符池
        /// </summary>
        private const string mCharPool = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string String(int length = 10)
        {
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < length; index++)
                sb.Append(mCharPool[Random.Next(mCharPool.Length)]);
            return sb.ToString();
        }

        /// <summary>
        /// 每分钟随机累加，当前时间的数量
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="daySum">一天结束达到总数</param>
        /// <param name="refrshMinute">刷新间隔</param>
        public static int MintueCount(DateTime time, int daySum, int refrshMinute)
        {
            //数据存储路径
            string key = string.Format("random-minute-{0}-{1}", daySum, refrshMinute);
            //查找缓存
            var sums = CacheUtil.GetFromFile<List<int>>(key);
            if (sums == null)
            {
                //一天有60*24分钟，分割间隔refrshMinute
                var splitCount = 60 * 24 / refrshMinute;
                //每分钟的平均数量
                var avg = daySum / splitCount;
                //振幅30%
                var value = (int)(avg * 0.3);
                if (value < 3)
                    throw new Exception("RandomUtil.MintueCount：拆分数据太小！");
                //记录每分钟应该达到的数量
                sums = new List<int>();
                int sum = 0;
                for (int index = 0; index < splitCount; index++)
                {
                    sum += avg + Intger(-value, value);
                    sums.Add(sum);
                }
                //写入缓存
                CacheUtil.SaveWithFile(key, sums);
            }
            //获取当前分钟序列
            var sunIndex = time.MinuteIndex() / refrshMinute;
            return sums[sunIndex];
        }
    }
}
