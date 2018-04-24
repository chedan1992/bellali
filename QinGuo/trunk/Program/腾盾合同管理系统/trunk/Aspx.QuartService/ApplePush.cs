using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using QINGUO.Apple.Apns.Notifications;
using System.Data;
using System.Web;

namespace QINGUO.QuartService
{
   public class ApplePush
    {
        //证书文件名放在bin下
        private string p12File = ConfigurationManager.AppSettings["AppleCertification"];//"aps_developer_identity.p12";
        //证书密码
        private string p12FilePassword = ConfigurationManager.AppSettings["ApplePassword"];//"82009668";
        //是否沙盒测试
        private bool sandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["SandBox"]);

        public ApplePush()
        {

        }

        public ApplePush(string strFile)
        {
            p12File = strFile;
            sandbox = true;
        }


        /// <summary>
        /// 添加推送信息时推送给会员
        /// </summary>
        /// <param name="Info">推送信息</param>
        /// <param name="userdt">会员信息集</param>
        public void pushInfo(List<SMSModel> smsModeList)
        {
            if (smsModeList.Count > 0)
            {
                string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,p12File);
                NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);
                service.SendRetries = 5; //5 retries before generating notificationfailed event
                service.ReconnectDelay = 5000; //5 seconds
                service.Error += new NotificationService.OnError(service_Error);//出错事件
                service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);//超时
                service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);//机器码错误
                service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);//推送失败
                service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);//推送成功
                service.Connecting += new NotificationService.OnConnecting(service_Connecting);//正在连接
                service.Connected += new NotificationService.OnConnected(service_Connected);//已连接
                service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);//断开连接
                foreach (SMSModel mode in smsModeList)
                {
                    try
                    {
                        //4a4ca8981340c42872ae498641d540eea1a6ebdfed0caf6306cff0c01c943269
                        string testDeviceToken =mode.MobileCode;//获取机器码
                        Notification alertNotification = new Notification(testDeviceToken);
                        alertNotification.Payload.Alert.Body = mode.Info;//推送内容
                        alertNotification.Payload.AddCustom("h", mode.PTitle); //标题  
                        alertNotification.Payload.AddCustom("p", mode.PushUserId); //推送编号 
                        alertNotification.Payload.AddCustom("t", mode.Model); //类型
                        //alertNotification.Payload.AddCustom("c", mode.RelationID); //关联信息编号
                        alertNotification.Payload.AddCustom("u", mode.UserID); //会员编号
                        alertNotification.Payload.Sound = "default";//可不变
                        try
                        {
                            //MsgBIZ bll = new MsgBIZ();
                            //int c = bll.GetRecordCountUser("state=0 and userid='" + dr["userid"].ToString() + "'");
                           // alertNotification.Payload.Badge =5;
                        }
                        catch
                        {

                        }
                        service.QueueNotification(alertNotification);
                    }
                    catch
                    {
                    
                    }
                }
                service.Close();
                service.Dispose();
            }

        }

        #region MyRegion
        /// <summary>
        /// 会员登陆时接收推送信息
        /// </summary>
        /// <param name="Info">要推送的信息集</param>
        /// <param name="Token">会员手机编码</param>
        public void pushInfo(DataTable Info, string Token)
        {
            try
            {
                string testDeviceToken = Token;//机器码     
                string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);
                //string p12Filename = "";//HttpContext.Current.Server.MapPath(p12File);
                NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);
                service.SendRetries = 5; //5 retries before generating notificationfailed event
                service.ReconnectDelay = 5000; //5 seconds
                service.Error += new NotificationService.OnError(service_Error);//出错事件
                service.NotificationTooLong += new NotificationService.OnNotificationTooLong(service_NotificationTooLong);//超时
                service.BadDeviceToken += new NotificationService.OnBadDeviceToken(service_BadDeviceToken);//机器码错误
                service.NotificationFailed += new NotificationService.OnNotificationFailed(service_NotificationFailed);//推送失败
                service.NotificationSuccess += new NotificationService.OnNotificationSuccess(service_NotificationSuccess);//推送成功
                service.Connecting += new NotificationService.OnConnecting(service_Connecting);//正在连接
                service.Connected += new NotificationService.OnConnected(service_Connected);//已连接
                service.Disconnected += new NotificationService.OnDisconnected(service_Disconnected);//断开连接
                int i = 0;
                foreach (DataRow dr in Info.Rows)
                {
                    try
                    {
                        i++;
                        Notification alertNotification = new Notification(testDeviceToken);
                        alertNotification.Payload.Alert.Body = dr["PTitle"].ToString().Trim();//推送内容
                        alertNotification.Payload.AddCustom("h", dr["PTitle"].ToString()); //标题
                        alertNotification.Payload.AddCustom("p", dr["PropelID"].ToString()); //信息编号
                        alertNotification.Payload.AddCustom("t", dr["typeid"]); //类型
                        alertNotification.Payload.AddCustom("c", dr["CorrelativeID"]); //关联信息编号
                        alertNotification.Payload.AddCustom("u", dr["UserID"]); //关联信息编号
                        alertNotification.Payload.Sound = "default";//可不变
                        alertNotification.Payload.Badge = i;
                        service.QueueNotification(alertNotification);
                        break;
                        //System.Threading.Thread.Sleep(5000);
                    }
                    catch
                    {
                        i--;
                    }
                }
                service.Close();
                service.Dispose();
            }
            catch
            {
            }
        }
        #endregion

        #region 推送过程事件
        /// <summary>
        /// 机器码错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            Console.WriteLine("机器码错误");
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="sender"></param>
        static void service_Disconnected(object sender)
        {
            Console.WriteLine("链接已经断开");
        }

        /// <summary>
        /// 已连接
        /// </summary>
        /// <param name="sender"></param>
        static void service_Connected(object sender)
        {
            Console.WriteLine("已经链接");
        }

        /// <summary>
        /// 正在连接
        /// </summary>
        /// <param name="sender"></param>
        static void service_Connecting(object sender)
        {
            Console.WriteLine("链接中...");
        }

        /// <summary>
        /// 请求超时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        static void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            Console.WriteLine("请求超时");
        }

        /// <summary>
        /// 推送成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notification"></param>
        static void service_NotificationSuccess(object sender, Notification notification)
        {
            try
            {
                try
                {
                    Console.WriteLine("苹果信息推送成功!");
                    //修改发送短信的状态
                    //int cci = notification.Payload.ToString().Length;
                    //string propelid = (string)notification.Payload.CustomItems["p"].GetValue(0);
                    //string userid = notification.Payload.CustomItems["u"].GetValue(0).ToString();
                    //SMSDBVisit.UpdateModelState(propelid, userid);
                    //修改发送短信状态
                    //MsgBIZ bll = new MsgBIZ();
                    //bll.UpdatePuser(userid, propelid, 1);
                }
                catch
                {
                }

            }
            catch
            {

            }
        }


        /// <summary>
        /// 推送失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notification"></param>
        static void service_NotificationFailed(object sender, Notification notification)
        {
            Console.WriteLine("推送失败");
        }

        /// <summary>
        /// 出错事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        static void service_Error(object sender, Exception ex)
        {
            Console.WriteLine("I'm So Sorry ,推送失败,错误原因：" + ex.Message);
        }

        #endregion
    }
}
