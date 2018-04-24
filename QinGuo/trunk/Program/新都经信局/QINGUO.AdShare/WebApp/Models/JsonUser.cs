using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class JsonUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 用户性别
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 用户收藏数量
        /// </summary>
        public int numcollcet { get; set; }

        /// <summary>
        /// 用户是否是会员
        /// </summary>
        public bool ismember { get; set; }

        /// <summary>
        /// 商家
        /// </summary>
        public string cname { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int age { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public string cid { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }
    }
}