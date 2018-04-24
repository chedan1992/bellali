using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.QuartService
{
    public class JobInfo
    {
        /// <summary>
        /// 工作项名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 工作项组名
        /// </summary>
        public string JobGroup { get; set; }
        /// <summary>
        /// 当前Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 触发器ID集合
        /// </summary>
        public List<string> JobTriggers { get; set; }
    }
}
