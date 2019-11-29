using System;
using System.IO;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 路径处理
    /// </summary>
    public static class PathUtil
    {
        /// <summary>
        /// 更改后缀名
        /// </summary>
        public static string ChangeExtension(string path, string extension)
        {
            var index = path.LastIndexOf('.');
            if (index < 0)
                return path + extension;
            return path.Substring(0, index) + extension;
        }

        /// <summary>
        /// 名字附加
        /// </summary>
        public static string NameAppend(string path, string append)
        {
            var index = path.LastIndexOf('.');
            if (index < 0)
                return path + append;
            return path.Substring(0, index) + append + path.Substring(index);
        }

        /// <summary>
        /// 分析相对路径
        /// ../../处理
        /// System.IO.Path.Combine(paths)如果right是/开头，那么left就会丢失，../也不会处理
        /// </summary>
        private static string Get(string right, out int leftUp)
        {
            var rightResult = right.Replace('\\', '/');
            leftUp = rightResult.CountOf("../");
            rightResult = rightResult.Remove("../", "./");
            return rightResult;
        }

        /// <summary>
        /// 通用 带/
        /// </summary>
        public static string Get(string right = null)
        {
            if (string.IsNullOrWhiteSpace(right))
                return AppBase;

            right = Get(right, out int leftUp);
            var leftFloder = new DirectoryInfo(AppBase);
            while (leftUp-- > 0)
                leftFloder = leftFloder.Parent;

            return Combine(leftFloder.FullName, right);
        }

        /// <summary>
        /// 通过磁盘路径，获取虚拟路径
        /// </summary>
        public static string Virtual(string path)
        {
            string rootPath = AppBase.Replace('\\', '/');
            path = path.Replace('\\', '/');
            return "/" + path.Replace(rootPath, "");
        }

        /// <summary>
        /// 路径合并
        /// </summary>
        public static string Combine(this string left, params string[] rights)
        {
            StringBuilder left_ = new StringBuilder(left.Replace('\\', '/'));
            foreach (var value in rights)
            {
                var right = value.Replace('\\', '/');
                var leftHas = left_[left_.Length - 1] == '/';
                var rightHas = right[0] == '/';

                if (leftHas && rightHas)
                {
                    left_.Append(right.Substring(1));
                    continue;
                }

                if (!leftHas && !rightHas)
                {
                    left_.Append('/');
                    left_.Append(right);
                    continue;
                }

                left_.Append(right);
            }
            return left_.ToString();
        }

        #region 获取路径的集中方式，还有一种Server.MapPath，.Net Core不支持
        /// <summary>
        /// 推荐 带/
        /// </summary>
        public static string AppBase
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// 服务及IIS不准 不带/
        /// </summary>
        public static string DirBase
        {
            get
            {
                return Directory.GetCurrentDirectory();
            }
        }

        /// <summary>
        /// 服务及IIS不准 带/
        /// </summary>
        public static string FileBase
        {
            get
            {
                return new FileInfo("./").FullName;
            }
        }

        /// <summary>
        /// 服务及IIS不准 带/
        /// </summary>
        public static string FloderBase
        {
            get
            {
                return new DirectoryInfo("./").FullName;
            }
        }
        #endregion
    }
}
