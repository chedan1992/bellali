using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Common;
using System.IO;
using System.Web.Script.Serialization;

namespace WebPortalAdmin.Controllers
{
    public class CommonController : BaseController<ModSysCompany>
    {
        /// <summary>
        /// 图片上传公用方法,谨慎修改
        /// </summary>
        public void UpImage()
        {
            var json = new ModJsonResult();
            try
            {
                string filepath = Request["filepath"].ToString();
                string vpath = "~/UploadFile/" + CurrentMaster.Cid + "/" + filepath + "/";
                string uppath = Server.MapPath(vpath);
                FileHelper fh = new FileHelper();
                fh.CreateDirectory(uppath);//创建文件夹

                HttpFileCollectionBase files = Request.Files;

                if (files != null && files.Count > 0)
                {
                    HttpPostedFileBase file = files["userFile"];
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            string key = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString();

                            string geshi = Path.GetExtension(file.FileName);
                            string guidName = key + geshi;
                            string str = "";
                            if (UploadFile.Upload(file, uppath + guidName, 1024, out str))
                            {
                                json.success = true;
                                json.msg = "/UploadFile/" + CurrentMaster.Cid + "/" + filepath + "/" + guidName;
                            }
                            else
                            {
                                json.success = false;
                                json.msg = str;
                            }
                            json.data = key;
                        }
                        else
                        {
                            json.success = false;
                            json.msg = "请选择上传图片";
                        }
                    }
                    else
                    {
                        json.success = false;
                        json.msg = "请选择上传图片";
                    }
                }
            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = ex.Message.ToString();
            }
            var result = new JavaScriptSerializer().Serialize(json);
            Response.Write(result);
            Response.End();
        }


        /// <summary>
        /// 文件上传公用方法,谨慎修改
        /// </summary>
        public void UpFile()
        {
            var json = new ModJsonResult();
            try
            {
                string filepath = Request["filepath"].ToString();
                string vpath = "~/UploadFile/" + CurrentMaster.Cid + "/" + filepath + "/";
                string uppath = Server.MapPath(vpath);
                FileHelper fh = new FileHelper();
                fh.CreateDirectory(uppath);//创建文件夹

                HttpFileCollectionBase files = Request.Files;

                if (files != null && files.Count > 0)
                {
                    HttpPostedFileBase file = files["userFile"];
                    if (file != null)
                    {
                        if (file.ContentLength > 0)
                        {
                            string key = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString();
                            string geshi = Path.GetExtension(file.FileName);
                            string guidName = key + geshi;
                            string str = "";
                            if (UploadFile.Upload(file, uppath + guidName, 1024, out str))
                            {
                                json.success =true;
                                json.msg = "/UploadFile/" + CurrentMaster.Cid + "/" + filepath + "/" + guidName;//文件新名称
                            }
                            else
                            {
                                json.success =false;
                                json.msg = str;
                            }
                            json.data = file.FileName;//文件原来名称
                        }
                        else
                        {
                            json.success = false;
                            json.msg = "请选择上传图片";
                        }
                    }
                    else
                    {
                        json.success = false;
                        json.msg = "请选择上传图片";
                    }
                }
            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = ex.Message.ToString();
            }


            var result = new JavaScriptSerializer().Serialize(json);
            Response.Write(result);
            Response.End();
        }
    }
}
