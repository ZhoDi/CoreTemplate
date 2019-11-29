using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 反射处理
    /// </summary>
    public static class ReflectionUtil
    {
        /// <summary>
        /// 应用名
        /// </summary>
        public static string AppName
        {
            get
            {
                return AppDomain.CurrentDomain.FriendlyName;
            }
        }

        /// <summary>
        /// 方法名
        /// </summary>
        public static string MethodName
        {
            get
            {
                MethodBase methodBase = new StackTrace().GetFrame(1).GetMethod();
                string className = methodBase.ReflectedType.FullName;
                int index = className.IndexOf('+');
                if (index != -1)
                    className = className.Substring(0, index);
                return className + "." + methodBase.Name;
            }
        }

        /// <summary>
        /// 类名
        /// </summary>
        public static string ClassName
        {
            get
            {
                string name = new StackTrace().GetFrame(1).GetMethod().ReflectedType.FullName;
                int index = name.IndexOf('+');
                if (index != -1)
                    name = name.Substring(0, index);
                return name;
            }
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        public static string Namespace
        {
            get
            {
                return new StackTrace().GetFrame(1).GetMethod().ReflectedType.Namespace;
            }
        }

        /// <summary>
        /// 类名目录下的文件
        /// </summary>
        public static string FilePath(string fileName)
        {
            string name = new StackTrace().GetFrame(1).GetMethod().ReflectedType.FullName;
            int index = name.IndexOf('+');
            if (index != -1)
                name = name.Substring(0, index);
            return Path.Combine(name, fileName);
        }


        /// <summary>
        /// 获取程序集
        /// </summary>
        public static Assembly AssemblyLoad(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return Assembly.Load(bytes);
        }

        /// <summary>
        /// 获取第一个公共类
        /// </summary>
        public static string AssemblyClass(string path)
        {
            var assembly = AssemblyLoad(path);
            var types = assembly.GetExportedTypes();
            if (types.Length == 0)
                return null;
            return types[0].FullName;
        }

        /// <summary>
        /// 获取第一个公共类
        /// </summary>
        public static string Class(this Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            if (types.Length == 0)
                return null;
            return types[0].FullName;
        }

        /// <summary>
        /// 获取类列表
        /// </summary>
        public static string[] AssemblyClasses(string path)
        {
            var assembly = AssemblyLoad(path);
            var types = assembly.GetExportedTypes();
            return types.Select(m => m.FullName).ToArray();
        }
    }
}
