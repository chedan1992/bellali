using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    public class ModChartElevatorBrandView
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }


        /// <summary>
        /// Img
        /// </summary>		
        public string Img { get; set; }

        /// <summary>
        /// Status
        /// </summary>		
        public int Status { get; set; }

        /// <summary>
        /// 总平台管理
        /// </summary>
        public string SysId { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 电梯数量
        /// </summary>
        public int CountNum { get; set; }
    }
}
