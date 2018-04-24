using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;
using System.Data;

namespace QINGUO.DAL
{
    public class OrderRunOrderDAL : BaseDAL<ModOrderRunOrder>, IOrderRunOrder
    {

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModviewOrderList GetModelByWhere(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from view_OrderList");
            sb.Append("  where 1=1 " + where);
            return dabase.ReadDataBase.SingleOrDefault<ModviewOrderList>(sb.ToString());
        }

        /// <summary>
        ///获取司机接的最新订单信息
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
        public List<ModviewOrderList> GetOrderList(Search search,string UserId, int? top)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select");
            if (top > 0)
            {
                sb.Append(" top "+top+" *");
            }
            else
            {
                sb.Append(" *");
            }
            sb.Append(" from view_OrderList where OrderUserId='" + UserId + "' and " + search .GetConditon()+ " order by CreateTime desc");
            return dabase.ReadDataBase.Query<ModviewOrderList>(sb.ToString()).ToList();
        }


        /// <summary>
        /// 查询当前用户是否还有未支付的订单，如果没有，就不能再次订单添加
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int IsNoPayOrder(string UserId)
        {
            int money = 0;
            string sql = "select COUNT(*) from view_OrderList where CreaterId='" + UserId + "' and PushType=1 and FlowStatus!=1";
            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                money = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
            }
            return money;
        }


        /// <summary>
        /// 订单取消
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int CancelOrder(string keyId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update Order_RunOrder set [Status]=-1 where RunId='" + keyId + "'");
            sb.AppendLine();
            sb.Append("update Bus_CarOrder set [Status]=-1 where Id='" + keyId + "'");
            sb.AppendLine();
            sb.Append("update Bus_Run set [Status]=-1 where Id='" + keyId + "'");
            int result = dabase.ExecuteNonQuery(sb.ToString());
            return result;
        }
    }
}
