/*---------------------------------------------------------------- 
// @青果科技. All Rights Reserved. 版权所有。  
// 
// 文件名：品牌管理
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
namespace QINGUO.Model
{

    //E_Elevator
    [Serializable]
    [Dapper.TableNameAttribute("Sys_QRCode")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysQRCode
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
        /// 分组名称
        /// </summary>		
        public string GroupName { get; set; }
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
        /// 总平台管理
        /// </summary>
        public string SysId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string CreaterId { get; set; }
    }
}