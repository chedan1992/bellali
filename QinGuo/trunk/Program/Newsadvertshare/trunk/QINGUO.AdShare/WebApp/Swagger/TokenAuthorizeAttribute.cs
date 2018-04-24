using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            if (filterContext.RouteData.Values["token"].ToString() != "1")
            {
                filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                filterContext.RequestContext.HttpContext.Response.End();
                return;
            }
            base.OnAuthorization(filterContext);
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request["token"].ToString() != "1")
            {
                httpContext.Response.Write("无权访问");
                httpContext.Response.End();
                return false;
            }
            return base.AuthorizeCore(httpContext);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            if (filterContext.RouteData.Values["token"].ToString() != "1")
            {
                filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                filterContext.RequestContext.HttpContext.Response.End();
                return;
            }
            base.HandleUnauthorizedRequest(filterContext);
        }  
    }
}