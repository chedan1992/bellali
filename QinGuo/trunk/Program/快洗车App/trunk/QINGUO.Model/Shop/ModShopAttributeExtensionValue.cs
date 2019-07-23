using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 商品属性类别规格值表
    /// </summary>
    public class ModShopAttributeExtensionValue
    {
        /// <summary>
        /// 编号
        /// </summary>		
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 规格编号
        /// </summary>		
        private string _extensionid;
        public string ExtensionId
        {
            get { return _extensionid; }
            set { _extensionid = value; }
        }
        /// <summary>
        /// 规格值名称
        /// </summary>		
        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        /// <summary>
        /// 创建人
        /// </summary>		
        private string _creatorid;
        public string CreatorId
        {
            get { return _creatorid; }
            set { _creatorid = value; }
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
        /// 创建时间
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }        
    }
}
