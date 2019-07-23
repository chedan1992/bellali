using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Commonlication
{
    public class ReplayTextMessage:BaseMessage
    {
        //回复文本内容
        public string Content { get; set; }

        //<xml>
        //<ToUserName><![CDATA[toUser]]></ToUserName>
        //<FromUserName><![CDATA[fromUser]]></FromUserName>
        //<CreateTime>12345678</CreateTime>
        //<MsgType><![CDATA[text]]></MsgType>
        //<Content><![CDATA[你好]]></Content>
        //</xml>
        public string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<ToUserName><![CDATA[" + this.ToUserName + "]]></ToUserName>");
            sb.Append("<FromUserName><![CDATA[" + this.FromUserName + "]]></FromUserName>");
            sb.Append("<CreateTime>" + this.CreateTime + "</CreateTime>");
            sb.Append("<MsgType><![CDATA[text]]></MsgType>");
            sb.Append("<Content><![CDATA[" + this.Content + "]]></Content>");
            sb.Append("</xml>");
            return sb.ToString();
        }
    }
}