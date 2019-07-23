using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Commonlication
{
    [Serializable]
    public class Active
    {
        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public int Id { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Title { get; set; }

        public string Catory { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Img { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 单次还是多次1 是单次 
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? CountStart { get; set; }
        
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? CountEnd { get; set; }

        /// <summary>
        /// 活动金额
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        public string Payurl { get; set; }

    }
}
