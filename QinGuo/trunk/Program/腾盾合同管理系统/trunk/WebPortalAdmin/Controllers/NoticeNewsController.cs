using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 新闻公告管理类
    /// </summary>
    public class NoticeNewsController : BaseController<ModAdActive>
    {
       
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
         
            return View();
        }

        /// <summary>
        /// 公告浏览
        /// </summary>
        /// <returns></returns>
        public ActionResult LookView()
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

       
            return View();
        }


        /// <summary>
        /// 编辑公告
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            ViewData["ID"] = "";//编辑选中记录ID
            if (!String.IsNullOrEmpty(Request["Id"]))
                ViewData["Id"] = Request["Id"];
            ViewData["LookView"] = "";
            if (!String.IsNullOrEmpty(Request["LookView"]))
                ViewData["LookView"] = Request["LookView"];

            return View();
        }

        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");//自己查看自己的
                }
            }
            var jsonResult = new BllAdActive().SearchData(search);

            LogInsert(OperationTypeEnum.访问, "新闻公告", CurrentMaster.UserName + "页面访问正常.");

            WriteJsonToPage(jsonResult);
        }
        #endregion

        #region ==禁用状态 EnableUse
        /// <summary>
        /// 禁用状态
        /// </summary>
        public void EnableUse()
        {
            var msg = new ModJsonResult();
            string key = Request["id"];
            int result = new BllAdActive().UpdateStatus(1, key);
            if (result > 0)
            {
                msg.success = true;

                LogInsert(OperationTypeEnum.操作, "新闻公告", CurrentMaster.UserName + "公告启用操作成功.");
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";

                LogInsert(OperationTypeEnum.异常, "新闻公告", CurrentMaster.UserName + "公告启用操作,操作失败.");
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

            string key = Request["id"];
            int result = new BllAdActive().UpdateStatus(0, key);
            if (result > 0)
            {
                msg.success = true;
                LogInsert(OperationTypeEnum.操作, "新闻公告", CurrentMaster.UserName + "公告禁用操作成功.");
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";

                LogInsert(OperationTypeEnum.异常, "新闻公告", CurrentMaster.UserName + "公告禁用操作,操作失败.");
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

            t.CreaterId = CurrentMaster.Id;//添加人
            t.Info = t.Info == null ? "" : t.Info;//公告内容

            if (t.ActionType == 2)
            {
                t.ActiveStartTime = Convert.ToDateTime(Request["ActiveStartTime"]);//开始时间
                t.ActiveEndTime = Convert.ToDateTime(Request["ActiveEndTime"]);//结束时间
            }
            t.ActionFormId = Request["ActionFormId"];

            #region ===保存修改数据
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
                if (t.ActionType == 1)//无时间限制
                {
                    if (t.Status == (int)StatusEnum.未激活)//已过期
                    {
                        t.Status = (int)StatusEnum.正常;//数据状态
                    }
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
            }
            else
            {
                t.Id = Guid.NewGuid().ToString();
                t.Status = (int)StatusEnum.正常;//状态
                t.CreateTime = DateTime.Now;

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
            }
            #endregion

            LogInsert(OperationTypeEnum.操作, "新闻公告", CurrentMaster.UserName + "公告新增或修改操作正常.");

            WriteJsonToPage(json.ToString());
        }

        #region ===删除添加的公告,以便同步数据 Delete
        /// <summary>
        /// 删除添加的公告,以便同步数据
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
                    LogInsert(OperationTypeEnum.操作, "新闻公告", CurrentMaster.UserName + "公告删除操作正常.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "新闻公告", CurrentMaster.UserName + "公告删除操作异常." + msg.msg);
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
