using System;
using System.Collections.Generic;
using System.Text;
using CoreTemplate.Application.IServices;
using CoreTemplate.Application.Model.Test.Param;
using CoreTemplate.Domain.Shared.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CoreTemplate.Application.Services
{
    /// <summary>
    /// ValueServices
    /// </summary>
    public class ValuesServices : IValuesServices
    {
        public int Get(ValueParam param)
        {
            Console.WriteLine("获取值");
            return param.Value;
        }
    }
}
