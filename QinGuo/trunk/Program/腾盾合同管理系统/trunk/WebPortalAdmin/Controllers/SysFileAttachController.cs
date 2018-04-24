using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using System.IO;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 系统附件表
    /// </summary>
    public class SysFileAttachController : BaseController<ModSysFileAttach>
    {
        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();

            string ModelCode = Request["ModelCode"];
            string KeyId = Request["KeyId"];

            search.AddCondition("ModelCode='" + ModelCode+"'");
            search.AddCondition("KeyId='" + KeyId + "'");

            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysFileAttach().SearchData(search);
            WriteJsonToPage(jsonResult);
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
                BllSysFileAttach Back = new BllSysFileAttach();
                var Model = Back.LoadData(id);
                if (Model != null)
                {
                    string path = Model.FilePath;//文件路径
                    string savepath = System.Web.HttpContext.Current.Server.MapPath(path);
                    string ContractId = Model.KeyId;//关联合同ID
                    string code = Model.ModelCode;

                    int result = Back.Delete(id);
                    if (result > 0)
                    {
                        msg.success = true;
                        if (System.IO.File.Exists(savepath))
                        {
                            System.IO.File.Delete(savepath);//删除文件
                        }

                        switch (code)
                        {
                            case "InCome"://收入合同 
                            case "OutCome"://支付合同
                                var list= Back.QueryToAll().Where(p => p.KeyId == ContractId).ToList();
                                if (list.Count == 0)
                                {
                                    var temp = new BllContractInOut().LoadData(ContractId);
                                    if (temp != null)
                                    {
                                        temp.HasFileAttach = false;
                                        new BllContractInOut().Update(temp);
                                    }
                                }
                                break;
                        }
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

        #region ==附件下载 FileDownInfo
        /// <summary>
        /// 附件下载
        /// </summary>
        /// <param name="mod"></param>
        public ActionResult FileDownInfo()
        {
            var msg = new ModJsonResult();
            //主键
            var Id = (Request["Id"] == null ? "" : Request["Id"]);
            BllSysFileAttach Back = new BllSysFileAttach();
            var Model = Back.LoadData(Id);
            if (Model != null)
            {
                string filePath = Model.FilePath;
                string fileName = Model.NameOld;
                string extions = Model.Extension;
                filePath = Server.MapPath(filePath);
                if (System.IO.File.Exists(filePath) == true)
                {
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                    Response.ContentType = "application/octet-stream";

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName + extions));
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
            return new EmptyResult();
        }
        #endregion

        #region ===附件上传
        /// <summary>
        ///   附件上传
        /// </summary>
        public void FileUpload()
        {
            var msg = new ModJsonResult();
            try
            {
                if (Request.Files.Count > 0)
                {
                    string FileAttachPath = System.Configuration.ConfigurationManager.AppSettings["FileAttach"];//文件保存路径
                    string Dirc= "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM");

                    string savepath = System.Web.HttpContext.Current.Server.MapPath(FileAttachPath)+Dirc;//获取保存路径

                    if (!Directory.Exists(savepath))
                        Directory.CreateDirectory(savepath);
                    for (var i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase postedFile = Request.Files[i];
                        string sExtension = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));//获取拓展名
                        string fileOldName = postedFile.FileName.Substring(0,postedFile.FileName.LastIndexOf('.'));
                        string sNewFileName = DateTime.Now.ToString("yyyyMMdd-" + Guid.NewGuid().ToString());//文件新名称
                        savepath = savepath + @"/" + sNewFileName + sExtension;//保存路径

                        postedFile.SaveAs(savepath);

                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(savepath);


                        //添加附件到数据库
                        ModSysFileAttach model = new ModSysFileAttach();
                        model.Id = Guid.NewGuid().ToString();
                        model.NameOld = fileOldName;
                        model.NameNew = sNewFileName;
                        model.FilePath = FileAttachPath + Dirc + @"/" + sNewFileName + sExtension;
                        model.FileType = sExtension;
                        model.Extension = sExtension;

                        model.FileSize =new FileHelper().CountSize(fileInfo.Length);
                        
                        model.Cid=CurrentMaster.Cid;
                        model.CreaterId=CurrentMaster.Id;
                        model.CreateTime=DateTime.Now;
                        model.Status=(int)StatusEnum.正常;


                        model.KeyId = Request["KeyId"].ToString();
                        model.ModelCode = Request["ModelCode"].ToString();

                        int result =new BllSysFileAttach().Insert(model);
                    }

                    //修改主表附件信息
                    switch (Request["ModelCode"].ToString())
                    {
                        case "InCome"://收入合同 
                        case "OutCome"://支付合同
                            var temp = new BllContractInOut().LoadData(Request["KeyId"].ToString());
                            if (temp != null)
                            {
                                temp.HasFileAttach = true;
                                new BllContractInOut().Update(temp);
                            }
                            break;

                    }
                    msg.success = true;
                }
            }
            catch (Exception ex)
            {
                msg.success = false;
                msg.msg = "上传失败," + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==文件重命名 ReName
        /// <summary>
        /// 文件重命名
        /// </summary>
        public void ReName(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllSysFileAttach Back = new BllSysFileAttach();
                var Model = Back.LoadData(id);
                if (Model != null)
                {
                    Model.NameOld = Request["FileName"].ToString();

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

    }
}
