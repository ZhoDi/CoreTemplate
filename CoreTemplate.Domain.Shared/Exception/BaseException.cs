using System;
using System.Collections.Generic;
using System.Text;
using CoreTemplate.Domain.Shared.Enum;
using EnumsNET;

namespace CoreTemplate.Domain.Shared.Exception
{
    /// <summary>
    /// 异常基类
    /// </summary>
    public class BaseException : System.Exception
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public DetailedStatus DetailedStatus;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailedMessage"></param>
        /// <param name="detailedStatus"></param>
        public BaseException(string detailedMessage = null, DetailedStatus detailedStatus = DetailedStatus.Fail)
            : base(string.IsNullOrEmpty(detailedMessage) ? detailedStatus.AsString(EnumFormat.Description) : detailedMessage)
        {
            DetailedStatus = detailedStatus;
        }
    }
}
