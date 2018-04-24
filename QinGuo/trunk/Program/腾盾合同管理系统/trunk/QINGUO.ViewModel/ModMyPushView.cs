using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 我的发布(跑腿需求,搭车需求)(已用)
    /// </summary>
    public class ModMyPushView
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
        /// Infomation
        /// </summary>		
        private string _infomation;
        public string Infomation
        {
            get { return _infomation; }
            set { _infomation = value; }
        }
        /// <summary>
        /// CreateTime
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// Nickname
        /// </summary>		
        private string _nickname;
        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        /// <summary>
        /// HeadImg
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
        /// 发布人
        /// </summary>		
        private string _createrid;
        public string CreaterId
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
    }
}
