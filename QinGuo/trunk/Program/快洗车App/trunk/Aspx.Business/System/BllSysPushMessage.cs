using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;

namespace QINGUO.Business
{
    public class BllSysPushMessage : BllBase<ModSysPushMessage>
    {
        private ISysPushMessage dal = CreateDalFactory.CreateSysPushMessage();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        /// <summary>
        /// 分页查询消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Dapper.Page<ModSysPushMessage> GetMyMessageSearch(Common.Search search)
        {

            search.TableName = @"Sys_PushMessage";//视图
            search.KeyFiled = "Id";//主键
            search.StatusDefaultCondition = "";
            search.SortField = "Status desc,CreateTime desc";//排序
            return dal.GetMyMessageSearch(search);
        }
        /// <summary>
        /// 查询用户消息未读数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int MsgCount(string uid)
        {
            return dal.MsgCount(uid);
        }

        /// <summary>
        /// 根据主键id修改消息读状态
        /// </summary>
        /// <param name="msgid"></param>
        public bool UpdateMsgId(string msgid)
        {
            return dal.UpdateMsgId(msgid);
        }

        /// <summary>
        /// 删除用户所有消息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeleteAllStatus(string userid)
        {
            return dal.DeleteAllStatus(userid);
        }
    }
}
