using System;
using System.Collections.Generic;
using System.Web;

namespace Commonlication
{
    [Serializable]
    public class ResultToken
    {
        /*{ "access_token":"ACCESS_TOKEN",
"expires_in":7200,
"refresh_token":"REFRESH_TOKEN",
"openid":"OPENID",
"scope":"SCOPE" }*/

        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
        public int errcode { get; set; }

    }

    [Serializable]
    public class ResultTicket
    {
        /*
        {"errcode":0,"errmsg":"ok","ticket":"HoagFKDcsGMVCIY2vOjf9m1S4-LkdUY2c9c8xmH1pKVg_RjXeMP2uRBuuyoZVwJL-pa6WxdDFqkqLrvpAYnxLQ","expires_in":7200}*/

        public string errcode { get; set; }
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }
    
[Serializable]
    public class resultinfo
    {
        /*{    "openid":" OPENID",
" nickname": NICKNAME,
"sex":"1",
"province":"PROVINCE"
"city":"CITY",
"country":"COUNTRY",
"headimgurl":    "http://thirdwx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/46",
"privilege":[ "PRIVILEGE1" "PRIVILEGE2"     ],
"unionid": "o6_bmasdasdsad6_2sgVt7hMZOPfL"
}*/

        public string nickname { get; set; }
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        //public string privilege { get; set; }
        public string unionid { get; set; }


    }

    [Serializable]
    public class jscode2session
    {
        /*满足UnionID返回条件时，返回的JSON数据包
        {
            "openid": "OPENID",
            "session_key": "SESSIONKEY",
            "unionid": "UNIONID"
        }*/

        public string openid { get; set; }
        public string session_key { get; set; }
        public string unionid { get; set; }
        public int errcode { get; set; }

    }
}
