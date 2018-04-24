using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.Business
{
    public class BllOrderEvaluation : BllBase<ModOrderEvaluation>
    {
        private IOrderEvaluation DAL = CreateDalFactory.CreateOrderEvaluation();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
          /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModOrderEvaluationView GetModelByWhere(string where)
        {
            return DAL.GetModelByWhere(where);
        }
        /// <summary>
        /// 获取评价列表信息
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModCompanyOrder</returns>
        public List<ModOrderEvaluationView> SearchData(Search search, out int total)
        {
            //查询条件
            search.TableName = @"view_OrderEvaluation as a left join Sys_User as b on a.CreateId=b.Id";//表名
            search.KeyFiled = "a.OrderId";//主键
            search.SelectedColums = "a.*,b.Name";
            DataSet ds = QueryPageToDataSet(out total, search);
            List<ModOrderEvaluationView> model = CommonFunction.DataSetToList<ModOrderEvaluationView>(ds, 0);
            return model;
        }

        /// <summary>
        /// 查询评价订单信息
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public ModOrderEvaluation GetViewModel(string OrderId)
        {
            return DAL.GetViewModel(OrderId);
        }

        /// <summary>
        /// 统计我对他人评价信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetAverageCar(string userId, int type)
        {
            return DAL.GetAverageCar(userId, type);
        }
        /// <summary>
        /// 统计客户对我的评价信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetAverageUser(string userId, int type)
        {
            return DAL.GetAverageUser(userId, type);
        }

          /// <summary>
        /// 判断是否评价
       /// </summary>
       /// <param name="OrderId"></param>
       /// <param name="where"></param>
       /// <returns></returns>
        public int IsIsEvaluation(string OrderId, string where)
        {
            return DAL.IsIsEvaluation(OrderId, where);
        }
    }
}
