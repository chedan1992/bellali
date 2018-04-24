using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using System.Data;

namespace QINGUO.DAL
{
    public class SysDircDAL : BaseDAL<ModSysDirc>, ISysDirc
    {
        /// <summary>
        /// 获得系统班级树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id as id,Name as text,parentId='0'");
            strSql.Append(" FROM Sys_Dirc where 1=1");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            strSql.Append(" order by OrderNum asc");
            return dabase.ExecuteDataSet(strSql.ToString());
        }
        /// <summary>
        /// 获取类型列表
        /// </summary>
        /// <returns></returns>
        public List<ModSysGroup> GetGroupList()
        {
            return dabase.ReadDataBase.Query<ModSysGroup>("select * FROM Sys_Dirc where status=1").ToList();
        }
    }
}
