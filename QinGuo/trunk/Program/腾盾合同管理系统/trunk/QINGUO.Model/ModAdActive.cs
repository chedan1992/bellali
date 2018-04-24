using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 数据库备份表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Ad_Active")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModAdActive
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
        /// 广告名称
        /// </summary>		
        private string _activename;
        public string ActiveName
        {
            get { return _activename; }
            set { _activename = value; }
        }
        private string _Img;
        /// <summary>
        /// 广告图片
        /// </summary>
        public string Img
        {
            get { return _Img; }
            set { _Img = value; }
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
        /// 展示方式(1:无时间限制 2:自动下架)
        /// </summary>		
        private int _ShowType;
        public int ShowType
        {
            get { return _ShowType; }
            set { _ShowType = value; }
        }
        /// <summary>
        /// 广告类型(1:内部广告 2:外部广告 3:资讯广告)
        /// </summary>		
        private int _actiontype;
        public int ActionType
        {
            get { return _actiontype; }
            set { _actiontype = value; }
        }
        /// <summary>
        /// 资讯广告外键
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
        /// 状态
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

       
    }
}
