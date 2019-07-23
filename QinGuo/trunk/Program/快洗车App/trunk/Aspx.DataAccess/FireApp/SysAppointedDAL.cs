using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using Dapper;
using QINGUO.Common;
using QINGUO.DataAccessBase;
using System.Data;

namespace QINGUO.DAL
{
    public class SysAppointedDAL : BaseDAL<ModSysAppointed>, ISysAppointed
    {

        /// <summary>
        /// 根据单位统计设备信息 已过期的数量（红色），异常状态设备数量（黄色），半年内即将过期的数量（蓝色），其他正常数量（绿色）
        /// </summary>
        /// <returns></returns>
        public DataSet ChartAppointed(string sysId)
        {
            StringBuilder sb = new StringBuilder();
            //已过期的数量
            sb.Append("Select (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and Cid='" + sysId + "' and MaintenanceStatus=-1) as Count1, ");
            //异常状态设备数量
            sb.Append(" (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and Cid='" + sysId + "' and MaintenanceStatus=1) as Count2,");
            //半年内即将过期的数量（蓝色）
            sb.Append(" (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and Cid='" + sysId + "' and DATEADD(mm,-6,LostTime)<getDate() and MaintenanceStatus=0 and MaintenanceStatus!=-1) as Count3,");
            //其他正常数量（绿色）
            sb.Append(" (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and Cid='" + sysId + "' and MaintenanceStatus=0) as Count4");

            return dabase.ExecuteDataSet(sb.ToString());
        }


         /// <summary>
        /// 单位设备过期数量统计
        /// </summary>
        public DataSet ChartPart()
        {
            string sql = "select Name,(select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and MaintenanceStatus=-1 and Cid=Sys_Company.Id) as NumCount from Sys_Company where Attribute=" + (int)CompanyType.单位 + " and Status>" + (int)StatusEnum.删除 + "";
            return dabase.ExecuteDataSet(sql.ToString());
        }
        /// <summary>
        /// 统计信息
        /// </summary>
        /// <returns></returns>
        public DataSet Total(string sysId)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(sysId))
            {
                sb.Append("Select (select COUNT(Id) from Sys_Master where IsMain =1 and (Status!=" + (int)StatusEnum.删除 + " and Status!=-2) and  Cid='" + sysId + "' and (Attribute>=" + (int)AdminTypeEnum.汽配商管理员 + " and Attribute<=" + (int)AdminTypeEnum.维修厂管理员 + ")) as UserCount, ");
                sb.Append(" (select count(Id) from Sys_Company where Attribute=" + (int)CompanyType.单位 + " and Status!=" + (int)StatusEnum.删除 + " and Id='" + sysId + "') as CompanyCount,");
                sb.Append(" (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and Cid='" + sysId + "') as EventCount,");
                sb.Append(" (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and MaintenanceStatus=-1 and Cid='" + sysId + "') as LostEvent,");
                sb.Append(" (select COUNT(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and MaintenanceDate<getDate() and Cid='" + sysId + "') as LostTime");//超期未巡检
            }
            else
            {
                sb.Append("Select (select COUNT(Id) from Sys_Master where IsMain =1 and (Status!=" + (int)StatusEnum.删除 + " and Status!=-2) and (Attribute>=" + (int)AdminTypeEnum.汽配商管理员 + " and Attribute<=" + (int)AdminTypeEnum.维修厂管理员 + ")) as UserCount, ");
                sb.Append(" (select count(Id) from Sys_Company where  Status!=" + (int)StatusEnum.删除 + " and Attribute=" + (int)CompanyType.单位 + ") as CompanyCount,");
                sb.Append(" (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + ") as EventCount,");
                sb.Append(" (select count(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and MaintenanceStatus=-1) as LostEvent,");
                sb.Append(" (select COUNT(Id) from View_Appointed where Status!=" + (int)StatusEnum.删除 + " and MaintenanceDate<getDate()) as LostTime");
            }
            return dabase.ExecuteDataSet(sb.ToString());
        }
        /// <summary>
        /// 图表统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet ChartTotla(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select YEAR(CreateTime)as '年份',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=1 " + where + " ) as '1月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=2 " + where + " )  as '2月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=3 " + where + " ) as '3月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=4 " + where + " ) as '4月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=5 " + where + " ) as '5月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=6 " + where + " )  as '6月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=7 " + where + " ) as '7月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=8 " + where + " )  as '8月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=9 " + where + " ) as '9月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=10 " + where + " )  as '10月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=11 " + where + " ) as '11月',");
            sb.Append(" (select COUNT(Id) from Sys_AppointCheckNotes where MONTH(CreateTime)=12 " + where + " ) as '12月'");
            sb.Append(" From Sys_AppointCheckNotes where YEAR(CreateTime)=YEAR(GETDATE())");
            if (!string.IsNullOrEmpty(where))
            {
                sb.Append( where);
            }
            sb.Append(" Group By YEAR(CreateTime)");

            return dabase.ExecuteDataSet(sb.ToString());
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Sys_Appointed where Status!=" + (int)StatusEnum.删除 + "");
            sb.Append(" and " + where);
            List<ModSysAppointed> list = dabase.ReadDataBase.Query<ModSysAppointed>(sb.ToString()).ToList();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysAppointed GetModelByWhere(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from View_Appointed");
            sb.Append(" where 1=1 " + where);

            return dabase.ReadDataBase.SingleOrDefault<ModSysAppointed>(sb.ToString());
        }
        /// <summary>
        /// 查询List实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ModSysAppointed> GetListByWhere(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from View_Appointed");
            sb.Append(" where 1=1 " + where);
            return dabase.ReadDataBase.Query<ModSysAppointed>(sb.ToString()).ToList();
        }
        /// <summary>
        /// 修改设备状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateStatue(int status, string Id)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Status", status);
            paras.Add("@Id", Id);
            int result = 0;
            if (status == (int)StatusEnum.禁用)
            {
                result = dabase.ExecuteNonQueryByText("update Sys_Appointed set Status=@Status where Id=@Id", paras);
            }
            else
            {
                result = dabase.ExecuteNonQueryByText("update Sys_Appointed set Status=@Status where Id ='" + Id + "'", paras);
            }
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ModSysAppointed GetAppointDetailQRCode(string QRCode)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModSysAppointed>("select top(1) * from View_Appointed where (QRName=@0 or 'XF'+QRCode+'S'=@1) order by CreateTime desc", QRCode, QRCode);
        }

        public ModSysAppointed GetAppointDetailId(string Id)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModSysAppointed>("select * from View_Appointed where Id=@0 and status=1", Id);
        }

        public Page<ModSysAppointed> GetAppointList(Search search)
        {
            return dabase.ReadDataBase.Page<ModSysAppointed>(search.CurrentPageIndex, search.PageSize, search.SqlString2);
        }


        /// <summary>
        /// 获取推送巡检设备列表信息
        /// </summary>
        /// <returns></returns>
        public List<ModSysAppointed> GetMaintenancePush()
        {
            return dabase.ReadDataBase.Query<ModSysAppointed>("select * from Sys_Appointed where datediff(day,GETDATE(),MaintenanceDate) = 1 and status=1 ").ToList();
        }


        //根据位置id查询设备列表
        public List<ModSysAppointed> GetByPlacesList(string Places)
        {
            return dabase.ReadDataBase.Query<ModSysAppointed>("select * from View_Appointed where Places=@0 and status=1", Places).ToList();
        }
    }
}
