using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.IDAL;

namespace QINGUO.DAL
{
    public class SysCompanyPaySetDAL : BaseDAL<ModSysCompanyPaySet>, ISysCompanyPaySet
    {

        /// <summary>
        /// 获取付款方式类型
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetSelectAll(string where)
        {
            string sql = "select * from Sys_CompanyPayType";
            if (!String.IsNullOrEmpty(where))
            {
                sql += " where " + where;
            }
            return dabase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 根据公司id查询支付信息
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public ModSysCompanyPaySet getByCId(string Cid)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModSysCompanyPaySet>("select * from Sys_CompanyPaySet where CompanyID=@0 and PayType=3", Cid);
        }
    }
}
