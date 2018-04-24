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
    /// 权限设置--功能权限操作类
    /// </summary>
    public class SysRoleFunDAL : BaseDAL<ModSysRoleFun>, ISysRoleFun
    {
        /// <summary>
        /// 返回插入角色信息数据库
        /// </summary>
        /// <returns></returns>
        public string InsertRole(ModSysRoleFun model)
        {
            return "insert into Sys_RoleFun values('" + model.Id + "','" + model.RoleId + "','" + model.FunId + "','" +
                   model.BtnId + "','" + model.UniteId + "','" + model.CreaterId + "','" + model.CreaterName + "','" + model.CId + "')";
        }

        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSysRoleFun(ModSysRoleFun model)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", model.Id);
            paras.Add("@RoleID", model.RoleId);
            paras.Add("@FunID", model.FunId);
            paras.Add("@CreateID", model.CreaterId);
            paras.Add("@CreateName", model.CreaterName);
            paras.Add("@CID", model.CId);
            int result = dabase.ExecuteNonQueryByStore("Add_RoleFun", paras);
            return result == 1;
        }

        /// <summary>
        /// 删除角色所有的权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public bool DeleteRoleFun(string roleID, string cid)
        {
            bool isHave = GetFunByRoleID(roleID, cid);
            if (isHave)
            {
                var parameters = new DataParameters();
                parameters.Add("@RoleID", roleID);
                parameters.Add("@CID", cid);
                string sql = "delete from Sys_RoleFun where RoleId=@RoleID and CId=@CID";
                return dabase.ExecuteNonQueryByText(sql, parameters) > 0;
            }
            return true;
        }

        /// <summary>
        /// 获取角色权限根据角色编号和权限编号
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public bool GetFunByRoleID(string roleID, string Cid)
        {
            DataParameters pams = new DataParameters();
            pams.Add("@roleID", roleID);
            pams.Add("@cid", Cid);
            string sql = "select count(*) from Sys_RoleFun where RoleId=@roleID and CId=@cid";
            DataRow row = dabase.ExecuteDataRow(sql, pams);
            return Convert.ToInt32(row[0]) > 0;

        }

        /// <summary>
        /// 保存用户角色菜单信息
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="sql">插入sql</param>
        /// <returns></returns>
        public int InsertRoleByRoleId(string roleId, string sql)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("delete from Sys_RoleFun where RoleId=@RoleId;");
            sb.AppendLine();
            sb.Append(sql);

            var parameters = new DataParameters();
            parameters.Add("@RoleId", roleId);

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
        /// 查询已选中的菜单节点
        /// </summary>
        /// <param name="funId">菜单主键</param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetSelectFun(string funId, string roleId)
        {
            var parameters = new DataParameters();
            parameters.Add("@FunId", funId);
            parameters.Add("@roleId", roleId);

            string sql = "select * from Sys_RoleFun where FunId=@FunId and RoleId=@roleId";

            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters).Tables[0];
        }

        /// <summary>
        /// 获取权限根据角色Id和父权限为0
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public DataSet GetFunByRoleIDParentIDZero(string roleID, string companyID, bool isMultilRoleString = false)
        {

            if (isMultilRoleString)
            {
                var parameters = new DataParameters();
                parameters.Add("@Cid", companyID);

                string sql = @"select sf.Id,sf.FunName,sf.PageUrl,sf.iconCls,sf.FunSort 
from Sys_Fun as sf inner join Sys_RoleFun as srf on sf.Id=srf.FunId
where RoleId in (" + roleID + ") and sf.ParentID='0' and CId=@Cid group by sf.Id,sf.FunName,sf.PageUrl,sf.iconCls,sf.FunSort order by sf.FunSort asc";
                return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);

            }
            else
            {
                var parameters = new DataParameters();
                parameters.Add("@roleID", roleID);
                parameters.Add("@cid", companyID);
                string strSql = @"select sf.Id,sf.FunName,sf.PageUrl,sf.iconCls,sf.FunSort 
from Sys_Fun as sf inner join Sys_RoleFun as srf on sf.Id=srf.FunId
where RoleId=@roleID and sf.ParentID='0' and CId=@cid order by sf.FunSort asc";

                return dabase.ExecuteDataSet(strSql, CommandType.Text, parameters);
            }

        }


        /// <summary>
        /// 获取子权限根据角色Id和权限父Id
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="parentFunID"></param>
        /// <returns></returns>
        public DataSet GetChildFunByRoleID(string roleID, string parentFunID, string companyID, bool isMultilRoleString = false)
        {
            //是否是多个角色查询
            if (isMultilRoleString)
            {
                var parameters = new DataParameters();
                parameters.Add("@Cid", companyID);
                parameters.Add("@parentID", parentFunID);

                string sql = @"select sf.Id,sf.FunName,sf.PageUrl,sf.iconCls,sf.FunSort
from Sys_Fun as sf inner join Sys_RoleFun as srf on sf.Id=srf.FunId
where RoleId in (" + roleID + ") and sf.ParentID=@parentID and CId=@Cid  group by sf.Id,sf.FunName,sf.PageUrl,sf.iconCls,sf.FunSort order by sf.FunSort asc";
                return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
            }
            else
            {
                var parameters = new DataParameters();
                parameters.Add("@roleID", roleID);
                parameters.Add("@funID", parentFunID);
                parameters.Add("@cid", companyID);
                string strSql = @"select sf.Id,sf.FunName,sf.PageUrl,sf.iconCls,sf.FunSort
from Sys_Fun as sf inner join Sys_RoleFun as srf on sf.Id=srf.FunId
where RoleId=@roleID and sf.ParentID=@funID and CId=@cid order by sf.FunSort asc";

                return dabase.ExecuteDataSet(strSql, CommandType.Text, parameters);
            }

        }

        /// <summary>
        /// 删除角色权限,根据权限编号和公司编号
        /// </summary>
        /// <returns></returns>
        public bool DeleteRoleFunByFunIDCID(string funID, string Cid)
        {
            DataParameters pms = new DataParameters();
            pms.Add("@FunID", funID);
            pms.Add("@Cid", Cid);
            return dabase.ExecuteNonQueryByStore("DeleteRoleFun", pms) > 0;
        }

        /// <summary>
        /// 获取角色权限根据权限Id和公司编号
        /// </summary>
        /// <returns></returns>
        public int GetRoleByFunIDCID(string funID, string Cid)
        {
            DataParameters pms = new DataParameters();
            pms.Add("@funid", funID);
            pms.Add("@Cid", Cid);
            DataRow row = dabase.ExecuteDataRow("select COUNT(*) from Sys_RoleFun where FunId=@funid and CId=@Cid", pms);
            return Convert.ToInt32(row[0]);
        }

        /// <summary>
        /// 根据公司id查询所有funID
        /// </summary>
        /// <param name="cid"></param>
        public DataSet GetFunIdByCID(string cid)
        {
            string sql = "select FunId from Sys_RoleFun where CId=@CId  group by FunId";

            DataParameters pms = new DataParameters();
            pms.Add("@CId", cid);
            DataSet ds = dabase.ExecuteDataSet(sql, CommandType.Text, pms);
            return ds;
        }

        /// <summary>
        /// 根据局角色和公司cid查询功能权限（公司接口端使用）
        /// </summary>
        /// <returns></returns>
        public List<ModSysRoleFun> GetRoleCidByList(string rold, string Cid)
        {
            return dabase.ReadDataBase.Query<ModSysRoleFun>("").ToList();
        }

    }
}
