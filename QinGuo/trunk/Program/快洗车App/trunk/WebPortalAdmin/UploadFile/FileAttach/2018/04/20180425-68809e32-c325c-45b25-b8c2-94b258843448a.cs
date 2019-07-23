using System;
using Aspose.Cells;
using System.Data;

namespace SY.SharingCenter.Web.Controllers.ForeignAcct
{
    public static class ExcelHelper
    {
        private static object _obj = new object();
        private static int _root = int.MinValue;

        /// <summary>
        /// Excel文件导出
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="templateFullPath"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ExcelExportWithTemplate(string filePath,string templateFullPath, System.Data.DataTable dt)
        {
            Workbook wb = null;
            Worksheet ws = null;

            string res = string.Empty;

            try
            {
                res = createExcelFileName(true);
                wb = new Workbook(templateFullPath);
                ws = wb.Worksheets[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ws.Cells[i+1, j].Value = dt.Rows[i][j].GetType() == typeof(DBNull) ? "" : dt.Rows[i][j].ToString();
                    }
                }

                wb.Save(filePath + "\\" + res, SaveFormat.Xlsm);
            }
            catch (Exception ex)
            {
                res = string.Empty;
                throw ex;
            }
            finally
            {
                try
                {
                    ws = null;
                    wb = null;
                }
                catch
                {
                }
            }
            return res;
        }

        /// <summary>
        /// Excel导入DataTable
        /// </summary>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static DataTable ExcelImport(string fullFilePath)
        {
            System.Data.DataTable dt = null;
            Workbook wb = null;
            Worksheet ws = null;
            Cells cells = null;
            try
            {
                wb = new Workbook(fullFilePath);
                ws = wb.Worksheets[0];
                cells = ws.Cells;
                dt = cells.ExportDataTableAsString(0, 0, cells.MaxRow, cells.MaxColumn + 1, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                try
                {
                    ws = null;
                    wb = null;
                }
                catch
                {
                }
            }
            return dt;
        }

        /// <summary>
        /// 生成本机文件唯一路径
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        /// <remarks>在任何时候调用本方法生成路径，保证本机唯一性</remarks>
        private static string createExcelFileName(bool hasMacro)
        {
            return "temp_" + RandomString + "." + (hasMacro ? "xlsm" : "xlsx");
        }

        /// <summary>
        /// 生成本机唯一随机数
        /// </summary>
        /// <remarks>在任何时候调用本方法生成随机数，保证本机唯一性</remarks>
        public static string RandomString
        {
            get
            {
                lock (_obj)
                {
                    if (_root == int.MaxValue)
                    {
                        _root = int.MinValue;
                    }
                    string res = DateTime.Now.ToString("yyyyMMddHHmmss.ffff") + "." + new Random(_root).Next(1000, 9999).ToString();
                    _root++;
                    return res;
                }
            }
        }
    }
}