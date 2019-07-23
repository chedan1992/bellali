using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("H_PurchaseRelation")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public  class ModHPurchaseRelation
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
        /// PurchaseId
        /// </summary>		
        private string _purchaseid;
        public string PurchaseId
        {
            get { return _purchaseid; }
            set { _purchaseid = value; }
        }
        /// <summary>
        /// Status
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
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
        /// 原单据金额
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 是否分解
        /// </summary>
        public bool IsDecompose { get; set; }
        /// <summary>
        /// 分解金额
        /// </summary>
        public decimal DecomposePrice { get; set; }
        /// <summary>
        /// 拆分退货单号
        /// </summary>
        public string OutNumberS { get; set; }

        //父节点
        public string PID { get; set; }
    }
}
