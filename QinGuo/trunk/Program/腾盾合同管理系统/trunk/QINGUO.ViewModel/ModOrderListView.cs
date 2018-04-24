using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 全城接单和附近接单列表(已用)
    /// </summary>
    public class ModOrderListView
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
        /// 经度
        /// </summary>		
        private string _Mapx;
        public string Mapx
        {
            get { return _Mapx; }
            set { _Mapx = value; }
        }
        /// <summary>
        /// 纬度
        /// </summary>		
        private string _Mapy;
        public string Mapy
        {
            get { return _Mapy; }
            set { _Mapy = value; }
        }
        /// <summary>
        /// 地址
        /// </summary>		
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// 需求详细
        /// </summary>		
        private string _infomation;
        public string Infomation
        {
            get { return _infomation; }
            set { _infomation = value; }
        }
        /// <summary>
        /// 金额
        /// </summary>		
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
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
        /// 用户昵称
        /// </summary>		
        private string _nickname;
        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        /// <summary>
        /// 用户头像
        /// </summary>		
        private string _headimg;
        public string HeadImg
        {
            get { return _headimg; }
            set { _headimg = value; }
        }
        /// <summary>
        /// 发布类型(0:跑腿需求 1:搭车需求)
        /// </summary>		
        private Int32 _PushType;
        public Int32 PushType
        {
            get { return _PushType; }
            set { _PushType = value; }
        }
        /// <summary>
        /// 距离
        /// </summary>		
        private decimal _Potint;
        public decimal Potint
        {
            get { return _Potint; }
            set { _Potint = value; }
        }
    }
}
