/*---------------------------------------------------------------- 
// @青果科技. All Rights Reserved. 版权所有。  
// 
// 文件名：设备管理
// 文件功能描述：  
// 
// 创建人： zj
// 创建日期： 2016-2-7
// 
// 修改标识： 
// 修改描述： 
// 
// 修改标识： 
// 修改描述： 
//----------------------------------------------------------------*/
using System;
using QINGUO.Common;
namespace QINGUO.Model
{

    //Sys_Appointed
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Appointed")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysAppointed
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
        /// 型号
        /// </summary>		
        public string Model { get; set; }
        /// <summary>
        /// 规格
        /// </summary>		
        public string Specifications { get; set; }
        /// <summary>
        /// 设备箱子编码
        /// </summary>		
        public string Places { get; set; }

        /// <summary>
        /// 设备位置
        /// </summary>		
        public string Placesed { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>		
        public string Gid { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>		
        public DateTime ? ProductionDate { get; set; }

        /// <summary>
        /// 维修日期
        /// </summary>		
        public DateTime ? MaintenanceDate { get; set; }

        /// <summary>
        /// 维修间隔天数
        /// </summary>		
        public int MaintenanceDay { get; set; }
        /// <summary>
        /// 设备过期时间
        /// </summary>		
        public DateTime? LostTime { get; set; }
        /// <summary>
        /// 责任人id
        /// </summary>		
        public string ResponsibleId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>		
        public string Mark { get; set; }

        /// <summary>
        /// 二维码编码
        /// </summary>		
        public string QRCode { get; set; }
        /// <summary>
        /// 二维码查询码
        /// </summary>		
        public string QrName { get; set; }
        /// <summary>
        /// 设备数量
        /// </summary>		
        public int StoreNum { get; set; }
     
        /// <summary>
        /// Img
        /// </summary>		
        public string Img { get; set; }
        /// <summary>
        /// Status
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 单位id
        /// </summary>
        public string Cid { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime ? CreateTime { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string CreaterId { get; set; }

        /// <summary>
        /// 巡检状态，-1:设备过期  0:设备正常,1:设备异常
        /// </summary>
        public int MaintenanceStatus { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string GroupName { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string Responsible { get; set; }


        /// <summary>
        /// 单位名称
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string Cname { get; set; }

        /// <summary>
        /// 设备位置查询简码
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string PlacesCode { get; set; }
        /// <summary>
        /// 设备位置地址
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string PlacesName { get; set; }

    }
}