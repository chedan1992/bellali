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
    public class SysBtnDAL : BaseDAL<ModSysBtnValue>, ISysBtnValue
    {
        /// <summary>
        /// 获得按钮选择左边列表
        /// </summary>
        /// <param name="Id">页面导航id</param>
        /// <returns></returns>
        public DataSet GetBtnLeftSelect(string Id)
        {
            StringBuilder strSql = new StringBuilder();
            if (Id == "")
            {
                strSql.Append("select Id+'|'+Name as value,Name as text from Sys_BtnValue where Status=1");
            }
            else
            {
                strSql.Append("select Id+'|'+Name as value,Name as text from Sys_BtnValue where Status=1 and Id not in(select BtnId from Sys_FunLinkBtnValue where FunId='" + Id + "')");
            }
            strSql.AppendLine();

            return dabase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 获得按钮选择右边列表
        /// </summary>
        /// <param name="Id">页面导航id</param>
        /// <returns></returns>
        public DataSet GetBtnRightSelect(string Id)
        {
            var parameters = new DataParameters();
            parameters.Add("@FunId", Id);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sys_BtnValue.Id+'|'+Sys_BtnValue.Name as value,Sys_BtnValue.Name as text from Sys_FunLinkBtnValue inner join Sys_BtnValue");
            strSql.Append(" on Sys_FunLinkBtnValue.BtnId=Sys_BtnValue.Id and FunId=@FunId order by FunSort asc");

            return dabase.ExecuteDataSet(strSql.ToString(), CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据页面主键,获取页面的按钮
        /// </summary>
        /// <param name="key">页面主键</param>
        /// <returns></returns>
        public DataSet GetBtnByPage(string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@FunId", key);
            string sql = "select [Id]+'|'+FunId as Id,[Name],[IConName],[FunSort],[FunId] from v_FunLinkBtnValue_BtnValue where FunId=@FunId and Status=1 order by FunSort asc";
            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
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

            string sql = "update Sys_BtnValue set Status=@Status where Id=@Id";
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
