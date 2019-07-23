using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("H_Finance")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModHFinance
    {
        /// <summary>
        /// Id
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
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
        /// 创建人编号
        /// </summary>		
        private string _createrid;
        public string CreaterId
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
        /// <summary>
        /// 数据状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// ReturnAmount
        /// </summary>		
        private decimal _returnamount;
        public decimal ReturnAmount
        {
            get { return _returnamount; }
            set { _returnamount = value; }
        }
        /// <summary>
        /// ProfitTotal
        /// </summary>		
        private decimal _profittotal;
        public decimal ProfitTotal
        {
            get { return _profittotal; }
            set { _profittotal = value; }
        }
        /// <summary>
        /// PaymentAmmount
        /// </summary>		
        private decimal _paymentammount;
        public decimal PaymentAmmount
        {
            get { return _paymentammount; }
            set { _paymentammount = value; }
        }
        /// <summary>
        /// PayStatus
        /// </summary>		
        private int _paystatus;
        public int PayStatus
        {
            get { return _paystatus; }
            set { _paystatus = value; }
        }
        /// <summary>
        /// PayTime
        /// </summary>		
        private DateTime? _paytime;
        public DateTime? PayTime
        {
            get { return _paytime; }
            set { _paytime = value; }
        }
        /// <summary>
        /// FinanceRemark
        /// </summary>		
        private string _financeremark;
        public string FinanceRemark
        {
            get { return _financeremark; }
            set { _financeremark = value; }
        }
        /// <summary>
        /// RelationId
        /// </summary>		
        private string _relationid;
        public string RelationId
        {
            get { return _relationid; }
            set { _relationid = value; }
        }
        /// <summary>
        /// OrderInId
        /// </summary>		
        private string _orderinid;
        public string OrderInId
        {
            get { return _orderinid; }
            set { _orderinid = value; }
        }

        /// <summary>
        /// 结账方式：0未设置 1月结 2日结
        /// </summary>
        public int CheckoutType { get; set; }
        /// <summary>
        /// 付款方式：1支付宝 2工行  3农行
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string AccountNum { get; set; }
    }
}
