using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace Commonlication
{
    public class Common
    {
        public static string ReadStream2String(Stream stream)
        {
            if (null == stream)
            {
                return string.Empty;
            }
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Dictionary<string, string> Xml2Dict(string xml)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement root = doc.DocumentElement;
            foreach (XmlNode node in root.ChildNodes)
            {
                result.Add(node.Name, node.InnerText);
            }
            return result;
        }
        public static string GetTimeStamp()
        {

            #region linux系统时间转换
            /*
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return Convert.ToInt64(ts.TotalSeconds * 1000); 
             * */
            #endregion
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        }

        public static string Valid(string token,string signature, string timestamp, string nonce, string echostr)
        {
            if (CheckSignature(token,signature, timestamp, nonce))
            {
                if (!string.IsNullOrEmpty(echostr))
                {
                    return echostr;
                }
            }

            return "";
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        public static bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp); //字典排序
            string tmpStr = string.Join("", ArrTmp);

            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(tmpStr));
            tmpStr = BitConverter.ToString(hash).Replace("-", "").ToLower();

            Log.WriteLog(signature);
            Log.WriteLog(tmpStr);
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static HttpResponseMessage ToHttpMsgForWeChat(string strMsg)
        {
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(strMsg, Encoding.GetEncoding("UTF-8"), "application/x-www-form-urlencoded") };
            return result;
        }
    }
}