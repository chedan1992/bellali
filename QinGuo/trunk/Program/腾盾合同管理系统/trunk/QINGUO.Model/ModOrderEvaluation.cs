using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    ///评价列表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Order_Evaluation")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModOrderEvaluation
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
        /// 订单编号
        /// </summary>		
        private string _orderid;
        public string OrderId
        {
            get { return _orderid; }
            set { _orderid = value; }
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
        /// <summary>
        /// 评价(1:满意，0:一般，-1很差)
        /// </summary>		
        private int _evaluation;
        public int Evaluation
        {
            get { return _evaluation; }
            set { _evaluation = value; }
        }
        /// <summary>
        /// 用户信用评级(一共5星)
        /// </summary>		
        private int _mystartnum;
        public int MyStartNum
        {
            get { return _mystartnum; }
            set { _mystartnum = value; }
        }
        /// <summary>
        /// 他人用户信用评级(一共5星)
        /// </summary>		
        private int _otherestartnum;
        public int OthereStartNum
        {
            get { return _otherestartnum; }
            set { _otherestartnum = value; }
        }
        /// <summary>
        /// 我对别人的评价时间
        /// </summary>		
        private DateTime? _mycreatetime;
        public DateTime? MyCreateTime
        {
            get { return _mycreatetime; }
            set { _mycreatetime = value; }
        }
        /// <summary>
        /// 他人对我的评价时间
        /// </summary>		
        private DateTime? _otherecreatetime;
        public DateTime? OthereCreateTime
        {
            get { return _otherecreatetime; }
            set { _otherecreatetime = value; }
        }
        /// <summary>
        /// 司机是否评价
        /// </summary>		
        private bool _HasCarEvaluation;
        public bool HasCarEvaluation
        {
            get { return _HasCarEvaluation; }
            set { _HasCarEvaluation = value; }
        }
        /// <summary>
        /// 用户是否评价
        /// </summary>		
        private bool _HasUserEvaluation;
        public bool HasUserEvaluation
        {
            get { return _HasUserEvaluation; }
            set { _HasUserEvaluation = value; }
        }
    }
}
