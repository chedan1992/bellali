using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Common;

namespace QINGUO.Model
{
    /// <summary>
    /// 用户提现记录表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Order_UserMoneyRecord")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModOrderUserMoneyRecord
    {
        /// <summary>
        /// 编号
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 批次编号
        /// </summary>		
        private string _batch_no;
        public string Batch_no
        {
            get { return _batch_no; }
            set { _batch_no = value; }
        }

        /// <summary>
        /// 用户编号
        /// </summary>		
        private string _userid;
        public string UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 金额类型（1为增加金额，0为减少金额）
        /// </summary>		
        private int _moneytype;
        public int MoneyType
        {
            get { return _moneytype; }
            set { _moneytype = value; }
        }
        /// <summary>
        /// 金额数量
        /// </summary>		
        private decimal _moneynum;
        public decimal MoneyNum
        {
            get { return _moneynum; }
            set { _moneynum = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }


        /// <summary>
        /// 操作名称()
        /// </summary>		
        private string _BankName;
        public string BankName
        {
            get { return _BankName; }
            set { _BankName = value; }
        }
        /// <summary>
        /// 支付宝
        /// </summary>		
        private string _Card;
        public string Card
        {
            get { return _Card; }
            set { _Card = value; }
        }
        /// <summary>
        /// 用户名
        /// </summary>		
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// 状态(-1为删除，0为申请中,1:正在支付,2为已完成)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
         /// <summary>
        /// 审核时间
        /// </summary>		
        private DateTime? _AgreeTime;
        public DateTime? AgreeTime
        {
            get { return _AgreeTime; }
            set { _AgreeTime = value; }
        }

        /// <summary>
        /// 支付宝账号
        /// </summary>
        [Property(IsDataBaseField = false)]
        [Dapper.ResultColumn]
        public string Alipay { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        [Property(IsDataBaseField = false)]
        [Dapper.ResultColumn]
        public decimal Money { get; set; }

    }
}
