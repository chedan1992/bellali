using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Data;
using System.Web.Script.Serialization;
using ThoughtWorks.QRCode.Codec;
using System.Configuration;
using System.Drawing;
using AppLibrary.WriteExcel;
using NPOI.HSSF.UserModel;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;
using System.IO;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 二维码管理
    /// </summary>
    public class SysQRCodeController : BaseController<ModSysQRCode>
    {
        string[] title;  //导出的标题
        string[] field;  //导出对应字段
        /// <summary>
        /// 页面管理
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
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                    search.AddCondition("Status=" + (int)StatusEnum.禁用);
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位下的
                    search.AddCondition("SysId='" + CurrentMaster.Cid + "'");
                    search.AddCondition("Status=" + (int)StatusEnum.禁用);
                }
                LogInsert(OperationTypeEnum.访问, "二维码列表模块", "访问页面成功.");

                WriteJsonToPage(new BllSysQRCode().SearchData(search));
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "二维码列表模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData()
        {
            BllSysQRCode bll = new BllSysQRCode();
            ModJsonResult json = new ModJsonResult();
            ModSysQRCode t = new ModSysQRCode();
            try
            {
                string GroupName = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now).ToString();//时间戳进行分组
                int CountNum = int.Parse(Request["CountNum"].ToString());
                int resultCount = 0;
                for (int i = 0; i < CountNum; i++)
                {
                    resultCount++;
                    t.Id = Guid.NewGuid().ToString();
                    t.Name = GroupName + resultCount.ToString("00000");//二维码编号
                    t.GroupName = GroupName;//二维码分组
                    t.Status = (int)StatusEnum.禁用;
                    t.Img = QrCode(t.Id+"S");//设备二维码最后大写S
                    t.QrCode = t.Id;
                    t.SysId = CurrentMaster.Cid;
                    t.CreateTime = DateTime.Now;//创建时间
                    t.CreaterId = CurrentMaster.Id;//创建人编号
                    int result = bll.Insert(t);
                }
                if (resultCount <= 0)
                {
                    json.success = false;
                    json.msg = "生成失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "批量二维码", "生成失败.");
                }
                else
                {
                    LogInsert(OperationTypeEnum.操作, "批量二维码", "生成成功.");
                }
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "批量二维码", "操作异常信息:" + ex);
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
            var model = new BllSysQRCode().LoadData(id);
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
                string sql = "update Sys_QRCode set Status="+(int)StatusEnum.删除+" where Id in(" + id + ")";
                if (new BllSysQRCode().ExecuteNonQueryByText(sql) > 0)
                //int result = new BllSysQRCode().DeleteStatus(id);
                {
                    LogInsert(OperationTypeEnum.操作, "二维码删除", "操作成功.");
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "二维码删除", "操作失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "二维码删除", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region 查询型号列表 SearchModelData

        /// <summary>
        /// 查询型号列表
        /// </summary>
        public void SearchModelData()
        {
            Search search = this.GetSearch();
            if (!string.IsNullOrEmpty(Request["key"]))
            {
                search.AddCondition("LinkId='" + Request["key"].ToString() + "'");
            }
            WriteJsonToPage(new BllSysModel().SearchData(search));
        }
        #endregion

        #region 保存表单 SaveModelData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveModelData(ModSysModel mod)
        {
            BllSysModel Bll = new BllSysModel();
            ModJsonResult json = new ModJsonResult();

            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                ModSysModel modLod = Bll.LoadData(mod.Id);
                modLod.ModelName = mod.ModelName;
                modLod.Sort = mod.Sort;
                modLod.Remark = mod.Remark;
                int result = Bll.Update(modLod);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else
            {
                mod.CreateTime = DateTime.Now;
                mod.CompanyId = CurrentMaster.Cid;
                mod.Id = Guid.NewGuid().ToString();
                mod.Status = (int)StatusEnum.正常;
                int result = Bll.Insert(mod);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "新增失败,请稍后再操作!";
                }
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ==删除 DeleteModelData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteModelData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllSysQRCode Bll = new BllSysQRCode();
                if (Bll.DeleteStatus(id) > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #region ==弹框根据加载数据 SearchViewData()
        /// <summary>
        /// 弹框根据加载数据
        /// </summary>
        public void SearchViewData()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("Status=" + (int)StatusEnum.禁用);
                string CompanyId = Request["CompanyId"].ToString().Trim();//
                if (CompanyId != "")
                {
                    search.AddCondition("SysId='" + CompanyId + "'");
                }
                else
                {
                    search.AddCondition("SysId='" + CurrentMaster.Cid + "'");
                }
                WriteJsonToPage(new BllSysQRCode().SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
            }
        }
        #endregion

        #region ==导出数据ImportOut
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="mod"></param>
        public void ImportOut()
        {
            string hearder = "二维码编号,图片,添加时间";
            string column = "Name,Img,CreateTime";
            title = hearder.Split(',');  //导出的标题
            field = column.Split(',');  //导出对应字段
            var search = base.GetSearch();

            try
            {
                string IdList = Request["IdList"].ToString();
                search.AddCondition("Id in(" + IdList + ")");//过滤选中的记录
                search.AddCondition("Status!=" + (int)StatusEnum.删除);

                DataTable ds = new BllSysQRCode().GetList("Sys_QRCode", " and " + search.GetConditon(), "", 0).Tables[0];
                if (ds.Rows.Count > 0)
                {
                    ToExcel(ds);
                }
                LogInsert(OperationTypeEnum.操作, "二维码导出", "二维码导出成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "二维码导出", "操作异常信息:" + ex);
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
                xlWorkSheet.Cells[1, 3] = "添加时间";
                //设置宽度            
                ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 2]).ColumnWidth = 50;//图片的宽度
                //设置字体
                xlWorkSheet.Cells.Font.Size = 12;
                xlWorkSheet.Cells.Rows.RowHeight = 100;
                #region 为excel赋值
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //为单元格赋值。
                    xlWorkSheet.Cells[i + 2, 1] = dt.Rows[i]["Name"].ToString();
                    xlWorkSheet.Cells[i + 2, 2] = "";
                    xlWorkSheet.Cells[i + 2, 3] = dt.Rows[i]["CreateTime"].ToString();
                    string filename = Server.MapPath("~/" + dt.Rows[i]["Img"].ToString());
                    if (System.IO.File.Exists(filename))
                    {
                        //声明一个pictures对象,用来存放sheet的图片
                        //xlWorkSheet.Shapes.AddPicture(filename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 220, 100 + i * 100, 100, 100);
                    }
                }
                #endregion
                #region 保存excel文件
                string filePath = Server.MapPath("/UploadFile/QrExport/");
                new FileHelper().CreateDirectory(filePath);
                filePath += DateTime.Now.ToString("yyyymmddHHmmss")+"设备二维码导出.xls";
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
                Response.AppendHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("导出", System.Text.Encoding.UTF8) + ".xls");
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
