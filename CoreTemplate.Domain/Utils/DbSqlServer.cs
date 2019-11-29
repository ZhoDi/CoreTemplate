using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;

namespace CommonUtils
{
    /// <summary>
    /// ADO.NET for SqlServer
    /// </summary>
    public class DbSqlServer : DbBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DbSqlServer(string connectString)
        {
            SetConnectString(connectString);
        }

        /// <summary>
        /// 获取连接器
        /// </summary>
        protected override DbConnection GetConnection()
        {
            return new SqlConnection(ConnectString);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        protected override DbParameter[] GetParameters(MapKeyObject map)
        {
            var list = new List<SqlParameter>();
            foreach (var keyValue in map)
                list.Add(new SqlParameter(keyValue.Key, keyValue.Value));
            return list.ToArray();
        }

        /// <summary>
        /// 获取数据器
        /// </summary>
        protected override DbDataAdapter GetDataAdapter(DbCommand cmd)
        {
            return new SqlDataAdapter((SqlCommand)cmd);
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        public override string[] GetTableNames()
        {
            string sql = "show tables;";
            List<string> names = new List<string>();
            DbDataReader dr = GetExecuteReader(sql);
            while (dr.Read())
                names.Add(dr[0].ToString());
            return names.ToArray();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        public override void BulkInsert(DataTable table, string tableName)
        {
            using (SqlBulkCopy bulk = new SqlBulkCopy(ConnectString))
            {
                bulk.BatchSize = table.Rows.Count;
                bulk.DestinationTableName = tableName;
                foreach (DataColumn dc in table.Columns)
                {
                    bulk.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }
                bulk.WriteToServer(table);
            }
        }
    }
}
