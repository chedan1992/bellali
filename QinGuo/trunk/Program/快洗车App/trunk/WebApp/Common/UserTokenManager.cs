using QINGUO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class UserTokenManager
    {
        public static JsonUser GetCache(string token)
        {
            try
            {
                JsonUser r = (JsonUser)CacheHelper.GetCache(token);
                if (r != null)
                {
                    return r;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetCache(string token, JsonUser r)
        {
            CacheHelper.SetCache(token, r, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromDays(90));
        }

        public static void Remove(string token)
        {
            CacheHelper.Remove(token);
        }

    }
}