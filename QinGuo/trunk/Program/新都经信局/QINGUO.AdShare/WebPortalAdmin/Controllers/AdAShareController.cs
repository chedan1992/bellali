using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPortalAdmin.Controllers;
using QINGUO.Model;
using QINGUO.Business;

namespace BackstageWeb.Controllers
{
    /// <summary>
    /// 广告审核操作类
    /// </summary>
    public class AdAShareController : BaseController<ModAAShare>
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
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
            var jsonResult = new BllAAShare().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion


        #region ==审批意见

        public void Audit(string id, int status, string Introduction)
        {
            var msg = new ModJsonResult();
            try
            {
                var bll = new BllAAShare();
                var mod = bll.LoadData(id);
                if (mod != null)
                {
                    mod.Status = (FlowEnum)status;
                    mod.Introduction = Introduction;
                    mod.AuditorTime = DateTime.Now;
                    mod.Auditor = CurrentMaster.Id;
                    if (bll.Update(mod) > 0)
                        msg.success = true;
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
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

    }
}
