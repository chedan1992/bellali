using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Common
{ 
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PropertyAttribute : Attribute
    {
        private bool _isDataBaseField = true;
        private bool _isAdd = true;
        private bool _isUpdate = true;
        /// <summary>
        /// 是否数据库字段，默认为True
        /// </summary>
        public bool IsDataBaseField { get { return _isDataBaseField; } set { _isDataBaseField = value; } }

        /// <summary>
        ///该字段是否需要添加，默认为True
        /// </summary>
        public bool IsInsertToDabaBase { get { return _isAdd; } set { _isAdd = value; } }

        /// <summary>
        /// 该字段是否需要更新，默认为True
        /// </summary>
        public bool IsUpdateToDabaBase { get { return _isUpdate; } set { _isUpdate = value; } }
    }
}
