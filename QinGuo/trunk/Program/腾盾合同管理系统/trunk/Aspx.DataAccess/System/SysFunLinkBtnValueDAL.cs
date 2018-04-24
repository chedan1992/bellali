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
    public class SysFunLinkBtnValueDAL : BaseDAL<ModSysFunLinkBtnValue>, ISysFunLinkBtnValue
    {

        /// <summary>
        /// 批量添加按钮信息
        /// </summary>
        /// <param name="BtnId">按钮列表</param>
        /// <param name="FunId">页面主键</param>
        /// <returns></returns>
        public void BatchInsert(string BtnId, string FunId)
        {
            
            StringBuilder strSql = new StringBuilder();

            var parameters = new DataParameters();
            parameters.Add("@FunId", FunId);

            strSql.Append("delete from Sys_FunLinkBtnValue where FunId=@FunId");
            strSql.AppendLine();

            if (!String.IsNullOrEmpty(BtnId))
            {
                string[] str = BtnId.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    parameters.Add("@BtnId" + i + "", str[i]);
                    strSql.Append(" insert into Sys_FunLinkBtnValue([Id],[FunId],[BtnId]) values( newId(),@FunId,@BtnId" + i + ");");
                }
            }
           
            try
            {
                dabase.ExecuteNonQueryByText(strSql.ToString(), parameters);
                dabase.CommitTransaction();
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                throw;
            }
        }
    }
}
