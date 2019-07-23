using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using QINGUO.Model;
using QINGUO.Common;

namespace WebApp
{
    /// <summary>
    ///爱普--短信验证码
    /// </summary>
    public class IPuCode
    {
        /// <summary>
        /// 获取短信验证码实体
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="TypeInt"></param>
        /// <returns></returns>
        public ModSysMessageAuthCode GetCode(string Phone, int TypeInt)
        {
            //验证码
            string vcode = "";
            Random r = new Random();
            //生成验证码
            for (int i = 0; i < 6; i++)
            {
                vcode += Convert.ToString(r.Next(0, 10));
            }
            ModSysMessageAuthCode model = new ModSysMessageAuthCode();
            model.Id = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.Code = vcode;
            model.Tel = Phone;
            model.MsgState = 0;//(0:发送有效 1:已验证 -1:过期)
            model.EndTime = DateTime.Now.AddMinutes(5);//5分钟有效
            model.CreaterId = "";
            model.TypeInt = TypeInt;
            return model;
        }

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="tel">联系电话</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public ModJsonResult SendCode(string tel, string message)
        {
            ModJsonResult result = new ModJsonResult();

            Encoding myEncoding = Encoding.GetEncoding("GBK");
            string cpid = ConfigurationManager.AppSettings["用户编号"];//文件保存路劲 "11696";//用户id：11696
            string pwd = ConfigurationManager.AppSettings["用户密码"];
            /*通道id：1462 名称：行业应用(禁发广告) 价格：10.00分
             通道id：4765 名称：广告通道 价格：10.00分*/
            string channelid = ConfigurationManager.AppSettings["通道编号"];

            /*Md5加密后的字符串：md5(password_timestamp_topsky)
            注： password为密码，timestamp是当前的UNIX时间戳(精确到秒)，用下划线和topsky连成一个字符串，用md5加密
            示例：假设当前时间戳为1421042878，密码为123456，则将字符串“123456_1421042878_topsky”进行md5加密得到密码8bc01d8a2d6f36961e1d3474ff27b684*/
            long time = DateTimeHelper.DateTimeToUnixTimestamp(DateTime.Now);
            string password = DESEncrypt.md5(pwd + "_" + time + "_topsky", 32);//32位加密
            string msg = HttpUtility.UrlEncode(message, myEncoding);//信息内容(每70个字为1条短信，系统自动拆分，汉字内容请使用gbk格式的urlencode编码形式)
            string tele = tel;//电话号码（多个号码用半角逗号分开，最多500个号码）

            StringBuilder sb = new StringBuilder();
            sb.Append("http://admin.sms9.net/houtai/sms.php?");
            sb.Append("cpid=" + cpid);
            sb.Append("&password=" + password);
            sb.Append("&channelid=" + channelid);
            sb.Append("&tele=" + tele);
            sb.Append("&msg=" + msg);
            sb.Append("&timestamp=" + time);
            string strResult = new Html().RequestGet(sb.ToString());
            if (strResult.IndexOf("error") >= 0)
            {
                int errorNum = int.Parse(strResult.Split(':')[1]);
                switch (errorNum)
                {
                    case -1:
                        strResult = "传递参数错误";
                        break;
                    case -2:
                        strResult = "用户id或密码错误";
                        break;
                    case -3:
                        strResult = "通道id错误";
                        break;
                    case -4:
                        strResult = "手机号码错误";
                        break;
                    case -5:
                        strResult = "短信内容错误";
                        break;
                    case -6:
                        strResult = "余额不足错误";
                        break;
                    case -7:
                        strResult = "绑定ip错误";
                        break;
                    case -8:
                        strResult = "未带签名";
                        break;
                    case -9:
                        strResult = "签名字数不对";
                        break;
                    case -10:
                        strResult = "通道暂停";
                        break;
                    case -11:
                        strResult = "该时间禁止发送";
                        break;
                    case -12:
                        strResult = "时间戳错误";
                        break;
                    case -13:
                        strResult = "编码异常";
                        break;
                    case -14:
                        strResult = "发送被限制";
                        break;
                }
                result.success = false;
                result.msg = strResult;
            }
            else
            {
                result.success = true;
            }
            return result;
        }


        

    }
}