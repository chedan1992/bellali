using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Commonlication
{
    public class WxXinController : BaseController
    {

        //修改订单信息
        public void SuccOrder()
        {
            WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
            try
            {
                Log.WriteLog("SuccOrder:开始了");
                WxPayAPI.WxPayData notifyData = GetNotifyData();
                string out_trade_no = notifyData.GetValue("out_trade_no").ToString();

                Log.WriteLog("SuccOrder:" + out_trade_no);
                //查询订单，判断订单真实性
                if (!string.IsNullOrEmpty(out_trade_no))
                {
                    Order order = db.Orders.FirstOrDefault(c => c.Payno == out_trade_no);
                    if (order != null)
                    {
                        order.Status = 1;
                        order.PayTime = DateTime.Now;
                        db.Orders.Update(order);
                        if (db.SaveChanges() > 0)
                        {
                            Log.WriteLog("SuccOrder:Status");

                            var active = db.Actives.Find(order.ActiveId);
                            active.Count++;
                            db.Actives.Update(active);
                            db.SaveChanges();

                            res.SetValue("return_code", "SUCCESS");
                            res.SetValue("return_msg", "OK");
                            Response.WriteAsync(res.ToXml());
                            Log.WriteLog("SuccOrder:SUCCESS");
                        }
                    }
                    else
                    {
                        Log.WriteLog("SuccOrder:订单查询失败");
                        res.SetValue("return_code", "FAIL");
                        res.SetValue("return_msg", "订单查询失败");
                        Response.WriteAsync(res.ToXml());
                    }
                }
            }
            catch (Exception e)
            {
                //打印日志
                Log.WriteLog("SuccOrder:Message=" + e.Message);
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", e.Message);
                Response.WriteAsync(res.ToXml());
            }
        }


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
                        mod = userinfo(r.openid, r.access_token);
                    }
                    if (mod != null)
                    {
                        Response.Redirect("http://hjj.bellali.cn/Files/app/" + returnurl + "&userid=" + mod.Id);
                    }
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
                Log.WriteLog("Index错误日志：" + ex.Message);
            }
            return;
        }


        public ResultToken ACCESS_TOKEN(string code)
        {
            //2.得到code作为一个参数调用https://api.weixin.qq.com/sns/oauth2/access_token接口获取到openid
            StringBuilder url = new StringBuilder(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?secret={0}&appid={1}&grant_type=authorization_code&code={2}&state=1#wechat_redirect", APPSECRET, APPID, code));

            string r = HttpService.Get(url.ToString());

            Log.WriteLog("接口获取到ACCESS_TOKEN：" + r);
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

        public User userinfo(string openid, string access_token)
        {
            StringBuilder userurl = new StringBuilder(string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid=OPENID&lang=zh_CN", access_token, openid));
            string userjson = HttpService.Get(userurl.ToString());
            Log.WriteLog("接口获取到userinfo：" + userjson);
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
                    return user;
                }
            }
            return null;
        }

        public void Authorize(string returnurl = "")
        {
            //http://hjj.bellali.cn/WxXin/Index?returnurl=volunteer.html
            //1.调用https://open.weixin.qq.com/connect/oauth2/authorize接口获取到code
            try
            {
                Log.WriteLog("Authorize/returnurl:" + returnurl);
                string redirect_uri = "http://hjj.bellali.cn/WxXin/Index";
                StringBuilder url = new StringBuilder(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?redirect_uri={0}&appid={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect", redirect_uri, APPID, returnurl));
                //string r = HttpService.Get(url.ToString());
                Response.Redirect(url.ToString());
                //LogErrorRecord.InfoFormat("接口获取到code：{0}", r);
            }
            catch (Exception ex)
            {
                Log.WriteLog("Authorize错误日志：" + ex.Message);
            }
        }


    }
}
