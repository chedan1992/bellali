using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 操作日志
    /// </summary>
    /// <summary>
    /// 用户扩展表 用户钱包信息
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Sys_OperateLog")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysOperateLog
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
        /// 用户名
        /// </summary>		
        private string _username;
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        /// <summary>
        /// 访问IP地址
        /// </summary>		
        private string _IPAddress;
        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }

        /// <summary>
        /// 访问地址
        /// </summary>		
        private string _linkurl;
        public string LinkUrl
        {
            get { return _linkurl; }
            set { _linkurl = value; }
        }
        /// <summary>
        /// 操作的详细描述
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// 表名
        /// </summary>		
        private string _tablename;
        public string TableName
        {
            get { return _tablename; }
            set { _tablename = value; }
        }
        /// <summary>
        /// 模块名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 访问类型(0:访问 1:操作  2:异常)
        /// </summary>
        public string LogType { get; set; }
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
    }
}
