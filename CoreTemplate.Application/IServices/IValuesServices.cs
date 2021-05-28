using System;
using System.Collections.Generic;
using System.Text;
using CoreTemplate.Application.Model.Test.Param;

namespace CoreTemplate.Application.IServices
{
    /// <summary>
    /// IValuesServices
    /// </summary>
    public interface IValuesServices
    {
        int Get(ValueParam param);
    }
}
