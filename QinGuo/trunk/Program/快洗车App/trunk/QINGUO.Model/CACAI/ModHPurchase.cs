using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 采购退货表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("H_Purchase")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModHPurchase
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
        /// OutNumber
        /// </summary>		
        private string _outnumber;
        public string OutNumber
        {
            get { return _outnumber; }
            set { _outnumber = value; }
        }
        /// <summary>
        /// StoreId
        /// </summary>		
        private string _storeid;
        public string StoreId
        {
            get { return _storeid; }
            set { _storeid = value; }
        }
        /// <summary>
        /// CusterId
        /// </summary>		
        private string _custerid;
        public string CusterId
        {
            get { return _custerid; }
            set { _custerid = value; }
        }
        /// <summary>
        /// GetNumber
        /// </summary>		
        private string _getnumber;
        public string GetNumber
        {
            get { return _getnumber; }
            set { _getnumber = value; }
        }
        /// <summary>
        /// BillDate
        /// </summary>		
        private DateTime? _billdate;
        public DateTime? BillDate
        {
            get { return _billdate; }
            set { _billdate = value; }
        }
        /// <summary>
        /// OutStatus
        /// </summary>		
        private int _outstatus;
        public int OutStatus
        {
            get { return _outstatus; }
            set { _outstatus = value; }
        }
        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// Maker
        /// </summary>		
        private string _maker;
        public string Maker
        {
            get { return _maker; }
            set { _maker = value; }
        }
        /// <summary>
        /// Judger
        /// </summary>		
        private string _judger;
        public string Judger
        {
            get { return _judger; }
            set { _judger = value; }
        }
        /// <summary>
        /// JudgeDate
        /// </summary>		
        private DateTime? _judgedate;
        public DateTime? JudgeDate
        {
            get { return _judgedate; }
            set { _judgedate = value; }
        }
        /// <summary>
        /// 财务状态（0：未提交 1：已提交 2：已审核）
        /// </summary>
        public int FinancialState { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? FinancialDT { get; set; }
        /// <summary>
        /// 退货金额
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 盈亏金额
        /// </summary>
        public decimal LossPrice { get; set; }
        /// <summary>
        /// 是否有附件
        /// </summary>
        public bool HasFileAttach { get; set; }
        /// <summary>
        /// 原始金额
        /// </summary>
        public decimal oldPrice { get; set; }
    }
}
