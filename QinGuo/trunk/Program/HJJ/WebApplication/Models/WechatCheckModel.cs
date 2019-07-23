using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Commonlication
{
    public class WechatCheckModel
    {
        //微信接口配置验证所需要的四个参数
        public string signature { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }
        public string echostr { get; set; }
    }
}