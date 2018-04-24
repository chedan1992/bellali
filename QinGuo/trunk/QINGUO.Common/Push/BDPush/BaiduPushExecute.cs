using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace QINGUO.Common
{
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
            uint device_type = platform ==1 ? (uint)4 : (uint)3;
            //获取总秒数
            uint unixTime = (uint)ts.TotalSeconds;
            //当前消息类型
            uint message_type;
            //消息签名一一对应
            string messageksy = "xxx000";
            //通知
            message_type =1;//0：消息；1：通知。默认为0

            //如果选择的是通知并且是苹果,那么设备类型改变为4
            if (platform ==1) //1:苹果 2:安卓
            {
                IOSNotification notification = new IOSNotification();
                notification.title = NoticeContent;
                notification.description =NoticeTitle ;
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
            if (platform ==1&& sandbox)
            {
                //则当前是开发状态
                pOpts.deploy_status = 1;
            }
            else if (platform ==1 && !sandbox)//如果是苹果开发版本
            {
                //则当前是生产状态
                pOpts.deploy_status = 2;
            }
            string response = Bpush.PushMessage(pOpts);
            return response;
        }
    }
}
