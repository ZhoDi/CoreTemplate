using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreTemplate.Application.Enum
{
    /// <summary>
    /// 网关状态代码
    /// </summary>
    [Description("网关状态代码")]
    public enum GatewayStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 1,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 2,
    }
}
