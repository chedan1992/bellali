using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;

namespace QINGUO.IDAL
{
    public interface IOrderRunOrder : IBaseDAL<ModOrderRunOrder>
    {
          /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModviewOrderList GetModelByWhere(string where);
          /// <summary>
        ///获取司机接的最新订单信息
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
        List<ModviewOrderList> GetOrderList(Search search,string UserId, int? top);

          /// <summary>
        /// 查询当前用户是否还有未支付的订单，如果没有，就不能再次订单添加
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int IsNoPayOrder(string UserId);
          /// <summary>
        /// 订单取消
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int CancelOrder(string keyId);
    }
}
