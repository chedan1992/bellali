using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Commonlication
{
    public class WechatUtil
    {
        public static readonly string appID = "wxb4ac7965dfa29be7";// 此处填写微信公众账号appID
        public static readonly string appsecret = "4aac6d1ecfbc9ab892dbb14f79ec2245";//此处填写微信公众账号appsecret

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            string res = "";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appID + "&secret=" + appsecret);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                
                Dictionary<string, string> data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                res = data["access_token"];
            }
            return res;
        }

        /// <summary>
        /// 处理微信发过来的消息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string ParseMessage(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return "err";
            }
            var dict =Common.Xml2Dict(xml);
            string result = string.Empty;
            switch (dict["MsgType"].ToLower())
            {
                //文本类型
                case "text":
                    TextMessageHandler(dict, ref result);
                    break;

                //事件类型
                case "event":
                    string openid = dict["FromUserName"];
                    switch (dict["Event"].ToLower())
                    {
                        //用户关注事件
                        case "subscribe":
                            SubscribeEvent(dict, ref result);
                            break;
                        ////取消关注事件
                        //case "unsubscribe":
                        //    UnsubscribeEvent(openid, ref result);
                        //    break;
                        ////上报地理位置信息
                        //case "location":
                        //    LocationEvent(dict, ref result);
                        //    break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

       
        /// <summary>
        /// 自动回复文本消息
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="result"></param>
        private static void TextMessageHandler(Dictionary<string, string> dict, ref string result)
        {
            string returnMeg = "您好！感谢发送消息。公众号还在测试中。";

            ReplayTextMessage message = new ReplayTextMessage()
            {
                ToUserName = dict["FromUserName"],
                FromUserName = dict["ToUserName"],
                CreateTime = Common.GetTimeStamp(),
                Content = returnMeg
            };
            result = message.ToXml();
            Log.WriteLog("用户：" + dict["FromUserName"] + " 回复消息:" + dict["Content"] + " 时间：" + message.CreateTime);
        }

        /// <summary>
        /// 当用户关注公众号时，进行的操作
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        private static void SubscribeEvent(Dictionary<string, string> dict, ref string result)
        {
            try
            {
                string openid = dict["FromUserName"];
                //关注时发送的消息
                ReplayTextMessage message = new ReplayTextMessage()
                {
                    ToUserName = dict["FromUserName"],
                    FromUserName = dict["ToUserName"],
                    CreateTime = Common.GetTimeStamp(),
                    Content = "您好，欢迎关注微信公众号-测试号！"
                };
                result = message.ToXml();
                Log.WriteLog("用户：" + dict["FromUserName"] + " 关注了公众号 。时间：" + message.CreateTime);
                /*
                //存储用户信息
                
                 * */
            }
            catch (Exception ex)
            {
                //WeLogger.Error("用户关注事件，操数据库失败，错误消息：" + ex.Message);
            }
        }

       
    }
}