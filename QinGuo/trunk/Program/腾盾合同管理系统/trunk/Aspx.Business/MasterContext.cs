using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using QINGUO.Business;
using QINGUO.Model;

namespace QINGUO.Business
{
    /// <summary>
    /// 存储后台用户所有信息
    /// </summary>
    public class MasterContext
    {
        /// <summary>
        /// 登录后放在Session的用户对象
        /// </summary>
        public ModSysMaster Master
        {
            get
            {
                if (!IsLogined)
                    return null;
                else
                {
                    ModSysMaster master = null;
                    if (HttpContext.Current.Session[BllStaticStr.SessionMaster] != null)
                    {
                        master = (ModSysMaster)HttpContext.Current.Session[BllStaticStr.SessionMaster];
                    }
                    return master;
                }
            }
        }

        /// <summary>
        /// 登录用户Id
        /// </summary>
        public string MasterID
        {
            get
            {
                return Master.Id;
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogined
        {
            get
            {
                //return HttpContext.Current.User.Identity.IsAuthenticated;
                return HttpContext.Current.Session[BllStaticStr.SessionMaster] != null;
            }
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户登录名</param>
        /// <param name="password">密码</param>
        /// <param name="ip">登录ip</param>
        /// <returns></returns>
        public LoginEnum Login(string username, string password, string ip)
        {
            var reult = new BllSysMaster().Login(username, password, ip);
            if (reult != LoginEnum.登录成功)
            {
                HttpContext.Current.Session.Remove(BllStaticStr.SessionMaster);
            }

            return reult;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户登录名</param>
        /// <param name="password">密码</param>
        /// <param name="ip">登录ip</param>
        /// <returns></returns>
        public LoginEnum LoginCustomer(string username, string password, string ip)
        {
            var reult = new BllSysMaster().LoginCustomer(username, password, ip);
            if (reult != LoginEnum.登录成功)
            {
                HttpContext.Current.Session.Remove(BllStaticStr.SessionMaster);
            }
            return reult;
        }
    }
}
