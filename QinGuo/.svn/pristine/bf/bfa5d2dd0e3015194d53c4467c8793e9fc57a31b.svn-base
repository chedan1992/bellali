using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Common;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.ViewModel;
using Dapper;

namespace QINGUO.Business
{
    public class BllSysAppointed : BllBase<ModSysAppointed>
    {
        ISysAppointed dal = CreateDalFactory.CreateDalSysAppointed();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }
         /// <summary>
        /// 根据单位统计设备信息 已过期的数量（红色），异常状态设备数量（黄色），半年内即将过期的数量（蓝色），其他正常数量（绿色）
        /// </summary>
        /// <returns></returns>
        public DataSet ChartAppointed(string sysId)
        {
            return dal.ChartAppointed(sysId);
        }
         /// <summary>
        /// 单位设备过期数量统计
        /// </summary>
        public DataSet ChartPart()
        {
            return dal.ChartPart();
        }
          /// <summary>
        /// 统计信息
        /// </summary>
        /// <returns></returns>
        public DataSet Total(string sysId)
        {
            return dal.Total(sysId);
        }
         /// <summary>
        /// 图表统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet ChartTotla(string where)
        {
            return dal.ChartTotla(where);
        }
         /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            return dal.Exists(where);
        }
         /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysAppointed GetModelByWhere(string where)
        {
            return dal.GetModelByWhere(where);
        }
        /// <summary>
        /// 查询List实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ModSysAppointed> GetListByWhere(string where)
        {
            return dal.GetListByWhere(where);
        }
         /// <summary>
        /// 修改设备状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateStatue(int status, string Id)
        {
            return dal.UpdateStatue(status, Id);
        }
        /// <summary>
        /// 根据二维码查询
        /// </summary>
        /// <param name="QRCode"></param>
        /// <returns></returns>
        public ModSysAppointed GetAppointDetailQRCode(string QRCode)
        {
            return dal.GetAppointDetailQRCode(QRCode);
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="QRCode"></param>
        /// <returns></returns>
        public ModSysAppointed GetAppointDetailId(string Id)
        {
            return dal.GetAppointDetailId(Id);
        }

        /// <summary>
        /// 分页查询设备
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModSysAppointed> GetAppointList(Search search)
        {
            search.TableName = @"View_Appointed";//视图
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "(CASE MaintenanceStatus WHEN 0 THEN 2 ELSE MaintenanceStatus END ) asc,CreateTime desc";//排序
            //search.SelectedColums = "Id,Name,ReadNum,Mark,Img,CreateTime";
            return dal.GetAppointList(search);
        }

        /// <summary>
        /// 后台分页查询设备
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_Appointed";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.AddCondition("Status!=" + (int)StatusEnum.删除);//过滤条件
            search.StatusDefaultCondition = "";
            //search.SortField = "CreateTime desc";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 获取推送巡检设备列表信息
        /// </summary>
        /// <returns></returns>
        public List<ModSysAppointed> GetMaintenancePush()
        {
            return dal.GetMaintenancePush();
        }

        //根据位置id查询设备列表
        public List<ModSysAppointed> GetByPlacesList(string Places)
        {
            return dal.GetByPlacesList(Places);
        }
    }
}
