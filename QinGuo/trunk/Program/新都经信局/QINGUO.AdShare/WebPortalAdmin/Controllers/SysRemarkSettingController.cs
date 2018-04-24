using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Business;
using QINGUO.Model;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 基本信息介绍
    /// </summary>
    public class SysRemarkSettingController : BaseController<ModSysMaster>
    {
        /// <summary>
        /// 注册协议
        /// </summary>
        /// <returns></returns>
        public ActionResult MustRead()
        {
            string content = new BllSysCompany().GetSetting(1);
            ViewData["MustRead"] = content;

            return View();
        }

         /// <summary>
        /// 工程师合作流程
        /// </summary>
        /// <returns></returns>
        public ActionResult Purserule()
        {
            string content = new BllSysCompany().GetSetting(3);
            ViewData["Purserule"] = content;

            return View();
        }

        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            string content = new BllSysCompany().GetSetting(2);
            ViewData["About"] = content;

            return View();
        }
        /// <summary>
        /// 帮助中心
        /// </summary>
        /// <returns></returns>
        public ActionResult Helper()
        {
            string content = new BllSysCompany().GetSetting(4);
            ViewData["Helper"] = content;
            return View();
        }
        /// <summary>
        /// 免责申明
        /// </summary>
        /// <returns></returns>
        public ActionResult MZSM()
        {
            string content = new BllSysCompany().GetSetting(5);
            ViewData["MZSM"] = content;
            return View();
        }

        /// <summary>
        /// 保存注册协议
        /// </summary>
        [ValidateInput(false)]
        public void SavaMustRead()
        {
            var msg = new ModJsonResult();
            try
            {
                string txteditor = Request["txteditor"];
                int result = new BllSysCompany().UpdateSetting(1,txteditor);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                }
            }
            catch (Exception a)
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
            WriteJsonToPage(msg.ToString());
        }

        /// <summary>
        /// 保存工程师合作流程
        /// </summary>
        [ValidateInput(false)]
        public void SavaPurserule()
        {
            var msg = new ModJsonResult();
            try
            {
                string txteditor = Request["txteditor"];
                int result = new BllSysCompany().UpdateSetting(3, txteditor);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                }
            }
            catch (Exception a)
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
            WriteJsonToPage(msg.ToString());
        }

        /// <summary>
        /// 保存关于我们
        /// </summary>
        [ValidateInput(false)]
        public void SavaAbout()
        {
            var msg = new ModJsonResult();
            try
            {
                string txteditor = Request["txteditor"];
                int result = new BllSysCompany().UpdateSetting(2, txteditor);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                }
            }
            catch (Exception a)
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
            WriteJsonToPage(msg.ToString());
        }

        /// <summary>
        /// 保存帮助中心
        /// </summary>
        [ValidateInput(false)]
        public void SavaHelper()
        {
            var msg = new ModJsonResult();
            try
            {
                string txteditor = Request["txteditor"];
                int result = new BllSysCompany().UpdateSetting(4, txteditor);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                }
            }
            catch (Exception a)
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
            WriteJsonToPage(msg.ToString());
        }

        /// <summary>
        /// 保存免责申明
        /// </summary>
        [ValidateInput(false)]
        public void SavaMZSM()
        {
            var msg = new ModJsonResult();
            try
            {
                string txteditor = Request["txteditor"];
                int result = new BllSysCompany().UpdateSetting(5, txteditor);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                }
            }
            catch (Exception a)
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
            WriteJsonToPage(msg.ToString());
        }
    }
}
