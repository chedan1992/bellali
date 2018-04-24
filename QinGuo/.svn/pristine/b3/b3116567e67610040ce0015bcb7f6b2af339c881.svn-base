using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;

namespace QINGUO.DAL
{
    public class SysPushMessageDAL : BaseDAL<ModSysPushMessage>, ISysPushMessage
    {
        public Dapper.Page<ModSysPushMessage> GetMyMessageSearch(Common.Search search)
        {
            return dabase.ReadDataBase.Page<ModSysPushMessage>(search.CurrentPageIndex, search.PageSize, search.SqlString);
        }


        /// <summary>
        /// 查询用户消息未读数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int MsgCount(string uid)
        {
            return dabase.ReadDataBase.ExecuteScalar<int>("select isnull(sum(1),0) from Sys_PushMessage where Status=1 and userid=@0", uid);
        }



        /// <summary>
        /// 根据主键id修改消息读状态
        /// </summary>
        /// <param name="msgid"></param>
        public bool UpdateMsgId(string msgid)
        {
            return dabase.WriteDataBase.Execute("update Sys_PushMessage set Status=2 where id=@0", msgid) > 0;
        }


        /// <summary>
        /// 删除用户所有消息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeleteAllStatus(string userid)
        {
            return dabase.WriteDataBase.Execute("delete from Sys_PushMessage where userid=@0", userid);
        }

    }
}
