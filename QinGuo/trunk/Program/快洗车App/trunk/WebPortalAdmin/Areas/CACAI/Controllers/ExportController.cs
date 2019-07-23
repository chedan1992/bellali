using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using System.Data;
using WebPortalAdmin.Controllers;
using WebPortalAdmin.Code;
using System.IO;
using QINGUO.Business;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;
using System.Text.RegularExpressions;
using QINGUO.Common;

namespace WebPortalAdmin.Areas.CACAI.Controllers
{
    public class ExportController : BaseController<ModSysCompany>
    {
        /// <summary>
        /// 供应商导入
        /// </summary>
        /// <returns></returns>
        public ActionResult Supplier()
        {
            return View();
        }
        /// <summary>
        /// 采购入库单
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderIn()
        {
            return View();
        }
        /// <summary>
        /// 采购退货单
        /// </summary>
        /// <returns></returns>
        public ActionResult Purchase()
        {
            return View();
        }


        #region ===导入 ImportDate
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void ImportDate()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                //获取文件
                HttpPostedFileBase excel = Request.Files["excel"];
                DataTable errortab = null;
                string errorMsg = "";
                int rowAffected = ExportData(excel.FileName, excel.InputStream, out errortab, out errorMsg, CurrentMaster);
                if (rowAffected > 0)
                {
                    json.msg = "成功导入" + rowAffected + "条数据！";
                    json.success = true;
                    LogInsert(OperationTypeEnum.操作, "供应商数据导入", "导入操作成功.");
                }
                else
                {
                    json.msg = "导入失败,导入" + rowAffected + "条数据！";
                    json.success = false;
                }
                if (errortab != null)
                {
                    json.success = false;
                    string savepath = System.Web.HttpContext.Current.Server.MapPath("/Project/Template/Error/");
                    if (!Directory.Exists(savepath))
                        Directory.CreateDirectory(savepath);

                    string FileName = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();
                    ExcelRender.RenderToExcel(errortab, Request.MapPath("/Project/Template/Error/" + FileName + "Error.xls"));
                    json.data = "\"" + "/Project/Template/Error/" + FileName + "Error.xls\"";
                    if (rowAffected > 0)
                    {
                        json.msg = "成功导入" + rowAffected + "条数据！错误" + errortab.Rows.Count + "条数据.";
                    }
                    else
                    {
                        json.msg = "导入失败,导入" + rowAffected + "条数据！错误" + errortab.Rows.Count + "条数据.";
                    }
                }
                if (errorMsg != "")
                {
                    json.success = false;
                    json.msg = "导入异常,异常原因：" + errorMsg;
                }
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "供应商数据导入", "操作异常信息:" + ex);
                json.msg = "导入异常,异常原因：" + ex.Message;
            }

            WriteJsonToPage(json.ToString());
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string ToDateTime(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                //校验是否全数字
                if (IsNumber(value) == true)
                {
                    return ToDateTimeValue(value);
                }
                else if (IsDate(value) == true)
                {
                    return Convert.ToDateTime(value).ToString();
                }
                else
                {
                    return "";
                }
            }
            return "";
        }
        /// <summary>
        /// 数字转换时间格式
        /// </summary>
        /// <param name="timeStr">数字,如:42095.7069444444/0.650694444444444</param>
        /// <returns>日期/时间格式</returns>
        private string ToDateTimeValue(string strNumber)
        {
            if (!string.IsNullOrWhiteSpace(strNumber))
            {
                Decimal tempValue;
                //先检查 是不是数字;
                if (Decimal.TryParse(strNumber, out tempValue))
                {
                    //天数,取整
                    int day = Convert.ToInt32(Math.Truncate(tempValue));
                    //这里也不知道为什么. 如果是小于32,则减1,否则减2
                    //日期从1900-01-01开始累加 
                    // day = day < 32 ? day - 1 : day - 2;
                    DateTime dt = new DateTime(1900, 1, 1).AddDays(day < 32 ? (day - 1) : (day - 2));

                    //小时:减掉天数,这个数字转换小时:(* 24) 
                    Decimal hourTemp = (tempValue - day) * 24;//获取小时数
                    //取整.小时数
                    int hour = Convert.ToInt32(Math.Truncate(hourTemp));
                    //分钟:减掉小时,( * 60)
                    //这里舍入,否则取值会有1分钟误差.
                    Decimal minuteTemp = Math.Round((hourTemp - hour) * 60, 2);//获取分钟数
                    int minute = Convert.ToInt32(Math.Truncate(minuteTemp));
                    //秒:减掉分钟,( * 60)
                    //这里舍入,否则取值会有1秒误差.
                    Decimal secondTemp = Math.Round((minuteTemp - minute) * 60, 2);//获取秒数
                    int second = Convert.ToInt32(Math.Truncate(secondTemp));
                    //时间格式:00:00:00
                    string resultTimes = string.Format("{0}:{1}:{2}",
                            (hour < 10 ? ("0" + hour) : hour.ToString()),
                            (minute < 10 ? ("0" + minute) : minute.ToString()),
                            (second < 10 ? ("0" + second) : second.ToString()));

                    if (day > 0)
                        return string.Format("{0} {1}", dt.ToString("yyyy-MM-dd HH:ii:ss"), resultTimes);
                    else
                        return resultTimes;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public bool IsDate(string date)
        {
            //如果为空，认为验证合格
            if (string.IsNullOrEmpty(date))
            {
                return true;
            }
            //清除要验证字符串中的空格
            date = date.Trim();
            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");
            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }
            //判断是否是纯数字
            if (IsNumber(date))
            {
                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }
                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }
                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }
                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }
                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }
                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion
            }
            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 校验是否是数字
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public bool IsNumber(String strNumber)
        {
            if (!string.IsNullOrEmpty(strNumber))
            {
                Regex objNotNumberPattern = new Regex("[^0-9.-]");
                Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
                Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
                String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
                String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
                Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

                return !objNotNumberPattern.IsMatch(strNumber) &&
                       !objTwoDotPattern.IsMatch(strNumber) &&
                       !objTwoMinusPattern.IsMatch(strNumber) &&
                       objNumberPattern.IsMatch(strNumber);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Excel文档导入到数据库
        /// 默认取Excel的第一个表
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <returns></returns>
        public int ExportData(string FileName, Stream excelFileStream, out DataTable tab, out string ErrorMsg, ModSysMaster CurrentMaster)
        {
            string CType = Request["CType"].ToString();
            string insertSql = "";
            DataTable dt = null;
            string error = "";
            int result = 0;
            switch (int.Parse(CType))
            {
                case 1: //导入供应商
                    insertSql = @"insert into Sys_Company(
	                                   Id
                                      ,Name
                                      ,NameTitle
                                      ,[Address]
                                      ,Phone
                                      ,Code
                                      ,[Level]
                                      ,Attribute
                                      ,[Type]
                                      ,[Status]
                                      ,CreateCompanyId
                                      ,CreaterUserId
                                      ,CreateTime
                                      ,Tel
                                      ,CheckoutType
                                      ,PaymentType
                                      ,AccountName
                                      ,AccountNum
                                      )";
                    result = RenderToCompany(int.Parse(CType), FileName, excelFileStream, insertSql, 0, 0, out dt, out error, CurrentMaster);
                    break;
                case 2:
                    result = RenderToOrderIn(int.Parse(CType), FileName, excelFileStream, 0, 0, out dt, out error, CurrentMaster);
                    break;
                case 3:
                    result = RenderToPurchase(int.Parse(CType), FileName, excelFileStream, 0, 0, out dt, out error, CurrentMaster);
                    break;
            }
            tab = dt;
            ErrorMsg = error;
            return result;
        }

        #region ===Excel文档导入供应商
        /// <summary>
        /// Excel文档导入到数据库
        /// </summary>
        /// <param name="ctype">合同类型（1：收入合同 2：支出合同）</param>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <param name="shopTypeid">商家分类</param>
        /// <returns></returns>

        public int RenderToCompany(int ctype, string FileName, Stream excelFileStream, string insertSql, int sheetIndex, int headerRowIndex, out DataTable tab, out string ErrorMsg, ModSysMaster CurrentMaster)
        {

             /// <summary>
            /// 供应商列表
            /// </summary>
              List<ModSysCompany> DicCompany = new List<ModSysCompany>();
            /// <summary>
            /// 获取供应商分类
            /// </summary>
              List<ModSysDirc> DicSys = new List<ModSysDirc>();

            #region ===构建错误表头
            DataTable ErrorTab = new DataTable();
            DataColumn 供应商内部编号 = new DataColumn("供应商内部编号", Type.GetType("System.String"));
            DataColumn 供应商名 = new DataColumn("供应商名", Type.GetType("System.String"));
            DataColumn 供应商编码 = new DataColumn("供应商编码", Type.GetType("System.String"));
            DataColumn 启用 = new DataColumn("启用", Type.GetType("System.String"));
            DataColumn 联系地址 = new DataColumn("联系地址", Type.GetType("System.String"));
            DataColumn 供应商分类 = new DataColumn("供应商分类", Type.GetType("System.String"));
            DataColumn 结账方式 = new DataColumn("结账方式", Type.GetType("System.String"));
            DataColumn 电话 = new DataColumn("电话", Type.GetType("System.String"));
            DataColumn 手机 = new DataColumn("手机", Type.GetType("System.String"));
            DataColumn 付款方式 = new DataColumn("付款方式", Type.GetType("System.String"));
            DataColumn 名字 = new DataColumn("名字", Type.GetType("System.String"));
            DataColumn 账号 = new DataColumn("账号", Type.GetType("System.String"));
            DataColumn 创建时间 = new DataColumn("创建时间", Type.GetType("System.String"));
            DataColumn 错误原因 = new DataColumn("错误原因", Type.GetType("System.String"));

            ErrorTab.Columns.Add(供应商内部编号);
            ErrorTab.Columns.Add(供应商名);
            ErrorTab.Columns.Add(供应商编码);
            ErrorTab.Columns.Add(启用);
            ErrorTab.Columns.Add(联系地址);
            ErrorTab.Columns.Add(供应商分类);
            ErrorTab.Columns.Add(结账方式);
            ErrorTab.Columns.Add(电话);
            ErrorTab.Columns.Add(手机);
            ErrorTab.Columns.Add(付款方式);
            ErrorTab.Columns.Add(名字);
            ErrorTab.Columns.Add(账号);
            ErrorTab.Columns.Add(创建时间);
            ErrorTab.Columns.Add(错误原因);
            #endregion

            //当前用户公司编号
            string CompanyId = CurrentMaster.Cid;
            //获取所有列表
            DicCompany = new BllSysCompany().QueryToAll().Where(p => p.Attribute == (int)CompanyType.供应商).ToList();
            //获取供应商分类
            DicSys = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 1).ToList();

            int rowAffected = 0;
            using (excelFileStream)//读取流
            {
                try
                {
                    //根据类型查询题库信息
                    string sExtension = FileName.Substring(FileName.LastIndexOf('.'));//获取拓展名
                    IWorkbook workbook = null;
                    if (sExtension == ".xls")
                    {
                        workbook = new HSSFWorkbook(excelFileStream);
                    }
                    else if (sExtension == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(excelFileStream);
                    }
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);
                    StringBuilder builder = new StringBuilder();
                    IRow headerRow = sheet.GetRow(headerRowIndex);
                    int cellCount = headerRow.LastCellNum;//
                    int rowCount = sheet.LastRowNum;//
                    int Insertcount = 0;  //插入总行数
                    if (cellCount != 13)
                    {
                        ErrorMsg = "上传Excel与模板不服，请检查。";
                        tab = null;
                        return 0;
                    }
                    else
                    {
                        //循环表格
                        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            //判断空行
                            if (row != null)
                            {
                                #region  数据验证判断

                                var column0 = ExcelRender.GetCellValue(row.GetCell(0)).Replace("'", "''");//供应商内部编号
                                var column1 = ExcelRender.GetCellValue(row.GetCell(1)).Replace("'", "''");//供应商名
                                var column2 = ExcelRender.GetCellValue(row.GetCell(2)).Replace("'", "''");//供应商编码
                                var column3 = ExcelRender.GetCellValue(row.GetCell(3)).Replace("'", "''");//启用
                                var column4 = ExcelRender.GetCellValue(row.GetCell(4)).Replace("'", "''");//联系地址
                                var column5 = ExcelRender.GetCellValue(row.GetCell(5)).Replace("'", "''"); //供应商分类
                                var column6 = ExcelRender.GetCellValue(row.GetCell(6)).Replace("'", "''");//结账方式
                                var column7 = ExcelRender.GetCellValue(row.GetCell(7)).Replace("'", "''");///电话
                                var column8 = ExcelRender.GetCellValue(row.GetCell(8)).Replace("'", "''");//手机
                                var column9 = ExcelRender.GetCellValue(row.GetCell(9)).Replace("'", "''"); //付款方式
                                var column10 = ExcelRender.GetCellValue(row.GetCell(10)).Replace("'", "''");//名字
                                var column11 = ExcelRender.GetCellValue(row.GetCell(11)).Replace("'", "''"); //账号
                                var column12 = ToDateTime(ExcelRender.GetCellValue(row.GetCell(12)).Replace("'", "''"));//创建时间


                                string errorStr = "";
                                string DicCompanyId = ""; //供应商ID
                                string DicSysId = "";//供应商分类ID

                                //校验信息是否完整 (合同名称,合同性质,项目,项目阶段,合同状态阶段)
                                if (CheckSyscompan(CurrentMaster, column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10, column11, CompanyId, ref  DicCompanyId, ref  DicSysId, ref errorStr, ref DicCompany, ref DicSys) == true)
                                {
                                    //校验不通过
                                    if (errorStr != "供应商状态已更改")
                                    {
                                        if (column0 == "" && column1 == "" && column2 == "" && column4 == "" && column5 == "")
                                        {
                                            break;//遇到空行,直接跳转
                                        }
                                        else
                                        {
                                            ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                                column11, column12, errorStr);
                                        }
                                    }
                                }
                                else
                                {
                                    #region===校验是否有效日期
                                    if (!IsDate(column12))
                                    {
                                        errorStr = "创建时间不是有效日期。";
                                    }

                                    #endregion
                                    if (!string.IsNullOrEmpty(errorStr))
                                    {
                                        ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                              column11, column12, errorStr);
                                    }
                                    else
                                    {
                                        string newId = Guid.NewGuid().ToString();//主键编号
                                        #region ==添加信息
                                        builder.Append(insertSql);
                                        builder.Append(" values (");
                                        //主键
                                        builder.AppendFormat("'{0}',", newId);
                                        //供应商名
                                        builder.AppendFormat("'{0}',", column1);
                                        //供应商内部编号
                                        builder.AppendFormat("'{0}',", column0);
                                        //联系地址
                                        builder.AppendFormat("'{0}',", column4);
                                        //电话
                                        builder.AppendFormat("'{0}',", column7);
                                        //供应商编码
                                        builder.AppendFormat("'{0}',", column2);
                                        //[Level]
                                        builder.AppendFormat("{0},", 0);
                                        //Attribute
                                        builder.AppendFormat("{0},", (int)CompanyType.供应商);
                                        //供应商分类
                                        builder.AppendFormat("'{0}',", DicSysId);
                                        //启用
                                        int Status = (int)StatusEnum.正常;
                                        if (!string.IsNullOrEmpty(column3))
                                        {
                                            if (column3.ToLower() == "false")
                                            {
                                                Status = (int)StatusEnum.禁用;
                                            }
                                        }
                                        builder.AppendFormat("'{0}',", Status);
                                        //CreateCompanyId
                                        builder.AppendFormat("'{0}',", CurrentMaster.Cid);
                                        //CreaterUserId
                                        builder.AppendFormat("'{0}',", CurrentMaster.Id);
                                        //创建时间
                                        builder.AppendFormat("{0},", string.IsNullOrEmpty(column12) == true ? "getdate()" : "'" + column12 + "'");
                                        //手机
                                        builder.AppendFormat("'{0}',", column8);
                                        //结账方式
                                        int Checkout = 2;//结账方式 0未设置 1月结 2日结
                                        if (!string.IsNullOrEmpty(column6))
                                        {
                                            switch (column6)
                                            {
                                                case "月结":
                                                    Checkout = 1;
                                                    break;
                                                case "日结":
                                                    Checkout = 2;
                                                    break;
                                            }
                                        }
                                        builder.AppendFormat("'{0}',", Checkout);
                                        //付款方式
                                        int Payment = 0;//付款方式：0:未设置 1支付宝 2工行  3农行
                                        if (!string.IsNullOrEmpty(column9))
                                        {
                                            switch (column9)
                                            {
                                                case "支付宝":
                                                    Payment = 1;
                                                    break;
                                                case "工行":
                                                    Payment = 2;
                                                    break;
                                                case "农行":
                                                    Payment = 3;
                                                    break;
                                            }
                                        }
                                        builder.AppendFormat("'{0}',", Payment);
                                        //名字
                                        builder.AppendFormat("'{0}',", column10);
                                        //账号
                                        builder.AppendFormat("'{0}',", column11);

                                        //去掉最后一个“,”
                                        builder.Length = builder.Length - 1;
                                        builder.Append(");");
                                        #endregion
                                        Insertcount++;
                                    }
                                }
                                if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                                {
                                    //LogErrorRecord.Debug("供应商导入sql:" + builder.ToString());
                                    rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                                    builder.Clear();
                                    builder.Length = 0;

                                }
                                #endregion
                            }
                        }

                        if (builder.Length > 0)
                        {
                            //LogErrorRecord.Debug("供应商导入sql:" + builder.ToString());
                            rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                            builder.Clear();
                            builder.Length = 0;
                        }
                        if (ErrorTab.Rows.Count >= 1)
                        {
                            tab = ErrorTab;
                        }
                        else
                        {
                            tab = null;
                        }

                        if (rowAffected == 0)
                        {
                            Insertcount = 0;
                        }
                        ErrorMsg = "";
                        return Insertcount;
                    }
                }
                catch (Exception ex)
                {
                    if (ErrorTab.Rows.Count >= 1)
                    {
                        tab = ErrorTab;
                    }
                    else
                    {
                        tab = null;
                    }
                    ErrorMsg = "供应商导入异常：" + ex.Message;
                    return 0;
                }
            }
        }


        /// <summary>
        /// 校验供应商信息
        /// </summary>
        /// <param name="CurrentMaster"></param>
        /// <param name="val1">供应商内部编号</param>
        /// <param name="val2">供应商名</param>
        /// <param name="val3">供应商编码</param>
        /// <param name="val4">启用</param>
        /// <param name="val5">联系地址</param>
        /// <param name="val6">供应商分类</param>
        /// <param name="val7">结账方式</param>
        /// <param name="val8">电话</param>
        /// <param name="val9">手机</param>
        /// <param name="val10">付款方式</param>
        /// <param name="val11">名字</param>
        /// <param name="val12">账号</param>
        /// <param name="CompanyId">添加公司</param>
        /// <param name="DicCompanyId">返回供应商ID</param>
        /// <param name="SysDirc0Id">返回供应商类别</param>
        /// <param name="errorStr"></param>
        /// <returns></returns>
        public bool CheckSyscompan(ModSysMaster CurrentMaster, string val1, string val2, string val3, string val4, string val5, string val6, string val7, string val8, string val9, string val10, string val11, string val12, string CompanyId, ref string DicCompanyId, ref string SysDirc0Id, ref string errorStr, ref List<ModSysCompany> DicCompany,ref List<ModSysDirc> DicSys)
        {
            bool flag = false;
            bool AddCom = false;
            bool AddDirc = false;
            #region ===校验供应商内部编号
            if (val1.Length == 0 || string.IsNullOrEmpty(val1.Trim()) || val1.Length == 0 || string.IsNullOrEmpty(val1.Trim())) //验证长度是否合理
            {

                //flag = true;
                //errorStr += "供应商内部编号不能为空,为必填项";
                //return flag;
            }
            else
            {
                if (val1.Length > 50 || val1.Length > 50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "供应商内部编号长度过长,有效范围1-100字符.";
                    return flag;
                }
            }
            #endregion

            #region ===校验供应商名
            if (val2.Length == 0 || string.IsNullOrEmpty(val2.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "供应商名不能为空,为必填项.";
                return flag;
            }
            else {
                AddCom = true;
            }
            #endregion

            //#region ===校验供应商编码
            //if (val3.Length == 0 || string.IsNullOrEmpty(val3.Trim())) //验证长度是否合理
            //{
            //    flag = true;
            //    errorStr += "供应商编码不能为空,为必填项.";
            //    return flag;
            //}
            //#endregion

            #region ===校验供应商分类
            if (val6.Length == 0 || string.IsNullOrEmpty(val6.Trim())) //验证长度是否合理
            {
                AddDirc = false;
                //flag = true;
                //errorStr += "供应商分类不能为空,为必填项.";
                //return flag;
            }
            else
            {
                AddDirc = true;
            }
            #endregion

            //添加供应商分类
            if (AddDirc == true)
            {
                List<ModSysDirc> newCategory = DicSys.Where(p => p.Type == 1 && p.Name.Trim() == val6.Trim() && p.Status!=(int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    SysDirc0Id = AddSysDirc(1, val6, CurrentMaster, CompanyId, ref DicSys);
                }
                else
                {
                    SysDirc0Id = newCategory[0].Id;
                }
            }
            string dicId = SysDirc0Id;
            //添加供应商名
            if (AddCom == true)
            {
                List<ModSysCompany> newCategory = DicCompany.Where(p => p.Name == val2.Trim()&&p.Status!=(int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    //查询删除的
                    newCategory = DicCompany.Where(p => p.Name == val2.Trim()).ToList();
                    if (newCategory.Count > 0)
                    {
                        flag = true;
                        if (newCategory[0].Status == (int)StatusEnum.删除)
                        {
                            //删除的供应商改为正常状态
                            int Checkout = 2;//结账方式 0未设置 1月结 2日结
                            if (!string.IsNullOrEmpty(val7))
                            {
                                switch (val7)
                                {
                                    case "月结":
                                        Checkout = 1;
                                        break;
                                    case "日结":
                                        Checkout = 2;
                                        break;
                                }
                            }
                            //付款方式
                            int Payment = 0;//付款方式：0:未设置 1支付宝 2工行  3农行
                            if (!string.IsNullOrEmpty(val10))
                            {
                                switch (val10)
                                {
                                    case "支付宝":
                                        Payment = 1;
                                        break;
                                    case "工行":
                                        Payment = 2;
                                        break;
                                    case "农行":
                                        Payment = 3;
                                        break;
                                }
                            }
                            newCategory[0].Address = val5;
                            newCategory[0].Phone = val8;
                            newCategory[0].Code = val3;
                            newCategory[0].CheckoutType = Checkout;
                            newCategory[0].PaymentType = Payment;
                            newCategory[0].AccountName = val11;
                            newCategory[0].AccountNum = val12;
                            newCategory[0].Status = (int)StatusEnum.正常;
                            new BllSysCompany().Update(newCategory[0]);
                            errorStr = "供应商状态已更改";
                        }
                        return flag;
                    }
                    //DicCompanyId = AddCompany(val1, val2, val3, val4, val5, SysDirc0Id, val7, val8, val9, val10, val11, val12, (int)CompanyType.供应商, CurrentMaster, CompanyId);
                }
                else
                {
                    flag = true;
                    errorStr += "供应商名已经存在，不能导入相同的数据.";
                    return flag;
                    //DicCompanyId = newCategory[0].Id;
                }
            }

            return flag;
        }

        #endregion

        #region ===Excel文档导入采购订单
        
        /// <summary>
        /// Excel文档导入到数据库
        /// </summary>
        /// <param name="ctype">合同类型（1：收入合同 2：支出合同）</param>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <param name="shopTypeid">商家分类</param>
        /// <returns></returns>

        public int RenderToOrderIn(int ctype, string FileName, Stream excelFileStream, int sheetIndex, int headerRowIndex, out DataTable tab, out string ErrorMsg, ModSysMaster CurrentMaster)
        {

            /// <summary>
        /// 供应商列表
        /// </summary>
          List<ModSysCompany> DicGYS = new List<ModSysCompany>();
        /// <summary>
        /// 仓库分类
        /// </summary>
          List<ModSysDirc> DicCK = new List<ModSysDirc>();
        /// <summary>
        /// 人员列表
        /// </summary>
          List<ModSysMaster> DicMaster = new List<ModSysMaster>();
        /// <summary>
        /// 商品列表
        /// </summary>
          List<ModShopGoods> DicGood = new List<ModShopGoods>();
            /// <summary>
            ///主表信息
            /// </summary>
           List<ModHOrderIn> ListMain = new List<ModHOrderIn>();
           /// <summary>
           ///明细表信息
           /// </summary>
           List<ModHOrderInDetail> ListDetail = new List<ModHOrderInDetail>();
            #region ===构建错误表头
            DataTable ErrorTab = new DataTable();
            DataColumn 入仓单号 = new DataColumn("入仓单号", Type.GetType("System.String"));
            DataColumn 仓库 = new DataColumn("仓库", Type.GetType("System.String"));
            DataColumn 供应商 = new DataColumn("供应商", Type.GetType("System.String"));
            DataColumn 采购单号 = new DataColumn("采购单号", Type.GetType("System.String"));
            DataColumn 单据日期 = new DataColumn("单据日期", Type.GetType("System.String"));
            DataColumn 状态 = new DataColumn("状态", Type.GetType("System.String"));
            DataColumn 备注 = new DataColumn("备注", Type.GetType("System.String"));
            DataColumn 物流单号 = new DataColumn("物流单号", Type.GetType("System.String"));
            DataColumn 商品编码 = new DataColumn("商品编码", Type.GetType("System.String"));
            DataColumn 商品名称 = new DataColumn("商品名称", Type.GetType("System.String"));
            DataColumn 颜色及规格 = new DataColumn("颜色及规格", Type.GetType("System.String"));
            DataColumn 数量 = new DataColumn("数量", Type.GetType("System.String"));
            DataColumn 单价 = new DataColumn("单价", Type.GetType("System.String"));
            DataColumn 金额 = new DataColumn("金额", Type.GetType("System.String"));
            DataColumn 批次号 = new DataColumn("批次号", Type.GetType("System.String"));
            DataColumn 入库明细备注 = new DataColumn("入库明细备注", Type.GetType("System.String"));
            DataColumn 款式编号 = new DataColumn("款式编号", Type.GetType("System.String"));
            DataColumn 供应商货号 = new DataColumn("供应商货号", Type.GetType("System.String"));
            DataColumn 制单人 = new DataColumn("制单人", Type.GetType("System.String"));
            DataColumn 财审人 = new DataColumn("财审人", Type.GetType("System.String"));
            DataColumn 财审日期 = new DataColumn("财审日期", Type.GetType("System.String"));
            DataColumn 错误原因 = new DataColumn("错误原因", Type.GetType("System.String"));

            ErrorTab.Columns.Add(入仓单号);
            ErrorTab.Columns.Add(仓库);
            ErrorTab.Columns.Add(供应商);
            ErrorTab.Columns.Add(采购单号);
            ErrorTab.Columns.Add(单据日期);
            ErrorTab.Columns.Add(状态);
            ErrorTab.Columns.Add(备注);
            ErrorTab.Columns.Add(物流单号);
            ErrorTab.Columns.Add(商品编码);
            ErrorTab.Columns.Add(商品名称);
            ErrorTab.Columns.Add(颜色及规格);
            ErrorTab.Columns.Add(数量);
            ErrorTab.Columns.Add(单价);
            ErrorTab.Columns.Add(金额);
            ErrorTab.Columns.Add(批次号);
            ErrorTab.Columns.Add(入库明细备注);
            ErrorTab.Columns.Add(款式编号);
            ErrorTab.Columns.Add(供应商货号);
            ErrorTab.Columns.Add(制单人);
            ErrorTab.Columns.Add(财审人);
            ErrorTab.Columns.Add(财审日期);
            ErrorTab.Columns.Add(错误原因);
            #endregion

            //当前用户公司编号
            string CompanyId = CurrentMaster.Cid;
            //获取所有供应商列表
            DicGYS = new BllSysCompany().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Attribute == (int)CompanyType.供应商).ToList();
            //获取DicCK分类
            DicCK = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 0).ToList();
            //人员列表
            DicMaster = new BllSysMaster().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Attribute == (int)AdminTypeEnum.系统管理员).ToList();
            //商品列表
            DicGood = new BllShopGoods().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除).ToList();

           
            int rowAffected = 0;
            using (excelFileStream)//读取流
            {
                try
                {
                    //根据类型查询题库信息
                    string sExtension = FileName.Substring(FileName.LastIndexOf('.'));//获取拓展名
                    IWorkbook workbook = null;
                    if (sExtension == ".xls")
                    {
                        workbook = new HSSFWorkbook(excelFileStream);
                    }
                    else if (sExtension == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(excelFileStream);
                    }
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);
                    StringBuilder builder = new StringBuilder();
                    IRow headerRow = sheet.GetRow(headerRowIndex);
                    int cellCount = headerRow.LastCellNum;//
                    int rowCount = sheet.LastRowNum;//
                    int Insertcount = 0;  //插入总行数

                    if (cellCount != 21)
                    {
                        ErrorMsg = "上传Excel与模板不服，请检查。";
                        tab = null;
                        return 0;
                    }
                    else
                    {
                        //循环表格
                        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            //判断空行
                            if (row != null)
                            {
                                #region  数据验证判断
                                var column0 = ExcelRender.GetCellValue(row.GetCell(0)).Replace("'", "''");//入仓单号
                                var column1 = ExcelRender.GetCellValue(row.GetCell(1)).Replace("'", "''");//仓库
                                var column2 = ExcelRender.GetCellValue(row.GetCell(2)).Replace("'", "''");//供应商
                                var column3 = ExcelRender.GetCellValue(row.GetCell(3)).Replace("'", "''");//采购单号
                                var column4 = ToDateTime(ExcelRender.GetCellValue(row.GetCell(4)).Replace("'", "''"));//单据日期
                                var column5 = ExcelRender.GetCellValue(row.GetCell(5)).Replace("'", "''"); //状态
                                var column6 = ExcelRender.GetCellValue(row.GetCell(6)).Replace("'", "''");//备注
                                var column7 = ExcelRender.GetCellValue(row.GetCell(7)).Replace("'", "''");///物流单号
                                var column8 = ExcelRender.GetCellValue(row.GetCell(8)).Replace("'", "''");//商品编码
                                var column9 = ExcelRender.GetCellValue(row.GetCell(9)).Replace("'", "''"); //商品名称
                                var column10 = ExcelRender.GetCellValue(row.GetCell(10)).Replace("'", "''");//颜色及规格
                                var column11 = ExcelRender.GetCellValue(row.GetCell(11)).Replace("'", "''"); //数量
                                var column12 = ExcelRender.GetCellValue(row.GetCell(12)).Replace("'", "''"); //单价
                                var column13 = ExcelRender.GetCellValue(row.GetCell(13)).Replace("'", "''"); //金额
                                var column14 = ExcelRender.GetCellValue(row.GetCell(14)).Replace("'", "''"); //批次号
                                var column15 = ExcelRender.GetCellValue(row.GetCell(15)).Replace("'", "''"); //入库明细备注
                                var column16 = ExcelRender.GetCellValue(row.GetCell(16)).Replace("'", "''"); //款式编号
                                var column17 = ExcelRender.GetCellValue(row.GetCell(17)).Replace("'", "''"); //供应商货号
                                var column18 = ExcelRender.GetCellValue(row.GetCell(18)).Replace("'", "''"); //制单人
                                var column19 = ExcelRender.GetCellValue(row.GetCell(19)).Replace("'", "''"); //财审人
                                var column20 = ToDateTime(ExcelRender.GetCellValue(row.GetCell(20)).Replace("'", "''"));//财审日期


                                string errorStr = "";
                                string DicCKId = ""; //仓库ID
                                string DicCompanyId = "";//供应商ID
                                string DicGoodId = "";//商品ID
                                string DicMasterId = "";//制单人人员ID
                                string DicCSRId = "";//财审人员ID

                                //校验信息是否完整
                                if (CheckOrderIn(CurrentMaster, column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10, column11, column12, column13, column14, column15, column16, column17, column18, column19, column20, CompanyId, ref  DicCKId, ref  DicCompanyId, ref DicGoodId, ref DicMasterId, ref DicCSRId, ref errorStr, ref DicGYS, ref DicCK, ref DicMaster, ref DicGood) == true)
                                {
                                    //校验不通过
                                    if (column0 == "" && column1 == "" && column2 == "" && column4 == "" && column5 == "")
                                    {
                                        break;//遇到空行,直接跳转
                                    }
                                    else
                                    {
                                        ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                            column11, column12, column13, column14, column15, column16, column17, column18, column19, column20, errorStr);
                                    }
                                }
                                else
                                {
                                    #region===检验是否有效数字
                                    if (!IsNumber(column11))
                                    {
                                        errorStr = "数量不是有效数字。";
                                    }
                                    else if (!IsNumber(column12))
                                    {
                                        errorStr = "单价不是有效数字。";
                                    }
                                    else if (!IsNumber(column13))
                                    {
                                        errorStr = "金额不是有效数字。";
                                    }
                                    #endregion

                                    #region===校验是否有效日期
                                    if (!IsDate(column20))
                                    {
                                        errorStr = "创建时间不是有效日期。";
                                    }
                                    else if (!IsDate(column4))
                                    {
                                        errorStr = "单据日期不是有效日期。";
                                    }
                                    #endregion


                                    if (!string.IsNullOrEmpty(errorStr))
                                    {
                                        ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                              column11, column12, column13, column14, column15, column16, column17, column18, column19, column20, errorStr);
                                    }
                                    else
                                    {
                                        string mainId = "";//主键编号

                                        #region==添加数据
                                        //判断主表是否存在单号
                                        //List<ModHOrderIn> main = ListMain.Where(p => p.InNumber == column0.Trim() && p.StoreId == DicCKId && p.GetNumber == column3.Trim()).ToList();
                                        List<ModHOrderIn> main = ListMain.Where(p => p.CusterId == DicCompanyId).ToList();
                                        if (main.Count > 0)
                                        {
                                            mainId = main[0].Id;
                                            //把金额合计
                                            main[0].TotalPrice = main[0].TotalPrice + (string.IsNullOrEmpty(column13) == true ? 0 : Convert.ToDecimal(column13));
                                            //入库单号
                                            if (!string.IsNullOrEmpty(column0.Trim()))
                                            {
                                                if (main[0].InNumber.IndexOf(column0.Trim()) == -1)
                                                {
                                                    main[0].InNumber = main[0].InNumber + "," + column0.Trim();
                                                }
                                            }
                                            //采购单号
                                            if (!string.IsNullOrEmpty(column3.Trim()))
                                            {
                                                if (main[0].GetNumber.IndexOf(column3.Trim()) == -1)
                                                {
                                                    main[0].GetNumber = main[0].GetNumber + "," + column3.Trim();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            #region===添加主表信息
                                            mainId = Guid.NewGuid().ToString();//主键编号
                                            ModHOrderIn In = new ModHOrderIn();
                                            In.Id = mainId;//主键编号
                                            In.CreateTime = DateTime.Now;
                                            In.CreaterId = CurrentMaster.Id;
                                            In.Status = (int)StatusEnum.正常;
                                            In.InNumber = column0;
                                            In.StoreId = DicCKId;
                                            In.CusterId = DicCompanyId;
                                            In.GetNumber = column3;
                                            In.BillDate = null;
                                            In.TotalPrice = (string.IsNullOrEmpty(column13) == true ? 0 : Convert.ToDecimal(column13));
                                            In.LossPrice = 0;//盈亏金额
                                            if (string.IsNullOrEmpty(column4) != true)
                                            {
                                                In.BillDate = Convert.ToDateTime(column4);
                                            }
                                            int Status = 1;
                                            if (!string.IsNullOrEmpty(column5))
                                            {
                                                if (column5 == "已入库")
                                                {
                                                    Status = 1;
                                                }
                                                else
                                                {
                                                    Status = 0;
                                                }
                                            }
                                            In.InStatus = Status;
                                            In.Remark = column6;
                                            In.LogisticsNumber = column17;
                                            In.Maker = DicMasterId;
                                            In.Judger = DicCSRId;
                                            In.JudgeDate = null;
                                            if (string.IsNullOrEmpty(column20) != true)
                                            {
                                                In.JudgeDate = Convert.ToDateTime(column20);
                                            }

                                            builder.Append(@"insert into H_OrderIn(Id,CreateTime,CreaterId,[Status],InNumber,StoreId
                                                  ,CusterId
                                                  ,GetNumber
                                                  ,BillDate
                                                  ,InStatus
                                                  ,Remark
                                                  ,LogisticsNumber
                                                  ,Maker
                                                  ,Judger
                                                  ,JudgeDate,FinancialState) values(");
                                            //Id
                                            builder.Append("'" + In.Id + "',");
                                            //CreateTime
                                            builder.Append("'" + In.CreateTime + "',");
                                            //CreaterId
                                            builder.Append("'" + In.CreaterId + "',");
                                            //Status
                                            builder.Append("'" + In.Status + "',");
                                            //InNumber
                                            builder.Append("'" + In.InNumber + "',");
                                            //StoreId
                                            builder.Append("'" + In.StoreId + "',");
                                            //CusterId
                                            builder.Append("'" + In.CusterId + "',");
                                            //GetNumber
                                            builder.Append("'" + In.GetNumber + "',");
                                            //BillDate
                                            builder.AppendFormat("{0}", string.IsNullOrEmpty(column4) == true ? "null," : "'" + In.BillDate + "',");
                                            //InStatus
                                            builder.Append("'" + In.InStatus + "',");
                                            //Remark
                                            builder.Append("'" + In.Remark + "',");
                                            //LogisticsNumber
                                            builder.Append("'" + In.LogisticsNumber + "',");
                                            //Maker
                                            builder.Append("'" + In.Maker + "',");
                                            //Judger
                                            builder.Append("'" + In.Judger + "',");
                                            //JudgeDate
                                            builder.AppendFormat("{0}", string.IsNullOrEmpty(column20) == true ? "null," : "'" + In.JudgeDate + "',");
                                            //FinancialState
                                            builder.Append("'0'");
                                            builder.Append(");");
                                            #endregion
                                            ListMain.Add(In);

                                            //int result = new BllHOrderIn().Insert(In);
                                            //if (result > 0)
                                            //{
                                            //    ListMain.Add(In);
                                            //    add = true;//可以继续添加明细
                                            //}
                                            //else {
                                            //    add = false;
                                            //    ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                            //    column11, column12, column13, column14, column15, column16, column17, column18, column19, column20, "主表数据添加失败，请联系开发查找原因。");
                                            //}
                                        }

                                        ///添加明细
                                        string DetailId = Guid.NewGuid().ToString();//主键编号
                                        #region ==添加明细
                                        ModHOrderInDetail detail = new ModHOrderInDetail();
                                        detail.Id = Guid.NewGuid().ToString();//主键编号
                                        detail.CreateTime = DateTime.Now;
                                        detail.CreaterId = CurrentMaster.Id;
                                        detail.Status = (int)StatusEnum.正常;
                                        detail.MainId = mainId;
                                        detail.GoodId = DicGoodId;
                                        detail.Code = column8;
                                        detail.GoodName = column9;
                                        detail.GoodUnit = column10;
                                        detail.Count = string.IsNullOrEmpty(column11) == true ? 0 : Convert.ToInt32(column11);
                                        detail.Price = string.IsNullOrEmpty(column12) == true ? 0 : Convert.ToDecimal(column12);
                                        detail.Money = string.IsNullOrEmpty(column13) == true ? 0 : Convert.ToDecimal(column13);
                                        detail.Batch = column14;
                                        detail.Remark = column15;
                                        detail.StyleNum = column16;
                                        detail.FreightNum = column17;
                                        detail.ListOrder = i;
                                        detail.StyleCount = 1;//款式数量
                                        detail.StylePrice = detail.Price;
                                        builder.AppendLine();
                                        builder.Append(@"insert into H_OrderInDetail(Id,CreateTime
                                                  ,CreaterId
                                                  ,Status
                                                  ,GoodId
                                                  ,Code
                                                  ,GoodName
                                                  ,GoodUnit
                                                  ,Count
                                                  ,Price
                                                  ,Money
                                                  ,Batch
                                                  ,Remark
                                                  ,StyleNum
                                                  ,FreightNum
                                                  ,ListOrder
                                                  ,MainId,StyleCount,StylePrice,BillNum,BillPrice,BillMoney) values(");
                                        //Id
                                        builder.Append("'" + detail.Id + "',");
                                        //CreateTime
                                        builder.Append("'" + detail.CreateTime + "',");
                                        //CreaterId
                                        builder.Append("'" + detail.CreaterId + "',");
                                        //Status
                                        builder.Append("'" + detail.Status + "',");
                                        //GoodId
                                        builder.Append("'" + detail.GoodId + "',");
                                        //Code
                                        builder.Append("'" + detail.Code + "',");
                                        //GoodName
                                        builder.Append("'" + detail.GoodName + "',");
                                        //GoodUnit
                                        builder.Append("'" + detail.GoodUnit + "',");
                                        //Count
                                        builder.Append("'" + detail.Count + "',");
                                        //Price
                                        builder.Append("'" + detail.Price + "',");
                                        //Money
                                        builder.Append("'" + detail.Money + "',");
                                        //Batch
                                        builder.Append("'" + detail.Batch + "',");
                                        //Remark
                                        builder.Append("'" + detail.Remark + "',");
                                        //StyleNum
                                        builder.Append("'" + detail.StyleNum + "',");
                                        //FreightNum
                                        builder.Append("'" + detail.FreightNum + "',");
                                        //ListOrder
                                        builder.Append("'" + detail.ListOrder + "',");
                                        //MainId
                                        builder.Append("'" + detail.MainId + "',");
                                        //StyleCount
                                        builder.Append("'" + detail.StyleCount + "',");
                                        //StylePrice
                                        builder.Append("'" + (string.IsNullOrEmpty(column13) == true ? 0 : Convert.ToDecimal(column13)) + "',");
                                        //BillNum
                                        builder.Append("'" + detail.Count + "',");
                                        //BillPrice
                                        builder.Append("'" + detail.Price + "',");
                                        //BillMoney
                                        builder.Append("'" + detail.Money + "'");
                                        builder.Append(");");
                                        #endregion
                                        //int detailResult = new BllHOrderInDetail().Insert(detail);
                                        //if (detailResult > 0)
                                        //{
                                        //    Insertcount++;
                                        //}
                                        //else
                                        //{
                                        //    ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                        //       column11, column12, column13, column14, column15, column16, column17, column18, column19, column20, "明细数据添加失败，请联系开发查找原因。");
                                        //}
                                        #endregion


                                        Insertcount++;
                                    }
                                }
                                #endregion
                                if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                                {
                                    //LogErrorRecord.Debug("采购入库单导入sql:" + builder.ToString());
                                    rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                                    builder.Clear();
                                    builder.Length = 0;

                                }
                            }
                        }


                        //更改统计金额
                        for (int a = 0; a < ListMain.Count; a++)
                        {
                            builder.AppendLine();
                            builder.Append("update H_OrderIn set oldPrice='" + ListMain[a].TotalPrice + "',TotalPrice='" + ListMain[a].TotalPrice + "',InNumber='" + ListMain[a].InNumber + "',GetNumber='" + ListMain[a].GetNumber + "' where Id='" + ListMain[a].Id + "';");
                        }

                        if (builder.Length > 0)
                        {
                            //LogErrorRecord.Debug("采购入库单导入sql:" + builder.ToString());
                            rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                            builder.Clear();
                            builder.Length = 0;
                        }
                        if (ErrorTab.Rows.Count >= 1)
                        {
                            tab = ErrorTab;
                        }
                        else
                        {
                            tab = null;
                        }

                        if (rowAffected == 0)
                        {
                            Insertcount = 0;
                        }
                        ErrorMsg = "";
                        return Insertcount;
                    }
                }
                catch (Exception ex)
                {
                    if (ErrorTab.Rows.Count >= 1)
                    {
                        tab = ErrorTab;
                    }
                    else
                    {
                        tab = null;
                    }
                    ErrorMsg = "采购入库单导入异常：" + ex.Message;
                    return 0;
                }
            }
        }


        /// <summary>
        /// 校验采购入库单数据
        /// </summary>
        /// <param name="CurrentMaster"></param>
        /// <param name="val1">入仓单号</param>
        /// <param name="val2">仓库</param>
        /// <param name="val3">供应商</param>
        /// <param name="val4">采购单号</param>
        /// <param name="val5">单据日期</param>
        /// <param name="val6">状态</param>
        /// <param name="val7">备注</param>
        /// <param name="val8">物流单号</param>
        /// <param name="val9">商品编码</param>
        /// <param name="val10">商品名称</param>
        /// <param name="val11">颜色及规格</param>
        /// <param name="val12">数量</param>
        /// <param name="val13">单价</param>
        /// <param name="val14">金额</param>
        /// <param name="val15">批次号</param>
        /// <param name="val16">入库明细备注</param>
        /// <param name="val17">款式编号</param>
        /// <param name="val18">供应商货号</param>
        /// <param name="val19">制单人</param>
        /// <param name="val20">财审人</param>
        /// <param name="val21">财审日期</param>
        /// <param name="CompanyId"></param>
        /// <param name="DicCKId">返回的仓库Id</param>
        /// <param name="DicCompanyId">返回的供应商ID</param>
        /// <param name="DicGoodId">返回的商品ID</param>
        /// <param name="DicMasterId">返回的制单人ID</param>
        /// <param name="DicCSRId">返回的财审人Id</param>
        /// <param name="errorStr"></param>
        /// <returns></returns>
        public bool CheckOrderIn(ModSysMaster CurrentMaster, string val1, string val2, string val3, string val4, string val5, string val6, string val7, string val8, string val9, string val10, string val11, string val12, string val13, string val14, string val15, string val16, string val17, string val18, string val19, string val20, string val21, string CompanyId, ref string DicCKId, ref string DicCompanyId, ref string DicGoodId, ref string DicMasterId, ref string DicCSRId, ref string errorStr, ref List<ModSysCompany> DicGYS, ref  List<ModSysDirc> DicCK, ref  List<ModSysMaster> DicMaster, ref  List<ModShopGoods> DicGood)
        {
            bool flag = false;
            bool AddDirc = false;//是否添加仓库
            bool AddGood = false;//是否添加商品

            #region ===校验入仓单号
            if (val1.Length == 0 || string.IsNullOrEmpty(val1.Trim()) || val1.Length == 0 || string.IsNullOrEmpty(val1.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "入仓单号不能为空,为必填项";
                return flag;
            }
            else
            {
                if (val1.Length > 50 || val1.Length > 50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "入仓单号长度过长,有效范围1-100字符.";
                    return flag;
                }
            }
            #endregion

            #region ===校验仓库
            if (val2.Length == 0 || string.IsNullOrEmpty(val2.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "仓库名称不能为空,为必填项.";
                return flag;
            }
            else
            {
                AddDirc = true;
            }
            #endregion

            #region ===校验供应商名称
            if (val3.Length == 0 || string.IsNullOrEmpty(val3.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "供应商名称不能为空,为必填项.";
                return flag;
            }
            else
            {
                List<ModSysCompany> newCategory = DicGYS.Where(p => p.Name == val3.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    //供应商不存在，自动导入
                    DicCompanyId = AddCompany(val3, val3, "", "", "", "", "0", "", "", "0", "", "", (int)CompanyType.供应商, CurrentMaster, CompanyId, ref DicGYS);
                }
                else
                {
                    DicCompanyId = newCategory[0].Id;//供应商ID
                }
            }
            #endregion

            #region ===校验采购单号
            if (val4.Length == 0 || string.IsNullOrEmpty(val4.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "采购单号不能为空,为必填项";
                return flag;
            }
            #endregion

            #region ===校验商品编码
            if (val9.Length == 0 || string.IsNullOrEmpty(val9.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "商品编码不能为空,为必填项";
                return flag;
            }
            #endregion

            #region ===校验商品名称
            if (val10.Length == 0 || string.IsNullOrEmpty(val10.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "商品名称不能为空,为必填项";
                return flag;
            }
            #endregion

            //添加仓库
            if (AddDirc == true)
            {
                List<ModSysDirc> newCategory = DicCK.Where(p => p.Type == 0 && p.Name.Trim() == val2.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    DicCKId = AddSysDirc(0, val2, CurrentMaster, CompanyId, ref DicCK);
                }
                else
                {
                    DicCKId = newCategory[0].Id;
                }
            }
            //添加商品 
            if (AddGood == true)
            {
                List<ModShopGoods> newGood = DicGood.Where(p => p.GoodName == val10.Trim() && p.GoodsCode == val9.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newGood.Count <= 0)
                {
                    DicGoodId = AddGoods(val9, val10, val17, val13, val14,val12, val15, val18, val11, CurrentMaster, CompanyId, ref DicGood);
                }
                else
                {
                    DicGoodId = newGood[0].ID;
                }
            }

            #region ===校验制单人
            if (!string.IsNullOrEmpty(val19))
            {
                List<ModSysMaster> newmaster = DicMaster.Where(p => p.UserName == val19.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newmaster.Count <= 0)
                {
                    DicMasterId = AddMaster(val19, ConvertTo.convert(val19,true), "", CurrentMaster, CompanyId, ref DicMaster);
                }
                else
                {
                    DicMasterId = newmaster[0].Id;
                }
            }
            #endregion

            #region ===校验财审人
            if (!string.IsNullOrEmpty(val20))
            {
                List<ModSysMaster> newmaster = DicMaster.Where(p => p.UserName == val20.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newmaster.Count <= 0)
                {
                    DicCSRId = AddMaster(val19, ConvertTo.convert(val19, true), "", CurrentMaster, CompanyId, ref DicMaster);
                }
                else
                {
                    DicCSRId = newmaster[0].Id;
                }
            }
            #endregion

            return flag;
        }

        #endregion

        #region ===Excel文档导入采购退货
       
        /// <summary>
        /// Excel文档导入到数据库
        /// </summary>
        /// <param name="ctype">合同类型（1：收入合同 2：支出合同）</param>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <param name="shopTypeid">商家分类</param>
        /// <returns></returns>

        public int RenderToPurchase(int ctype, string FileName, Stream excelFileStream, int sheetIndex, int headerRowIndex, out DataTable tab, out string ErrorMsg, ModSysMaster CurrentMaster)
        {

            /// <summary>
            /// 供应商列表
            /// </summary>
            List<ModSysCompany> DicGYS = new List<ModSysCompany>();
            /// <summary>
            /// 仓库分类
            /// </summary>
            List<ModSysDirc> DicCK = new List<ModSysDirc>();
            /// <summary>
            /// 人员列表
            /// </summary>
            List<ModSysMaster> DicMaster = new List<ModSysMaster>();
            /// <summary>
            /// 商品列表
            /// </summary>
            List<ModShopGoods> DicGood = new List<ModShopGoods>();

             /// <summary>
        ///主表信息
        /// </summary>
          List<ModHPurchase> ListPurchase = new List<ModHPurchase>();

            #region ===构建错误表头
            DataTable ErrorTab = new DataTable();
            DataColumn 退货单号 = new DataColumn("退货单号", Type.GetType("System.String"));
            DataColumn 仓库 = new DataColumn("仓库", Type.GetType("System.String"));
            DataColumn 供应商 = new DataColumn("供应商", Type.GetType("System.String"));
            DataColumn 采购单号 = new DataColumn("采购单号", Type.GetType("System.String"));
            DataColumn 单据日期 = new DataColumn("单据日期", Type.GetType("System.String"));
            DataColumn 状态 = new DataColumn("状态", Type.GetType("System.String"));
            DataColumn 备注 = new DataColumn("备注", Type.GetType("System.String"));
            DataColumn 商品编码 = new DataColumn("商品编码", Type.GetType("System.String"));
            DataColumn 商品名称 = new DataColumn("商品名称", Type.GetType("System.String"));
            DataColumn 颜色及规格 = new DataColumn("颜色及规格", Type.GetType("System.String"));
            DataColumn 数量 = new DataColumn("数量", Type.GetType("System.String"));
            DataColumn 单价 = new DataColumn("单价", Type.GetType("System.String"));
            DataColumn 金额 = new DataColumn("金额", Type.GetType("System.String"));
            DataColumn 财审人 = new DataColumn("财审人", Type.GetType("System.String"));
            DataColumn 财审日期 = new DataColumn("财审日期", Type.GetType("System.String"));
            DataColumn 备注1 = new DataColumn("备注1", Type.GetType("System.String"));
            DataColumn 款式编号 = new DataColumn("款式编号", Type.GetType("System.String"));
            DataColumn 供应商货号 = new DataColumn("供应商货号", Type.GetType("System.String"));
            DataColumn 错误原因 = new DataColumn("错误原因", Type.GetType("System.String"));

            ErrorTab.Columns.Add(退货单号);
            ErrorTab.Columns.Add(仓库);
            ErrorTab.Columns.Add(供应商);
            ErrorTab.Columns.Add(采购单号);
            ErrorTab.Columns.Add(单据日期);
            ErrorTab.Columns.Add(状态);
            ErrorTab.Columns.Add(备注);
            ErrorTab.Columns.Add(商品编码);
            ErrorTab.Columns.Add(商品名称);
            ErrorTab.Columns.Add(颜色及规格);
            ErrorTab.Columns.Add(数量);
            ErrorTab.Columns.Add(单价);
            ErrorTab.Columns.Add(金额);
            ErrorTab.Columns.Add(财审人);
            ErrorTab.Columns.Add(财审日期);
            ErrorTab.Columns.Add(备注1);
            ErrorTab.Columns.Add(款式编号);
            ErrorTab.Columns.Add(供应商货号);
            ErrorTab.Columns.Add(错误原因);
            #endregion

            //当前用户公司编号
            string CompanyId = CurrentMaster.Cid;
            //获取所有供应商列表
            DicGYS = new BllSysCompany().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Attribute == (int)CompanyType.供应商).ToList();
            //获取DicCK分类
            DicCK = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 0).ToList();
            //商品列表
            DicGood = new BllShopGoods().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除).ToList();
            //人员列表
            DicMaster = new BllSysMaster().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Attribute == (int)AdminTypeEnum.系统管理员).ToList();

            int rowAffected = 0;
            using (excelFileStream)//读取流
            {
                try
                {
                    //根据类型查询题库信息
                    string sExtension = FileName.Substring(FileName.LastIndexOf('.'));//获取拓展名
                    IWorkbook workbook = null;
                    if (sExtension == ".xls")
                    {
                        workbook = new HSSFWorkbook(excelFileStream);
                    }
                    else if (sExtension == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(excelFileStream);
                    }
                    ISheet sheet = workbook.GetSheetAt(sheetIndex);
                    StringBuilder builder = new StringBuilder();
                    IRow headerRow = sheet.GetRow(headerRowIndex);
                    int cellCount = headerRow.LastCellNum;//
                    int rowCount = sheet.LastRowNum;//
                    int Insertcount = 0;  //插入总行数
                    if (cellCount != 18)
                    {
                        ErrorMsg = "上传Excel与模板不服，请检查。";
                        tab = null;
                        return 0;
                    }
                    else
                    {
                        //循环表格
                        for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            //判断空行
                            if (row != null)
                            {
                                #region  数据验证判断
                                var column0 = ExcelRender.GetCellValue(row.GetCell(0)).Replace("'", "''");//退货单号
                                var column1 = ExcelRender.GetCellValue(row.GetCell(1)).Replace("'", "''");//仓库
                                var column2 = ExcelRender.GetCellValue(row.GetCell(2)).Replace("'", "''");//供应商
                                var column3 = ExcelRender.GetCellValue(row.GetCell(3)).Replace("'", "''");//采购单号
                                var column4 = ToDateTime(ExcelRender.GetCellValue(row.GetCell(4)).Replace("'", "''"));//单据日期
                                var column5 = ExcelRender.GetCellValue(row.GetCell(5)).Replace("'", "''"); //状态
                                var column6 = ExcelRender.GetCellValue(row.GetCell(6)).Replace("'", "''");//备注
                                var column7 = ExcelRender.GetCellValue(row.GetCell(7)).Replace("'", "''");///商品编码
                                var column8 = ExcelRender.GetCellValue(row.GetCell(8)).Replace("'", "''");//商品名称
                                var column9 = ExcelRender.GetCellValue(row.GetCell(9)).Replace("'", "''"); //颜色及规格
                                var column10 = ExcelRender.GetCellValue(row.GetCell(10)).Replace("'", "''");//数量
                                var column11 = ExcelRender.GetCellValue(row.GetCell(11)).Replace("'", "''"); //单价
                                var column12 = ExcelRender.GetCellValue(row.GetCell(12)).Replace("'", "''"); //金额
                                var column13 = ExcelRender.GetCellValue(row.GetCell(13)).Replace("'", "''"); //财审人
                                var column14 = ToDateTime(ExcelRender.GetCellValue(row.GetCell(14)).Replace("'", "''"));//财审日期
                                var column15 = ExcelRender.GetCellValue(row.GetCell(15)).Replace("'", "''"); //入库明细备注
                                var column16 = ExcelRender.GetCellValue(row.GetCell(16)).Replace("'", "''"); //款式编号
                                var column17 = ExcelRender.GetCellValue(row.GetCell(17)).Replace("'", "''"); //供应商货号

                                string errorStr = "";
                                string DicCKId = ""; //仓库ID
                                string DicCompanyId = "";//供应商ID
                                string DicGoodId = "";//商品ID
                                string DicCSRId = "";//财审人员ID

                                //校验信息是否完整
                                if (CheckPurchase(CurrentMaster, column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10, column11, column12, column13, column14, column15, column16, column17, CompanyId, ref  DicCKId, ref  DicCompanyId, ref DicGoodId, ref DicCSRId, ref errorStr, ref DicGYS, ref DicCK, ref DicMaster, ref DicGood) == true)
                                {
                                    //校验不通过
                                    if (column0 == "" && column1 == "" && column2 == "" && column4 == "" && column5 == "")
                                    {
                                        break;//遇到空行,直接跳转
                                    }
                                    else
                                    {
                                        ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                            column11, column12, column13, column14, column15, column16, column17, errorStr);
                                    }
                                }
                                else
                                {
                                    #region===检验是否有效数字
                                    if (!IsNumber(column10))
                                    {
                                        errorStr = "数量不是有效数字。";
                                    }
                                    else if (!IsNumber(column11))
                                    {
                                        errorStr = "单价不是有效数字。";
                                    }
                                    else if (!IsNumber(column12))
                                    {
                                        errorStr = "金额不是有效数字。";
                                    }
                                    #endregion

                                    #region===校验是否有效日期
                                    if (!IsDate(column14))
                                    {
                                        errorStr = "财审日期不是有效日期。";
                                    }
                                    else if (!IsDate(column4))
                                    {
                                        errorStr = "单据日期不是有效日期。";
                                    }
                                    #endregion


                                    if (!string.IsNullOrEmpty(errorStr))
                                    {
                                        ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                              column11, column12, column13, column14, column15, column16, column17, errorStr);
                                    }
                                    else
                                    {
                                        string mainId = "";//主键编号

                                        #region==添加数据
                                        //判断主表是否存在单号
                                        //List<ModHPurchase> main = ListPurchase.Where(p => p.OutNumber == column0.Trim() && p.StoreId == DicCKId && p.GetNumber == column3.Trim()).ToList();
                                        List<ModHPurchase> main = ListPurchase.Where(p => p.CusterId == DicCompanyId).ToList();
                                        if (main.Count > 0)
                                        {
                                            mainId = main[0].Id;
                                            //把金额合计
                                            main[0].TotalPrice = main[0].TotalPrice + (string.IsNullOrEmpty(column12) == true ? 0 : Convert.ToDecimal(column12));
                                            //入库单号
                                            if (!string.IsNullOrEmpty(column0.Trim()))
                                            {
                                                if (main[0].OutNumber.IndexOf(column0.Trim()) == -1)
                                                {
                                                    main[0].OutNumber = main[0].OutNumber + "," + column0.Trim();
                                                }
                                            }
                                            //采购单号
                                            if (!string.IsNullOrEmpty(column3.Trim()))
                                            {
                                                if (main[0].GetNumber.IndexOf(column3.Trim()) == -1)
                                                {
                                                    main[0].GetNumber = main[0].GetNumber + "," + column3.Trim();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            #region===添加主表信息
                                            mainId = Guid.NewGuid().ToString();//主键编号
                                            ModHPurchase In = new ModHPurchase();
                                            In.Id = mainId;//主键编号
                                            In.CreateTime = DateTime.Now;
                                            In.CreaterId = CurrentMaster.Id;
                                            In.Status = (int)StatusEnum.正常;
                                            In.OutNumber = column0;
                                            In.StoreId = DicCKId;
                                            In.CusterId = DicCompanyId;
                                            In.GetNumber = column3;
                                            In.BillDate = null;
                                            In.TotalPrice = (string.IsNullOrEmpty(column12) == true ? 0 : Convert.ToDecimal(column12));
                                            In.LossPrice = 0;//盈亏金额
                                            if (string.IsNullOrEmpty(column4) != true)
                                            {
                                                In.BillDate = Convert.ToDateTime(column4);
                                            }
                                            int Status = 1;
                                            if (!string.IsNullOrEmpty(column5))
                                            {
                                                if (column5 == "已确认")
                                                {
                                                    Status = 1;
                                                }
                                                else
                                                {
                                                    Status = 0;
                                                }
                                            }
                                            In.OutStatus = Status;
                                            In.Remark = column6;
                                            In.Judger = DicCSRId;
                                            In.JudgeDate = null;
                                            if (string.IsNullOrEmpty(column14) != true)
                                            {
                                                In.JudgeDate = Convert.ToDateTime(column14);
                                            }

                                            builder.Append(@"insert into H_Purchase(Id,CreateTime,CreaterId,[Status],OutNumber,StoreId
                                                  ,CusterId
                                                  ,GetNumber
                                                  ,BillDate
                                                  ,OutStatus
                                                  ,Remark
                                                  ,Maker
                                                  ,Judger
                                                  ,JudgeDate,FinancialState) values(");
                                            //Id
                                            builder.Append("'" + In.Id + "',");
                                            //CreateTime
                                            builder.Append("'" + In.CreateTime + "',");
                                            //CreaterId
                                            builder.Append("'" + In.CreaterId + "',");
                                            //Status
                                            builder.Append("'" + In.Status + "',");
                                            //OutNumber
                                            builder.Append("'" + In.OutNumber + "',");
                                            //StoreId
                                            builder.Append("'" + In.StoreId + "',");
                                            //CusterId
                                            builder.Append("'" + In.CusterId + "',");
                                            //GetNumber
                                            builder.Append("'" + In.GetNumber + "',");
                                            //BillDate
                                            builder.AppendFormat("{0}", string.IsNullOrEmpty(column4) == true ? "null," : "'" + In.BillDate + "',");
                                            //OutStatus
                                            builder.Append("'" + In.OutStatus + "',");
                                            //Remark
                                            builder.Append("'" + In.Remark + "',");
                                            //Maker
                                            builder.Append("'" + In.Maker + "',");
                                            //Judger
                                            builder.Append("'" + In.Judger + "',");
                                            //JudgeDate
                                            builder.AppendFormat("{0}", string.IsNullOrEmpty(column14) == true ? "null," : "'" + In.JudgeDate + "',");
                                            //FinancialState
                                            builder.Append("'0'");
                                            builder.Append(");");
                                            #endregion
                                            ListPurchase.Add(In);
                                            //int result = new BllHOrderIn().Insert(In);
                                            //if (result > 0)
                                            //{
                                            //    ListMain.Add(In);
                                            //    add = true;//可以继续添加明细
                                            //}
                                            //else {
                                            //    add = false;
                                            //    ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                            //    column11, column12, column13, column14, column15, column16, column17, column18, column19, column20, "主表数据添加失败，请联系开发查找原因。");
                                            //}
                                        }

                                        ///添加明细
                                        string DetailId = Guid.NewGuid().ToString();//主键编号
                                        #region ==添加明细
                                        ModHPurchaseDetail detail = new ModHPurchaseDetail();
                                        detail.Id = Guid.NewGuid().ToString();//主键编号
                                        detail.CreateTime = DateTime.Now;
                                        detail.CreaterId = CurrentMaster.Id;
                                        detail.Status = (int)StatusEnum.正常;
                                        detail.MainId = mainId;
                                        detail.GoodId = DicGoodId;
                                        detail.Code = column7;
                                        detail.GoodName = column8;
                                        detail.GoodUnit = column9;
                                        detail.Count = string.IsNullOrEmpty(column10) == true ? 0 : Convert.ToInt32(column10);
                                        detail.Price = string.IsNullOrEmpty(column11) == true ? 0 : Convert.ToDecimal(column11);
                                        detail.Money = string.IsNullOrEmpty(column12) == true ? 0 : Convert.ToDecimal(column12);
                                        detail.Remark = column15;
                                        detail.StyleNum = column16;
                                        detail.FreightNum = column17;
                                        detail.ListOrder = i;
                                        builder.AppendLine();
                                        builder.Append(@"insert into H_PurchaseDetail(Id,CreateTime
                                                  ,CreaterId
                                                  ,Status
                                                  ,GoodId
                                                  ,Code
                                                  ,GoodName
                                                  ,GoodUnit
                                                  ,Count
                                                  ,Price
                                                  ,Money
                                                  ,Remark
                                                  ,StyleNum
                                                  ,FreightNum
                                                  ,ListOrder
                                                  ,MainId,StyleCount,StylePrice,BillNum,BillPrice,BillMoney) values(");
                                        //Id
                                        builder.Append("'" + detail.Id + "',");
                                        //CreateTime
                                        builder.Append("'" + detail.CreateTime + "',");
                                        //CreaterId
                                        builder.Append("'" + detail.CreaterId + "',");
                                        //Status
                                        builder.Append("'" + detail.Status + "',");
                                        //GoodId
                                        builder.Append("'" + detail.GoodId + "',");
                                        //Code
                                        builder.Append("'" + detail.Code + "',");
                                        //GoodName
                                        builder.Append("'" + detail.GoodName + "',");
                                        //GoodUnit
                                        builder.Append("'" + detail.GoodUnit + "',");
                                        //Count
                                        builder.Append("'" + detail.Count + "',");
                                        //Price
                                        builder.Append("'" + detail.Price + "',");
                                        //Money
                                        builder.Append("'" + detail.Money + "',");
                                        //Remark
                                        builder.Append("'" + detail.Remark + "',");
                                        //StyleNum
                                        builder.Append("'" + detail.StyleNum + "',");
                                        //FreightNum
                                        builder.Append("'" + detail.FreightNum + "',");
                                        //ListOrder
                                        builder.Append("'" + detail.ListOrder + "',");
                                        //MainId
                                        builder.Append("'" + detail.MainId + "',");
                                        //StyleCount
                                        builder.Append("'" + detail.StyleCount + "',");
                                        //StylePrice
                                        builder.Append("'" + (string.IsNullOrEmpty(column13) == true ? 0 : Convert.ToDecimal(column13)) + "',");
                                        //BillNum
                                        builder.Append("'" + detail.Count + "',");
                                        //BillPrice
                                        builder.Append("'" + detail.Price + "',");
                                        //BillMoney
                                        builder.Append("'" + detail.Money + "'");
                                        builder.Append(")");
                                        #endregion
                                        //int detailResult = new BllHOrderInDetail().Insert(detail);
                                        //if (detailResult > 0)
                                        //{
                                        //    Insertcount++;
                                        //}
                                        //else
                                        //{
                                        //    ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                        //       column11, column12, column13, column14, column15, column16, column17, column18, column19, column20, "明细数据添加失败，请联系开发查找原因。");
                                        //}
                                        #endregion

                                        Insertcount++;
                                    }
                                }
                                #endregion
                                if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                                {
                                    //LogErrorRecord.Debug("采购退货单导入sql:" + builder.ToString());
                                    rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                                    builder.Clear();
                                    builder.Length = 0;

                                }
                            }
                        }
                        //更改统计金额
                        for (int a = 0; a < ListPurchase.Count; a++)
                        {
                            builder.AppendLine();
                            builder.Append("update H_Purchase set oldPrice='" + ListPurchase[a].TotalPrice + "',TotalPrice='" + ListPurchase[a].TotalPrice + "',OutNumber='" + ListPurchase[a].OutNumber + "',GetNumber='" + ListPurchase[a].GetNumber + "' where Id='" + ListPurchase[a].Id + "';");
                        }
                        if (builder.Length > 0)
                        {
                            //LogErrorRecord.Debug("采购退货单导入sql:" + builder.ToString());
                            rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                            builder.Clear();
                            builder.Length = 0;
                        }
                        if (ErrorTab.Rows.Count >= 1)
                        {
                            tab = ErrorTab;
                        }
                        else
                        {
                            tab = null;
                        }

                        if (rowAffected == 0)
                        {
                            Insertcount = 0;
                        }
                        ErrorMsg = "";
                        return Insertcount;
                    }
                }
                catch (Exception ex)
                {
                    if (ErrorTab.Rows.Count >= 1)
                    {
                        tab = ErrorTab;
                    }
                    else
                    {
                        tab = null;
                    }
                    ErrorMsg = "采购退货单导入异常：" + ex.Message;
                    return 0;
                }
            }
        }


        /// <summary>
        /// 校验采购退货单数据
        /// </summary>
        /// <param name="CurrentMaster"></param>
        /// <param name="val1">退货单号</param>
        /// <param name="val2">仓库</param>
        /// <param name="val3">供应商</param>
        /// <param name="val4">采购单号</param>
        /// <param name="val5">单据日期</param>
        /// <param name="val6">状态</param>
        /// <param name="val7">备注</param>
        /// <param name="val8">商品编码</param>
        /// <param name="val9">商品名称</param>
        /// <param name="val10">颜色及规格</param>
        /// <param name="val11">数量</param>
        /// <param name="val12">单价</param>
        /// <param name="val13">金额</param>
        /// <param name="val14">财审人</param>
        /// <param name="val15">财审日期</param>
        /// <param name="val16">入库明细备注</param>
        /// <param name="val17">款式编号</param>
        /// <param name="val18">供应商货号</param>
        /// <param name="CompanyId"></param>
        /// <param name="DicCKId">返回的仓库Id</param>
        /// <param name="DicCompanyId">返回的供应商ID</param>
        /// <param name="DicGoodId">返回的商品ID</param>
        /// <param name="DicCSRId">返回的财审人Id</param>
        /// <param name="errorStr"></param>
        /// <returns></returns>
        public bool CheckPurchase(ModSysMaster CurrentMaster, string val1, string val2, string val3, string val4, string val5, string val6, string val7, string val8, string val9, string val10, string val11, string val12, string val13, string val14, string val15, string val16, string val17, string val18, string CompanyId, ref string DicCKId, ref string DicCompanyId, ref string DicGoodId, ref string DicCSRId, ref string errorStr, ref List<ModSysCompany> DicGYS, ref  List<ModSysDirc> DicCK, ref  List<ModSysMaster> DicMaster, ref  List<ModShopGoods> DicGood)
        {
            bool flag = false;
            bool AddDirc = false;//是否添加仓库
            bool AddGood = false;//是否添加商品

            #region ===校验退货单号
            if (val1.Length == 0 || string.IsNullOrEmpty(val1.Trim()) || val1.Length == 0 || string.IsNullOrEmpty(val1.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "退货单号不能为空,为必填项";
                return flag;
            }
            else
            {
                if (val1.Length > 50 || val1.Length > 50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "退货单号长度过长,有效范围1-100字符.";
                    return flag;
                }
            }
            #endregion

            #region ===校验仓库
            if (val2.Length == 0 || string.IsNullOrEmpty(val2.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "仓库名称不能为空,为必填项.";
                return flag;
            }
            else
            {
                AddDirc = true;
            }
            #endregion

            #region ===校验供应商名称
            if (val3.Length == 0 || string.IsNullOrEmpty(val3.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "供应商名称不能为空,为必填项.";
                return flag;
            }
            else
            {
                List<ModSysCompany> newCategory = DicGYS.Where(p => p.Name == val3.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    flag = true;
                    errorStr += "供应商名不存在，不能导入数据.";
                    return flag;
                }
                else
                {
                    DicCompanyId = newCategory[0].Id;//供应商ID
                }
            }
            #endregion

            //#region ===校验采购单号
            //if (val4.Length == 0 || string.IsNullOrEmpty(val4.Trim())) //验证长度是否合理
            //{
            //    flag = true;
            //    errorStr += "采购单号不能为空,为必填项";
            //    return flag;
            //}
            //#endregion

            #region ===校验商品编码
            if (val8.Length == 0 || string.IsNullOrEmpty(val8.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "商品编码不能为空,为必填项";
                return flag;
            }
            #endregion

            #region ===校验商品名称
            if (val9.Length == 0 || string.IsNullOrEmpty(val9.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "商品名称不能为空,为必填项";
                return flag;
            }
            #endregion

            //添加仓库
            if (AddDirc == true)
            {
                List<ModSysDirc> newCategory = DicCK.Where(p => p.Type == 0 && p.Name.Trim() == val2.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    DicCKId = AddSysDirc(0, val2, CurrentMaster, CompanyId, ref DicCK);
                }
                else
                {
                    DicCKId = newCategory[0].Id;
                }
            }
            //添加商品 
            if (AddGood == true)
            {
                List<ModShopGoods> newGood = DicGood.Where(p => p.GoodName == val9.Trim() && p.GoodsCode == val8.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newGood.Count <= 0)
                {
                    DicGoodId = AddGoods(val8, val9, val17, val12, val13, val11, "", val17, val10, CurrentMaster, CompanyId, ref DicGood);
                }
                else
                {
                    DicGoodId = newGood[0].ID;
                }
            }

            #region ===校验财审人
            if (!string.IsNullOrEmpty(val14))
            {
                List<ModSysMaster> newmaster = DicMaster.Where(p => p.UserName == val14.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newmaster.Count <= 0)
                {
                    DicCSRId = AddMaster(val14, ConvertTo.convert(val14, true), "", CurrentMaster, CompanyId, ref DicMaster);
                }
                else
                {
                    DicCSRId = newmaster[0].Id;
                }
            }
            #endregion

            return flag;
        }

        #endregion

        #endregion


        /// <summary>
        /// 添加公司/部门/供应商
        /// </summary>
        /// <param name="NameTitle"></param>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        /// <param name="Status"></param>
        /// <param name="Address"></param>
        /// <param name="type"></param>
        /// <param name="CheckoutType"></param>
        /// <param name="Phone"></param>
        /// <param name="Tel"></param>
        /// <param name="PaymentType"></param>
        /// <param name="AccountName"></param>
        /// <param name="AccountNum"></param>
        /// <param name="Attribute"></param>
        /// <param name="CurrentMaster"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string AddCompany(string NameTitle, string Name, string Code, string Status, string Address, string type, string CheckoutType, string Phone, string Tel, string PaymentType, string AccountName, string AccountNum, int Attribute, ModSysMaster CurrentMaster, string companyId, ref List<ModSysCompany> model)
        {
            ModSysCompany t = new ModSysCompany();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status.ToLower() == "false")
                {
                    t.Status = (int)StatusEnum.禁用;
                }
            }
            t.CreaterUserId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.NameTitle = NameTitle;
            t.Name = Name;
            t.Address = Address;
            t.Phone = Phone;
            t.Tel = Tel;
            t.Code = Code;
            t.Attribute = Attribute;
            t.Type = type;

            int Checkout =2;//结账方式 0未设置 1月结 2日结
            if (!string.IsNullOrEmpty(CheckoutType))
            {
                switch (CheckoutType)
                {
                    case "月结":
                        Checkout = 1;
                        break;
                    case "日结":
                        Checkout = 2;
                        break;
                }
            }
            t.CheckoutType = Checkout;

            int Payment = 0;//付款方式：0:未设置 1支付宝 2工行  3农行
            if (!string.IsNullOrEmpty(PaymentType))
            {
                switch (PaymentType)
                {
                    case "支付宝":
                        Payment = 1;
                        break;
                    case "工行":
                        Payment = 2;
                        break;
                    case "农行":
                        Payment = 3;
                        break;
                }
            }
            t.PaymentType = Payment;
            t.AccountName = AccountName;
            t.AccountNum = AccountNum;
            if (companyId != "0")
            {
                var resu = new BllSysCompany().LoadData(companyId);
                t.Path = resu.Path + "/" + t.Id;
            }
            else
            {
                t.Path = t.Id;
            }
            t.CreateCompanyId = companyId;
            int result = new BllSysCompany().Insert(t);
            if (result > 0)
            {
                model.Add(t);
            }
            return t.Id;
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddMaster(string name, string LoginName, string DeptId, ModSysMaster CurrentMaster, string companyId,ref List<ModSysMaster> dic)
        {
            ModSysMaster t = new ModSysMaster();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.Cid = companyId;
            t.OrganizaId = DeptId;//部门编号
            t.IsSystem = false;
            t.IsMain = true;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.HeadImg = "";
            t.CardNum = "";
            t.Attribute = (int)AdminTypeEnum.系统管理员;//用户类型
            t.LoginName = LoginName;
            t.UserName = name;
            t.Pwd = DESEncrypt.Encrypt("666666");//默认密码
            new BllSysMaster().Insert(t);
            dic.Add(t);
            return t.Id;
        }
        /// <summary>
        /// 添加类别分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddCategoryId(string name, ModSysMaster CurrentMaster, string companyId)
        {
            ModSysGroup t = new ModSysGroup();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.ParentId = "0";
            t.Name = name;
            t.CompanyId = companyId;
            new BllSysGroup().Insert(t);
            //SysGroup.Add(t);
            return t.Id;
        }
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddSysDirc(int type, string name, ModSysMaster CurrentMaster, string companyId, ref List<ModSysDirc> dic)
        {
            ModSysDirc t = new ModSysDirc();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.ParentId = "0";
            t.Name = name;
            t.CompanyId = companyId;
            t.Name = name;
            t.Type = type;
            new BllSysDirc().Insert(t);
            dic.Add(t);
            return t.Id;
        }

        /// <summary>
        /// 添加商品信息
        /// </summary>
        /// <param name="GoodsCode">产品编号</param>
        /// <param name="GoodName">产品名称</param>
        /// <param name="styleNum">款式编号</param>
        /// <param name="price">单价</param>
        /// <param name="money">金额</param>
        /// <param name="CountNum">数量</param>
        /// <param name="Batch">批次号</param>
        /// <param name="FreightNumber">供应商货号</param>
        /// <param name="attribute">颜色及规格</param>
        /// <param name="CurrentMaster"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public string AddGoods(string GoodsCode, string GoodName, string styleNum, string price, string money, string CountNum, string Batch, string FreightNumber, string attribute, ModSysMaster CurrentMaster, string companyId,ref List<ModShopGoods> dic)
        {
            ModShopGoods t = new ModShopGoods();
            t.ID = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.CreatorId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.GoodsCode = GoodsCode;
            t.GoodName = GoodName;
            t.CompanyId = companyId;
            t.GoodsCategoryId = "";
            t.StyleNumber = styleNum;
            t.RawPrice = string.IsNullOrEmpty(price) ? 0 : Convert.ToDecimal(price);
            t.NowPrice = string.IsNullOrEmpty(money) ? 0 : Convert.ToDecimal(money);
            t.GoodType = 0;
            t.StoreNum = string.IsNullOrEmpty(CountNum) ? 0 : Convert.ToInt32(CountNum);
            t.Batch = Batch;
            t.FreightNumber = FreightNumber;
            new BllShopGoods().Insert(t);
            dic.Add(t);
            return t.ID;
        }
    }
}
