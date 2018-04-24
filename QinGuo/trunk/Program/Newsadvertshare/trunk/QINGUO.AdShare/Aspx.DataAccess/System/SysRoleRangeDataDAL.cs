using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;


namespace QINGUO.DAL
{
    public class SysRoleRangeDataDAL : BaseDAL<ModSysRoleRangeData>, ISysRoleRangeData
    {
        /// <summary>
        /// 根据角色类型获取所有数据集合
        /// </summary>
        /// <param name="RoleId">角色类型编号</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAllData(string RoleId,string where)
        {
            var parameters = new DataParameters();
            parameters.Add("@RoleId", RoleId);
            string sql = "select * from Sys_RoleRangeData where RoleId=@RoleId";
            if (where != "")
            {
                sql += " and " + where;
            }
            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        public int UpdateData(ModSysRoleRangeData model)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", model.Id);
            parameters.Add("@FunId", model.FunId);
            parameters.Add("@FunName", model.FunName);
            parameters.Add("@DeptId", model.DeptId);
            parameters.Add("@DeptName", model.DeptName);
            parameters.Add("@lookPower", model.lookPower);
            parameters.Add("@RoleId", model.RoleId);

            string sql = "update Sys_RoleRangeData set FunId=@FunId,FunName=@FunName,DeptId=@DeptId,DeptName=@DeptName,lookPower=@lookPower,RoleId=@RoleId where Id=@Id";

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
        /// 删除数据
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int DeleteData(string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", key);

            string sql = "delete from Sys_RoleRangeData where Id in (@Id)";
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
    }
}
