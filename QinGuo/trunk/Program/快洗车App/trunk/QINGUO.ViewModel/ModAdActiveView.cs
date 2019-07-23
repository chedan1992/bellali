using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 公告视图
    /// </summary>
    public class ModAdActiveView
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
        /// 公告名称
        /// </summary>		
        private string _activename;
        public string ActiveName
        {
            get { return _activename; }
            set { _activename = value; }
        }

        /// <summary>
        /// 开始时间
        /// </summary>		
        private DateTime? _activestarttime;
        public DateTime? ActiveStartTime
        {
            get { return _activestarttime; }
            set { _activestarttime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>		
        private DateTime? _activeendtime;
        public DateTime? ActiveEndTime
        {
            get { return _activeendtime; }
            set { _activeendtime = value; }
        }
        /// <summary>
        /// 说明
        /// </summary>		
        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
        /// <summary>
        /// 公司编号
        /// </summary>		
        private string _companyid;
        public string CompanyId
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        /// <summary>
        /// 公告方式(1:无时间限制 2:自动下架)
        /// </summary>		
        private int _actiontype;
        public int ActionType
        {
            get { return _actiontype; }
            set { _actiontype = value; }
        }
        /// <summary>
        /// 公告类型外键
        /// </summary>		
        private string _actionformid;
        public string ActionFormId
        {
            get { return _actionformid; }
            set { _actionformid = value; }
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
        /// 状态(-1为删除，0为未启动,1为正在进行 2:已结束 3:已坏)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
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
        /// 公告类型名称
        /// </summary>
        public string ActionTypeName { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadNum { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 是否私信(管理员针对个人发送消息公告)
        /// </summary>
        public bool IsPersonalMsg { get; set; }
        /// <summary>
        /// 个人主键编号,多个人用,隔开,最多10人
        /// </summary>
        public string PersonalId { get; set; }
        /// <summary>
        /// 个人名称
        /// </summary>
        public string PersonalName { get; set; }
    }
}
