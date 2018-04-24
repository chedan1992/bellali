using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 登录页
    /// </summary>
    public class LoginController : BaseController<ModSysOperateLog>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

       /// <summary>
       /// 注册
       /// </summary>
       /// <returns></returns>
        public ActionResult Regedit()
        {
            return View();
        }
    }
}
