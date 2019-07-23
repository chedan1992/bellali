using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 申请汽车维保信息表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("W_Order")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModCarOrder
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
        private string _cid;
        /// <summary>
        /// 汽车ID
        /// </summary>
        public string CId
        {
            get { return _cid; }
            set { _cid = value; }
        }

        private string _name;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _phone;
        /// <summary>
        /// 最近维修电话
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private int _termOfValidity;
        /// <summary>
        /// TermOfValidity(天)
        /// </summary>
        public int TermOfValidity
        {
            get { return _termOfValidity; }
            set { _termOfValidity = value; }
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
        /// 创建者 维修厂id
        /// </summary>		
        private string _createId;
        public string CreateId
        {
            get { return _createId; }
            set { _createId = value; }
        }

        /// <summary>
        /// 创建者
        /// </summary>		
        private string _createName;
        public string CreateName
        {
            get { return _createName; }
            set { _createName = value; }
        }


        /// <summary>
        /// 汽车用户id
        /// </summary>		
        private string _cUserId;
        public string CUserId
        {
            get { return _cUserId; }
            set { _cUserId = value; }
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
        /// 审核人 供应商id
        /// </summary>		
        private string _auditorId;
        public string AuditorId
        {
            get { return _auditorId; }
            set { _auditorId = value; }
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
        /// 备注
        /// </summary>		
        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        /// <summary>
        /// 审核备注
        /// </summary>		
        private string _auditorRemarks;
        public string AuditorRemarks
        {
            get { return _auditorRemarks; }
            set { _auditorRemarks = value; }
        }
    }
}
