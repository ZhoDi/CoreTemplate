using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTemplate.Application
{
    public static class AppConst
    {
        public static readonly string ProjectName = "SDJC";

        //token加密 码
        public const string DefaultPassPhrase = "AbcGC2019MK2ccc";

        //(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$ 至少8位 包含大写 小写和数字
        //当前至少包含三位
        public const string PasswordRegex = "(?=^.{3,}$)";

    }
}
