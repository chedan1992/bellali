using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityAttribute : Attribute
    {
        private bool _isAllowRealDelete = false;
        /// <summary>
        /// 实体对应的数据库表
        /// </summary>
        public String TableName { get; set; }

        /// <summary>
        /// 实体对于的数据库表中文描述
        /// </summary>
        public string TableDescription { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 是否允许真删除,默认为false假删除
        /// </summary>
        public bool IsAllowRealDelete
        {
            get { return _isAllowRealDelete; }
            set { _isAllowRealDelete = value; }

        }
    }
}
