using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commonlication
{
    public class Token
    {
        private Token() { }
        private String token = string.Empty;
        private static Token instance = new Token();
        public static Token getInstance()
        {
            return instance;
        }
        public String getToken()
        {
            if (token == string.Empty || TokenExpired(token))
            {
                token = WechatUtil.GetToken();
            }
            return token;
        }
        public void setToken(String token)
        {
            this.token = token;
        }
        /// <summary>
        /// 验证token是否超时
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns>如果超时，返回true</returns>
        public bool TokenExpired(string access_token)
        {
            string jsonStr = HttpRequestUtil.RequestUrl(string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token));
            if (HttpRequestUtil.GetJsonValue(jsonStr, "errcode") == "42001")
            {
                return true;
            }
            return false;
        }

    }
}