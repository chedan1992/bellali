using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InterFaceWeb
{
    /// <summary>
    /// 环信接口
    /// </summary>
    public class HXComm
    {
        string reqUrlFormat = "https://a1.easemob.com/{0}/{1}/";
        public string clientID = "YXA6m3ZDkCVxEea7r0dujlP4_A";
        public string clientSecret = "YXA6m-ah-hD1kZ4oUEg4KxhV1ZaZSyY";
        public string appName = "corporate";//应用
        public string orgName = "goldensoft";//企业
        public string AppKey = "goldensoft#corporate";
        public string token { get; set; }
        public string easeMobUrl { get { return string.Format(reqUrlFormat, orgName, appName); } }

        //构造函数
        public HXComm()
        {
            this.token = QueryToken();
        }


        //使用app的client_id 和 client_secret登陆并获取授权token
        string QueryToken()
        {
            if (string.IsNullOrEmpty(clientID) || string.IsNullOrEmpty(clientSecret)) { return string.Empty; }
            string cacheKey = clientID + clientSecret;
            if (System.Web.HttpRuntime.Cache.Get(cacheKey) != null &&
                System.Web.HttpRuntime.Cache.Get(cacheKey).ToString().Length > 0)
            {
                return System.Web.HttpRuntime.Cache.Get(cacheKey).ToString();
            }

            string postUrl = easeMobUrl + "token";
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"grant_type\": \"client_credentials\",\"client_id\": \"{0}\",\"client_secret\": \"{1}\"", clientID, clientSecret);
            _build.Append("}");

            string postResultStr = ReqUrl(postUrl, "POST", _build.ToString(), string.Empty);
            string token = string.Empty;
            int expireSeconds = 0;
            try
            {
                JObject jo = JObject.Parse(postResultStr);
                token = jo.GetValue("access_token").ToString();
                int.TryParse(jo.GetValue("expires_in").ToString(), out expireSeconds);

                //设置缓存
                if (!string.IsNullOrEmpty(token) && token.Length > 0 && expireSeconds > 0)
                {
                    System.Web.HttpRuntime.Cache.Insert(cacheKey, token, null, DateTime.Now.AddSeconds(expireSeconds), System.TimeSpan.Zero);
                }
            }
            catch { return postResultStr; }
            return token;
        }

        //创建用户(账号,密码)  返回:创建成功的用户JSON
        public string AccountCreate(string userName, string password)
        {
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"username\": \"{0}\",\"password\": \"{1}\"", userName, password);
            _build.Append("}");

            return AccountCreate(_build.ToString());
        }

        //创建用户(可以批量创建)
        ///<param name="postData">创建账号JSON数组--可以一个，也可以多个</param>
        ///<returns>创建成功的用户JSON</returns>
        public string AccountCreate(string postData) { return ReqUrl(easeMobUrl + "users", "POST", postData, token); }

        //获取指定用户详情
        /// <param name="userName">账号</param>
        /// <returns>会员JSON</returns>
        public string AccountGet(string userName) { return ReqUrl(easeMobUrl + "users/" + userName, "GET", string.Empty, token); }

        //重置用户密码
        /// <param name="userName">账号</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>重置结果JSON(如：{ "action" : "set user password",  "timestamp" : 1404802674401,  "duration" : 90})</returns>
        public string AccountResetPwd(string userName, string newPassword)
        {
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"newpassword\": \"{0}\"", newPassword);
            _build.Append("}");
            return ReqUrl(easeMobUrl + "users/" + userName + "/password", "PUT", _build.ToString().ToLower(), token);
          //  return ReqUrl(easeMobUrl + "users/" + userName + "/password", "PUT", "{\"newpassword\" : \"" + newPassword + "\"}", token);
        }

        //删除用户
        /// <param name="userName">账号</param>
        /// <returns>成功返回会员JSON详细信息，失败直接返回：系统错误信息</returns>
        public string AccountDel(string userName) { return ReqUrl(easeMobUrl + "users/" + userName, "DELETE", string.Empty, token); }

        //获取用户状态
        public string AccountStatus(string userName)
        {
            string status = "offline";
            string json = ReqUrl(easeMobUrl + "users/" + userName + "/status", "GET", string.Empty, token);
            try
            {
                if (json.IndexOf("online") != -1)
                {
                    status = "online";
                }
            }
            catch { }
            return status;
        }

        //获取聊天记录
        public string QueryMsgList()
        {
            //查询最新20条所有聊天记录
            //ReqUrl(easeMobUrl + "chatmessages?ql=order+by+timestamp+desc&limit=20", "GET", string.Empty, token);
            string json = ReqUrl(easeMobUrl + "chatmessages?ql=order+by+timestamp+desc&limit=20", "GET", string.Empty, token);
            try
            {

            }
            catch { }
            return json;
        }


        //查看某个IM用户的好友信息
        public string AccountContacts(string userName) { return ReqUrl(easeMobUrl + "users/" + userName + "/contacts/users", "GET", string.Empty, token); }

        /// <summary>
        /// 创建一个群组 chatgroups
        /// </summary>
        /// <param name="groupname">群组名称, 此属性为必须的</param>
        /// <param name="desc">群组描述, 此属性为必须的</param>
        /// <param name="owner">群组的管理员</param>
        /// <returns></returns>
        public string CreateChatgroups(string groupname, string desc, string owner)
        {
            /*  "groupname":"testrestgrp12", //群组名称, 此属性为必须的
                "desc":"server create group", //群组描述, 此属性为必须的
                "public":true, //是否是公开群, 此属性为必须的
                "maxusers":300, //群组成员最大数(包括群主), 值为数值类型,默认值200,此属性为可选的
                "approval":true, //加入公开群是否需要批准, 没有这个属性的话默认是true（不需要群主批准，直接加入）, 此属性为可选的
                "owner":"jma1", //群组的管理员, 此属性为必须的
                "members":["jma2","jma3"] //群组成员,此属性为可选的,但是如果加了此项,数组元素至少一个（注：群主jma1
             */
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"groupname\": \"{0}\",\"desc\": \"{1}\",\"public\":{2},\"maxusers\":{3},\"owner\": \"{4}\"", groupname, desc, true, 300, owner);
            _build.Append("}");

            return ReqUrl(easeMobUrl + "chatgroups", "POST", _build.ToString().ToLower(), token);
        }

        /// <summary>
        /// 群里加人
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GroupsAddUser(string groupid, string username)
        {
            return ReqUrl(easeMobUrl + "chatgroups/" + groupid + "/users/" + username, "POST", string.Empty, token);
        }

        /// <summary>
        /// 获取群信息
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public string GetGroupInfo(string groupid)
        {
            return ReqUrl(easeMobUrl + "chatgroups/" + groupid, "GET", string.Empty, token);
        }

        /// <summary>
        /// 群里减人
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GroupsDeleteUser(string groupid, string username)
        {
            return ReqUrl(easeMobUrl + "chatgroups/" + groupid + "/users/" + username, "DELETE", string.Empty, token);
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="UserName">用户名称</param>
        /// <param name="FriendsName">好友名称</param>
        /// <returns></returns>
        public string AddFrieds(string UserName, string FriendsName)
        {
            return ReqUrl(easeMobUrl + "users/" + UserName + "/contacts/users/" + FriendsName, "POST", string.Empty, token);
        }

        /// <summary>
        /// 解除好友
        /// </summary>
        /// <param name="UserName">用户名称</param>
        /// <param name="FriendsName">好友名称</param>
        /// <returns></returns>
        public string DeleteFrieds(string UserName, string FriendsName)
        {
            return ReqUrl(easeMobUrl + "users/" + UserName + "/contacts/users/" + FriendsName, "DELETE", string.Empty, token);
        }

        /// <summary>
        /// 添加黑名单
        /// </summary>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public string AddBlacklist(string ownerName, string blockedusername)
        {
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"usernames\": [\"{0}\"]", blockedusername);
            _build.Append("}");
            return ReqUrl(easeMobUrl + "users/" + ownerName + "/blocks/users", "POST", _build.ToString().ToLower(), token);
        }


        /// <summary>
        /// 取消黑名单
        /// </summary>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public string DeleteBlacklist(string ownerName, string blockedusername)
        {
            return ReqUrl(easeMobUrl + "users/" + ownerName + "/blocks/users/" + blockedusername, "DELETE", string.Empty, token);
        }
        /// <summary>
        /// 查询用户下所有好友信息
        /// </summary>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public string GetContactsUsers(string ownerName)
        {
            return ReqUrl(easeMobUrl + "users/" + ownerName + "/contacts/users", "GET", string.Empty, token);
        }

        /// <summary>
        /// 查询用户下所有黑名单信息
        /// </summary>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public string GetblocksUsers(string ownerName)
        {
            return ReqUrl(easeMobUrl + "users/" + ownerName + "/blocks/users", "GET", string.Empty, token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqUrl"></param>
        /// <param name="method"></param>
        /// <param name="paramData"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public string ReqUrl(string reqUrl, string method, string paramData, string token)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(reqUrl) as HttpWebRequest;
                request.Method = method.ToUpperInvariant();

                if (!string.IsNullOrEmpty(token) && token.Length > 1) { request.Headers.Add("Authorization", "Bearer " + token); }
                if (request.Method.ToString() != "GET" && !string.IsNullOrEmpty(paramData) && paramData.Length > 0)
                {
                    request.ContentType = "application/json";
                    byte[] buffer = Encoding.UTF8.GetBytes(paramData);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string result = stream.ReadToEnd();
                        return result;
                    }
                }
            }
            catch (Exception ex) { return ex.ToString(); }
        }
    }
}