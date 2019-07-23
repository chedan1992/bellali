using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commonlication
{
    public class BaseMessage
    {
        // 开发者微信号  
        public string ToUserName { get; set; }
        // 发送方帐号（一个OpenID）  
        public string FromUserName { get; set; }
        // 消息创建时间 （整型）  
        public string CreateTime { get; set; }
        // 消息类型（text/image/location/link）  
        public string MsgType { get; set; }

    }
}