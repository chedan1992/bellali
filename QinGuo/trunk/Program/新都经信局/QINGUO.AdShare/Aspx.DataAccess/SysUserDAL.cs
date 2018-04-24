using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class SysUserDAL : BaseDAL<ModSysUser>, ISysUser
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysUser GetModelByWhere(string where)
        {
            return dabase.ReadDataBase.SingleOrDefault<ModSysUser>("select * from Sys_User where 1=1 " + where);
        }

        /// <summary>
        /// 修改用户登录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLogin(ModSysUser model)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", model.Id);
            paras.Add("@LastLoginTime", model.LastLoginTime);
            paras.Add("@LoginCount", model.LoginCount);
            paras.Add("@MobileCode", model.MobileCode);
            paras.Add("@BDUserId", model.BDUserId);
            paras.Add("@BDChannelId", model.BDChannelId);
            paras.Add("@PaltForm", model.PaltForm);

            int result = dabase.ExecuteNonQueryByText("update Sys_User set BDChannelId=@BDChannelId,BDUserId=@BDUserId,LastLoginTime=@LastLoginTime,LoginCount=@LoginCount,PaltForm=@PaltForm,MobileCode=@MobileCode where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改用户头像信息
        /// </summary>
        /// <param name="HeadImg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateHeadImg(string HeadImg,string userId)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", userId);
            paras.Add("@HeadImg", HeadImg);
            int result = dabase.ExecuteNonQueryByText("update Sys_User set HeadImg=@HeadImg where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(string Userid,string Column)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", Userid);

            int result = dabase.ExecuteNonQueryByText("update Sys_User set " + Column + " where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", flag);
            parameters.Add("@Id", key);

            StringBuilder sb = new StringBuilder();
            sb.Append("update Sys_User set Status=@Status where Id=@Id;");
            try
            {
                dabase.ExecuteNonQueryByText(sb.ToString(), parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 查询用户详细信息
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysUser GetUserByTel(string Tel)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Sys_User  where Tel='" + Tel + "'");
            return dabase.ReadDataBase.SingleOrDefault<ModSysUser>(sb.ToString());
        }
        /// <summary>
        /// 查询用户详细信息
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysUser GetUserById(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Sys_User  where Id='" + id + "'");

            return dabase.ReadDataBase.SingleOrDefault<ModSysUser>(sb.ToString());
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="Id">用户主键</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        public int ReSetPwd(string Id, string Pwd)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", Id);
            parameters.Add("@Pwd", Pwd);

            string sql = "update Sys_User set Pwd=@Pwd where Id=@Id";
            try
            {
                dabase.ExecuteNonQueryByText(sql, parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 获得系统用户树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id as id,Nickname as text,OrganizaId");
            strSql.Append(" FROM Sys_User where Status=1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            return dabase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获取推送用户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public List<ModPushUserView> GetPushUserList(string userList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select Id,BDUserId,BDChannelId,PaltForm,MobileCode from Sys_Master where Id in (" + userList + ")");
            return dabase.ReadDataBase.Query<ModPushUserView>(sb.ToString()).ToList();
        }


        /// <summary>
        /// 根据通讯录查询不是Iwill用户
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetIwillUser(string list, string userId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select Id,Name,Nickname,Tel,HeadImg from Sys_User where Name in(" + list + ")");
            sb.Append(" and Name not in (");
            sb.Append(" select FriendsName from Sys_UserFriends where UserName=");
            sb.Append(" (select Name from Sys_User where Id='" + userId + "'))");
            sb.Append(" and Name!=(select Name from Sys_User where Id='" + userId + "')");
            return dabase.ExecuteDataSet(sb.ToString());
        }
    }
}
