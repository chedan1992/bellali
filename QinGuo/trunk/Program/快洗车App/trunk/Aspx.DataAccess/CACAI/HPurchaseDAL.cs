using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using System.Data;

namespace QINGUO.DAL
{
    public class HPurchaseDAL : BaseDAL<ModHPurchase>, IHPurchase
    {
        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAllList(string IdList)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select 
   view_HPurchase.OutNumber,view_HPurchase.StoreName,view_HPurchase.CusterName,view_HPurchase.GetNumber,view_HPurchase.BillDate,
  case OutStatus when 1 then '已入库' else '' end as InStatus,view_HPurchase.Remark as MainRemark,
  H_PurchaseDetail.Code,H_PurchaseDetail.GoodName,H_PurchaseDetail.GoodUnit,H_PurchaseDetail.[count],H_PurchaseDetail.Price,H_PurchaseDetail.Money,
  view_HPurchase.JudgerName,view_HPurchase.JudgeDate,H_PurchaseDetail.Remark,H_PurchaseDetail.StyleNum,H_PurchaseDetail.FreightNum
  from H_PurchaseDetail inner join view_HPurchase on H_PurchaseDetail.MainId=view_HPurchase.Id
  where view_HPurchase.Id in(" + IdList + ")  order by FinancialState asc");
            return dabase.ExecuteDataSet(strSql.ToString());
        }
    }
}
