using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 常用树控件返回json
    /// </summary>
    public class JsonTree
    {
        public String id { get; set; }

        public String text { get; set; }

        public Boolean leaf { get; set; }

        public Boolean expanded { get; set; }

        public String iconCls { get; set; }

        public String pageUrl { get; set; }

        public List<JsonTree> children { get; set; } // 存放子结点 
    }
}
