using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.QuartService
{
    public class TriggerInfo
    {
        /// <summary>
        /// 触发器名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 触发器组名
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 触发器时间类型
        /// </summary>
        public string TriggerTimeType { get; set; }
        /// <summary>
        /// 组值
        /// </summary>
        public string TimeGroupValue { get; set; }
    }
}
