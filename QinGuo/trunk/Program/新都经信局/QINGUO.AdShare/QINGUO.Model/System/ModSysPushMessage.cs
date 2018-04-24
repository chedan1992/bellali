using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_PushMessage")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysPushMessage
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
        /// 类型（推送类型，依次往后推(0:巡检通知)）
        /// </summary>		
        private int _type;
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 推送标题
        /// </summary>		
        private string _ptitle;
        public string PTitle
        {
            get { return _ptitle; }
            set { _ptitle = value; }
        }
        /// <summary>
        /// 推送内容
        /// </summary>		
        private string _info;
        public string Info
        {
            get { return _info; }
            set { _info = value; }
        }
        /// <summary>
        /// 关联编号
        /// </summary>		
        private string _correlativeid;
        public string CorrelativeId
        {
            get { return _correlativeid; }
            set { _correlativeid = value; }
        }
        /// <summary>
        /// 所属公司
        /// </summary>		
        private string _companyid;
        public string CompanyId
        {
            get { return _companyid; }
            set { _companyid = value; }
        }
        /// <summary>
        /// 创建者
        /// </summary>		
        private string _userid;
        public string UserId
        {
            get { return _userid; }
            set { _userid = value; }
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
        /// 状态(1:未读，2：已读)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
