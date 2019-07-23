using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{

    /// <summary>
    /// 当前角色权限树
    /// </summary>
    public class JsonRoleFunZtree
    {

        public string ScfID { get; set; }
        /// <summary>
        /// 权限ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 页面路径
        /// </summary>
        public string PageURL { get; set; }
        /// <summary>
        /// 类样式
        /// </summary>
        public string IconClass { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public string Checked { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public string ParentID { get; set; }
        /// <summary>
        /// 子节点集合
        /// </summary>
        public List<JsonRoleFunZtree> children { get; set; }

    }
}
