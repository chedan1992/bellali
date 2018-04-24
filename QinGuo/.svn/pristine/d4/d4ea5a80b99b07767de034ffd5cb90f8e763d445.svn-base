using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace QINGUO.Model
{
    /// <summary>
    /// 单位关联供应商，消费部门，维保公司
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Sys_CompanyCognate")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysCompanyCognate
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 社会单位编号
        /// </summary>		
        private string _employerId;
        public string EmployerId
        {
            get { return _employerId; }
            set { _employerId = value; }
        }

        /// <summary>
        /// 关联id（存储消防，供应商，维保单位编号）
        /// </summary>		
        private string _cId;
        public string CId
        {
            get { return _cId; }
            set { _cId = value; }
        }
        /// <summary>
        /// 选择类型（1：社会单位选上级单位  2：上级单位选社会单位）
        /// </summary>
        public int SelectType { get; set; }
        /// <summary>
        /// 状态(0：默认未审批  1：正常 -1:删除  -2：审批未通过)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 关联流程id
        /// </summary>
        public string FlowId { get; set; }
    }
}
