using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("H_FinanceDetail")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModHFinanceDetail
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
        /// DeductingStatus
        /// </summary>		
        private int _deductingstatus;
        public int DeductingStatus
        {
            get { return _deductingstatus; }
            set { _deductingstatus = value; }
        }
        /// <summary>
        /// DetailRemark
        /// </summary>		
        private string _detailremark;
        public string DetailRemark
        {
            get { return _detailremark; }
            set { _detailremark = value; }
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
        /// PurchaseId
        /// </summary>		
        private string _purchaseid;
        public string PurchaseId
        {
            get { return _purchaseid; }
            set { _purchaseid = value; }
        }
        /// <summary>
        /// CGDH
        /// </summary>		
        private string _cgdh;
        public string CGDH
        {
            get { return _cgdh; }
            set { _cgdh = value; }
        }
        /// <summary>
        /// DeductingTime
        /// </summary>		
        private DateTime? _deductingtime;
        public DateTime? DeductingTime
        {
            get { return _deductingtime; }
            set { _deductingtime = value; }
        }
                                     /// <summary>
                                     /// 主表ID
                                     /// </summary>
        public string FinanceId { get; set; }
    }
}
