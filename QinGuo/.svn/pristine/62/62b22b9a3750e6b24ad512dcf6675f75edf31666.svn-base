using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspx.Model;
using Aspx.Business;

namespace InterFaceWeb.Common
{
    public class CommonFun
    {
        /// <summary>
        /// 注册用户钱包
        /// </summary>
        /// <param name="UserId"></param>
        public void RegeditUserExtended(string UserId)
        {
            //注册用户钱包信息
            ModOrderUserExtended mode = new ModOrderUserExtended();
            mode.UserId = UserId;
            new BllOrderUserExtended().Insert(mode);
        }
    }
}