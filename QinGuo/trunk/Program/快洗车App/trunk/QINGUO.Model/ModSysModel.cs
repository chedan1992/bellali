using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Model")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysModel
    {  /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>		
        public string ModelName { get; set; }
        /// <summary>
        /// 状态(-1: 删除 0:禁用 1:正常)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime ? CreateTime { get; set; }
        /// <summary>
        /// 排序
        /// </summary>		
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 公司id
        /// </summary>
        public string CompanyId { get; set; }

         /// <summary>
        /// 外键id
        /// </summary>
        public string LinkId { get; set; }
        
    }
}
