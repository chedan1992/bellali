using System.Data;
using System.IO;
using System.Text;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Diagnostics;
using QINGUO.Common;
using QINGUO.Model;
using QINGUO.Business;

namespace WebPortalAdmin.Code
{
    /// <summary>
    /// 使用NPOI操作Excel，无需Office COM组件
    /// Created By 囧月 http://lwme.cnblogs.com
    /// 部分代码取自http://msdn.microsoft.com/zh-tw/ee818993.aspx
    /// NPOI是POI的.NET移植版本，目前稳定版本中仅支持对xls文件（Excel 97-2003）文件格式的读写
    /// NPOI官方网站http://npoi.codeplex.com/
    /// </summary>
    public class ExcelRender
    {

        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        public static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.BLANK:
                    return string.Empty;
                case CellType.BOOLEAN:
                    return cell.BooleanCellValue.ToString();
                case CellType.ERROR:
                    return cell.ErrorCellValue.ToString();
                case CellType.NUMERIC:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.STRING:
                    return cell.StringCellValue;
                case CellType.FORMULA:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        /// <summary>
        /// 自动设置Excel列宽
        /// </summary>
        /// <param name="sheet">Excel表</param>
        public static void AutoSizeColumns(ISheet sheet)
        {
            if (sheet.PhysicalNumberOfRows > 0)
            {
                IRow headerRow = sheet.GetRow(0);

                for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }
        }

        /// <summary>
        /// 保存Excel文档流到文件
        /// </summary>
        /// <param name="ms">Excel文档流</param>
        /// <param name="fileName">文件名</param>
        public static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();

                fs.Write(data, 0, data.Length);
                fs.Flush();

                data = null;
            }
        }

        /// <summary>
        /// 输出文件到浏览器
        /// </summary>
        /// <param name="ms">Excel文档流</param>
        /// <param name="context">HTTP上下文</param>
        /// <param name="fileName">文件名</param>
        public static void RenderToBrowser(MemoryStream ms, HttpContext context, string fileName)
        {
            if (context.Request.Browser.Browser == "IE")
                fileName = HttpUtility.UrlEncode(fileName);
            context.Response.AddHeader("Content-Disposition", "attachment;fileName=" + fileName);
            context.Response.BinaryWrite(ms.ToArray());
        }

        /// <summary>
        /// DataReader转换成Excel文档流
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(IDataReader reader)
        {
            MemoryStream ms = new MemoryStream();

            using (reader)
            {
                IWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = workbook.CreateSheet();

                IRow headerRow = sheet.CreateRow(0);
                int cellCount = reader.FieldCount;

                // handling header.
                for (int i = 0; i < cellCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(reader.GetName(i));
                }

                // handling value.
                int rowIndex = 1;
                while (reader.Read())
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    for (int i = 0; i < cellCount; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(reader[i].ToString());
                    }

                    rowIndex++;
                }

                AutoSizeColumns(sheet);

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

            }
            return ms;
        }

        /// <summary>
        /// DataReader转换成Excel文档流，并保存到文件
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="fileName">保存的路径</param>
        public static void RenderToExcel(IDataReader reader, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(reader))
            {
                SaveToFile(ms, fileName);
            }
        }

        /// <summary>
        /// DataReader转换成Excel文档流，并输出到客户端
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="context">HTTP上下文</param>
        /// <param name="fileName">输出的文件名</param>
        public static void RenderToExcel(IDataReader reader, HttpContext context, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(reader))
            {
                RenderToBrowser(ms, context, fileName);
            }
        }

        /// <summary>
        /// DataTable转换成Excel文档流
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static MemoryStream RenderToExcel(DataTable table)
        {
            MemoryStream ms = new MemoryStream();

            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = workbook.CreateSheet();

                IRow headerRow = sheet.CreateRow(0);

                // handling header.
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }
                AutoSizeColumns(sheet);

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        /// <summary>
        /// DataTable转换成Excel文档流，并保存到文件
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName">保存的路径</param>
        public static void RenderToExcel(DataTable table, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(table))
            {
                SaveToFile(ms, fileName);
            }
        }

        /// <summary>
        /// DataTable转换成Excel文档流，并输出到客户端
        /// </summary>
        /// <param name="table"></param>
        /// <param name="response"></param>
        /// <param name="fileName">输出的文件名</param>
        public static void RenderToExcel(DataTable table, HttpContext context, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(table))
            {
                RenderToBrowser(ms, context, fileName);
            }
        }

        /// <summary>
        /// Excel文档流是否有数据
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream)
        {
            return HasData(excelFileStream, 0);
        }

        /// <summary>
        /// Excel文档流是否有数据
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <returns></returns>
        public static bool HasData(Stream excelFileStream, int sheetIndex)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);

                if (workbook.NumberOfSheets > 0)
                {
                    if (sheetIndex < workbook.NumberOfSheets)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndex);

                        return sheet.PhysicalNumberOfRows > 0;

                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="sheetName">表名称</param>
        /// <returns></returns>
        public static DataTable RenderFromExcel(Stream excelFileStream, string sheetName)
        {
            return RenderFromExcel(excelFileStream, sheetName, 0);
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="sheetName">表名称</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        public static DataTable RenderFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex)
        {
            DataTable table = null;

            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                ISheet sheet = workbook.GetSheet(sheetName);

                table = RenderFromExcel(sheet, headerRowIndex);

            }
            return table;
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// 默认转换Excel的第一个表
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <returns></returns>
        public static DataTable RenderFromExcel(Stream excelFileStream)
        {
            return RenderFromExcel(excelFileStream, 0, 0);
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <returns></returns>
        public static DataTable RenderFromExcel(Stream excelFileStream, int sheetIndex)
        {
            return RenderFromExcel(excelFileStream, sheetIndex, 0);
        }

        /// <summary>
        /// Excel文档流转换成DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        public static DataTable RenderFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex)
        {
            DataTable table = null;

            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);

                ISheet sheet = workbook.GetSheetAt(sheetIndex);

                table = RenderFromExcel(sheet, headerRowIndex);


            }
            return table;
        }

        /// <summary>
        /// Excel表格转换成DataTable
        /// </summary>
        /// <param name="sheet">表格</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        public static DataTable RenderFromExcel(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();

            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

            //handling header.
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();

                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = GetCellValue(row.GetCell(j));
                    }
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }



        //#region ===导入数据入口

        ///// <summary>
        ///// Excel文档导入到数据库
        ///// 默认取Excel的第一个表
        ///// 第一行必须为标题行
        ///// </summary>
        ///// <param name="excelFileStream">Excel文档流</param>
        ///// <param name="insertSql">插入语句</param>
        ///// <param name="dbAction">更新到数据库的方法</param>
        ///// <returns></returns>
        //public static int ExportTest(string StoreType,Stream excelFileStream, string insertSql, out DataTable tab, ModSysMaster CurrentMaster)
        //{
        //    return 0;// RenderToDbCompany(StoreType, excelFileStream, insertSql, 0, 0, out tab, CurrentMaster);
        //}

        //#endregion

        #region 代理执行sql
        public delegate int DBImport(string sql);
        public static int DBImportFun(string sql)
        {
            BllSysAppointed bllsysCom = new BllSysAppointed();
            return bllsysCom.ExecuteNonQueryByText(sql);
        }

        #endregion

        //#region Excel文档导入到数据库
        ///// <summary>
        ///// Excel文档导入到数据库
        ///// </summary>
        ///// <param name="excelFileStream">Excel文档流</param>
        ///// <param name="insertSql">插入语句</param>
        ///// <param name="dbAction">更新到数据库的方法</param>
        ///// <param name="sheetIndex">表索引号，如第一个表为0</param>
        ///// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        ///// <param name="shopTypeid">商家分类</param>
        ///// <returns></returns>
        //public static int RenderToDbCompany(string StoreType,Stream excelFileStream, string insertSql, int sheetIndex, int headerRowIndex, out DataTable tab, ModSysMaster CurrentMaster)
        //{
        //    #region 构建错误表头
        //    DataTable ErrorTab = new DataTable();
        //    DataColumn dc1 = new DataColumn("产品线", Type.GetType("System.String"));
        //    DataColumn dc2 = new DataColumn("证书类型", Type.GetType("System.String"));
        //    DataColumn dc3 = new DataColumn("技术领域", Type.GetType("System.String"));
        //    DataColumn dc4 = new DataColumn("知识点", Type.GetType("System.String"));
        //    DataColumn dc5 = new DataColumn("题型名称", Type.GetType("System.String"));
        //    DataColumn dc6 = new DataColumn("分级", Type.GetType("System.String"));
        //    DataColumn dc7 = new DataColumn("试题分数", Type.GetType("System.String"));
        //    DataColumn dc8 = new DataColumn("试题内容", Type.GetType("System.String"));
        //    DataColumn dc9 = new DataColumn("试题选项", Type.GetType("System.String"));
        //    DataColumn dc10 = new DataColumn("标准答案", Type.GetType("System.String"));
        //    DataColumn dc11 = new DataColumn("试题解析", Type.GetType("System.String"));
        //    DataColumn dc12 = new DataColumn("错误原因", Type.GetType("System.String"));
        //    ErrorTab.Columns.Add(dc1);
        //    ErrorTab.Columns.Add(dc2);
        //    ErrorTab.Columns.Add(dc3);
        //    ErrorTab.Columns.Add(dc4);
        //    ErrorTab.Columns.Add(dc5);
        //    ErrorTab.Columns.Add(dc6);
        //    ErrorTab.Columns.Add(dc7);
        //    ErrorTab.Columns.Add(dc8);
        //    ErrorTab.Columns.Add(dc9);
        //    ErrorTab.Columns.Add(dc10);
        //    ErrorTab.Columns.Add(dc11);
        //    ErrorTab.Columns.Add(dc12);
        //    #endregion
        //    //获取所有类型
        //    SysCategory = new BllSysCategory().QueryToAll();
        //    int rowAffected = 0;
        //    using (excelFileStream)//读取流
        //    {
        //        //根据类型查询题库信息
        //        //List<ModBsTestStore> list = new BllBsTestStore().GetListByWhere(" and StoreType=" + StoreType+" and Status!="+(int)StatusEnum.删除);

        //        IWorkbook workbook = new HSSFWorkbook(excelFileStream);
        //        ISheet sheet = workbook.GetSheetAt(sheetIndex);
        //        StringBuilder builder = new StringBuilder();
        //        StringBuilder builderValue = new StringBuilder();
        //        IRow headerRow = sheet.GetRow(headerRowIndex);
        //        int cellCount = headerRow.LastCellNum;//
        //        int rowCount = sheet.LastRowNum;//
        //        int Insertcount = 0;  //插入总行数
        //        //循环表格
        //        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
        //        {
        //            IRow row = sheet.GetRow(i);
        //            //判断空行
        //            if (row != null)
        //            {
        //                bool VisResult = false; //过滤空行
        //                builderValue.Length = 0;

        //                #region  数据验证判断

        //                var column0 = GetCellValue(row.GetCell(0)).Replace("'", "''");//验证产品线
        //                var column0Id="";
        //                var column1 = GetCellValue(row.GetCell(1)).Replace("'", "''");//证书类型
        //                var column1Id="";
        //                var column2 = GetCellValue(row.GetCell(2)).Replace("'", "''");//技术领域
        //                var column2Id="";
        //                var column3 = GetCellValue(row.GetCell(3)).Replace("'", "''");//知识点
        //                var column3Id="";
        //                var column4 = GetCellValue(row.GetCell(4)).Replace("'", "''");//题型名称
        //                var column5 = GetCellValue(row.GetCell(5)).Replace("'", "''");//试题等级
        //                var column5Id=1;
        //                var column6 = GetCellValue(row.GetCell(6)).Replace("'", "''");//试题分数
        //                var column7 = GetCellValue(row.GetCell(7)).Replace("'", "''");//试题内容
        //                var column8 = GetCellValue(row.GetCell(8)).Replace("'", "''");//试题选项
        //                var column9 = GetCellValue(row.GetCell(9)).Replace("'", "''");
        //                var column10 = GetCellValue(row.GetCell(10)).Replace("'", "''");

        //                string parentId = "0";
        //                for (int j = 0; j < cellCount; j++)
        //                {
        //                    string newid = "0";
        //                    string errorStr = "";
        //                    //数验证是否通过 true:不通过
        //                    if (RowDataVis(column4,CurrentMaster, GetCellValue(row.GetCell(j)).Replace("'", "''").Trim(), j, parentId, ref newid, ref errorStr))
        //                    {
        //                        if (column0 == "" && column1 == "" && column2 == "" && column3 == "" && column4 == "" && column5 == "" && column6 == "" && column7 == "" && column8 == "" && column9 == "" && column10 == "")
        //                        {
        //                            break;
        //                        }
        //                        else {
        //                            ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9,column10, errorStr);
        //                        }
        //                        VisResult = true;
        //                        break;
        //                    }
        //                    parentId = newid;
        //                    //装载到StringBuilder
        //                    #region ==转换数据格式

        //                    var cellValue = GetCellValue(row.GetCell(j)).Replace("'", "''").Trim();

        //                    switch (j)
        //                    {
        //                        case 0:  //验证产品线
        //                            column0Id=newid;
        //                            builderValue.AppendFormat("'{0}',", newid);
        //                            break;
        //                        case 1:  //证书类型
        //                            column1Id=newid;
        //                            builderValue.AppendFormat("'{0}',", newid);
        //                            break;
        //                        case 2:  //技术领域
        //                            column2Id=newid;
        //                            builderValue.AppendFormat("'{0}',", newid);
        //                            break;
        //                        case 3:  //知识点
        //                            column3Id=newid;
        //                            builderValue.AppendFormat("'{0}',", newid);
        //                            break;
        //                        case 4:  //题型名称 1:单选 2:多选 3:判断 4:填空 5:问答
        //                            #region
        //                            if ("单选题".Contains(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", 1);
        //                            }
        //                            else if ("多选题".Contains(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", 2);
        //                            }
        //                            else if ("判断题".Contains(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", 3);
        //                            }
        //                            else if ("填空题".Contains(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", 4);
        //                            }
        //                            else if ("问答题".Contains(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", 5);
        //                            }
        //                            #endregion
        //                            break;
        //                        case 5:  //分级
        //                            #region
        //                            if ("初级".Contains(cellValue))
        //                            {
        //                                column5Id=1;
        //                                builderValue.AppendFormat("{0},", 1);
        //                            }
        //                            else if ("中级".Contains(cellValue))
        //                            {
        //                                column5Id=2;
        //                                builderValue.AppendFormat("{0},", 2);
        //                            }
        //                            else if ("高级".Contains(cellValue))
        //                            {
        //                                column5Id=3;
        //                                builderValue.AppendFormat("{0},", 3);
        //                            }
        //                            #endregion
        //                            break;
        //                        case 6:  //试题分数
        //                            if (string.IsNullOrEmpty(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", 1);
        //                            }
        //                            else
        //                            {
        //                                builderValue.AppendFormat("{0},", cellValue);
        //                            }
        //                            break;
        //                        case 7:  //试题内容
        //                            //装载到StringBuilder
        //                            builderValue.AppendFormat("'{0}',",cellValue);
        //                            builderValue.AppendFormat("'{0}',", new Html().StripHtml(cellValue).Trim());
        //                            break;
        //                        case 8:  //试题选项
        //                            //拆分答案
        //                            var TestStoreAnswe = cellValue.Split('|');
        //                            if ("单选题".Contains(column3) || "多选题".Contains(column3))
        //                            {
        //                                 //标准答案
        //                                var Answer = column8.Split('|');
        //                                if (TestStoreAnswe.Length < Answer.Length)
        //                                {
        //                                    errorStr = "试题选项与标准答案不相符";
        //                                    ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, errorStr);
        //                                    VisResult = true;
        //                                    break;
        //                                }
        //                            }
        //                            break;
        //                        case 9:  //标准答案
        //                            if ("单选题".Contains(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", cellValue);
        //                            }
        //                            else if ("多选题".Contains(cellValue))
        //                            {
        //                                builderValue.AppendFormat("{0},", cellValue);
        //                            }
        //                            else if ("判断题".Contains(cellValue))
        //                            {
        //                                if (cellValue == "正确")
        //                                {
        //                                    builderValue.Append('1');
        //                                }
        //                                else
        //                                {
        //                                    builderValue.Append('0');
        //                                }
        //                            }
        //                            else
        //                            {
        //                                //装载到StringBuilder
        //                                builderValue.AppendFormat("'{0}',",cellValue);
        //                            }
        //                            break;
        //                        case 10:  //试题解析
        //                            //装载到StringBuilder
        //                            builderValue.AppendFormat("'{0}',", cellValue);
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                    #endregion
        //                }
        //                if (VisResult)//验证数据合法性true表示不合法
        //                {
        //                    if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
        //                    {
        //                        rowAffected += ExcelRender.DBImportFun(builder.ToString());
        //                        logError.ErrorFormat(builder.ToString());
        //                        builder.Length = 0;
        //                    }
        //                    continue;
        //                }
        //                #endregion

        //                #region ==构建sql语句

                       
        //                if (builderValue.ToString() != "")
        //                {
        //                    //string newTitle = new Html().StripHtml(column7).Trim();
        //                    //List<ModBsTestStore> listwhere = list.Where(p => p.Type1 == column0Id && p.Type2 == column1Id && p.Type3 == column2Id && p.Type4 == column3Id && p.EasyIndex == column5Id && (p.Content.ToString().Contains(column7) || column7.Contains(p.Content.ToString()) || column7 == p.Content.ToString() || column7.Length == p.Content.ToString().Length)).ToList();
        //                    if (listwhere.Count > 0)
        //                    {
        //                        //for (int a = 0;a< listwhere.Count;a++)
        //                        //{
        //                        //    string queryTitle = listwhere[a].Title;
        //                        //    if (queryTitle.Contains(column7) == true)
        //                        //    { 
        //                        //        bre
        //                        //    }
        //                        //}
        //                        ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10, "该试题已经存在");
        //                    }
        //                    else
        //                    {
        //                        #region ==添加试题库
        //                        builder.Append(insertSql);
        //                        builder.Append(" values (");
        //                        string ID = Guid.NewGuid().ToString();
        //                        builder.Append("'" + ID + "',");
        //                        //添加value赋值
        //                        builder.Append(builderValue.ToString());
        //                        //经纬度
        //                        builder.AppendFormat("'{0}',", CurrentMaster.Id);
        //                        builder.AppendFormat("{0},", (int)StatusEnum.正常);

        //                        builder.AppendFormat("{0},", StoreType);

        //                        //去掉最后一个“,”
        //                        builder.Length = builder.Length - 1;
        //                        builder.Append(");");
        //                        #endregion
        //                        //添加试题答案sql
        //                        if ("单选题".Contains(column4) || "多选题".Contains(column4))
        //                        {
        //                            var Answer = column8.Split('|'); //标准答案
        //                            for (int a = 0; a < Answer.Length; a++)
        //                            {
        //                                //标准答案 column9
        //                                if (!string.IsNullOrEmpty(Answer[a]))
        //                                {
        //                                    int IsAnswer =0;
        //                                    string charA=ConvertTo.NumbertoString((a+1)).ToLower();
        //                                    if (column9.ToLower().Contains(charA))
        //                                    {
        //                                        IsAnswer = 1;
        //                                    }
        //                                    builder.Append("insert into bs_TestStoreAnswer values(NEWID(),'" + ID + "','" + Answer[a] + "'," + a + "," + IsAnswer + ");");
        //                                }
        //                            }
        //                        }
        //                        Insertcount++;
        //                    }
        //                }
        //                #endregion
        //            }
        //            #region 执行sql
        //            if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
        //            {
        //                rowAffected += ExcelRender.DBImportFun(builder.ToString());
        //                logError.ErrorFormat(builder.ToString());
        //                builder.Length = 0;
        //            }
        //            #endregion
        //        }
        //        if (ErrorTab.Rows.Count >= 1)
        //        {
        //            tab = ErrorTab;
        //        }
        //        else
        //        {
        //            tab = null;
        //        }
        //        return Insertcount;
        //    }
        //}
        //#endregion

        #region 数据校验
        public static BllSysCategory bllCategory = new BllSysCategory();
        public static List<ModSysCategory> SysCategory = new List<ModSysCategory>();
       
       /// <summary>
        /// 数据校验空
       /// </summary>
        /// <param name="testType">试题类型</param>
        /// <param name="typeName">试题类型名称</param>
        /// <param name="CurrentMaster">当前登录用户</param>
        /// <param name="val">Excel单元格值</param>
        /// <param name="j"></param>
        /// <param name="newid">返回类别主键</param>
        /// <param name="errorStr">返回错误提示</param>
       /// <returns></returns>
        public static bool RowDataVis(string testType,ModSysMaster CurrentMaster, string val, int j,string parentId, ref string newid, ref string errorStr)
        {
            bool flag = false;
            switch (j)
            {
                case 0:  //验证产品线
                    if (val.Length > 50 || val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                    {
                        flag = true;
                        errorStr = "验证产品线名称不能为空";
                    }
                    else
                    {
                        List<ModSysCategory> newCategory = SysCategory.Where(p => (p.Name == val && p.ParentCategoryId == parentId)).ToList();
                        if (newCategory.Count <= 0)
                        {
                            //知识点:2 ,产品类型:0 证书类型:1
                            //newid = AddGroup(j, val, CurrentMaster, parentId, true);
                            flag = true;
                            errorStr = "验证产品线名称不存在";
                        }
                        else
                        {
                            newid = newCategory[0].Id;
                        }
                    }
                    break;
                case 1:  //证书类型
                    if (val.Length > 50 || val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                    {
                        flag = true;
                        errorStr = "证书类型不能为空";
                    }
                    else
                    {
                        List<ModSysCategory> newCategory = SysCategory.Where(p => (p.Name == val && p.ParentCategoryId == parentId)).ToList();
                        if (newCategory.Count <= 0)
                        {
                            //知识点:2 ,产品类型:0 证书类型:1
                            //newid = AddGroup(j, val, CurrentMaster, parentId, true);
                            flag = true;
                            errorStr = "证书类型名称不存在";
                        }
                        else
                        {
                            newid = newCategory[0].Id;
                        }
                    }
                    break;
                case 2:  //技术领域
                    if (val.Length > 50 || val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                    {
                        flag = true;
                        errorStr = "技术领域不能为空";
                    }
                    else
                    {
                        List<ModSysCategory> newCategory = SysCategory.Where(p => (p.Name == val && p.ParentCategoryId == parentId)).ToList();
                        if (newCategory.Count <= 0)
                        {
                            //知识点:3 ,产品类型:0 证书类型:1 技术领域:2
                            //newid = AddGroup(j, val, CurrentMaster, parentId, true);
                            flag = true;
                            errorStr = "技术领域名称不存在";
                        }
                        else
                        {
                            newid = newCategory[0].Id;
                        }
                    }
                    break;
                case 3:  //知识点
                    if (val.Length > 50 || val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                    {
                        flag = true;
                        errorStr = "知识点不能为空";
                    }
                    else
                    {
                        List<ModSysCategory> newCategory = SysCategory.Where(p => (p.Name == val && p.ParentCategoryId == parentId)).ToList();
                        if (newCategory.Count <= 0)
                        {
                            //知识点:2 ,产品类型:0 证书类型:1
                            //newid = AddGroup(j, val, CurrentMaster, parentId, false);
                            flag = true;
                            errorStr = "知识点名称不存在";
                        }
                        else
                        {
                            newid = newCategory[0].Id;
                        }
                    }
                    break;
                case 4:  //题型类型名称
                    #region
                    if (val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                    {
                        flag = true;
                        errorStr = "试题类型名称不能为空";
                    }
                    else {
                        if (!"单选题、多选题、判断题、填空题,问答题".Contains(val))
                        {
                            flag = true;
                            errorStr = "题型类型错误,只能是单选题、多选题、判断题、填空题或者问答题";
                        }
                    }
#endregion
                    break;
                case 5:  //分级
                    #region
                    if (val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                    {
                        flag = true;
                        errorStr = "分级不能为空";
                    }
                    else
                    {
                        if (!"初级、中级、高级".Contains(val))
                        {
                            flag = true;
                            errorStr = "分级类型错误,只能是初级、中级、高级";
                        }
                    }
                #endregion
                    break;
                case 6:  //试题分数
                    #region
                    if (val.Length !=0) //验证是否是正确数字 0-100
                    {
                        if (ValidateHelper.IsNumber(val))//是否数字字符串
                        {
                            if (int.Parse(val) < 0 || int.Parse(val) >100)
                            flag = true;
                            errorStr = "有效分数在啊0-100之间";
                        }
                        else {
                            flag = true;
                            errorStr = "分数数字错误,只能是整形数字";
                        }
                    }
                    #endregion
                    break;
                case 7:  //试题内容
                    #region
                    if (!"判断题".Contains(testType))//只有判断题才不校验试题内容是否为空
                    {
                        if (val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                        {
                            flag = true;
                            errorStr = "试题内容不能为空";
                        }
                    }
                    #endregion
                    break;
                case 8:  //试题选项
                    #region
                    if (!"判断题".Contains(testType))//只有判断题才不校验试题内容是否为空
                    {
                        if (val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                        {
                            flag = true;
                            errorStr = "试题选项不能为空";
                        }
                    }
                  
                    #endregion
                    break;
                case 9:  //标准答案
                    if (val.Length == 0 || string.IsNullOrEmpty(val.Trim())) //验证长度是否合理
                    {
                        flag = true;
                        errorStr = "标准答案不能为空";
                    }
                    break;
                case 10:  //试题解析
                    break;
                default:
                    break;
            }

            return flag;
        }


        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="type">知识点:0 ,产品类型:2 证书类型:3</param>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public static string AddGroup(int type, string name, ModSysMaster CurrentMaster,string ParentId,bool HasChild)
        {
            ModSysCategory t = new ModSysCategory();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.CompanyId = CurrentMaster.Company.Id;
            t.Path = "";
            t.IsSystem = false;
            t.CreaterId = CurrentMaster.Id;
            t.CreaterName = CurrentMaster.UserName;
            t.ParentCategoryId = ParentId;
            t.Name = name;
            t.OrderNum ="0";
            t.HasChild = HasChild;
            t.Depth = type;
            bllCategory.Insert(t);
            SysCategory.Add(t);
            return t.Id;
        }

        #endregion

        //错误日志
        public static SharePresentationLog logError = new SharePresentationLog();

        #region ===获取经纬度
        /// <summary>
        /// 根据地质获取经纬度
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public delegate string Map(string sql);
        public static string MapLngLat(string httpUrl)
        {
            //SharePresentationLog logError = new SharePresentationLog();
            StringBuilder builder = new StringBuilder();
            try
            {
                //创建一个HTTP请求
                System.Net.HttpWebRequest requestAgin = (System.Net.HttpWebRequest)WebRequest.Create(httpUrl);
                requestAgin.Method = "GET";
                requestAgin.Timeout = 3000;
                StreamReader jsonStreamAgin = new StreamReader(requestAgin.GetResponse().GetResponseStream());
                string jsonObjectAgin = jsonStreamAgin.ReadToEnd();

                XDocument doc = XDocument.Parse(jsonObjectAgin);
                IEnumerable<XElement> latLngList = doc.Root.Element("result").Elements().Where<XElement>(x => x.Name == "location").FirstOrDefault().Elements();

                var lat = latLngList.Where<XElement>(x => x.Name == "lat").FirstOrDefault();
                var lng = latLngList.Where<XElement>(x => x.Name == "lng").FirstOrDefault();

                if (lat != null && lng != null)
                {
                    builder.AppendFormat("'{0}',", lng.Value);
                    builder.AppendFormat("'{0}',", lat.Value);
                }
            }
            catch (Exception ex)
            {
                //logError.Error(ex, "导入数据请求经纬度错误");
                builder.AppendFormat("'{0}',", 0);
                builder.AppendFormat("'{0}',", 0);
            }
            return builder.ToString();
        }

        #endregion

    }
}




