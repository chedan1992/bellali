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
    /// 意见反馈操作类
    /// </summary>
    public class AdviseController : BaseController<ModSysFeedback>
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Advise()
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
            var jsonResult = new BllSysFeedback().SearchData(search);
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
                int result = new BllSysFeedback().DeleteStatus(id);
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

    }
}
