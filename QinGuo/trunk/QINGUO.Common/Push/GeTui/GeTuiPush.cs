using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui.template;
using com.igetui.api.openservice.igetui;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using com.igetui.api.openservice.payload;


namespace QINGUO.Common
{
    public class GeTuiPush
    {
        //参数设置 <-----参数需要重新设置----->
        //http的域名
        private static String HOST = "http://sdk.open.api.igexin.com/apiex.htm";
        //https的域名
        //private static String HOST = "https://api.getui.com/apiex.htm";

        //定义常量, appId、appKey、masterSecret 采用本文档 "第二步 获取访问凭证 "中获得的应用配置
        private static String APPID = ConfigurationManager.AppSettings["APPID"];
        private static String APPKEY = ConfigurationManager.AppSettings["APPKEY"];
        private static String MASTERSECRET = ConfigurationManager.AppSettings["MASTERSECRET"];
        private static String CLIENTID = "";//"b7e40f9e811d93adc50a10267d8d881e";
        private static String DeviceToken ="";// ConfigurationManager.AppSettings["DeviceToken"];  //填写IOS系统的DeviceToken

       /// <summary>
        /// 推送
       /// </summary>
       /// <param name="PaltForm">1:苹果 2:安卓</param>
       /// <param name="clientId"></param>
       /// <param name="MobileCode"></param>
       /// <param name="NoticeTitle"></param>
       /// <param name="NoticeContent"></param>
       /// <param name="custom_content"></param>
       /// <returns></returns>
        public static PushResultView PushMessage(int PaltForm, string clientId,string MobileCode, string NoticeTitle, string NoticeContent, string custom_content)
        {
            DeviceToken = MobileCode;//针对苹果手机
            string result = GeTuiPush.PushMessageToSingle(PaltForm, clientId, NoticeTitle, NoticeContent, custom_content);
            if (PaltForm ==1)
            {
                //APN简化推送
                //apnPush(PaltForm, clientId, NoticeTitle, NoticeContent, custom_content);
            }
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result)))
            {
                DataContractJsonSerializer dcj = new DataContractJsonSerializer(typeof(PushResultView));
                return (PushResultView)dcj.ReadObject(ms);
            }
        }

        static void apnPush(int PaltForm, string clientId, string NoticeTitle, string NoticeContent, string custom_content)
        {
            //APN高级推送
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            APNTemplate template = new APNTemplate();
            APNPayload apnpayload = new APNPayload();
            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();
            alertMsg.Body = NoticeContent;
            alertMsg.ActionLocKey = "";
            alertMsg.LocKey = "";
            alertMsg.addLocArg("");
            alertMsg.LaunchImage = "";
            //IOS8.2支持字段
            alertMsg.Title = NoticeTitle;
            alertMsg.TitleLocKey = "";
            alertMsg.addTitleLocArg("");

            apnpayload.AlertMsg = alertMsg;
            apnpayload.Badge = 10;
            apnpayload.ContentAvailable = 1;
            apnpayload.Category = "";
            apnpayload.Sound = "";
            apnpayload.addCustomMsg("", "");
            template.setAPNInfo(apnpayload);


            /*单个用户推送接口*/
            SingleMessage Singlemessage = new SingleMessage();
            Singlemessage.Data = template;
            String pushResult = push.pushAPNMessageToSingle(APPID, DeviceToken, Singlemessage);

            /*多个用户推送接口*/
            //ListMessage listmessage = new ListMessage();
            //listmessage.Data = template;
            //String contentId = push.getAPNContentId(APPID, listmessage);
            ////Console.Out.WriteLine(contentId);
            //List<String> devicetokenlist = new List<string>();
            //devicetokenlist.Add(DeviceToken);
            //String ret = push.pushAPNMessageToList(APPID, contentId, devicetokenlist);
            //Console.Out.WriteLine(ret);
        }


        /// <summary>
        /// PushMessageToSingle接口测试代码
        /// </summary>
        /// <param name="PaltForm">平台类型  1:苹果 2:安卓</param>
        /// <param name="clientId"></param>
        /// <param name="NoticeTitle">通知标题</param>
        /// <param name="NoticeContent">通知内容</param>
        /// <param name="custom_content">自定义内容</param>
        /// <returns></returns>
        public static string PushMessageToSingle(int PaltForm, string clientId, string NoticeTitle, string NoticeContent, string custom_content)
        {
            CLIENTID = clientId;
            // 推送主类
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            /*消息模版：
                1.TransmissionTemplate:透传模板
                2.LinkTemplate:通知链接模板
                3.NotificationTemplate：通知透传模板
                4.NotyPopLoadTemplate：通知弹框下载模板
             */
            ITemplate template = null;
            if (PaltForm == 1)//苹果安卓
            {
                //数据经SDK传给您的客户端，由您写代码决定如何处理展现给用户
                template = TransmissionTemplateDemo(PaltForm, NoticeTitle, NoticeContent, custom_content);
            }
            else//在通知栏显示一条含图标、标题等的通知，用户点击后激活您的应用
            {
                template = NotificationTemplateDemo(PaltForm, NoticeTitle, NoticeContent, custom_content);
            }
            
            SingleMessage message = new SingleMessage();
            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;

            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
            target.appId = APPID;
            target.clientId = CLIENTID;

            String pushResult = push.pushMessageToSingle(message, target);
            return pushResult;
        }

        //PushMessageToList接口测试代码
        public static void PushMessageToList(int PaltForm, string clientId, string NoticeTitle, string NoticeContent, string custom_content)
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            ListMessage message = new ListMessage();
            /*消息模版：
                 1.TransmissionTemplate:透传功能模板
                 2.LinkTemplate:通知打开链接功能模板
                 3.NotificationTemplate：通知透传功能模板
                 4.NotyPopLoadTemplate：通知弹框下载功能模板
             */

            //TransmissionTemplate template =  TransmissionTemplateDemo();
            //NotificationTemplate template =  NotificationTemplateDemo();
            //LinkTemplate template = LinkTemplateDemo();
            NotyPopLoadTemplate template = NotyPopLoadTemplateDemo();

            message.IsOffline = false;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;

            //设置接收者
            List<com.igetui.api.openservice.igetui.Target> targetList = new List<com.igetui.api.openservice.igetui.Target>();
            com.igetui.api.openservice.igetui.Target target1 = new com.igetui.api.openservice.igetui.Target();
            target1.appId = APPID;
            target1.clientId = CLIENTID;

            // 如需要，可以设置多个接收者
            //com.igetui.api.openservice.igetui.Target target2 = new com.igetui.api.openservice.igetui.Target();
            //target2.AppId = APPID;
            //target2.ClientId = "ddf730f6cabfa02ebabf06e0c7fc8da0";

            targetList.Add(target1);
            //targetList.Add(target2);

            String contentId = push.getContentId(message);
            String pushResult = push.pushMessageToList(contentId, targetList);
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服务端返回结果:" + pushResult);
        }

        //pushMessageToApp接口测试代码
        public static void pushMessageToApp(int PaltForm, string clientId, string NoticeTitle, string NoticeContent, string custom_content)
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            AppMessage message = new AppMessage();
            /*消息模版：
                1.TransmissionTemplate:透传模板
                2.LinkTemplate:通知链接模板
                3.NotificationTemplate：通知透传模板
                4.NotyPopLoadTemplate：通知弹框下载模板
             */

            //TransmissionTemplate template =  TransmissionTemplateDemo();
            NotificationTemplate template = NotificationTemplateDemo(PaltForm, NoticeTitle, NoticeContent, custom_content);
            //LinkTemplate template = LinkTemplateDemo();
            //NotyPopLoadTemplate template = NotyPopLoadTemplateDemo();

            message.IsOffline = false;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;

            List<String> appIdList = new List<string>();
            appIdList.Add(APPID);

            List<String> phoneTypeList = new List<string>();    //通知接收者的手机操作系统类型
            //phoneTypeList.Add("ANDROID");
            //phoneTypeList.Add("IOS");

            List<String> provinceList = new List<string>();     //通知接收者所在省份
            //provinceList.Add("浙江");
            //provinceList.Add("上海");
            //provinceList.Add("北京");

            List<String> tagList = new List<string>();
            //tagList.Add("开心");

            message.AppIdList = appIdList;
            message.PhoneTypeList = phoneTypeList;
            message.ProvinceList = provinceList;
            message.TagList = tagList;


            String pushResult = push.pushMessageToApp(message);
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服务端返回结果：" + pushResult);
        }


        /*
         * 
         * 所有推送接口均支持四个消息模板，依次为透传模板，通知透传模板，通知链接模板，通知弹框下载模板
         * 注：IOS离线推送需通过APN进行转发，需填写pushInfo字段，目前仅不支持通知弹框下载功能
         *
         */
        //透传模板动作内容--模版1
        public static TransmissionTemplate TransmissionTemplateDemo(int PaltForm, string title, string content, string custom_content)
        {
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            template.TransmissionType = "1";            //应用启动类型，1：强制应用启动 2：等待应用启动
            template.TransmissionContent = custom_content;  //透传内容
            //iOS推送需要的pushInfo字段
            if (PaltForm ==1)
            {
                //iOS推送需要的pushInfo字段
                template.setPushInfo("actionLocKey", 1, content, "sound", custom_content, content, "locArgs", "launchImage");
            }
            //template.setPushInfo(actionLocKey, badge, message, sound, payload, locKey, locArgs, launchImage);
            //template.setPushInfo("", 4, "", "", "", "", "", "");
            return template;
        }

        /// <summary>
        /// 通知透传模板动作内容--模版2
        /// </summary>
        /// <param name="PaltForm">1:苹果 2:安卓</param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static NotificationTemplate NotificationTemplateDemo(int PaltForm, string title, string content, string custom_content)
        {
            NotificationTemplate template = new NotificationTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            template.Title = title;         //通知栏标题
            template.Text = content;          //通知栏内容
            template.Logo = "";               //通知栏显示本地图片
            template.LogoURL = "";                    //通知栏显示网络图标

            template.TransmissionType = "1";          //应用启动类型，1：强制应用启动  2：等待应用启动
            template.TransmissionContent = custom_content;   //透传内容

            if (PaltForm ==1)
            {
                //iOS推送需要的pushInfo字段
                template.setPushInfo("actionLocKey", 1, content, "sound", custom_content, content, "locArgs", "launchImage");
                //template.setPushInfo(actionLocKey, badge, message, sound, payload, locKey, locArgs, launchImage);
            }
            template.IsRing = true;                //接收到消息是否响铃，true：响铃 false：不响铃
            template.IsVibrate = true;               //接收到消息是否震动，true：震动 false：不震动
            template.IsClearable = true;             //接收到消息是否可清除，true：可清除 false：不可清除
            return template;
        }

        //通知链接动作内容
        /// <summary>
        /// 通知链接动作内容-IOS不推荐使用该模板
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static LinkTemplate LinkTemplateDemo(string title,string content)
        {
            LinkTemplate template = new LinkTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            template.Title = title;         //通知栏标题
            template.Text = content;          //通知栏内容
            template.Logo = "";               //通知栏显示本地图片
            template.LogoURL = "";  //通知栏显示网络图标，如无法读取，则显示本地默认图标，可为空
            template.Url = "";      //打开的链接地址

            //iOS推送需要的pushInfo字段
            //template.setPushInfo(actionLocKey, badge, content, sound, payload, content, locArgs, launchImage);

            template.IsRing = true;                 //接收到消息是否响铃，true：响铃 false：不响铃
            template.IsVibrate = true;               //接收到消息是否震动，true：震动 false：不震动
            template.IsClearable = true;             //接收到消息是否可清除，true：可清除 false：不可清除

            return template;
        }

        //通知弹框下载模板动作内容
        /// <summary>
        /// NotificationTemplate（IOS不推荐使用该模板）
        /// </summary>
        /// <returns></returns>
        public static NotyPopLoadTemplate NotyPopLoadTemplateDemo()
        {
            NotyPopLoadTemplate template = new NotyPopLoadTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            template.NotyTitle = "请填写通知标题";     //通知栏标题
            template.NotyContent = "请填写通知内容";   //通知栏内容
            template.NotyIcon = "icon.png";           //通知栏显示本地图片
            template.LogoURL = "http://www-igexin.qiniudn.com/wp-content/uploads/2013/08/logo_getui1.png";                    //通知栏显示网络图标

            template.PopTitle = "弹框标题";             //弹框显示标题
            template.PopContent = "弹框内容";           //弹框显示内容
            template.PopImage = "";                     //弹框显示图片
            template.PopButton1 = "下载";               //弹框左边按钮显示文本
            template.PopButton2 = "取消";               //弹框右边按钮显示文本

            template.LoadTitle = "下载标题";            //通知栏显示下载标题
            template.LoadIcon = "file://push.png";      //通知栏显示下载图标,可为空
            template.LoadUrl = "http://www.appchina.com/market/d/425201/cop.baidu_0/com.gexin.im.apk";//下载地址，不可为空

            template.IsActived = true;                  //应用安装完成后，是否自动启动
            template.IsAutoInstall = true;              //下载应用完成后，是否弹出安装界面，true：弹出安装界面，false：手动点击弹出安装界面

            template.IsBelled = true;                 //接收到消息是否响铃，true：响铃 false：不响铃
            template.IsVibrationed = true;               //接收到消息是否震动，true：震动 false：不震动
            template.IsCleared = true;             //接收到消息是否可清除，true：可清除 false：不可清除
            return template;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void getUserStatus()
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            String ret = push.getClientIdStatus(APPID, CLIENTID);
            Console.WriteLine("用户状态:" + ret);
        }
    }
}