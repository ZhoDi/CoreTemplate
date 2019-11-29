using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// sql相关
    /// </summary>
    public static class SqlUtil
    {
        /// <summary>
        /// TableToInsert
        /// </summary>
        public static string TableToInsert(DbBase db, string tableName)
        {
            DataTable table = db.GetDataTable(string.Format("select * from {0} limit 1;", tableName));
            StringBuilder sb = new StringBuilder(string.Format("string sql =\"insert into {0} set ", tableName));
            StringBuilder sbValue = new StringBuilder();
            for (int index = 0; index < table.Columns.Count; index++)
            {
                DataColumn column = table.Columns[index];

                if (index != table.Columns.Count - 1)
                    sb.Append(string.Format("`{0}`=@{0},", column.ColumnName));
                else
                    sb.AppendLine(string.Format("`{0}`=@{0};\";", column.ColumnName));

                sbValue.AppendLine(string.Format("sp.Add(\"@{0}\",item.{0});", column.ColumnName));
            }
            sb.AppendLine("var count = 0;");
            sb.AppendLine("foreach (var item in array){var sp = new SqlParameters();");
            sb.Append(sbValue);
            sb.AppendLine("count+=SqlHelper.GetExecutedCount(sql,sp);}");
            return sb.ToString();
        }

        /// <summary>
        /// TableToClass
        /// </summary>
        public static string TableToClass(DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn col in table.Columns)
                sb.AppendLine("public string " + col.ColumnName + " {get;set;}");
            return sb.ToString();
        }

        /// <summary>
        /// CsvToSql
        /// </summary>
        public static string FromCsv(string path, string tableName = "@tableName")
        {
            string[] lines = FileUtil.ReadLines(path);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("insert into {0} values ", tableName));
            for (int index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
                sb.Append('(');

                var cells = line.Split(',');
                for (int subIndex = 0; subIndex < cells.Length; subIndex++)
                {
                    var cell = cells[subIndex];
                    if (subIndex != cells.Length - 1)
                        sb.Append(string.Format("'{0}',", cell));
                    else
                        sb.Append(string.Format("'{0}'", cell));
                }

                if (index != lines.Length - 1)
                    sb.AppendLine("),");
                else
                    sb.AppendLine(");");
            }
            return sb.ToString();
        }
    }
}
