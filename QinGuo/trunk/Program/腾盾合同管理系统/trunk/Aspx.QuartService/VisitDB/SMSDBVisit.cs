using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace QINGUO.QuartService
{
    public class SMSDBVisit
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserID">当前用户编号</param>
        /// <returns></returns>
        public static DataRow GetUserInfo(string UserID)
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select Tel,MobileCode from Sys_User where Id=@UserID", CommandType.Text, new SqlParameter("UserID", UserID));
            if (dt != null && dt.Rows.Count > 0)
            {

                return dt.Rows[0];
            }
            return null;
        }

        /// <summary>
        /// 修改短信状态已发送
        /// </summary>
        /// <param name="syspushUserID">推送消息用户编号主键</param>
        /// <param name="UserID">该用户编号</param>
        /// <returns></returns>
        public static bool UpdateModelState(string syspushUserID, string UserID)
        {
            bool isSuccess = SqlHelper.ExecuteNonQuery("update Sys_PushUser set Msgstate=1 where PropelID=@PushUserId and UserID=@UserID", CommandType.Text, new SqlParameter("@PushUserId", syspushUserID), new SqlParameter("@UserID", UserID)) > 0;
            return isSuccess;
        }
    }
}
