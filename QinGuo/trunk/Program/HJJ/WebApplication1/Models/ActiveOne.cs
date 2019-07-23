using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Commonlication
{
    [Serializable]
    public class ActiveOne
    {
        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public int Id { get; set; }

        /// <summary>
        /// 活动I
        /// </summary>
        public int ActiceId { get; set; }

        /// <summary>
        /// 每次活动开始时间
        /// </summary>
        public DateTime? Start { get; set; }

        /// <summary>
        /// 每次活动结束时间
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int ManNumber { get; set; }

        /// <summary>
        /// 单次活动金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

    }
}
