using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemplate.Application.Helper
{
    public class Appsettings
    {
        private static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 传递根目录初始化Appsettings
        /// Directory.GetCurrentDirectory()
        /// </summary>
        /// <param name="basePath"></param>
        public Appsettings(string basePath)
        {
            Configuration = new ConfigurationBuilder()
               //将配置文件的数据加载到内存中
               .AddInMemoryCollection()
               //指定配置文件所在的目录
               .SetBasePath(basePath)
               .AddJsonFile("appsettings.json", optional : false, reloadOnChange: true)//这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
               .Build();
        }
        public Appsettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string App(params string[] sections)
        {
            try
            {
                if (sections.Any())
                {
                    return Configuration[string.Join(":", sections)];
                }
            }
            catch (Exception) { Console.WriteLine("Appsetting.json读取报错"); }
            return "";
        }

        /// <summary>
        /// 递归获取配置信息数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static List<T> App<T>(params string[] sections)
        {
            List<T> list = new List<T>();
            // 引用 Microsoft.Extensions.Configuration.Binder 包
            Configuration.Bind(string.Join(":", sections), list);
            return list;
        }
    }
}
