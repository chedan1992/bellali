using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 评论列表视图
    /// </summary>
    public class ModOrderEvaluationView
    {
        /// <summary>
        /// OrderId
        /// </summary>		
        private string _orderid;
        public string OrderId
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        /// <summary>
        /// MyContent
        /// </summary>		
        private string _mycontent;
        public string MyContent
        {
            get { return _mycontent; }
            set { _mycontent = value; }
        }
        /// <summary>
        /// OthereContent
        /// </summary>		
        private string _otherecontent;
        public string OthereContent
        {
            get { return _otherecontent; }
            set { _otherecontent = value; }
        }
        ///// <summary>
        ///// Evaluation
        ///// </summary>		
        //private int _evaluation;
        //public int Evaluation
        //{
        //    get { return _evaluation; }
        //    set { _evaluation = value; }
        //}
        /// <summary>
        /// MyStartNum
        /// </summary>		
        private int _mystartnum;
        public int MyStartNum
        {
            get { return _mystartnum; }
            set { _mystartnum = value; }
        }
        /// <summary>
        /// OthereStartNum
        /// </summary>		
        private int _otherestartnum;
        public int OthereStartNum
        {
            get { return _otherestartnum; }
            set { _otherestartnum = value; }
        }
        /// <summary>
        /// MyCreateTime
        /// </summary>		
        private DateTime ? _mycreatetime;
        public DateTime ? MyCreateTime
        {
            get { return _mycreatetime; }
            set { _mycreatetime = value; }
        }
        /// <summary>
        /// OthereCreateTime
        /// </summary>		
        private DateTime ? _otherecreatetime;
        public DateTime ? OthereCreateTime
        {
            get { return _otherecreatetime; }
            set { _otherecreatetime = value; }
        }
        /// <summary>
        /// OrderUserId
        /// </summary>		
        private string _orderuserid;
        public string OrderUserId
        {
            get { return _orderuserid; }
            set { _orderuserid = value; }
        }
        /// <summary>
        /// CreateId
        /// </summary>		
        private string _createid;
        public string CreateId
        {
            get { return _createid; }
            set { _createid = value; }
        }
        /// <summary>
        /// 登录帐号
        /// </summary>
        /// 字段长度:50
        /// 是否为空:true
        public string Name { get; set; }

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
