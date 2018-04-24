using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;

namespace QINGUO.DAL
{
    /// <summary>
    /// 管理员角色操作类
    /// </summary>
    public class SysMasterRoleDAL : BaseDAL<ModSysMasterRole>, ISysMasterRole
    {
        /// <summary>
        /// 根据管理员编号获取它的角色信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetMasterRole(string masterId)
        {
            var parameters = new DataParameters();
            parameters.Add("@MasterId", masterId);

            string sql = @"select smr.RoleId from Sys_MasterRole as smr
inner join Sys_Role  as sr on smr.RoleId=sr.Id
where MasterId=@MasterId and Status=1";


            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取角色根据公司编号
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public DataSet GetRoleByCompanyID(string companyID)
        {
            var parameters = new DataParameters();
            parameters.Add("@CompanyID", companyID);
            return dabase.ExecuteDataSetByProc("SP_GetRoleByCompanyID", parameters);
        }

        /// <summary>
        /// 获取用户的
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public DataSet GetMasterRoleByCompanyID(string companyID, string masterID)
        {
            var parameters = new DataParameters();
            parameters.Add("@CompanyID", companyID);
            parameters.Add("@MasterID", masterID);
            return dabase.ExecuteDataSetByProc("SP_GetUserRole", parameters);
        }


        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        public bool DeleteMasterRole(string MasterID)
        {
            var parameters = new DataParameters();
            parameters.Add("@MasterID", MasterID);
            return dabase.ExecuteNonQueryByStore("Del_MasterRole", parameters) > 0;
        }


        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        public bool AddMasterRole(string Id, string MasterID, string roleID)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", Id);
            parameters.Add("@MasterId", MasterID);
            parameters.Add("@RoleId", roleID);
            return dabase.ExecuteNonQueryByStore("AddMasterRole", parameters) > 0;
        }

        /// <summary>
        /// 获取角色所在的用户的个数
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool GetCountByRoleId(string roleID)
        {

            DataParameters pams = new DataParameters();
            pams.Add("@RoleID", roleID);
            DataRow row = dabase.ExecuteDataRow("select count(*) from Sys_MasterRole where RoleID=@RoleID", pams);
            return Convert.ToInt32(row[0]) > 0;
        }

        /// <summary>
        /// 根据角色编号删除管理员相关的角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool DeleteMasterRoleByRoleID(string roleID)
        {
            bool isExists = GetCountByRoleId(roleID);
            if (isExists)
            {
                DataParameters pams = new DataParameters();
                pams.Add("@roleID", roleID);
                return dabase.ExecuteNonQueryByText("Delete from Sys_MasterRole where RoleID=@roleID", pams) > 0;
            }

            return true;

        }

        /// <summary>
        /// 删除管理员角色信息
        /// </summary>
        /// <param name="MasterId">管理员编号</param>
        /// <returns></returns>
        public int DeleteAllByMasterId(string MasterId)
        {
            var parameters = new DataParameters();
            parameters.Add("@MasterId", MasterId);

            StringBuilder sb = new StringBuilder();
            sb.Append("delete from Sys_MasterRole where MasterId=@MasterId");


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
    }
}
