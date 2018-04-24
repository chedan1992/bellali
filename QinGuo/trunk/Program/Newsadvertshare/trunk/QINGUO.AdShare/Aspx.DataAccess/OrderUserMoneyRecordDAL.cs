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
    public class OrderUserMoneyRecordDAL : BaseDAL<ModOrderUserMoneyRecord>, IOrderUserMoneyRecord
    {
        /// <summary>
        /// 获取用户提现记录
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
        public List<ModOrderUserMoneyRecord> QueryAll(string UserId)
        {
            string sql = "select * from Order_UserMoneyRecord where UserId='" + UserId + "' order by CreateTime desc";
            return dabase.ReadDataBase.Query<ModOrderUserMoneyRecord>(sql).ToList();
        }

        /// <summary>
        /// 统计信息
        /// </summary>
        /// <returns></returns>
        public DataSet Total()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(" (select count(Id) from Sys_User where Status!=-1) as UserCount,");
            sb.Append(" (select COUNT(Id) from Order_UserMoneyRecord where Status=0 and MoneyType=0) as TakingCashCount,");
            sb.Append(" (select COUNT(Id) from Bus_Run where Status!=-1) as RunCount,");
            sb.Append(" (select COUNT(Id) from Bus_CarOrder where Status!=-1) as CarCount,");
            sb.Append(" (select isnull(sum(Amount),0) from view_OrderList where FlowStatus>-2) as SumAmount,");
            sb.Append(" (select isnull(sum(MoneyNum),0) from Order_UserMoneyRecord where Status=1 and MoneyType=0) as TakingSumAmount,");
            sb.Append(" (select count(OrderId) from view_OrderList where FlowStatus>-2) as OrderCount,");
            sb.Append(" (select count(OrderId) from view_OrderList where FlowStatus=-1) as OrderCancelCount");
            return dabase.ExecuteDataSet(sb.ToString());
        }


        /// <summary>
        /// 查询提现详细记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModOrderUserMoneyRecord> GetInId(string id)
        {
            id = id.Replace(",", "','");

            string sql = @"select a.UserName,a.Alipay,b.MoneyNum,a.Money from Sys_Master as a 
                           right join (select UserId,SUM(MoneyNum) as MoneyNum  from dbo.Order_UserMoneyRecord where Id in('" + id + "') and status=1 group by UserId) as b on a.Id=b.UserId where a.status=1";
            return dabase.ReadDataBase.Query<ModOrderUserMoneyRecord>(sql).ToList();
        }
        /// <summary>
        /// 修改批次号
        /// </summary>
        /// <param name="batch_no"></param>
        /// <returns></returns>
        public bool Updatebatch_no(string batch_no, string id)
        {
            var parameters = new DataParameters();
            parameters.Add("@Batch_no", batch_no);
            id = id.Replace(",", "','");

            string sql = "update Order_UserMoneyRecord set Batch_no=@Batch_no where id in ('" + id + "')";

            return dabase.ExecuteNonQueryByText(sql, parameters) > 0;
        }


        /// <summary>
        /// 根据批次号id查询
        /// </summary>
        /// <param name="batch_no"></param>
        /// <returns></returns>
        public List<ModOrderUserMoneyRecord> GetByBatch_no(string batch_no)
        {
            string sql = @"select UserId,SUM(MoneyNum) as MoneyNum from Order_UserMoneyRecord where batch_no=@0  group by UserId";
            return dabase.ReadDataBase.Query<ModOrderUserMoneyRecord>(sql, batch_no).ToList();
        }


        /// <summary>
        /// 根据批次号修改状态
        /// </summary>
        /// <param name="batch_no"></param>
        /// <returns></returns>
        public bool UpdateByBatch_no(string batch_no)
        {
            var parameters = new DataParameters();
            parameters.Add("@Batch_no", batch_no);
            string sql = "update Order_UserMoneyRecord set status=2 where Batch_no=@Batch_no";
            return dabase.ExecuteNonQueryByText(sql, parameters) > 0;
        }
    }
}
