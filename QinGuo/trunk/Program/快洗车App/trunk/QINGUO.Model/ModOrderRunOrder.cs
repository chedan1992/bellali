using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 用户订单表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Order_RunOrder")]
    [Dapper.PrimaryKeyAttribute("OrderId", autoIncrement = false)]
    public class ModOrderRunOrder
    {
        /// <summary>
        /// 订单编号
        /// </summary>		
        private string _orderid;
        public string OrderId
        {
            get { return _orderid; }
            set { _orderid = value; }
        }


        /// <summary>
        /// 跑腿编号
        /// </summary>		
        private string _runid;
        public string RunId
        {
            get { return _runid; }
            set { _runid = value; }
        }
        /// <summary>
        /// 抢单时间
        /// </summary>		
        private DateTime? _ordertime;
        public DateTime? OrderTime
        {
            get { return _ordertime; }
            set { _ordertime = value; }
        }
        /// <summary>
        /// 抢单人
        /// </summary>		
        private string _orderuserid;
        public string OrderUserId
        {
            get { return _orderuserid; }
            set { _orderuserid = value; }
        }
        /// <summary>
        /// 状态(0抢单 1:结束)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 订单类型(0:跑腿订单 1:搭车订单)
        /// </summary>		
        private int _OrderType;
        public int OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }
        /// <summary>
        /// 订单编码
        /// </summary>		
        private string _OrderCode;
        public string OrderCode
        {
            get { return _OrderCode; }
            set { _OrderCode = value; }
        }
        /// <summary>
        /// 我对别人的评价内容
        /// </summary>		
        private string _mycontent;
        public string MyContent
        {
            get { return _mycontent; }
            set { _mycontent = value; }
        }
        /// <summary>
        /// 他人对我的评价内容
        /// </summary>		
        private string _otherecontent;
        public string OthereContent
        {
            get { return _otherecontent; }
            set { _otherecontent = value; }
        }

    }
}
