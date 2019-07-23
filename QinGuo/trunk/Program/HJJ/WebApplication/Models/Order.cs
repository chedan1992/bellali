using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Commonlication
{
    [Serializable]
    public class Order
    {
        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public int Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 活动
        /// </summary>
        public int ActiveId { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayMoney { get; set; }

        public string Payno { get; set; }

        /// <summary>
        /// 支付状态 0 未支付，1，已支付，2取消
        /// </summary>
        public int Status { get; set; }

        public string RealName { get; set; }

        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Card { get; set; }
        public string Grade { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 到管时间
        /// </summary>
        public DateTime? ServerTime { get; set; }
        

        
    }
}
