using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface IOrderUserExtended : IBaseDAL<ModOrderUserExtended>
    {
          /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModOrderUserExtended GetModelByWhere(string where);
         /// <summary>
        /// 获取即将到账的金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        decimal sumArrivalMoney(string UserId);
          /// <summary>
        /// 用户充值
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">充值金额</param>
        /// <returns></returns>
        int Recharge(string UserId,decimal Amount);

         /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="CardId">司机编号</param>
        /// <param name="Amount">充值金额</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        int OrderRecharge(string UserId, string CardId, decimal Amount, string OrderId);
       /// <summary>
        /// 用户取现操作
       /// </summary>
        /// <param name="UserId">用户编号</param>
       /// <param name="Amount">取现金额</param>
       /// <param name="Card">银行卡号</param>
       /// <param name="UserName">用户名称</param>
       /// <returns></returns>
       int TakingCash(string UserId, decimal Amount);

        /// <summary>
       /// 用户账户操作
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Amount"></param>
        /// <param name="Phone"></param>
        /// <param name="MoneyType"></param>
        /// <param name="TypeName"></param>
        /// <param name="Remark"></param>
        /// <param name="status"></param>
        /// <returns></returns>
       int OperationAccount(string Id,string UserId, decimal Amount, string Phone, int MoneyType, string TypeName, string Remark, int status);
        
         /// <summary>
        /// 更新用户评分
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
       int UpdateUserScore(decimal num, string UserId);

          /// <summary>
        /// 添加工程师,同步金额账户
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">充值金额</param>
        /// <returns></returns>
       int AddMasterAnsy(string UserId, decimal Amount);
    }
}
