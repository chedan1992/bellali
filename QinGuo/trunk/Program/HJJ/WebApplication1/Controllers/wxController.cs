using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Common;
using System.Net.Http;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace Commonlication
{
    public class wxController : BaseController
    {
        ///声明Token
        public readonly string Token = "hjjtoken";//与微信公众账号后台配置接口的Token设置保持一致，区分大小写。

        ///接口配置验证方法
        [HttpGet]
        public void CheckWeChat(string signature, string timestamp, string nonce, string echostr)
        {
            try
            {
                string EchoStr = Common.Valid(Token, signature, timestamp, nonce, echostr);
                if (!string.IsNullOrEmpty(EchoStr))
                {
                    Response.WriteAsync(echostr);
                }
                else
                {
                    Response.WriteAsync("验证失败！");
                }
            }
            catch (Exception ex)
            {
                Response.WriteAsync(ex.ToString());
            }
        }

        public void CheckWeChat()// 服务器响应微信请求
        {
            try
            {
                string weixin = Common.ReadStream2String(Request.Body);
                Log.WriteLog("weixin:" + weixin);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(weixin);//读取xml字符串
                XmlElement root = doc.DocumentElement;
                ExmlMsg xmlMsg = GetExmlMsg(root);
                //XmlNode MsgType = root.SelectSingleNode("MsgType");
                //string messageType = MsgType.InnerText;
                string messageType = xmlMsg.MsgType;//获取收到的消息类型。文本(text)，图片(image)，语音等。

                switch (messageType)
                {
                    //当消息为文本时
                    case "text":
                        textCase(xmlMsg);
                        break;
                    case "event":
                        break;
                    case "image":
                        break;
                    case "voice":
                        break;
                    case "vedio":
                        break;
                    case "location":
                        break;
                    case "link":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
            }
        }

        private ExmlMsg GetExmlMsg(XmlElement root)
        {
            ExmlMsg xmlMsg = new ExmlMsg()
            {
                FromUserName = root.SelectSingleNode("FromUserName").InnerText,
                ToUserName = root.SelectSingleNode("ToUserName").InnerText,
                CreateTime = root.SelectSingleNode("CreateTime").InnerText,
                MsgType = root.SelectSingleNode("MsgType").InnerText,
                EventKey = root.SelectSingleNode("EventKey").InnerText
            };
            if (xmlMsg.MsgType.Trim().ToLower() == "text")
            {
                xmlMsg.Content = root.SelectSingleNode("Content").InnerText;
            }
            else if (xmlMsg.MsgType.Trim().ToLower() == "event")
            {
                xmlMsg.EventName = root.SelectSingleNode("Event").InnerText;
            }
            return xmlMsg;
        }
        private void textCase(ExmlMsg xmlMsg)
        {
            long nowtime = DateTime.Now.Ticks;
            string msg = "";
            msg = getText(xmlMsg);
            string resxml = "<xml><ToUserName><![CDATA[" + xmlMsg.FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + xmlMsg.ToUserName + "]]></FromUserName><CreateTime>" + nowtime + "</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[" + msg + "]]></Content><FuncFlag>0</FuncFlag></xml>";
            Response.WriteAsync(resxml);

        }
        private string getText(ExmlMsg xmlMsg)
        {
            //string con = xmlMsg.Content.Trim();

            System.Text.StringBuilder retsb = new StringBuilder(200);
            retsb.Append("四川红领巾少儿艺术团");
            //retsb.Append("接收到的消息：" + xmlMsg.Content);
            //retsb.Append("用户的OPEANID：" + xmlMsg.FromUserName);

            Log.WriteLog("APPSECRET:" + WxPayConfig.APPSECRET);

            return retsb.ToString();
        }
        /// <summary>
        /// 回复单图文
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="Title">标题</param>
        /// <param name="Description">详情</param>
        /// <param name="PicUrl">图片地址</param>
        /// <param name="Url">地址</param>
        /// <returns>拼凑的XML</returns>
        public static string ReArticle(string FromUserName, string ToUserName, string Title, string Description, string PicUrl, string Url)
        {
            long nowtime = DateTime.Now.Ticks;
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + nowtime + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[news]]></MsgType><Content><![CDATA[]]></Content><ArticleCount>1</ArticleCount><Articles>";
            XML += "<item><Title><![CDATA[" + Title + "]]></Title><Description><![CDATA[" + Description + "]]></Description><PicUrl><![CDATA[" + PicUrl + "]]></PicUrl><Url><![CDATA[" + Url + "]]></Url></item>";
            XML += "</Articles><FuncFlag>0</FuncFlag></xml>";
            return XML;
        }
        /// <summary>
        /// 回复文本
        /// </summary>
        /// <param name="FromUserName">发送给谁(openid)</param>
        /// <param name="ToUserName">来自谁(公众账号ID)</param>
        /// <param name="Content">回复类型文本</param>
        /// <returns>拼凑的XML</returns>
        public static string ReText(string FromUserName, string ToUserName, string Content)
        {
            long nowtime = DateTime.Now.Ticks;
            string XML = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName><FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>";//发送给谁(openid)，来自谁(公众账号ID)
            XML += "<CreateTime>" + nowtime + "</CreateTime>";//回复时间戳
            XML += "<MsgType><![CDATA[text]]></MsgType>";//回复类型文本
            XML += "<Content><![CDATA[" + Content + "]]></Content><FuncFlag>0</FuncFlag></xml>";//回复内容 FuncFlag设置为1的时候，自动星标刚才接收到的消息，适合活动统计使用
            return XML;
        }


        public void userinfo(string openid, string access_token)
        {
            StringBuilder userurl = new StringBuilder(string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", access_token, openid));

            Log.WriteLog("userurl：" + userurl.ToString());
            string userjson = HttpService.Get(userurl.ToString());
            if (!string.IsNullOrEmpty(userjson))
            {
                var userinfo = JsonConvert.DeserializeObject<resultinfo>(userjson);

                User user = db.Users.FirstOrDefault(c => c.LoginName == openid);
                if (user == null)
                {
                    user = new User();
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
                else
                {
                    user.LoginName = openid;
                    user.NickName = userinfo.nickname;
                    user.Sex = Convert.ToInt32(userinfo.sex);
                    user.HeadImg = userinfo.headimgurl;
                    db.Users.Update(user);
                    if (db.SaveChanges() > 0)
                    {
                        Log.WriteLog("修改一条数据");
                    }
                }
            }
        }

        public void GetToken(string APPID, string APPSECRET, string openid)
        {
            try
            {
                StringBuilder userurl = new StringBuilder(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", APPID, APPSECRET));
                string userjson = HttpService.Get(userurl.ToString());
                //{"access_token":"ACCESS_TOKEN","expires_in":7200}
                if (!string.IsNullOrEmpty(userjson))
                {
                    var tokens = JsonConvert.DeserializeObject<ResultToken>(userjson);

                    if (tokens.expires_in == 7200)
                    {
                        CacheHelper.Set("tokens", tokens.access_token);
                        userinfo(openid, tokens.access_token);
                    }
                }
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
            }
        }

        public void subscribemsg(string redirect_uri)
        {
            //https://mp.weixin.qq.com/mp/subscribemsg?action=get_confirm&appid=wxaba38c7f163da69b&scene=1000&template_id=1uDxHNXwYQfBmXOfPJcjAS3FynHArD8aWMEFNRGSbCc&redirect_url=http%3a%2f%2fsupport.qq.com&reserved=test#wechat_redirec
            try
            {
                string template_id = "k5yh9O7MmIDBw-4u843wQHb0uP0mmElAwvNzxsIQejI";
                StringBuilder url = new StringBuilder(string.Format("https://mp.weixin.qq.com/mp/subscribemsg?action=get_confirm&appid={0}&scene=1000&template_id={1}&redirect_url={2}&reserved=test#wechat_redirect", WxPayConfig.APPID, template_id, redirect_uri));
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
