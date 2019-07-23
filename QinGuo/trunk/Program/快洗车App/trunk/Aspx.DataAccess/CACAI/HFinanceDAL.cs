using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using System.Data;

namespace QINGUO.DAL
{
    public class HFinanceDAL : BaseDAL<ModHFinance>, IHFinance
    {
        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAllList(string IdList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select CusterName,SUM( ReturnAmount) as ReturnAmount,
                  SUM(ProfitTotal) as ProfitTotal,
                  SUM(PaymentAmmount)as PaymentAmmount,
                  CheckoutType,
                  PaymentType,
                  AccountName,
                  AccountNum
                  from view_HFinance
                  where Id in(" + IdList + ") group by CusterName,CheckoutType,PaymentType,AccountName,AccountNum");
            return dabase.ExecuteDataSet(strSql.ToString());
        }
    }
}
