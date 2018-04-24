using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 数据说明表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Sys_RemarkSetting")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysRemarkSetting
    {
        /// <summary>
        /// 编号
        /// </summary>		
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 内容
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>		
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 类型名称
        /// </summary>		
        private string _typename;
        public string TypeName
        {
            get { return _typename; }
            set { _typename = value; }
        }        
    }
}
