using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Aspx.ViewModel;
using Aspx.Business;

namespace Aspx.QuartService.Scheduler
{
    public class PushMsg
    {
        //推送Key
        string ApiKey = ConfigurationManager.AppSettings["ApiKey"];
        string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
        bool sandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["SandBox"]);
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();


        /// <summary>
        ///根据用户编号推送用户
        /// </summary>
        /// <param name="UserIdlist"></param>
        /// <param name="NoticeTitle">标题</param>
        /// <param name="NoticeContent">内容</param>
        /// <param name="custom_content">自定义内容</param>
        /// <returns></returns>
        public bool SendUserMsg(string UserIdlist, string NoticeTitle, string NoticeContent, string custom_content)
        {
            bool flag = false;
            try
            {
                if (!string.IsNullOrEmpty(UserIdlist))
                {
                    //获取推送用户列表
                    string newId = "";
                    string[] str = UserIdlist.Split(',');
                    if (str.Length > 1)
                    {
                        for (int i = 0; i < str.Length - 1; i++)
                        {
                            newId += "'" + str[i] + "',";
                        }
                        newId = newId.Substring(0, newId.Length - 1);
                    }
                    else
                    {
                        newId = "'" + str[0] + "'";
                    }

                    List<ModPushUserView> list = new BllSysUser().GetPushUserList(newId);
                    int sum = 0;
                    if (list != null && list.Count > 0)
                    {
                        foreach (ModPushUserView item in list)
                        {
                            if (!string.IsNullOrEmpty(item.BDUserId) && !string.IsNullOrEmpty(item.BDChannelId))
                            {
                                string response = BaiduPushExecute.SendPush(ApiKey, SecretKey, sandbox, item.PaltForm, item.BDUserId, item.BDChannelId, NoticeTitle, NoticeContent, custom_content);
                                BaiduRespons bdRespons = js.Deserialize<BaiduRespons>(response);
                                if (bdRespons.response_params.success_amount == 1)
                                    sum += 1;
                            }
                        }
                        if (sum == list.Count)
                            return true;
                        return true;
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
    public class BaiduRespons
    {
        public string request_id { get; set; }
        public ResponseParams response_params { get; set; }
    }
    public class ResponseParams
    {
        public int success_amount { get; set; }
        public List<string> msgids { get; set; }
    }
    public class BaiduPushExecute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ApiKey"></param>
        /// <param name="SecretKey"></param>
        /// <param name="sandbox"></param>
        /// <param name="platform">1:苹果 2:安卓</param>
        /// <param name="BDUserID"></param>
        /// <param name="BDChannelID"></param>
        /// <returns></returns>
        public static string SendPush(string ApiKey, string SecretKey, bool sandbox, int platform, string BDUserID, string BDChannelID, string NoticeTitle, string NoticeContent, string custom_content)
        {
            //当前APIKEY
            string sk = SecretKey;
            //当前SecreKey
            string ak = ApiKey;

            //百度推送
            BaiduPush Bpush = new BaiduPush("POST", sk);
            String apiKey = ak;
            String messages = "";
            String method = "push_msg";//默认消息推送

            //当前时间戳为 PushOptions服务
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            //默认设备为安卓
            /*
             云推送支持多种设备，各种设备的类型编号如下： 
3：Andriod设备； 
4：iOS设备； 
 如果存在此字段，则向指定的设备类型推送消息。 默认为android设备类型。 
             */
            uint device_type = platform == 1 ? (uint)4 : (uint)3;
            //获取总秒数
            uint unixTime = (uint)ts.TotalSeconds;
            //当前消息类型
            uint message_type;
            //消息签名一一对应
            string messageksy = "xxx000";
            //通知
            message_type = 1;

            //如果选择的是通知并且是苹果,那么设备类型改变为4
            if (platform == 1) //1:苹果 2:安卓
            {
                IOSNotification notification = new IOSNotification();
                notification.title = NoticeContent;
                notification.description = NoticeTitle;
                messages = notification.getJsonString();
            }
            else
            {
                BaiduPushNotification notification = new BaiduPushNotification();
                notification.title = NoticeTitle;
                notification.description = NoticeContent;
                notification.custom_content = custom_content;
                messages = notification.getJsonString();

            }
            PushOptions pOpts = new PushOptions(method, apiKey, BDUserID, BDChannelID, device_type, messages, messageksy, unixTime);

            //消息或则通知
            pOpts.message_type = message_type;

            //如果是苹果发布版本
            if (platform == 1 && sandbox)
            {
                //则当前是开发状态
                pOpts.deploy_status = 1;
            }
            else if (platform == 1 && !sandbox)//如果是苹果开发版本
            {
                //则当前是生产状态
                pOpts.deploy_status = 2;
            }
            string response = Bpush.PushMessage(pOpts);
            return response;
        }
    }
}
