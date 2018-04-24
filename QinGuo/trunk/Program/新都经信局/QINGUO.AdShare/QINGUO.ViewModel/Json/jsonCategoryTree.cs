using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{

    /// <summary>
    /// 带checkbox的菜单树
    /// 没有checkbox
    /// </summary>
    public class jsonCategoryTree
    {
        public String id { get; set; }

        public String text { get; set; }

        public bool HasChild { get; set; }

        public string iconCls { get; set; }

        public string parentId { get; set; }

        public List<jsonCategoryTree> children { get; set; } // 存放子结点 
    }
    /// <summary>
    /// 带checkbox的菜单树
    /// </summary>
    public class jsonCategoryTreeCheck
    {
        public String ID { get; set; }

        public String Name { get; set; }

        public bool Checked { get; set; }

        public List<jsonCategoryTreeCheck> children { get; set; }
    }
}
