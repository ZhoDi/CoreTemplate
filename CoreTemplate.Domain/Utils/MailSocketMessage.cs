using System;
using System.Collections.Generic;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 邮件通讯协议
    /// </summary>
    public class MailSocketMessage
    {
        /// <summary>
        /// 通讯秘钥
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="token"></param>
        public MailSocketMessage(string token)
        {
            Token = token;
        }

        /// <summary>
        /// 通讯类型
        /// </summary>
        public enum TypeEnum
        {
            /// <summary>
            /// 未定义
            /// </summary>
            Undefine = 0,

            /// <summary>
            /// 获取发送列表
            /// </summary>
            Get = 1,

            /// <summary>
            /// 待发送列表
            /// </summary>
            List = 2,

            /// <summary>
            /// 标记已发送
            /// </summary>
            Finish = 3
        }

        /// <summary>
        /// 通讯代码
        /// </summary>
        public TypeEnum Type { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public Mail[] Mails { get; set; }
    }
}
