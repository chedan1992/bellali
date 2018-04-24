using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
   public class jsonSysCategory
    {
        /// <summary>
        /// 分类编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>		
        public string OrderNum { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>		
        public string ParentCategoryId { get; set; }
        /// <summary>
        /// 数据状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 节点深度
        /// </summary>		
        public int Depth { get; set; }
        /// <summary>
        /// 节点路径
        /// </summary>		
        public string Path { get; set; }
        /// <summary>
        /// 分类图标
        /// </summary>		
        public string PicUrl { get; set; }
        /// <summary>
        /// 是否是子节点
        /// </summary>		
        public bool leaf { get; set; }
        /// <summary>
        /// 是否系统分类(系统定义的分类不能删除)
        /// </summary>		
        public bool IsSystem { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>		
        public string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>		
        public string CreaterName { get; set; }    
    }
}
