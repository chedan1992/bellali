using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QINGUO.Model;
using QINGUO.Business;
using System.Configuration;
using QINGUO.ViewModel;
using QINGUO.Common;

namespace WebPortalAdmin
{
    /// <summary>
    /// 推送操作类
    /// </summary>
    public class PushMsg
    {
        ////推送Key
        //string ApiKey = ConfigurationManager.AppSettings["ApiKey"];
        //string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        ////苹果推送Key
        //string IOSApiKey = ConfigurationManager.AppSettings["IOSApiKey"];
        //string IOSSecretKey = ConfigurationManager.AppSettings["IOSSecretKey"];

        //bool sandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["SandBox"]);

        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();


        /// <summary>
        ///根据用户编号推送用户
        /// </summary>
        /// <param name="UserIdlist"></param>
        /// <param name="NoticeTitle">标题</param>
        /// <param name="NoticeContent">内容</param>
        /// <param name="custom_content">自定义内容</param>
        /// <returns></returns>
        public bool SendUserMsg(string UserIdlist, string NoticeTitle, string NoticeContent, string custom_content)
        {
            bool flag = false;
            try
            {
                if (!string.IsNullOrEmpty(UserIdlist))
                {
                    //获取推送用户列表
                    string newId = "";
                    string[] str = UserIdlist.Split(',');
                    if (str.Length > 1)
                    {
                        for (int i = 0; i < str.Length - 1; i++)
                        {
                            newId += "'" + str[i] + "',";
                        }
                        newId = newId.Substring(0, newId.Length - 1);
                    }
                    else
                    {
                        newId = "'" + str[0] + "'";
                    }

                    List<ModPushUserView> list = new BllSysUser().GetPushUserList(newId);
                    int sum = 0;
                    if (list != null && list.Count > 0)
                    {
                        foreach (ModPushUserView item in list)
                        {
                            #region 百度推送

                            //if (!string.IsNullOrEmpty(item.BDUserId) && !string.IsNullOrEmpty(item.BDChannelId))
                            //{

                            //    if (item.PaltForm == 1)//苹果
                            //    {
                            //        string response = BaiduPushExecute.SendPush(IOSApiKey, IOSSecretKey, sandbox, item.PaltForm, item.BDUserId, item.BDChannelId, NoticeTitle, NoticeContent, custom_content);
                            //        BaiduRespons bdRespons = js.Deserialize<BaiduRespons>(response);
                            //        if (bdRespons.response_params.success_amount == 1)
                            //            sum += 1;
                            //    }
                            //    else //安卓
                            //    {
                            //        string response = BaiduPushExecute.SendPush(ApiKey, SecretKey, sandbox, item.PaltForm, item.BDUserId, item.BDChannelId, NoticeTitle, NoticeContent, custom_content);
                            //        BaiduRespons bdRespons = js.Deserialize<BaiduRespons>(response);
                            //        if (bdRespons.response_params.success_amount == 1)
                            //            sum += 1;
                            //    }
                            //}

                            #endregion

                            #region 个推

                            if (!string.IsNullOrEmpty(item.BDUserId))
                            {
                                PushResultView response = GeTuiPush.PushMessage(item.PaltForm, item.BDUserId, item.MobileCode, NoticeTitle, NoticeContent, custom_content);
                                if (response.result == "ok")
                                {
                                    sum += 1;
                                }
                            }
                            #endregion
                        }
                        if (sum == list.Count)
                            return true;
                        return true;
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }

}