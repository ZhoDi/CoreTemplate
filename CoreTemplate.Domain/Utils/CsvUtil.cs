using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace CommonUtils
{
    /// <summary>
    /// 处理csv
    /// </summary>
    public static class CsvUtil
    {
        /// <summary>
        /// 得到DataTabe
        /// </summary>
        public static DataTable GetDataTabe(string csv)
        {
            var table = new DataTable();
            var lines = csv.GetLines();
            for (int index = 0; index < lines.Length; index++)
            {
                var cells = lines[index].Split(',');
                if (index == 0)
                {
                    foreach (var cell in cells)
                        table.Columns.Add(cell);
                    continue;
                }

                table.LoadDataRow(cells, true);
            }
            return table;
        }

        /// <summary>
        /// 从文件中获取DataTabe
        /// </summary>
        public static DataTable GetDataTabeFromFile(string path)
        {
            return GetDataTabe(FileUtil.GetText(path));
        }

        /// <summary>
        /// 转换
        /// </summary>
        public static string ToCsv(this DataTable table)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataColumn column in table.Columns)
                sb.Append(column.ColumnName + ",");
            sb.AppendLine();
            foreach (DataRow row in table.Rows)
            {
                foreach (var cell in row.ItemArray)
                    sb.Append(cell + ",");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转换
        /// </summary>
        public static string ToCsv(this JArray array)
        {
            StringBuilder sb = new StringBuilder();
            if (array == null || array.Size() == 0)
                return sb.ToString();

            foreach (var item in array[0])
                sb.Append(item.Path + ",");
            return sb.ToString();
        }

        /// <summary>
        /// 获取数据矩阵
        /// </summary>
        public static List<string[]> GetCellsList(string csv)
        {
            string[] rows = csv.Split("\r\n");
            List<string[]> matrix = new List<string[]>();
            for (int index = 0; index < rows.Length; index++)
            {
                string row = rows[index];
                string[] cells = row.Split(',');
                matrix.Add(cells);
            }
            return matrix;
        }

        /// <summary>
        /// 通过数据矩阵获取csv
        /// </summary>
        public static string ToCsv(this List<string[]> cellsList)
        {
            StringBuilder csv = new StringBuilder();
            for (int rowIndex = 0; rowIndex < cellsList.Count; rowIndex++)
            {
                string[] cells = cellsList[rowIndex];
                for (int colIndex = 0; colIndex < cells.Length; colIndex++)
                {
                    csv.Append(cells[colIndex]);
                    if (colIndex == cells.Length - 1)
                        csv.AppendLine();
                    else
                        csv.Append(",");
                }
            }
            return csv.ToString();
        }

        /// <summary>
        /// 获取横向csv
        /// </summary>
        public static string GetRotate(string csv)
        {
            List<string[]> matrix = GetCellsList(csv);
            int rowCount = matrix.Count, colCount = matrix[0].Length;
            StringBuilder newCsv = new StringBuilder();
            for (int colIndex = 0; colIndex < colCount; colIndex++)
            {
                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                    newCsv.Append(matrix[rowIndex][colIndex]);
                    if (rowIndex == rowCount - 1)
                        newCsv.AppendLine();
                    else
                        newCsv.Append(",");
                }
            }
            return newCsv.ToString();
        }
    }
}
