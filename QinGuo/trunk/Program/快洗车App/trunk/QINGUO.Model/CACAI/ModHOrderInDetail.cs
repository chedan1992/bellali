using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// Id
    /// </summary>	
    [Serializable]
    [Dapper.TableNameAttribute("H_OrderInDetail")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModHOrderInDetail
    {
        
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
        /// GoodId
        /// </summary>		
        private string _goodid;
        public string GoodId
        {
            get { return _goodid; }
            set { _goodid = value; }
        }
        /// <summary>
        /// Code
        /// </summary>		
        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// GoodName
        /// </summary>		
        private string _goodname;
        public string GoodName
        {
            get { return _goodname; }
            set { _goodname = value; }
        }
        /// <summary>
        /// GoodUnit
        /// </summary>		
        private string _goodunit;
        public string GoodUnit
        {
            get { return _goodunit; }
            set { _goodunit = value; }
        }
        /// <summary>
        /// Count
        /// </summary>		
        private int _count;
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        /// <summary>
        /// Price
        /// </summary>		
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        /// <summary>
        /// Money
        /// </summary>		
        private decimal _money;
        public decimal Money
        {
            get { return _money; }
            set { _money = value; }
        }
        /// <summary>
        /// Batch
        /// </summary>		
        private string _batch;
        public string Batch
        {
            get { return _batch; }
            set { _batch = value; }
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
        /// StyleNum
        /// </summary>		
        private string _stylenum;
        public string StyleNum
        {
            get { return _stylenum; }
            set { _stylenum = value; }
        }
        /// <summary>
        /// FreightNum
        /// </summary>		
        private string _freightnum;
        public string FreightNum
        {
            get { return _freightnum; }
            set { _freightnum = value; }
        }
        /// <summary>
        /// ListOrder
        /// </summary>		
        private int _listorder;
        public int ListOrder
        {
            get { return _listorder; }
            set { _listorder = value; }
        }
        /// <summary>
        /// 主表Id
        /// </summary>
        public string MainId { get; set; }
        /// <summary>
        /// 款式数量
        /// </summary>
        public int StyleCount { get; set; }
        /// <summary>
        /// 款式金额
        /// </summary>
        public decimal StylePrice { get; set; }
        /// <summary>
        /// 单据数量
        /// </summary>
        public int BillNum { get; set; }
        /// <summary>
        /// 单据金额
        /// </summary>
        public decimal BillPrice { get; set; }
        /// <summary>
        /// 单据款金额
        /// </summary>
        public decimal BillMoney { get; set; }

        /// <summary>
        /// 核查补充数量
        /// </summary>
        public int CheckNum { get; set; }
        /// <summary>
        /// 核查补充金额
        /// </summary>
        public decimal CheckPrice { get; set; }
        /// <summary>
        /// 盈亏金额
        /// </summary>
        public decimal LosstPrice { get; set; }
    }
}
