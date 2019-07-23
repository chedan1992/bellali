using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Common;
using Newtonsoft.Json;

namespace Commonlication
{
    public class WxXinController : BaseController
    {

        public void Index(string code = "", string state = "", string returnurl = "register.html")
        {
            string phone = "";
            try
            {
                if (!string.IsNullOrEmpty(state))
                {
                    returnurl = state;
                }
                if (string.IsNullOrEmpty(code))
                {

                    object codeobj = CacheHelper.Get("code");

                    code = codeobj != null ? codeobj.ToString() : "";
                    if (string.IsNullOrEmpty(code))
                    {
                        Response.Redirect("/WxXin/Authorize?returnurl=" + returnurl);
                        return;
                    }
                }
                else
                {
                    CacheHelper.Set("code", code);
                }

                ResultToken r = ACCESS_TOKEN(code);
                if (r != null)
                {
                    var mod = db.Users.FirstOrDefault<User>(c => c.LoginName == r.openid);
                    if (mod == null)
                    {
                        userinfo(r.openid, r.access_token);
                    }
                    Response.Redirect("http://www.bellali.cn/Files/app/" + returnurl + "&id=" + mod.Id);
                    return;
                }
                else
                {
                    CacheHelper.Remove("code");
                    Response.Redirect("/WxXin/Authorize?returnurl=" + returnurl);
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("Index错误日志：", ex.Message);
            }
            return;
        }

        public ResultToken ACCESS_TOKEN(string code)
        {
            //2.得到code作为一个参数调用https://api.weixin.qq.com/sns/oauth2/access_token接口获取到openid
            StringBuilder url = new StringBuilder(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?secret={0}&appid={1}&grant_type=authorization_code&code={2}&state=1#wechat_redirect", WxPayConfig.APPSECRET, WxPayConfig.APPID, code));

            string r = HttpService.Get(url.ToString());

            Log.Debug("接口获取到ACCESS_TOKEN：", r);
            if (!string.IsNullOrEmpty(r))
            {
                var rm = JsonConvert.DeserializeObject<ResultToken>(r);
                if (rm != null && rm.errcode <= 0)
                {
                    string ACCESS_TOKEN = rm.access_token;
                    CacheHelper.Set("access_token", ACCESS_TOKEN);
                    return rm;
                }
            }
            return null;
        }

        public void userinfo(string openid, string access_token)
        {
            StringBuilder userurl = new StringBuilder(string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid=OPENID&lang=zh_CN", access_token, openid));
            string userjson = HttpService.Get(userurl.ToString());
            Log.Debug("接口获取到userinfo：", userjson);
            if (!string.IsNullOrEmpty(userjson))
            {
                var userinfo = JsonConvert.DeserializeObject<resultinfo>(userjson);

                User user = new User();
                user.CreateTime = DateTime.Now;
                user.LoginName = openid;
                user.Name = userinfo.nickname;
                user.NickName = userinfo.nickname;
                user.Sex = Convert.ToInt32(userinfo.sex);
                user.HeadImg = userinfo.headimgurl;
                user.Pwd = "666666";//666666
                user.Status = 1;

                //插入一条数据
                db.Users.Add(user);
                if (db.SaveChanges() > 0)
                {
                    Log.WriteLog("插入一条数据");
                }
            }
        }

        public void Authorize(string returnurl = "")
        {
            //http://www.bellali.cn/WxXin/Index?returnurl=volunteer.html
            //1.调用https://open.weixin.qq.com/connect/oauth2/authorize接口获取到code
            try
            {
                Log.Debug("Authorize/returnurl:", returnurl);
                string redirect_uri = "http://www.bellali.cn/WxXin/Index";
                StringBuilder url = new StringBuilder(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?redirect_uri={0}&appid={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect", redirect_uri, WxPayConfig.APPID, returnurl));
                //string r = HttpService.Get(url.ToString());
                Response.Redirect(url.ToString());
                //LogErrorRecord.InfoFormat("接口获取到code：{0}", r);
            }
            catch (Exception ex)
            {
                Log.Debug("Authorize错误日志：", ex.Message);
            }
        }

    }
}
