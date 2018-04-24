using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using System.Web.Script.Serialization;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers
{
    public class AdvertiseController : BaseController<ModAdActive>
    {
        /// <summary>
        /// 广告列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Advertise()
        {
            return View();
        }
        /// <summary>
        /// 广告浏览
        /// </summary>
        /// <returns></returns>
        public ActionResult AdvertiseView()
        {
            BllAdActive Bll = new BllAdActive();

            string key = "";
            if (!String.IsNullOrEmpty(Request["Id"]))
            {
                key = Request["Id"].ToString();
            }
            var model = Bll.GetModelByWhere(" and Id='" + key + "'");
            ViewData["Name"] = model.ActiveName;
            ViewData["ActionTypeName"] = model.ActionTypeName;
            ViewData["CreateTime"] = Convert.ToDateTime(model.CreateTime).ToString("yyyy-MM-dd hh:mm");
            ViewData["Info"] = model.Info;
            ViewData["ReadNum"] = 0;
            switch (model.ActionType)
            {
                case 1: 
                case 2:
                    ViewData["Info"] = model.Info;
                    break;
                case 3:
                      var Dymaic =new BllEDynamic().LoadData(model.ActionFormId);
                      ViewData["Info"] = Dymaic.Content;
                    break;
            }
            LogInsert(OperationTypeEnum.访问, "广告预览模块", "访问页面成功.");
            return View();
        }


        /// <summary>
        /// 编辑广告
        /// </summary>
        /// <returns></returns>
        public ActionResult AdvertiseEdit()
        {
            ViewData["ID"] = "";//编辑选中记录ID
            if (!String.IsNullOrEmpty(Request["Id"]))
                ViewData["Id"] = Request["Id"];
            ViewData["LookView"] = "";
            if (!String.IsNullOrEmpty(Request["LookView"]))
                ViewData["LookView"] = Request["LookView"];

            LogInsert(OperationTypeEnum.访问, "广告编辑页", "访问页面成功.");
            return View();
        }

        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            try
            {
             
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");//自己查看自己的
                    }
                }
                LogInsert(OperationTypeEnum.访问, "广告管理模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "单位管理模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(new BllAdActive().SearchData(search));
        }
        #endregion

        #region ==禁用状态 EnableUse
        /// <summary>
        /// 禁用状态
        /// </summary>
        public void EnableUse()
        {
            var msg = new ModJsonResult();
            try
            {
                string key = Request["id"];
                int result = new BllAdActive().UpdateStatus(1, key);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                }
                LogInsert(OperationTypeEnum.操作, "广告启用", "启用操作成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "广告启用", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==停用状态 DisableUse
        /// <summary>
        /// DisableUse
        /// </summary>
        public void DisableUse()
        {
            var msg = new ModJsonResult();
            try
            {
                string key = Request["id"];
                int result = new BllAdActive().UpdateStatus(0, key);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                }
                LogInsert(OperationTypeEnum.操作, "广告禁用", "禁用操作成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "广告禁用", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModAdActive t)
        {
            BllAdActive bll = new BllAdActive();
            ModJsonResult json = new ModJsonResult();

            try
            {
                //获取文件
                HttpPostedFileBase postedFile = Request.Files["Img"];
                string filename = postedFile.FileName;//获取上传的文件路径
                t.CreaterId = CurrentMaster.Id;//添加人
                t.Info = t.Info == null ? "" : t.Info;//广告内容
                t.ActionType = int.Parse(Request["ActionType"]);
                t.ShowType = int.Parse(Request["ShowType"]);


                if (t.ActionType == (int)AdvertiseEnum.内部广告 || t.ActionType == (int)AdvertiseEnum.外部广告 || t.ActionType == (int)AdvertiseEnum.资讯广告)
                {
                    if (t.ActionType == (int)AdvertiseEnum.外部广告)//外部广告
                    {
                        t.Info = Request["Link"];
                        t.ActionFormId = "";//清空外键
                    }
                    else if (t.ActionType == (int)AdvertiseEnum.资讯广告)//外部广告
                    {
                        t.Info = Request["Link"];
                        t.ActionFormId = Request["ActionFormId"];
                    }
                    else
                    {
                        t.ActionFormId = "";//清空外键
                    }

                }
                if (t.ShowType == 2)//自动下架
                {
                    t.ActiveStartTime = Convert.ToDateTime(Request["ActiveStartTime"]);//开始时间
                    t.ActiveEndTime = Convert.ToDateTime(Request["ActiveEndTime"]);//结束时间
                }

                string str = "";
                string path = "";//文件路径
                bool succ = true;

                #region ===上传广告图
                if (!String.IsNullOrEmpty(filename) || (!string.IsNullOrEmpty(Request["modify"]) && !string.IsNullOrEmpty(Request["isUpLoad"])))//判断是否修改图片
                {
                    string configPath = System.Configuration.ConfigurationManager.AppSettings["PathAdvertise"];//广告图片保存路径
                    if (new PicFileUpLoad().UpLoad("Advertise", postedFile, configPath, filename, out path, out str))
                    {
                        t.Img = path;
                    }
                    else
                    {
                        succ = false;
                    }
                }

                #endregion

                #region ===保存修改数据
                if (succ)//判断广告图是否上传正确
                {
                    if (!string.IsNullOrEmpty(Request["modify"])) //修改
                    {
                        var Category = bll.LoadData(t.Id);
                        if (String.IsNullOrEmpty(t.Img))
                        {
                            t.Img = Category.Img;//图片
                        }
                        t.CreateTime = Category.CreateTime;
                        t.Status = Category.Status;//数据状态
                        if (t.ActiveEndTime > DateTime.Now && t.Status != (int)StatusEnum.禁用)
                        {
                            t.Status = (int)StatusEnum.正常;
                        }
                        int result = bll.Update(t);
                        if (result <= 0)
                        {
                            json.success = false;
                            json.msg = "修改失败,请稍后再操作!";
                        }
                        else
                        {
                            json.msg = "/Advertise/SaveData?Id='" + t.Id + "'&modify=1";
                        }
                        LogInsert(OperationTypeEnum.操作, "广告修改操作", "广告修改操作成功");
                    }
                    else
                    {
                        t.Id = Guid.NewGuid().ToString();
                        t.Status = (int)StatusEnum.正常;//状态
                        t.CreateTime = DateTime.Now;
                        t.CompanyId = CurrentMaster.Cid;
                        if (t.Img == null)
                        {
                            t.Img = "/Resource/img/null.jpg";
                        }
                        int result = bll.Insert(t);
                        if (result <= 0)
                        {
                            json.success = false;
                            json.msg = " 保存失败,请稍后再操作!";
                        }
                        else
                        {
                            json.msg = "/Advertise/SaveData?ID='" + t.Id + "'&modify=1";
                        }
                        LogInsert(OperationTypeEnum.操作, "广告保存操作", "广告保存操作成功");
                    }
                }
                else
                {
                    json.success = false;
                    json.msg = str;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "广告保存/修改操作", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }

        #region ===删除添加的广告,以便同步数据 Delete
        /// <summary>
        /// 删除添加的广告,以便同步数据
        /// </summary>
        /// <param name="t"></param>
        public void Delete(ModAdActive t)
        {
            //删除数据
            new BllAdActive().Delete(t.Id);
            //删除图片
            if (!string.IsNullOrEmpty(t.Img))
            {
                string picPath = Server.MapPath("~" + t.Img.Replace("\\", "/"));//获取服务端文件的绝对路径，同时注意拼接成相对与网站根目录的字符串。     
                if (System.IO.File.Exists(picPath))
                {
                    try
                    {
                        System.IO.File.Delete(picPath);
                    }
                    catch
                    {
                        //错误处理：         
                    }
                }

            }
        }

        #endregion

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
                BllAdActive Master = new BllAdActive();
                var Model = Master.LoadData(id);
                if (Model != null)
                {
                    Model.Status = (int)StatusEnum.删除;
                    int result = Master.Update(Model);
                    if (result > 0)
                    {
                        msg.success = true;
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
                    }
                    LogInsert(OperationTypeEnum.操作, "广告删除", "广告删除操作成功.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "广告删除", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

       
        #region ==编辑加载数据 LoadData()
        /// <summary>
        /// 根据id 加载数据
        /// </summary>
        public void LoadData()
        {
            string Id = Request["Id"].ToString();
            var list = new BllAdActive().GetModelByWhere(" and Id='" + Id + "'");
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        #endregion

    }
}
