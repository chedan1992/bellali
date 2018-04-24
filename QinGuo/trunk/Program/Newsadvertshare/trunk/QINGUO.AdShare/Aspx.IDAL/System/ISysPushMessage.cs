using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysPushMessage : IBaseDAL<ModSysPushMessage>
    {
        /// <summary>
        /// 分页查询消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Dapper.Page<ModSysPushMessage> GetMyMessageSearch(Common.Search search);

        /// <summary>
        /// 查询用户消息未读数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        int MsgCount(string uid);

        /// <summary>
        /// 修改消息读状态
        /// </summary>
        /// <param name="msgid"></param>
        /// <returns></returns>
        bool UpdateMsgId(string msgid);

        int DeleteAllStatus(string userid);
    }
}
