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
    [Dapper.TableNameAttribute("Sys_DataBaseBack")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysDataBaseBack
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
        /// 链接地址
        /// </summary>		
        private string _linkurl;
        public string LinkUrl
        {
            get { return _linkurl; }
            set { _linkurl = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private string _Size;
        /// <summary>
        /// 大小
        /// </summary>
        public string Size
        {
            get { return _Size; }
            set { _Size = value; }
        }
        /// <summary>
        /// -1: 删除 0:禁用 1:正常
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
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
    }
}
