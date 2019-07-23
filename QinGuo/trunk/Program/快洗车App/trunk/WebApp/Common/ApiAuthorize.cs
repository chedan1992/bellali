using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApp
{
    public class ApiAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // 验证token
            var ts = actionContext.Request.Headers.Authorization;
            if (ts != null && !string.IsNullOrEmpty(ts.Scheme))
            {
                // 验证token
                if (UserTokenManager.GetCache(ts.Scheme) != null)
                {
                    return true;
                }
                //情况缓存
                UserTokenManager.Remove(ts.Scheme);
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            jsonResult.success = false;
            jsonResult.errorCode = 110;
            jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
            response.Content = new StringContent(Json.Encode(jsonResult), Encoding.UTF8, "application/json");
        }
    }
}