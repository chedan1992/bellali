using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPortalAdmin.Controllers
{
    public class TempleController : Controller
    {
        /// <summary>
        /// 错误页
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginError()
        {
            return View();
        }

        /// <summary>
        /// 错误页
        /// </summary>
        /// <returns></returns>
        public ActionResult PageError()
        {
            return View();
        }

        /// <summary>
        /// App下载页
        /// </summary>
        /// <returns></returns>
        public ActionResult DownLoad()
        {
            return View();
        }

        /// <summary>
        /// Web下载页
        /// </summary>
        /// <returns></returns>
        public ActionResult WebDownLoad()
        {
            return View();
        }
    }
}
