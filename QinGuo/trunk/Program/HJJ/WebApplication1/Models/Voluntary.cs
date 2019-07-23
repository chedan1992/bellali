using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Commonlication
{
    [Serializable]
    public class Voluntary
    {
        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 居住地
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 0:男 1:女
        /// </summary>
        public int Sex { get; set; }


        /// <summary>
        /// 省份证
        /// </summary>
        public string Card { get; set; }

        /// <summary>
        /// 服务时长
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        public string LinkPhone { get; set; }

        /// <summary>
        /// 是否团体
        /// </summary>
        public int IsT { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}
