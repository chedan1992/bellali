using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Common;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Flow")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysFlow
    {
        /// <summary>
        /// 审批主键编号
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 审批标题
        /// </summary>		
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 审批类型(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核 4:设备责任人变更)
        /// </summary>		
        private int _flowtype;
        public int FlowType
        {
            get { return _flowtype; }
            set { _flowtype = value; }
        }
        /// <summary>
        /// 审批状态(-1:未通过 0:待审核 1:已通过)
        /// </summary>		
        private int _flowstatus;
        public int FlowStatus
        {
            get { return _flowstatus; }
            set { _flowstatus = value; }
        }
        /// <summary>
        /// 审批备注
        /// </summary>		
        private string _reamrk;
        public string Reamrk
        {
            get { return _reamrk; }
            set { _reamrk = value; }
        }
        /// <summary>
        /// 审批发起人
        /// </summary>		
        private string _approvaluser;
        public string ApprovalUser
        {
            get { return _approvaluser; }
            set { _approvaluser = value; }
        }
        /// <summary>
        /// 发起时间
        /// </summary>		
        private DateTime ? _approvaltime;
        public DateTime ? ApprovalTime
        {
            get { return _approvaltime; }
            set { _approvaltime = value; }
        }
        /// <summary>
        /// 审核人
        /// </summary>		
        private string _audituser;
        public string AuditUser
        {
            get { return _audituser; }
            set { _audituser = value; }
        }
        /// <summary>
        /// 审批时间
        /// </summary>		
        private DateTime ? _audittime;
        public DateTime ? AuditTime
        {
            get { return _audittime; }
            set { _audittime = value; }
        }
        /// <summary>
        /// 所属公司编号
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public string MasterId { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string ApprovalUserName { get; set; }

        
        /// <summary>
        /// 审核人
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string AuditUserName { get; set; }
        
        
    }
}
