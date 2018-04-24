using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspx.Business;
using Aspx.Model;
using Aspx.Common;
using System.IO;
using System.Configuration;
using Dapper;

namespace InterFaceWeb.Controllers
{
    public class CommController : BaseController
    {
        //version
        //[HttpGet]
        public JsonResult CheckVersion(string phonetype, string version)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：version={0}&phonetype={1}", version, phonetype);
                CheckParams(version, "手机版本不能为空！");
                CheckParams(phonetype, "手机类型不能为空！");

                string tempversion = ConfigurationManager.AppSettings[phonetype + "V"];
                string managerdomain = ConfigurationManager.AppSettings["managerdomain"];

                if (!string.IsNullOrEmpty(tempversion) && tempversion != version)
                {
                    if (phonetype == "ios")
                    {
                        //jsonResult.data = managerdomain + "/file/" + phonetype + ".ipa";
                        jsonResult.data = managerdomain + "/file/ios.ipa";
                    }
                    else
                    {
                        //jsonResult.data = managerdomain + "/file/" + phonetype + ".apk";
                        jsonResult.data = managerdomain + "/file/android.apk";
                    }
                    jsonResult.success = true;
                    jsonResult.msg = "有新版本，快去更新吧！";
                }
                else
                {
                    jsonResult.success = false;
                    jsonResult.msg = "暂无更新";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }


        //意见反馈
        //[HttpGet]
        public JsonResult Feedback(string userid, string backinfo, string phonetype)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&backinfo={1}&phonetype={2}", userid, backinfo, phonetype);

                CheckParams(userid, "用户不能为空！");
                CheckParams(backinfo, "反馈不能为空！");
                CheckParams(phonetype, "手机类型不能为空！");

                if (jsonResult.success)
                {
                    BllSysFeedback bllSysFeedback = new BllSysFeedback();
                    ModSysFeedback mod = new ModSysFeedback();
                    mod.Id = Guid.NewGuid().ToString();
                    mod.CreateTime = DateTime.Now;
                    mod.BackInfo = backinfo;
                    mod.PhoneType = phonetype;
                    mod.Status = (int)StatusEnum.正常;
                    mod.UserId = userid;
                    mod.IsAccept = false;//默认没接受
                    mod.CreaterId = "";

                    if (bllSysFeedback.Insert(mod) > 0)
                    {
                        jsonResult.msg = "我们已经收到了您的反馈信息！谢谢您";
                        jsonResult.success = true;
                    }
                    else
                    {
                        jsonResult.msg = "系统繁忙，请稍后再试！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //页面（关于我们）
        public ActionResult Page(string id)
        {
            BllSysRemarkSetting Bll = new BllSysRemarkSetting();
            var mod = Bll.LoadData(id);
            return View(mod);
        }

        //上传图片
        //[HttpGet]
        public JsonResult Upimg()
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);

                if (jsonResult.success)
                {
                    HttpFileCollectionBase files = Request.Files;//获取文件

                    string seavPath = ConfigurationManager.AppSettings["UpImg"];//文件保存路劲
                    string BigPath = Server.MapPath("~" + seavPath + "BigImg/");//大图片
                    string tmpPath = Server.MapPath("~" + seavPath);//小图片
                    FileHelper fh = new FileHelper();
                    fh.CreateDirectory(BigPath);//创建文件夹
                    fh.CreateDirectory(tmpPath);//创建文件夹
                    if (files != null && files.Count > 0)
                    {
                        string r = "", msg = ""; ;
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string geshi = Path.GetExtension(file.FileName);//文件格式
                            string guidName = DateTime.Now.ToString("yyyyMMdd-" + Guid.NewGuid().ToString()) + geshi;

                            string str = "";
                            if (UploadFile.UploadSmallImg(file, BigPath + guidName, tmpPath + guidName, 256, 256, 5120, out str))
                            {
                                if (i == files.Count - 1)
                                {
                                    r += seavPath + guidName;//"\"" + seavPath + guidName + "\"";
                                }
                                else
                                {
                                    r += seavPath + guidName + ",";
                                }
                            }
                            else
                            {
                                if (i == files.Count - 1)
                                {
                                    msg += str;
                                }
                                else
                                {
                                    msg += str + ",";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(r))
                        {
                            jsonResult.data = r;
                            jsonResult.success = true;
                            jsonResult.msg = "上传成功！";
                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = msg;
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "未获取到客户端文件！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //上传语音
        //[HttpGet]
        public JsonResult Upvoice()
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);

                if (jsonResult.success)
                {
                    HttpFileCollectionBase files = Request.Files;//获取文件

                    string seavPath = ConfigurationManager.AppSettings["UpVoice"];//文件保存路劲
                    string tmpPath = Server.MapPath("~" + seavPath);//小图片
                    FileHelper fh = new FileHelper();
                    fh.CreateDirectory(tmpPath);//创建文件夹
                    if (files != null && files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string geshi = Path.GetExtension(file.FileName);//文件格式
                            string guidName = DateTime.Now.ToString("yyyyMMdd-" + Guid.NewGuid().ToString()) + geshi;
                            string str = "";
                            if (UploadFile.Upload(file, tmpPath + guidName, 5120, out str))
                            {
                                jsonResult.data = seavPath + guidName;//"\"" + seavPath + guidName + "\"";
                                jsonResult.success = true;
                                jsonResult.msg = "上传成功！";
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = str;
                            }
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "未获取到客户端文件！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //广告、幻灯片
        //[HttpGet]
        public JsonResult GetAdActive(int top = 8)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：top={0}", top);
                BllAdActive bllAdActive = new BllAdActive();

                if (jsonResult.success)
                {
                    List<ModAdActive> list = bllAdActive.QueryAll(top);

                    jsonResult.data = list;
                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //页面（广告）
        public ActionResult PageAdActive(string id)
        {
            BllAdActive bllAdActive = new BllAdActive();
            var mod = bllAdActive.LoadData(id);
            return View(mod);
        }

        //法律、新闻、消费知识 文档类型(2:消防知识 3:新闻管理 4:法律法规)
        //[HttpGet]
        public JsonResult GetDynamic(string GroupId)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：GroupId={0}", GroupId);
                CheckParams(GroupId, "文档类型不能为空！");
                BllEDynamic bllEDynamic = new BllEDynamic();
                if (jsonResult.success)
                {
                    Search search = GetSearch();
                    search.AddCondition("GroupId='" + GroupId + "'");

                    Page<ModEDynamic> r = bllEDynamic.GetDynamic(search);
                    jsonResult.data = r.Items;


                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //页面（法律、新闻、消费知识）
        public ActionResult PageDynamic(string id)
        {
            BllEDynamic bllEDynamic = new BllEDynamic();
            var mod = bllEDynamic.LoadData(id);

            if (mod != null)
            {//修改阅读数
                mod.ReadNum++;
                bllEDynamic.Update(mod);
            }
            return View(mod);
        }
    }
}
