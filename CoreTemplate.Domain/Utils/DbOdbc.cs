using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// ADO.NET for Odbc
    /// </summary>
    public class DbOdbc : DbBase
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DbOdbc(string connectString)
        {
            SetConnectString(connectString);
        }

        /// <summary>
        /// 获取连接器
        /// </summary>
        protected override DbConnection GetConnection()
        {
            return new OdbcConnection(ConnectString);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        protected override DbParameter[] GetParameters(MapKeyObject map)
        {
            var list = new List<OdbcParameter>();
            foreach (var keyValue in map)
                list.Add(new OdbcParameter(keyValue.Key, keyValue.Value));
            return list.ToArray();
        }

        /// <summary>
        /// 获取数据器
        /// </summary>
        protected override DbDataAdapter GetDataAdapter(DbCommand cmd)
        {
            return new OdbcDataAdapter((OdbcCommand)cmd);
        }

        /// <summary>
        /// 获取所有表名
        /// </summary>
        public override string[] GetTableNames()
        {
            OdbcConnection conn = new OdbcConnection(ConnectString);
            conn.Open();
            DataTable dt = conn.GetSchema("Tables");
            conn.Close();
            List<string> listName = new List<string>();
            string name;
            string[] sysKeys = { "MSys", "$'_", "$'Print_", "_xlnm" };
            bool isTableName;
            foreach (DataRow dr in dt.Rows)
            {
                name = dr["TABLE_NAME"].ToString();
                isTableName = true;
                foreach (string sysKey in sysKeys)
                {
                    if (name.Contains(sysKey))
                    {
                        isTableName = false;
                        break;
                    }
                }

                if (isTableName)
                    listName.Add(name);
            }
            return listName.ToArray();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        public override void BulkInsert(DataTable table, string tableName)
        {
            int colCount = table.Columns.Count;
            string columns = "";
            for (int colIndex = 0; colIndex < colCount; colIndex++)
            {
                if (colIndex != 0)
                {
                    columns += ",";
                }
                columns += "[" + table.Columns[colIndex].ColumnName + "]";
            }
            int rowCount = table.Rows.Count;
            OdbcConnection conn = new OdbcConnection(ConnectString);
            conn.Open();
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                string values = "";
                MapKeyObject map = new MapKeyObject();
                for (int colIndex = 0; colIndex < colCount; colIndex++)
                {
                    string value = "@r" + rowIndex + "c" + colIndex;
                    map.Add(value, table.Rows[rowIndex][colIndex]);
                    if (colIndex != 0)
                    {
                        values += ",";
                    }
                    values += value;
                }
                string sql = "insert into [" + tableName + "] (" + columns + ") values (" + values + ");";
                OdbcCommand cmd = new OdbcCommand(sql, conn);
                cmd.Parameters.AddRange(GetParameters(map));
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        /// <summary>
        /// 连接字符串
        /// value兼容空格，不要用"",''包裹
        /// </summary>
        public class ConnectStrings
        {
            /// <summary>
            /// 通过名字获取连接字符串
            /// </summary>
            /// <param name="name">系统中配置过的ODBC</param>
            public static string Name(string name)
            {
                return string.Format("Dsn={0};", name);
            }

            /// <summary>
            /// 通过驱动名和文件路径获取连接字符串
            /// </summary>
            /// <param name="driver">驱动名</param>
            /// <param name="path">文件路径</param>
            public static string Diver(string driver, string path)
            {
                return string.Format("Driver={0};DBQ={1};", driver, path);
            }

            /// <summary>
            /// Access07
            /// </summary>
            public static string AccessNew(string path)
            {
                return "Driver={Microsoft Access Driver (*.mdb, *.accdb)};" + string.Format("DBQ={0};", path);
            }

            /// <summary>
            /// Paradox
            /// </summary>
            public static string Paradox(string floder)
            {
                return "Driver={Microsoft Paradox Driver (*.db )};" + string.Format("DBQ={0};", floder);
            }
        }
    }
}
