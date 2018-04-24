using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllOrderUserExtended : BllBase<ModOrderUserExtended>
    {
        private IOrderUserExtended DAL = CreateDalFactory.CreateOrderUserExtended();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModOrderUserExtended GetModelByWhere(string where)
        {
            return DAL.GetModelByWhere(where);
        }
        /// <summary>
        /// 获取即将到账的金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal sumArrivalMoney(string UserId)
        {
            return DAL.sumArrivalMoney(UserId);
        }
        /// <summary>
        /// 用户充值
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">充值金额</param>
        /// <returns></returns>
        public int Recharge(string UserId, decimal Amount)
        {
            return DAL.Recharge(UserId, Amount);
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="CardId">司机编号</param>
        /// <param name="Amount">充值金额</param>
        /// <param name="OrderId">订单编号</param>
        /// <returns></returns>
        public int OrderRecharge(string UserId, string CardId, decimal Amount, string OrderId)
        {
            return DAL.OrderRecharge(UserId, CardId, Amount, OrderId);
        }
        /// <summary>
        /// 用户取现操作
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">取现金额</param>
        /// <param name="Card">银行卡号</param>
        /// <param name="UserName">用户名称</param>
        /// <returns></returns>
        public int TakingCash(string UserId, decimal Amount)
        {
            return DAL.TakingCash(UserId, Amount);
        }
        /// <summary>
        /// 更新用户评分
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int UpdateUserScore(decimal num, string UserId)
        {
            return DAL.UpdateUserScore(num, UserId);
        }
        /// <summary>
        /// 用户账户操作
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">炒作金额</param>
        /// <param name="Phone">支付宝账户</param>
        /// <param name="MoneyType">金额类型（1为增加金额，0为减少金额）</param>
        /// <param name="TypeName">操作类型（用户提现，巡检收入，退单扣除）</param>
        /// <param name="Remark">西部之谷D项 2单</param>
        /// <param name="status">状态(-1为删除，0为申请中,1:正在支付,2为已完成)</param>
        /// <returns></returns>
        public int OperationAccount(string Id,string UserId, decimal Amount, string Phone, int MoneyType, string TypeName, string Remark, int status)
        {
            return DAL.OperationAccount(Id,UserId, Amount, Phone, MoneyType, TypeName, Remark, status);
        }

          /// <summary>
        /// 添加工程师,同步金额账户
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">充值金额</param>
        /// <returns></returns>
        public int AddMasterAnsy(string UserId, decimal Amount)
        {
            return DAL.AddMasterAnsy(UserId, Amount);
        }
    }
}
