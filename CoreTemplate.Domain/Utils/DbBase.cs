using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// ADO.Net Base
    /// </summary>
    public abstract class DbBase
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string ConnectString { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        protected void SetConnectString(string connectString)
        {
            ConnectString = connectString;
        }

        /// <summary>
        /// 获取连接器
        /// </summary>
        protected abstract DbConnection GetConnection();

        /// <summary>
        /// 连接测试
        /// </summary>
        public string ConnectTest()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("Db Connect Test...");
            var connection = GetConnection();
            try
            {
                connection.Open();
                connection.Close();
                result.AppendLine("Db Connect Success.");
            }
            catch (Exception ex)
            {
                result.AppendLine("Db Connect Fail:" + ex.Message);
            }
            finally
            {
                connection.Dispose();
            }
            Console.WriteLine(result);
            return result.ToString();
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        protected abstract DbParameter[] GetParameters(MapKeyObject map);

        /// <summary>
        /// 获取Command
        /// </summary>
        private DbCommand GetCommand(string sql, MapKeyObject map, out DbConnection connection)
        {
            connection = GetConnection();
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            if (map != null)
                cmd.Parameters.AddRange(GetParameters(map));
            return cmd;
        }

        /// <summary>
        /// 获取执行行数
        /// </summary>
        public int GetExecuteNonQuery(string sql, MapKeyObject map = null)
        {
            var cmd = GetCommand(sql, map, out var connection);
            cmd.CommandTimeout = 0;
            connection.Open();
            var count = cmd.ExecuteNonQuery();
            connection.Close();
            return count;
        }

        /// <summary>
        /// 判断影响行数是否大于0
        /// </summary>
        public bool GetExecuteResult(string sql, MapKeyObject map = null)
        {
            return GetExecuteNonQuery(sql, map) > 0;
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        private object GetExecuteScalar(string sql, MapKeyObject map)
        {
            var cmd = GetCommand(sql, map, out var connection);
            connection.Open();
            var value = cmd.ExecuteScalar();
            connection.Close();
            return value;
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        public object SelectValue(string sql, MapKeyObject map = null)
        {
            return GetExecuteScalar(sql, map);
        }

        /// <summary>
        /// 获取单个字符串
        /// </summary>
        public string SelectString(string sql, MapKeyObject map = null)
        {
            return GetExecuteScalar(sql, map).ToString();
        }

        /// <summary>
        /// 获取单个数字
        /// </summary>
        public int SelectCount(string sql, MapKeyObject map = null)
        {
            return GetExecuteScalar(sql, map).ToInt();
        }

        /// <summary>
        /// 获取数据指针（需关闭）
        /// </summary>
        public DbDataReader GetExecuteReader(string sql, MapKeyObject map = null)
        {
            var cmd = GetCommand(sql, map, out var connection);
            connection.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 获取数据器
        /// </summary>
        protected abstract DbDataAdapter GetDataAdapter(DbCommand cmd);

        /// <summary>
        /// 获取数据表
        /// </summary>
        public DataTable GetDataTable(string sql, MapKeyObject map = null)
        {
            var cmd = GetCommand(sql, map, out var connection);
            cmd.CommandTimeout = 0;
            var adapter = GetDataAdapter(cmd);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        /// <summary>
        /// 获取数组
        /// </summary>
        public T[] GetArray<T>(string sql, MapKeyObject map = null)
        {
            return GetDataTable(sql, map).ToArray<T>();
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        public DataSet GetDataSet(string sql, MapKeyObject map = null)
        {
            var cmd = GetCommand(sql, map, out var connection);
            var adapter = GetDataAdapter(cmd);
            var set = new DataSet();
            adapter.Fill(set);
            return set;
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        public virtual string[] GetTableNames()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        public virtual void BulkInsert(DataTable table, string tableName)
        {
            throw new NotImplementedException();
        }

        

        
    }
}
