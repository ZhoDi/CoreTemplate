using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// ADO.NET for MySql
    /// </summary>
    public class DbMySql : DbBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DbMySql(string connectString)
        {
            SetConnectString(connectString);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public DbMySql(string host, int port, string user, string pwd, string database)
        {
            SetConnectString(string.Format("server={0};port={1};user={2};pwd={3};database={4};sslmode=none;", host, port, user, pwd, database));
        }

        /// <summary>
        /// 获取连接器
        /// </summary>
        protected override DbConnection GetConnection()
        {
            return new MySqlConnection(ConnectString);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        protected override DbParameter[] GetParameters(MapKeyObject map)
        {
            var list = new List<MySqlParameter>();
            foreach (var keyValue in map)
                list.Add(new MySqlParameter(keyValue.Key, keyValue.Value));
            return list.ToArray();
        }

        /// <summary>
        /// 获取数据器
        /// </summary>
        protected override DbDataAdapter GetDataAdapter(DbCommand cmd)
        {
            return new MySqlDataAdapter((MySqlCommand)cmd);
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
            string columns = "";
            int columnCount = table.Columns.Count;
            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                if (columnIndex != 0)
                {
                    columns += ",";
                }
                columns += "`" + table.Columns[columnIndex].ColumnName + "`";
            }
            string values = "";
            int rowCount = table.Rows.Count;
            MapKeyObject map = new MapKeyObject();
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                if (rowIndex != 0)
                {
                    values += ",";
                }
                values += "(";
                DataRow dr = table.Rows[rowIndex];
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    string parameterName = "@r" + rowIndex + "c" + columnIndex;
                    map.Add(parameterName, dr[columnIndex]);
                    if (columnIndex != 0)
                    {
                        values += ",";
                    }
                    values += parameterName;
                }
                values += ")";

            }
            string sql = "insert into `" + tableName + "` (" + columns + ") values " + values + ";";
            GetExecuteNonQuery(sql, map);
        }
    }
}
