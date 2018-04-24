using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    //E_Elevator
    [Serializable]
    [Dapper.TableNameAttribute("Fire_FireBox")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModFireBox
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 箱子简码
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 箱子地址
        /// </summary>		
        public string Address { get; set; }
        /// <summary>
        /// 二维码编码
        /// </summary>
        public string QrCode { get; set; }
        /// <summary>
        /// Img
        /// </summary>		
        public string Img { get; set; }
        /// <summary>
        /// Status(0:没绑定,1：绑定)
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public string SysId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string CreaterId { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public int? EquipmentCount { get; set; }
    }
}
