using System;
using System.Collections.Generic;
using System.Text;
using CoreTemplate.Domain.Shared.Enum;
using EnumsNET;

namespace CoreTemplate.Application.Dto.Base
{
    /// <summary>
    /// 基础返回结果(不返回数据)
    /// </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 详细状态
        /// </summary>
        public DetailedStatus DetailedStatus { get; set; }

        /// <summary>
        /// 详细状态描述
        /// </summary>
        public string DetailedMessage { get; set; }

        public BaseResponse()
        {
            DetailedStatus = DetailedStatus.Success;
            DetailedMessage = DetailedStatus.AsString(EnumFormat.Description);
        }
    }

    /// <summary>
    /// 基础返回结果(返回数据)
    /// </summary>
    public class BaseResponse<T>
    {
        /// <summary>
        /// 详细状态
        /// </summary>
        public DetailedStatus DetailedStatus { get; set; }

        /// <summary>
        /// 详细交易状态
        /// </summary>
        public string DetailedMessage { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseResponse()
        {
            DetailedStatus = DetailedStatus.Success;
            DetailedMessage = DetailedStatus.AsString(EnumFormat.Description);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseResponse(T data)
        {
            Data = data;
            DetailedStatus = DetailedStatus.Success;
            DetailedMessage = DetailedStatus.AsString(EnumFormat.Description);
        }
    }

}
