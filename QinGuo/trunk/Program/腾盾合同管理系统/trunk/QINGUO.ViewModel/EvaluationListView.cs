using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 用户所有评价列表
    /// </summary>
    public class EvaluationListView
    {
        /// <summary>
        /// 抢单人对我的评价
        /// </summary>
        public string MyList { get; set; }
        /// <summary>
        /// 我对抢单人的评价
        /// </summary>
        public string OtherList { get; set; }
    }
}
