using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;
using QINGUO.ViewModel;

namespace QINGUO.IDAL
{
    public interface IOrderEvaluation : IBaseDAL<ModOrderEvaluation>
    {
          /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModOrderEvaluationView GetModelByWhere(string where);
            /// <summary>
        /// 查询评价订单信息
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        ModOrderEvaluation GetViewModel(string OrderId);

         /// <summary>
        /// 统计我对他人评价信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataSet GetAverageCar(string userId, int type);
        /// <summary>
        /// 统计客户对我的评价信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataSet GetAverageUser(string userId, int type);

          /// <summary>
        /// 判断是否评价
       /// </summary>
       /// <param name="OrderId"></param>
       /// <param name="where"></param>
       /// <returns></returns>
        int IsIsEvaluation(string OrderId, string where);
    }
}
