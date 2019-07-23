using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using Dapper;
using QINGUO.Common;

namespace QINGUO.DAL
{
    public class SysFlowDAL : BaseDAL<ModSysFlow>, ISysFlow
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<ModSysFlow> getListAll(int? top, string where)
        {
            if (top > 0)
            {
                string sql = "select top " + top + " * from Sys_Flow where FlowStatus=" + (int)StatusEnum.禁用 + where + " order by ApprovalTime desc";
                return dabase.ReadDataBase.Query<ModSysFlow>(sql).ToList();
            }
            else
            {
                string sql = "select * from Sys_Flow where FlowStatus=" + (int)StatusEnum.禁用 + where + " order by ApprovalTime desc";
                return dabase.ReadDataBase.Query<ModSysFlow>(sql).ToList();
            }

        }

        /// <summary>
        /// 查询是否在审核中
        /// </summary>
        /// <param name="FlowType"></param>
        /// <param name="FlowStatus"></param>
        /// <param name="AppointId"></param>
        /// <returns></returns>
        public bool Exists(int FlowType, int FlowStatus, string AppointId)
        {
            return dabase.ReadDataBase.ExecuteScalar<int>("select count(id) from Sys_Flow where FlowType=@0 and FlowStatus=@1 and MasterId=@2", FlowType, FlowStatus, AppointId) > 0;
        }


        /// <summary>
        /// 分页查询审核列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModSysFlow> GetFlowList(Search search)
        {
            return dabase.ReadDataBase.Page<ModSysFlow>(search.CurrentPageIndex, search.PageSize, search.SqlString2);
        }


        /// <summary>
        /// 待办消息数
        /// </summary>
        /// <param name="sysMaster"></param>
        /// <returns></returns>
        public int flowmsg(ModSysMaster sysMaster)
        {
            string sql = "select count(1) from Sys_Flow where 1=1 ";
            int type = sysMaster.Attribute;
            //系统管理员 查看所有
            if (type == (int)AdminTypeEnum.汽配商管理员 || type == (int)AdminTypeEnum.维修厂管理员)
            {
                sql += " and CompanyId='" + sysMaster.Cid + "'";
            }

            if (type == (int)AdminTypeEnum.系统管理员 || type == (int)AdminTypeEnum.汽配商管理员 || type == (int)AdminTypeEnum.维修厂管理员 )
            {
                sql += " and FlowStatus=0";
            }
            return dabase.ReadDataBase.ExecuteScalar<int>(sql);
        }

    }
}
