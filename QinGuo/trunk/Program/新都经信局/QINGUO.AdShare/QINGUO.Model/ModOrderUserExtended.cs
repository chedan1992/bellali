using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Common;

namespace QINGUO.Model
{
    /// <summary>
    /// 用户扩展表 用户钱包信息
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Order_UserExtended")]
    [Dapper.PrimaryKeyAttribute("UserId", autoIncrement = false)]
    public partial class ModOrderUserExtended
    {
        /// <summary>
        /// 用户主键编号
        /// </summary>		
        private string _userid;
        public string UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 账户余额
        /// </summary>		
        private decimal _totalbalance;
        public decimal TotalBalance
        {
            get { return _totalbalance; }
            set { _totalbalance = value; }
        }
        /// <summary>
        /// 累计收入
        /// </summary>		
        private decimal _income;
        public decimal InCome
        {
            get { return _income; }
            set { _income = value; }
        }
        /// <summary>
        /// 累计提现
        /// </summary>		
        private decimal _withdrawalscount;
        public decimal WithdrawalsCount
        {
            get { return _withdrawalscount; }
            set { _withdrawalscount = value; }
        }
        /// <summary>
        /// 总评分(评价)
        /// </summary>		
        private int _totalscore;
        public int TotalScore
        {
            get { return _totalscore; }
            set { _totalscore = value; }
        }
        /// <summary>
        /// 总积分
        /// </summary>		
        private int _totalintegral;
        public int TotalIntegral
        {
            get { return _totalintegral; }
            set { _totalintegral = value; }
        }
    }
}
