using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using System.Data;
using QINGUO.DataAccessBase;

namespace QINGUO.DAL
{
    public class SysGroupDAL : BaseDAL<ModSysGroup>, ISysGroup
    {
        /// <summary>
        /// 获得系统班级树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id as id,Name as text");
            strSql.Append(" FROM Sys_Group where 1=1");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            return dabase.ExecuteDataSet(strSql.ToString());
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
            sb.Append("update Sys_Group set Status=@Status where Id=@Id;");
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
        /// 获取类型列表
        /// </summary>
        /// <returns></returns>
        public List<ModSysGroup> GetGroupList(string cid)
        {
            return dabase.ReadDataBase.Query<ModSysGroup>("select * FROM Sys_Group where status=1 and CompanyId=@0", cid).ToList();
        }

        public List<ModSysGroup> GetList(string type)
        {
            return dabase.ReadDataBase.Query<ModSysGroup>("select isnull(code,0) as code,name,id FROM Sys_Group where status=1 and Type=@0 order by OrderNum asc", type).ToList();

        }
    }
}
