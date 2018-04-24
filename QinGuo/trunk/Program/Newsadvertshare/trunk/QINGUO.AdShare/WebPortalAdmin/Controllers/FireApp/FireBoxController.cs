using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using System.Web.Script.Serialization;
using QINGUO.Common;
using System.Data;
using AppLibrary.WriteExcel;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Text;

namespace WebPortalAdmin.Controllers
{
    public class FireBoxController : BaseController<ModFireBox>
    {
        string[] title;  //导出的标题
        string[] field;  //导出对应字段

        /// <summary>
        /// 超级管理员设备箱子管理
        /// </summary>
        /// <returns></returns>
        public ActionResult SuperMainIndex()
        {
            return View();
        }
        /// <summary>
        /// 单位管理员设备箱子管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region ==根据加载数据 SearchData()
        /// <summary>
        /// 根据id 加载数据
        /// </summary>
        public void SearchData()
        {
            try
            {
                Search search = this.GetSearch();
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");//自己查看自己的
                    }
                }

                if (!string.IsNullOrEmpty(Request["txtAddress"]))
                {
                    search.AddCondition("Address like '%" + Request["txtAddress"].ToString() + "%'");
                }
                if (!string.IsNullOrEmpty(Request["txtName"]))
                {
                    search.AddCondition("Name like '%" + Request["txtName"].ToString() + "%'");
                }

                string treeNodeId = Request["treeNodeId"].ToString();//1:已绑定 2:未绑定
                if (treeNodeId != "-1")
                {
                    switch (int.Parse(treeNodeId))
                    {
                        case 1://全部
                            search.AddCondition("Status!=" + (int)StatusEnum.删除);
                            break;
                        case 2://已绑定
                            search.AddCondition("Status=" + (int)StatusEnum.正常);
                            break;
                        case 3:
                            search.AddCondition("Status=" + (int)StatusEnum.禁用);
                            break;
                        default:
                            search.AddCondition("Status!=" + (int)StatusEnum.删除);
                            break;
                    }
                }
                else
                {
                    search.AddCondition("Status!=" + (int)StatusEnum.删除);
                }

                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {

                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位下的
                    search.AddCondition("SysId='" + CurrentMaster.Cid + "'");
                }
                LogInsert(OperationTypeEnum.访问, "设备箱子列表模块", "访问页面成功.");

                WriteJsonToPage(new BllFireBox().SearchData(search));
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备箱子列表模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion

        #region==超级管理员查询 SearchDataBySuperMain
        /// <summary>
        /// 超级管理员查询
        /// </summary>
        public void SearchDataBySuperMain()
        {
            var search = base.GetSearch();
            try
            {
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                    }
                }
                string CompanyId = CurrentMaster.Cid;//
                if (!string.IsNullOrEmpty(Request["Id"]))
                {
                    string key = Request["Id"].ToString().Trim();
                    if (key != "-1")//查询全部数据
                    {
                        CompanyId = key;
                        search.AddCondition("SysId='" + CompanyId + "'");
                    }
                }
                else
                {
                    search.AddCondition("SysId='" + CompanyId + "'");
                }
                search.AddCondition("Status!=" + (int)StatusEnum.删除);
                LogInsert(OperationTypeEnum.访问, "设备箱子列表模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备箱子列表模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            var jsonResult = new BllFireBox().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModFireBox t)
        {
            BllFireBox bll = new BllFireBox();
            ModJsonResult json = new ModJsonResult();
            try
            {
                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    ModFireBox model = bll.LoadData(t.Id);
                    model.Name = t.Name;
                    model.Address = t.Address;
                    if (t.Name != "" && t.Address != "")
                    {
                        model.Status = (int)StatusEnum.正常;
                    }
                    int result = bll.Update(model);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "设备位置修改", "位置修改失败.");
                    }
                    else
                    {
                        LogInsert(OperationTypeEnum.操作, "设备位置修改", "位置修改成功.");
                    }
                }
                else
                {
                    //string QrCodeNum = t.Id + "X";
                    //Regex reg = new Regex("[-]+");
                    //QrCodeNum = reg.Replace(QrCodeNum, "");
                    t.Id = Guid.NewGuid().ToString();
                    t.Status = (int)StatusEnum.正常;
                    t.Img = QrCode(t.Id + "X");//箱子二维码最后大写X
                    t.QrCode = t.Id;
                    t.SysId = CurrentMaster.Cid;
                    t.EquipmentCount = 0;
                    if (!string.IsNullOrEmpty(Request["CompanyId"]))
                    {
                        if (Request["CompanyId"].ToString().Trim() != "0")
                        {
                            //单位管理员创建的设备.
                            t.SysId = Request["CompanyId"].ToString();
                        }
                    }
                    t.CreateTime = DateTime.Now;//创建时间
                    t.CreaterId = CurrentMaster.Id;//创建人编号
                    int result = bll.Insert(t);

                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "生成失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "设备位置添加", "生成失败.");
                    }
                    else
                    {
                        LogInsert(OperationTypeEnum.操作, "设备位置添加", "生成成功.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备位置添加", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }

        #endregion

        #region ==批量生成二维码 BatchImport
        /// <summary>
        /// 批量生成二维码
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void BatchImport()
        {
            BllFireBox bll = new BllFireBox();
            ModJsonResult json = new ModJsonResult();
            ModFireBox t = new ModFireBox();
            try
            {
                string GroupName = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();//时间戳进行分组
                int CountNum = int.Parse(Request["CountNum"].ToString());
                int resultCount = 0;
                for (int i = 0; i < CountNum; i++)
                {
                    resultCount++;
                    t.Id = Guid.NewGuid().ToString();
                    t.Name = "";//
                    t.Address = "";//
                    t.Status = (int)StatusEnum.禁用;
                    t.Img = QrCode(t.Id + "X");//箱子二维码最后大写X
                    t.QrCode = t.Id;
                    t.SysId = CurrentMaster.Cid;
                    t.EquipmentCount = 0;
                    if (!string.IsNullOrEmpty(Request["CompanyId"]))
                    {
                        if (Request["CompanyId"].ToString().Trim() != "0")
                        {
                            //单位管理员创建的设备.
                            t.SysId = Request["CompanyId"].ToString();
                        }
                    }
                    t.CreateTime = DateTime.Now;//创建时间
                    t.CreaterId = CurrentMaster.Id;//创建人编号
                    int result = bll.Insert(t);
                }
                if (resultCount <= 0)
                {
                    json.success = false;
                    json.msg = "生成失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "批量箱子二维码", "生成失败.");
                }
                else
                {
                    LogInsert(OperationTypeEnum.操作, "批量箱子二维码", "生成成功.");
                }
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "批量箱子二维码", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }

        #endregion

        #region ==加载数据 LoadData
        /// <summary>
        /// 删除
        /// </summary>
        public void LoadData()
        {
            string id = Request["id"].ToString();
            var msg = new ModJsonResult();
            var model = new BllFireBox().LoadData(id);
            WriteJsonToPage(new JavaScriptSerializer().Serialize(model));
        }

        #endregion

        #region ==删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                //判断类型是否还在使用
                var mode = new BllSysAppointed().Exists(" Places in(" + id + ")");
                if (mode == true)
                {
                    msg.success = false;
                    msg.msg = "此地址正在使用,暂不能删除.";
                }
                else
                {
                    if (new BllFireBox().DeleteStatus(id) > 0)
                    {
                        LogInsert(OperationTypeEnum.操作, "设备位置删除", "操作成功.");
                        msg.success = true;
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
                        LogInsert(OperationTypeEnum.操作, "设备位置删除", "操作失败.");
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "设备位置删除", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==导出数据ImportOut
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="mod"></param>
        public void ImportOut()
        {
            string hearder = "二维码编号,图片,箱子位置,查询简码,添加时间";
            string column = "QrCode,Img,Address,Name,CreateTime";
            title = hearder.Split(',');  //导出的标题
            field = column.Split(',');  //导出对应字段
            var search = base.GetSearch();

            try
            {
                string IdList = Request["IdList"].ToString();
                search.AddCondition("Id in(" + IdList + ")");//过滤选中的记录
                search.AddCondition("Status!=" + (int)StatusEnum.删除);

                DataTable ds = new BllSysQRCode().GetList("Fire_FireBox", " and " + search.GetConditon(), "", 0).Tables[0];
                if (ds.Rows.Count > 0)
                {
                    ToExcel(ds);
                }
                LogInsert(OperationTypeEnum.操作, "箱子二维码导出", "二维码导出成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "箱子二维码导出", "操作异常信息:" + ex);
            }
        }

        // <summary>
        /// 图片转换成字节流
        /// </summary>
        /// <param name="img">要转换的Image对象</param>
        /// <returns>转换后返回的字节流</returns>
        public byte[] ImgToByt(Image img)
        {
            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imagedata = ms.GetBuffer();
            return imagedata;
        }
        /// <summary>
        /// 字节流转换成图片
        /// </summary>
        /// <param name="byt">要转换的字节流</param>
        /// <returns>转换得到的Image对象</returns>
        public Image BytToImg(byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            Image img = Image.FromStream(ms);
            return img;
        }
        //
        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        public byte[] getImageByte(string imagePath)
        {
            FileStream files = new FileStream(imagePath, FileMode.Open);
            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dt"></param>
        public void ToExcel1(DataTable dt)
        {
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            doc.FileName = DateTime.Now.ToString("yyyyMMdd") + ".xls";
            string SheetName = "Sheet1";
            //记录条数
            int mCount = dt.Rows.Count;
            Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
            Cells cells = sheet.Cells;

            //第一行表头
            for (int i = 0; i < title.Length; i++)
            {
                cells.Add(1, i + 1, title[i].Trim());
            }
            for (int m = 0; m < mCount; m++)
            {
                for (int j = 0; j < title.Length; j++)
                {
                    //可以直接取图片的地址
                    string filename = Server.MapPath("~/" + dt.Rows[m]["Img"].ToString());
                    byte[] filedata = getImageByte(filename); ;
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(filedata);
                    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                    img.Save(filename);

                    // cells.Add(m + 2, j + 1, dt.Rows[m][j].ToString());

                    cells.Add(m + 2, j + 1, img);
                }
            }
            doc.Send();
            Response.End();
        }
        #endregion

        #region 导出
        protected void ToExcel(DataTable dt)
        {
            if (dt != null)
            {
                #region 操作excel
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                xlWorkBook = new Microsoft.Office.Interop.Excel.Application().Workbooks.Add(Type.Missing);
                xlWorkBook.Application.Visible = false;
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[1];
                //设置标题
                xlWorkSheet.Cells[1, 1] = "二维码编号";
                xlWorkSheet.Cells[1, 2] = "图片";
                xlWorkSheet.Cells[1, 3] = "箱子位置";
                xlWorkSheet.Cells[1, 4] = "查询简码";
                xlWorkSheet.Cells[1, 5] = "添加时间";
                //设置宽度            
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 2]).ColumnWidth = 50;//图片的宽度
                //设置字体
                xlWorkSheet.Cells.Font.Size = 12;
                xlWorkSheet.Cells.Rows.RowHeight = 100;
                #region 为excel赋值

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //为单元格赋值。
                    xlWorkSheet.Cells[i + 2, 1] = dt.Rows[i]["QrCode"].ToString();
                    xlWorkSheet.Cells[i + 2, 2] = "";
                    xlWorkSheet.Cells[i + 2, 3] = dt.Rows[i]["Address"].ToString();
                    xlWorkSheet.Cells[i + 2, 4] = dt.Rows[i]["Name"].ToString();
                    xlWorkSheet.Cells[i + 2, 5] = dt.Rows[i]["CreateTime"].ToString();
                    string filename = Server.MapPath("~/" + dt.Rows[i]["Img"].ToString());
                    if (System.IO.File.Exists(filename))
                    {
                        //声明一个pictures对象,用来存放sheet的图片
                       // xlWorkSheet.Shapes.AddPicture(filename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 220, 100 + i * 100, 100, 100);
                    }
                }
                #endregion
                #region 保存excel文件
                string filePath = Server.MapPath("/UploadFile/QrExport/");
                new FileHelper().CreateDirectory(filePath);
                filePath += DateTime.Now.ToString("yyyymmddHHmmss") + "箱子二维码导出.xls";
                try
                {
                    xlWorkBook.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
                }
                catch { }
                xlWorkBook.Application.Quit();
                xlWorkSheet = null;
                xlWorkBook = null;
                GC.Collect();
                System.GC.WaitForPendingFinalizers();
                #endregion
                #endregion
                #region 导出到客户端
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.AppendHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("箱子二维码导出", System.Text.Encoding.UTF8) + ".xls");
                Response.ContentType = "Application/excel";
                Response.WriteFile(filePath);
                Response.End();
                #endregion
                KillProcessexcel("EXCEL");
            }
        }
        #endregion

        #region 杀死进程
        private void KillProcessexcel(string processName)
        { //获得进程对象，以用来操作
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程
            try
            {
                //获得需要杀死的进程名
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                { //立即杀死进程
                    thisproc.Kill();
                }
            }
            catch (Exception Exc)
            {
                throw new Exception("", Exc);
            }
        }
        #endregion
    }
}
