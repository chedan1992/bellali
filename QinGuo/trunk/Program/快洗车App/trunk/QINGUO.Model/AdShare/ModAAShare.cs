using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 用户广告表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Ad_AShare")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModAAShare
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
        private string _img;
        /// <summary>
        /// 广告图片
        /// </summary>
        public string Img
        {
            get { return _img; }
            set { _img = value; }
        }

        private string _url;
        /// <summary>
        /// 链接
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
        /// <summary>
        /// 状态(0:待审核，1：审核通过，2：审核不通过)
        /// </summary>		
        private FlowEnum _status;
        public FlowEnum Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 是否开启
        /// </summary>		
        private bool _isshow;
        public bool IsShow
        {
            get { return _isshow; }
            set { _isshow = value; }
        }

        /// <summary>
        /// 广告点击量
        /// </summary>		
        private int _clicknum;
        public int Clicknum
        {
            get { return _clicknum; }
            set { _clicknum = value; }
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
        /// 类型 1：顶部广告，2：底部广告
        /// </summary>		
        private int _type;
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 审核人
        /// </summary>		
        private string _auditor;
        public string Auditor
        {
            get { return _auditor; }
            set { _auditor = value; }
        }

        /// <summary>
        /// 审核时间
        /// </summary>		
        private DateTime? _auditorTime;
        public DateTime? AuditorTime
        {
            get { return _auditorTime; }
            set { _auditorTime = value; }
        }
        
        /// <summary>
        /// 审批意见
        /// </summary>		
        private string _introduction;
        public string Introduction
        {
            get { return _introduction; }
            set { _introduction = value; }
        }
        
    }
}
