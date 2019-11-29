using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// ADO.NET for Oracle
    /// </summary>
    public class DbOracle : DbBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DbOracle(string connectString)
        {
            SetConnectString(connectString);
        }

        /// <summary>
        /// 获取连接器
        /// </summary>
        protected override DbConnection GetConnection()
        {
            return new OracleConnection(ConnectString);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        protected override DbParameter[] GetParameters(MapKeyObject map)
        {
            var list = new List<OracleParameter>();
            foreach (var keyValue in map)
                list.Add(new OracleParameter(keyValue.Key, keyValue.Value));
            return list.ToArray();
        }

        /// <summary>
        /// 获取数据器
        /// </summary>
        protected override DbDataAdapter GetDataAdapter(DbCommand cmd)
        {
            return new OracleDataAdapter((OracleCommand)cmd);
        }
    }
}
