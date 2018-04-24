using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net.Config;

namespace WebPortalAdmin
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
          public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Login", id = UrlParameter.Optional } // 参数默认值
            );

        }
       public void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            Application.Add("user_online", 0); // 在应用程序启动时运行的代码
        }

       public void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码

        }

       public void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

        }

       public void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码
            Application.Lock();
            Application["user_online"] = (int)Application["user_online"] + 1;
            Application.UnLock();
        }

       public void Session_End(object sender, EventArgs e)
       {
           // 在会话结束时运行的代码。 
           // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
           // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
           // 或 SQLServer，则不会引发该事件。
           Application.Lock();
           int count = (int)Application["user_online"];
           if (count > 1)
           {
               Application["user_online"] = count - 1;
           }
           Application.UnLock();
       }
    }
}