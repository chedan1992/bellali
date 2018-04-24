using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.ViewModel;

namespace QINGUO.Model
{
    public class jsonSysBusinessCircle
    {
        public String id { get; set; }

        public String text { get; set; }

        public Boolean leaf { get; set; }

        public Boolean expanded { get; set; }

        public String Code { get; set; }

        public List<JsonTree> children { get; set; } // 存放子结点 
    }
}
