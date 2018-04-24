using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using QINGUO.DapperAccessBase;
using System.Configuration;

namespace QINGUO.DAL
{
    public class SysFunDAL : BaseDAL<ModSysFun>, ISysFun
    {
        /// <summary>
        /// 获得树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id as id,ParentId as parentId,FunName as text,TypeId,[FunSort],[PageUrl],[ClassName],[CreateTime],[iconCls],[IsSystem] ,[Status],isChild,isCheckRole");
            strSql.Append(" FROM Sys_Fun where 1=1");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            strSql.Append(" order by FunSort asc ");
            return dabase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int UpdateDate(ModSysFun t)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", t.Id);
            parameters.Add("@ParentId", t.ParentId);
            parameters.Add("@FunSort", t.FunSort);
            parameters.Add("@FunName", t.FunName);
            parameters.Add("@PageUrl", t.PageUrl);
            parameters.Add("@ClassName", t.ClassName);
            parameters.Add("@iconCls", t.iconCls);
            parameters.Add("@IsSystem", t.IsSystem);
            parameters.Add("@isChild", t.isChild);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sys_Fun set ParentId=@ParentId,FunSort=@FunSort,FunName=@FunName,PageUrl=@PageUrl,ClassName=@ClassName,iconCls=@iconCls,IsSystem=@IsSystem,isChild=@isChild");
            strSql.Append(" where Id=@Id");

            try
            {
                dabase.ExecuteNonQueryByText(strSql.ToString(), parameters);
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
        /// 启用/停用
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateIsStatus(int status, string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", status);
            parameters.Add("@Id", key);

            StringBuilder sb = new StringBuilder();
            if (status == (int)StatusEnum.正常)//启用
            {
                sb.Append("update Sys_Fun set Status=@Status where Id in(select Id from GetSysFunByChild(@Id)) or Id in(select Id from GetSysFunByParent(@Id))");
            }
            else//禁用
            {
                sb.Append("update Sys_Fun set Status=@Status where Id in(select Id from GetSysFunByChild(@Id))");
            }
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
        /// 根据主键软删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int DeleteDate(string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", key);
            parameters.Add("@Status", (int)StatusEnum.删除);

            string sql = "update Sys_Fun set Status=@Status where Id=@Id";
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
