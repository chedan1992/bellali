using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WxPayAPI.lib;
using Microsoft.AspNetCore.Http;
using Common;
using System.IO;

namespace Commonlication
{

    public class BaseController : Controller
    {
        public DB db = new DB();

        public static string APPID = "wx31183ba598767f20";
        public static string MCHID = "1233410002";
        public static string KEY = "e10adc3849ba56abbe56e056f20f883e";
        public static string APPSECRET = "d2dce4d23cfe52bc056a59a8330de60e";
        public static string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public static string SSLCERT_PASSWORD = "1233410002";
        public static string NOTIFY_URL = "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";
        public static string IP = "8.8.8.8";
        public static string PROXY_URL = "http://10.152.18.220:8080";
        public static int REPORT_LEVENL = 1;
        public static int LOG_LEVENL = 0;


        /**
        * 根据当前系统时间加随机序列来生成订单号
         * @return 订单号
        */
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}{2}", MCHID, DateTime.Now.ToString("yyyyMMddHHmmss"), ran.Next(999));
        }


        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            RandomGenerator randomGenerator = new RandomGenerator();
            return randomGenerator.GetRandomUInt().ToString();
        }


        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayAPI.WxPayData GetNotifyData()
        {

            string builder = Common.ReadStream2String(Request.Body);
            Log.WriteLog("weixin:" + builder);

            /*
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.Body;

            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();*/

            Log.WriteLog("GetNotifyData:" + builder.ToString());

            //转换数据格式并验证签名
            WxPayAPI.WxPayData data = new WxPayAPI.WxPayData();
            try
            {
                Log.WriteLog(builder.ToString());
                data.FromXml(builder.ToString());
            }
            catch (WxPayAPI.WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayAPI.WxPayData res = new WxPayAPI.WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Response.WriteAsync(res.ToXml());

                Log.WriteLog("GetNotifyData:" + ex.Message);
            }

            return data;
        }

    }
}