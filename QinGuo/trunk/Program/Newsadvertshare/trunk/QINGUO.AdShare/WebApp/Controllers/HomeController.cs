using SwaggerApiDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    /// <summary>
    /// App接口集成
    /// </summary>
    public class HomeController :Controller
    {
        public ActionResult Index()
        {
            Response.Redirect("/webcode/view/home.html");
            return View();
        }
    }
}
