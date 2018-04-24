using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using System.Data;

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
        /// 获取类型列表
        /// </summary>
        /// <returns></returns>
        public List<ModSysGroup> GetGroupList(string cid)
        {
            return dabase.ReadDataBase.Query<ModSysGroup>("select * FROM Sys_Group where status=1 and CompanyId=@0", cid).ToList();
        }
    }
}
