using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreTemplate.Application.Model.Test.Param
{
    /// <summary>
    /// ValueParam
    /// </summary>
    public class ValueParam
    {
        /// <summary>
        /// 值
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "值超出范围")]
        public int Value { get; set; }
    }
}
