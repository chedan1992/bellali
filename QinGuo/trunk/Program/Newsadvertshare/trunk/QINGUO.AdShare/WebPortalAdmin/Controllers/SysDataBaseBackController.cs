using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.IO;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 数据库备份
    /// </summary>
    public class SysDataBaseBackController : BaseController<ModSysDataBaseBack>
    {
        public ActionResult SysDataBaseBack()
        {
            return View();
        }

        #region==数据查询 SearchData
        /// <summary>
        /// 数据查询
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            var jsonResult = new BllSysDataBaseBack().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region===数据备份 Back
        /// <summary>
        /// 数据备份 Back
        /// </summary>
        public void Back()
        {
            var msg = new ModJsonResult();
            try
            {
                BllSysDataBaseBack Bll = new BllSysDataBaseBack();
                string dbFileName =DateTime.Now.ToString("yyyyMMddHHmmss")+".bak";
                string path = Server.MapPath("/UploadFile/DataBase");
                new FileHelper().CreateDirectory(path);
                path = path+"//"+dbFileName;
                int result = Bll.BackDataBase(path);
                if (result > 0)
                {
                    //判断文件是否存在
                    if (System.IO.File.Exists(path))
                    {
                        //计算文件大小
                        System.IO.FileInfo objFI = new System.IO.FileInfo(path);
                        string length =new FileHelper().CountSize(objFI.Length);
                        //保存数据库
                        ModSysDataBaseBack model = new ModSysDataBaseBack();
                        model.Id = Guid.NewGuid().ToString();
                        model.LinkUrl = path;
                        model.CreaterId = CurrentMaster.Id;
                        model.CreateTime = DateTime.Now;
                        model.Size =length;
                        model.Status = (int)StatusEnum.正常;
                        model.Remark = dbFileName;
                        if (Bll.Insert(model) > 0)
                        {
                            msg.success = true;
                            msg.msg = "备份成功";
                        }
                        else
                        {
                            msg.success = false;
                            msg.msg = "备份失败";
                            //删除备份文件
                        }
                    }
                    else {
                        msg.success = false;
                        msg.msg = "备份失败";
                    }
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

        #region===数据还原 Rollback
        /// <summary>
        /// 数据还原 Rollback
        /// </summary>
        public void Rollback(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllSysDataBaseBack Bll = new BllSysDataBaseBack();
                var Model = Bll.LoadData(id);
                if (Model != null)
                {
                    int result = Bll.Rollback(Model.LinkUrl);
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
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
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
                BllSysDataBaseBack Back = new BllSysDataBaseBack();
                var Model = Back.LoadData(id);
                if (Model != null)
                {
                    Model.Status = (int)StatusEnum.删除;
                    int result = Back.Update(Model);
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
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
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
            var msg = new ModJsonResult();
            //主键
            var Id = (Request["Id"] == null ? "" : Request["Id"]);
            BllSysDataBaseBack Back = new BllSysDataBaseBack();
            var Model = Back.LoadData(Id);
            if (Model != null)
            {
                string newpath = Model.LinkUrl;
                FileInfo fi = new FileInfo(newpath);
                if (fi.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(Model.Remark));
                    Response.AddHeader("Content-Length", fi.Length.ToString());
                    Response.ContentType = "application/octet-stream;charset=gb2321";
                    Response.WriteFile(fi.FullName);
                    Response.Flush();
                    Response.Close();
                }
            }
        }
        #endregion
    }
}
