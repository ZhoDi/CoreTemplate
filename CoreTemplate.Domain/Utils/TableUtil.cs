using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 专门处理DataTable
    /// </summary>
    public static class TableUtil
    {
        /// <summary>
        /// 数组转为DataTable
        /// </summary>
        public static DataTable ToTable<T>(IEnumerable<T> array)
        {
            var json = JsonUtil.Serialize(array);
            return JsonUtil.Deserialize<DataTable>(json);
        }
    }
}
