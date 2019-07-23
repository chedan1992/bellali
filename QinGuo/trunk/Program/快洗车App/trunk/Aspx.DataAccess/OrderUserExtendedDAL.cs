using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using System.Data;

namespace QINGUO.DAL
{
    public class OrderUserExtendedDAL : BaseDAL<ModOrderUserExtended>, IOrderUserExtended
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModOrderUserExtended GetModelByWhere(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select UserId,TotalBalance,InCome,TotalScore,TotalIntegral,");
            sb.Append(" WithdrawalsCount=(select isnull(SUM(MoneyNum),0) from Order_UserMoneyRecord where UserId=a.UserId and MoneyType=0 and Status=1)");
            sb.Append(" from Order_UserExtended as a  where 1=1 " + where);
            return dabase.ReadDataBase.SingleOrDefault<ModOrderUserExtended>(sb.ToString());
        }

        /// <summary>
        /// 获取即将到账的金额
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal sumArrivalMoney(string OrderUserId)
        {
            decimal money = 0;
            string sql = "select isnull(sum(Amount),0) from view_OrderList where FlowStatus=0 and OrderUserId='" + OrderUserId + "'";
            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                money = Convert.ToDecimal(ds.Tables[0].Rows[0][0].ToString());
            }
            return money;
        }


        /// <summary>
        /// 用户充值
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">充值金额</param>
        /// <returns></returns>
        public int Recharge(string UserId, decimal Amount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + " where UserId='" + UserId + "';");
            sb.AppendLine();

            sb.Append("update Sys_Master set Money=Money+" + Amount + " where Id='" + UserId + "';");
            sb.AppendLine();
            return dabase.ExecuteNonQuery(sb.ToString());
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
            StringBuilder sb = new StringBuilder();
            //用户支付金额,司机账户加金额
            sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + ",InCome=InCome+" + Amount + " where UserId='" + CardId + "';");
            sb.AppendLine();

            //用户账户减去金额
            decimal money = 0;
            string sql = "select * from Order_UserExtended where UserId='" + UserId + "'";
            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                money = Convert.ToDecimal(ds.Tables[0].Rows[0]["TotalBalance"].ToString());
            }

            if (Amount > money)//金额如果大于账户余额时，则置账户余额为0
            {
                sb.Append("update Order_UserExtended set TotalBalance=0,InCome=InCome-" + money + " where UserId='" + UserId + "';");
                sb.AppendLine();
            }
            else if (Amount <= money)//金额小于等于账户余额，则把账户余额减去这单金额
            {
                sb.Append("update Order_UserExtended set TotalBalance=TotalBalance-" + Amount + ",InCome=InCome-" + Amount + " where UserId='" + UserId + "';");
                sb.AppendLine();
            }
            sb.Append("insert into Order_UserMoneyRecord(Id,UserId,MoneyType,MoneyNum,Remark,CreateTime,Status)");
            sb.Append(" values(NEWID(),'" + CardId + "',1,'" + Amount + "','订单支付',GETDATE(),1)");
            sb.AppendLine();
            sb.Append(" update Order_RunOrder set Status=1 where OrderId='" + OrderId + "';");
            return dabase.ExecuteNonQuery(sb.ToString());
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
            StringBuilder sb = new StringBuilder();
            sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + ",WithdrawalsCount=WithdrawalsCount-" + Amount + " where UserId='" + UserId + "';");
            sb.AppendLine();

            sb.Append("update Sys_Master set Money=Money+" + Amount + " where Id='" + UserId + "';");
            sb.AppendLine();
            return dabase.ExecuteNonQuery(sb.ToString());
        }

        /// <summary>
        /// 用户账户操作
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">账户金额</param>
        /// <param name="Phone">转入账户</param>
        /// <param name="TypeName">操作名称</param>
        /// <returns></returns>
        public int OperationAccount(string Id, string UserId, decimal Amount, string Phone, int MoneyType, string TypeName, string Remark, int status)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Order_UserMoneyRecord(Id,UserId,MoneyType,MoneyNum,Card,UserName,BankName,Remark,CreateTime,Status)");
            sb.Append(" values(NEWID(),'" + UserId + "'," + MoneyType + ",'" + Amount + "','" + Phone + "','','" + TypeName + "','" + Remark + "',GETDATE()," + status + ")");

            if (TypeName == "巡检收入")
            {
                sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + ",InCome=InCome+" + Amount + " where UserId='" + UserId + "';");
                sb.AppendLine();
            }
            else if (TypeName == "退单扣除")
            {
                sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + " where UserId='" + UserId + "';");
                sb.AppendLine();
            }
            else if (TypeName == "物业急修扣除")
            {
                sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + " where UserId='" + UserId + "';");
                sb.AppendLine();
            }
            else if (TypeName == "用户提现")
            {
                sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + ",WithdrawalsCount=WithdrawalsCount-" + Amount + " where UserId='" + UserId + "';");
                sb.AppendLine();
            }
            else if (TypeName == "物业急修")
            {
                sb.Append("update Order_UserExtended set TotalBalance=TotalBalance+" + Amount + ",InCome=InCome+" + Amount + " where UserId='" + UserId + "';");
                sb.AppendLine();
            }

            sb.Append("update Sys_Master set Money=Money+" + Amount + " where Id='" + UserId + "';");
            sb.AppendLine();
            return dabase.ExecuteNonQuery(sb.ToString());
        }

        /// <summary>
        /// 更新用户评分
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int UpdateUserScore(decimal num, string UserId)
        {
            string sql = "update Order_UserExtended set TotalScore='" + num + "' where UserId='" + UserId + "';";
            return dabase.ExecuteNonQuery(sql);
        }

        /*
        public List<ModOrderUserExtended> RevenueRanking(string CId,int jd) { 
            
        }*/



        /// <summary>
        /// 添加工程师,同步金额账户
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Amount">充值金额</param>
        /// <returns></returns>
        public int AddMasterAnsy(string UserId, decimal Amount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Order_UserExtended(UserId,TotalBalance,InCome,WithdrawalsCount,TotalScore,TotalIntegral) values('" + UserId + "','" + Amount + "',0,0,0,0)");
            sb.AppendLine();
            sb.Append("insert into Order_UserMoneyRecord(Id,UserId,MoneyType,MoneyNum,Remark,BankName,CreateTime,Status)");
            sb.Append(" values(NEWID(),'" + UserId + "',1,'" + Amount + "','系统保证金','系统保证金',GETDATE(),2)");
            return dabase.ExecuteNonQuery(sb.ToString());
        }
    }
}
