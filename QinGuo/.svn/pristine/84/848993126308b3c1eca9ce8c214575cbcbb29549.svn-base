using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Common
{
   public class ServiceInfo
    {
        /// <summary>
        /// 获取客户端ip地址
        /// </summary>
        /// <returns></returns>
       public static string IPAddressAll()
       {
           string result = String.Empty;
           result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
           if (string.IsNullOrEmpty(result))
           {
               result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
           }
           if (string.IsNullOrEmpty(result))
           {
               result = System.Web.HttpContext.Current.Request.UserHostAddress;
           }
           if (string.IsNullOrEmpty(result))
           {
               return "127.0.0.1";
           }
           return result;
       }
    }
}
