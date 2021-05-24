using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreTemplate.Application.Enum
{
    /// <summary>
    /// 接口详细描述代码
    /// </summary>
    [Description("接口详细描述代码")]
    public enum DetailedStatus
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

        /// <summary>
        /// 未知异常
        /// </summary>
        [Description("未知异常")]
        Error = 3,
        /// <summary>
        /// 参数异常
        /// </summary>
        [Description("参数异常")]
        ParamsError = 4,
        /// <summary>
        /// Token已过期
        /// </summary>
        [Description("Token已过期")]
        TokenExpire = 5,
        /// <summary>
        /// 无权限
        /// </summary>
        [Description("无权限")]
        NoPermission = 6,

        /// <summary>
        /// 暂无数据
        /// </summary>
        [Description("暂无数据")]
        DataIsNull = 7,

        /// <summary>
        /// 数据已存在
        /// </summary>
        [Description("数据已存在")]
        DataAlreadyExists = 8,
    }
}
