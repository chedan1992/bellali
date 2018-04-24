using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterFaceWeb
{
    public class jsonAppointCheckNotes
    {
        /// <summary>
        /// 巡检id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>		
        public string AppointId { get; set; }

        /// <summary>
        /// 巡检人
        /// </summary>		
        public string ResponsibleId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>		
        public string Mark { get; set; }

        /// <summary>
        /// Img巡检拍照
        /// </summary>		
        public string Img { get; set; }

        /// </summary>		
        public string Path { get; set; }

        /// <summary>
        /// Status 1外观正常，2状态正常，3有效期内（自动判断），4其他
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 巡检人
        /// </summary>
        public string Responsible { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 型号
        /// </summary>		
        public string Model { get; set; }
        /// <summary>
        /// 规格
        /// </summary>		
        public string Specifications { get; set; }
        /// <summary>
        /// 位置
        /// </summary>		
        public string Places { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>		
        public int StoreNum { get; set; }

    }
}