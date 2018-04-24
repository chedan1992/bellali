/*---------------------------------------------------------------- 
// @青果科技. All Rights Reserved. 版权所有。  
// 
// 文件名：设备巡检记录
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
    [Dapper.TableNameAttribute("Sys_AppointCheckNotes")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysAppointCheckNotes
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>		
        public string AppointId { get; set; }

        /// <summary>
        /// 单位id
        /// </summary>		
        public string CId { get; set; }

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

        /// <summary>
        /// Status -1：过期维修 0：设备正常，1：设备异常
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 巡检人
        /// </summary>
        [Dapper.ResultColumn]
        [Property(IsDataBaseField = false)]
        public string Responsible { get; set; }
    }
}