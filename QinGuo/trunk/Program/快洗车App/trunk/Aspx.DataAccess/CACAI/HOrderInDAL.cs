using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using System.Data;

namespace QINGUO.DAL
{
    public class HOrderInDAL : BaseDAL<ModHOrderIn>, IHOrderIn
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
   view_HOrderIn.InNumber,view_HOrderIn.StoreName,view_HOrderIn.CusterName,view_HOrderIn.GetNumber,view_HOrderIn.BillDate,
  case InStatus when 1 then '已入库' else '' end as InStatus,view_HOrderIn.Remark as MainRemark,view_HOrderIn.LogisticsNumber,
  H_OrderInDetail.Code,H_OrderInDetail.GoodName,H_OrderInDetail.GoodUnit,H_OrderInDetail.[count],H_OrderInDetail.Price,H_OrderInDetail.Money,
  H_OrderInDetail.Batch,H_OrderInDetail.Remark,H_OrderInDetail.StyleNum,H_OrderInDetail.FreightNum,
  view_HOrderIn.MakerName,view_HOrderIn.JudgerName,view_HOrderIn.JudgeDate 
  from H_OrderInDetail inner join view_HOrderIn on H_OrderInDetail.MainId=view_HOrderIn.Id
  where view_HOrderIn.Id in(" + IdList + ")  order by FinancialState asc");
            return dabase.ExecuteDataSet(strSql.ToString());
        }
    }
}
