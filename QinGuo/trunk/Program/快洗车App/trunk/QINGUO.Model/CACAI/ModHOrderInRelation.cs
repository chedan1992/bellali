using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("H_OrderInRelation")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModHOrderInRelation
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
        /// OrderInId
        /// </summary>		
        private string _orderinid;
        public string OrderInId
        {
            get { return _orderinid; }
            set { _orderinid = value; }
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
        /// 是否分解
        /// </summary>
        public bool IsDecompose { get; set; }
        /// <summary>
        /// 分解金额
        /// </summary>
        public decimal DecomposePrice { get; set; }   
    }
}
