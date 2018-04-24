using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_RemarkSetting")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModRemarkSetting
    {

        /// <summary>
        /// 
        /// </summary>
        /// 字段长度:4
        /// 是否为空:false
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 字段长度:6
        /// 是否为空:true
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 字段长度:6
        /// 是否为空:true
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// 字段长度:50
        /// 是否为空:true
        public string TypeName { get; set; }
    }
}
