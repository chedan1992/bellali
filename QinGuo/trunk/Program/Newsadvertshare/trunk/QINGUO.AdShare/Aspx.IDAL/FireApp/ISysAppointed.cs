using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysAppointed : IBaseDAL<ModSysAppointed>
    {
         /// <summary>
        /// 根据单位统计设备信息 已过期的数量（红色），异常状态设备数量（黄色），半年内即将过期的数量（蓝色），其他正常数量（绿色）
        /// </summary>
        /// <returns></returns>
        DataSet ChartAppointed(string sysId);
         /// <summary>
        /// 单位设备过期数量统计
        /// </summary>
        DataSet ChartPart();
          /// <summary>
        /// 统计信息
        /// </summary>
        /// <returns></returns>
        DataSet Total(string sysId);
         /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Exists(string where);
         /// <summary>
        /// 图表统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataSet ChartTotla(string where);
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModSysAppointed GetModelByWhere(string where);
        /// <summary>
        /// 查询List实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ModSysAppointed> GetListByWhere(string where);
         /// <summary>
        /// 修改设备状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        bool UpdateStatue(int status, string Id);
        ModSysAppointed GetAppointDetailQRCode(string QRCode);
        ModSysAppointed GetAppointDetailId(string Id);

        /// <summary>
        /// 分页查询设备
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Dapper.Page<ModSysAppointed> GetAppointList(Common.Search search);

        /// <summary>
        /// 获取推送巡检设备列表信息
        /// </summary>
        /// <returns></returns>
        List<ModSysAppointed> GetMaintenancePush();

        List<ModSysAppointed> GetByPlacesList(string Places);
    }
}
