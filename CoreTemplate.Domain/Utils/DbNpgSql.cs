using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// ADO.NET for NpgSql
    /// </summary>
    public class DbNpgSql : DbBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DbNpgSql(string connectString)
        {
            SetConnectString(connectString);
        }

        /// <summary>
        /// 获取连接器
        /// </summary>
        protected override DbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectString);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        protected override DbParameter[] GetParameters(MapKeyObject map)
        {
            var list = new List<NpgsqlParameter>();
            foreach (var keyValue in map)
                list.Add(new NpgsqlParameter(keyValue.Key, keyValue.Value));
            return list.ToArray();
        }

        /// <summary>
        /// 获取数据器
        /// </summary>
        protected override DbDataAdapter GetDataAdapter(DbCommand cmd)
        {
            return new NpgsqlDataAdapter((NpgsqlCommand)cmd);
        }
    }
}
