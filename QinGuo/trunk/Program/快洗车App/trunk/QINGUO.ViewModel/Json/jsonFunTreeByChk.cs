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
    public class jsonFunTreeByChk
    {
        public String id { get; set; }

        public String text { get; set; }

        public bool check{ get; set; }

        public Boolean leaf { get; set; }

        public String iconCls { get; set; }

        public List<jsonFunTreeByChk> children { get; set; } // 存放子结点 
    }
}
