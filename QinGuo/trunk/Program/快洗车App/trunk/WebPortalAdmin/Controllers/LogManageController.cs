using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using System.Data;
using AppLibrary.WriteExcel;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public class LogManageController : BaseController<ModSysOperateLog>
    {
        string[] title;  //导出的标题
        string[] field;  //导出对应字段
        /// <summary>
        /// 日志管理
        /// </summary>
        /// <returns></returns>
        public ActionResult LogManage()
        {
            return View();
        }

        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            string GroupId = Request["GroupId"].ToString();
            if (GroupId != "-1")
            {
                search.AddCondition("LogType='" + GroupId + "'");
            }
            var jsonResult = new BllSysOperateLog().SearchData(search);

            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region ==导出数据ImportOut
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="mod"></param>
        public void ImportOut()
        {
            string hearder = "用户名,访问地址,信息介绍,访问IP,访问时间";
            string column = "UserName,LinkUrl,Remark,IPAddress,CreateTime";
            title = hearder.Split(',');  //导出的标题
            field = column.Split(',');  //导出对应字段
            var search = base.GetSearch();
            DataTable ds = new BllSysOperateLog().ExportOut("").Tables[0];
            if (ds.Rows.Count > 0)
            {
                ToExcel(ds);
            }
        }
        #endregion

        #region ===导出 ToExcel
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dt"></param>
        public void ToExcel(DataTable dt)
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
                    cells.Add(m + 2, j + 1, dt.Rows[m][j].ToString());
                }
            }
            doc.Send();
            Response.End();
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
                int result = new BllSysOperateLog().Delete(id);
                if (result > 0)
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

        #region ==清空 DeleteAll
        /// <summary>
        /// 清空
        /// </summary>
        public void DeleteAll()
        {
            var msg = new ModJsonResult();
            try
            {
                string GroupId = Request["GroupId"].ToString();
                string where = "";
                if (GroupId != "-1")
                {
                    where = " and LogType=" + GroupId;
                }
                int result = new BllSysOperateLog().DeleteAll(where);
                msg.success = true;
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion
        

    }
}
