using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 返回导航树菜单
    /// </summary>
    public class JsonFunTree
    {
        public String id { get; set; }

        public string parentId { get; set; }

        public String text { get; set; }

        public Boolean leaf { get; set; }

        public Boolean expanded { get; set; }

        public String iconCls { get; set; }

        public String pageUrl { get; set; }

        public Boolean isChild { get; set; }

        public List<JsonFunTree> children { get; set; } // 存放子结点 

        /// <summary>
        /// 排序
        /// </summary>
        public int funSort { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 链接状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public String className { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否系统定义
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 节点深度0:一级，1：二级，
        /// </summary>		
        public int Depth { get; set; }
    }
}
