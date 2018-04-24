using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;

namespace QINGUO.DAL
{
    /// <summary>
    /// 角色操作类
    /// </summary>
    public class SysRoleDAL : BaseDAL<ModSysRole>, ISysRole
    {
        /// <summary>
        /// 查询角色列表信息
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAllRole(string where)
        {
            string sql = "select * from Sys_Role where Status=" + (int)StatusEnum.正常;
            if (where != "")
            {
                sql += " and " + where;
            }
            return dabase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取选中的角色列表
        /// </summary>
        /// <param name="masterId">管理员编号</param>
        /// <returns></returns>
        public DataSet GetCheckRoleList(string masterId)
        {
            var parameters = new DataParameters();
            parameters.Add("@MasterId", masterId);
            string sql = "select * from Sys_MasterRole where MasterId=@MasterId";
            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据主键进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteRoleById(string id)
        {
            var parameters = new DataParameters();
            parameters.Add("@id", id);
            string sql = "delete from Sys_Role where Id=@id";
            int flag = 0;
            try
            {
                flag = dabase.ExecuteNonQueryByText(sql, parameters);
                dabase.CommitTransaction();

            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                flag = 0;
            }
            return flag;
        }

        /// <summary>
        /// 角色下创建的其它角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public DataSet SearchClidRole(string roleId)
        {
            var parameters = new DataParameters();
            parameters.Add("@roleId", roleId);
            string sql = "select * from Sys_Role where CreaterId in(select MasterId from [Sys_MasterRole] where RoleId=@roleId)";
            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
        }


        /// <summary>
        /// 删除总角色下子角色不存在的链接树
        /// </summary>
        /// <param name="PraentRoleId"></param>
        /// <param name="ChildroleId"></param>
        /// <returns></returns>
        public int DelteClidRole(string PraentRoleId,string ChildroleId)
        {
            string sql = "delete from Sys_RoleFun where Id in (SELECT Id FROM [Sys_RoleFun] where RoleId=@ChildroleId and FunId not in(";
            sql += "SELECT FunId FROM [Sys_RoleFun] where RoleId=@PraentRoleId))";

            var parameters = new DataParameters();
            parameters.Add("@PraentRoleId", PraentRoleId);
            parameters.Add("@ChildroleId", ChildroleId);

            dabase.ExecuteNonQueryByText(sql, parameters);
            return 1;
        }


        /// <summary>
        /// 删除角色下拥有的管理员的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int DeleteRoleInMaster(string roleId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("update Sys_Role set Status=" + (int)StatusEnum.删除 + " where Id='" + roleId + "';");
            sb.AppendLine();
            sb.Append("delete from Sys_MasterRole where RoleId in('" + roleId + "');");
            GetMasterRole(roleId, ref sb);

            try
            {
                dabase.ExecuteNonQuery(sb.ToString());
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
        /// 根据角色.获取角色下的管理员
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="sb"></param>
        public void GetMasterRole(string roleId,ref StringBuilder sb)
        {
            string sql = "select * from dbo.Sys_MasterRole where RoleId='" + roleId + "';";
            DataSet ds = dabase.ExecuteDataSet(sql);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sb.AppendLine();
                sb.Append("update Sys_Role set Status=" + (int)StatusEnum.删除 + " where Id=(select MasterId from Sys_MasterRole where Id='" + ds.Tables[0].Rows[i]["Id"] + "');");
                sb.AppendLine();
                sb.Append("delete from Sys_MasterRole where RoleId in('" + ds.Tables[0].Rows[i]["RoleId"] + "');");

                string sqlstr = "select Id from dbo.Sys_Role where Id=(select Id from Sys_MasterRole where Id='" + ds.Tables[0].Rows[i]["Id"] + "')";
                DataSet dss = dabase.ExecuteDataSet(sqlstr);
                for (int j = 0;j < ds.Tables[0].Rows.Count;j++)
                {
                    GetMasterRole(ds.Tables[0].Rows[j]["Id"].ToString(),ref sb);
                }
            }
        }
    }
}
