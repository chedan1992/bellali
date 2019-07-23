using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Commonlication
{
    [Serializable]
    public class SysMenu
    {
        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public int PId { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}
