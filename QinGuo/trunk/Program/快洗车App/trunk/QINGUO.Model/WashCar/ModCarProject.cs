using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 维保项目表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("W_Project")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModCarProject
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
        private string _name;
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
        /// 备注
        /// </summary>		
        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
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
        /// 创建者
        /// </summary>		
        private string _createId;
        public string CreateId
        {
            get { return _createId; }
            set { _createId = value; }
        }
        /// <summary>
        /// 状态(0:待审核，1：审核通过，2：审核不通过)
        /// </summary>		
        private StatusEnum _status;
        public StatusEnum Status
        {
            get { return _status; }
            set { _status = value; }
        }
        
    }
}
