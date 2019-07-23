using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using System.Data;
using QINGUO.DataAccessBase;
using QINGUO.ViewModel;

namespace QINGUO.DAL
{
    public class OrderEvaluationDAL : BaseDAL<ModOrderEvaluation>, IOrderEvaluation
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModOrderEvaluationView GetModelByWhere(string where)
        {
            return dabase.ReadDataBase.SingleOrDefault<ModOrderEvaluationView>("select * from view_OrderEvaluation where 1=1 " + where);
        }

        /// <summary>
        /// 查询评价订单信息
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public ModOrderEvaluation GetViewModel(string OrderId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Order_Evaluation");
            sb.Append(" where OrderId='" + OrderId + "'");
            return dabase.ReadDataBase.SingleOrDefault<ModOrderEvaluation>(sb.ToString());
        }
        /// <summary>
        /// 统计我对他人评价信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetAverageCar(string userId, int type)
        {
            var parameters = new DataParameters();
            parameters.Add("@OrderUserId", userId);
            parameters.Add("@type", type);
            DataSet ds = dabase.ExecuteDataSetByProc("Evaluation_AverageCar", parameters);
            return ds;
        }
        /// <summary>
        /// 统计客户对我的评价信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetAverageUser(string userId, int type)
        {
            var parameters = new DataParameters();
            parameters.Add("@OrderUserId", userId);
            parameters.Add("@type", type);
            DataSet ds = dabase.ExecuteDataSetByProc("Evaluation_AverageUser", parameters);
            return ds;
        }
       /// <summary>
        /// 判断是否评价
       /// </summary>
       /// <param name="OrderId"></param>
       /// <param name="where"></param>
       /// <returns></returns>
        public int IsIsEvaluation(string OrderId,string where)
        {
            int result = 0;
            string sql = "select * from view_OrderEvaluation where OrderId='" + OrderId + "'";
            if (!string.IsNullOrEmpty(where))
            {
                sql += " and "+ where;
            }

            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                result = 1;
            }
            return result;
        }
    }
}
