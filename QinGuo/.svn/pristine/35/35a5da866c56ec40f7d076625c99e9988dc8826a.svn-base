using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Model;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysRole : BllBase<ModSysRole>
    {
        ISysRole DAL = CreateDalFactory.CreateDalSysRole();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取角色分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"view_Sys_Role";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc,RoleSort asc";//排序
            return base.QueryPageToJson(search);
        }


        /// <summary>
        /// 查询角色列表信息
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAllRole(string where)
        {
            return DAL.GetAllRole(where);
        }

        /// <summary>
        /// 获取选中的角色列表
        /// </summary>
        /// <param name="masterId">管理员编号</param>
        /// <returns></returns>
        public DataSet GetCheckRoleList(string masterId)
        {
            return DAL.GetCheckRoleList(masterId);
        }

        /// <summary>
        /// 角色下创建的其它角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        public DataSet SearchClidRole(string roleId)
        {
            return DAL.SearchClidRole(roleId);
        }

        /// <summary>
        /// 删除总角色下子角色不存在的链接树
        /// </summary>
        /// <param name="PraentRoleId"></param>
        /// <param name="ChildroleId"></param>
        /// <returns></returns>
        public int DelteClidRole(string PraentRoleId, string ChildroleId)
        {
            return DAL.DelteClidRole(PraentRoleId, ChildroleId);
        }

        /// <summary>
        /// 删除角色下拥有的管理员的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public int DeleteRoleInMaster(string roleId)
        {
            return DAL.DeleteRoleInMaster(roleId);
        }

         /// <summary>
        /// 获取用户模块下的隐藏列信息
       /// </summary>
       /// <param name="moudelId"></param>
       /// <param name="UserId"></param>
       /// <returns></returns>
        public DataSet GetGridColumn(string moudelId, string UserId)
        {
            return DAL.GetGridColumn(moudelId, UserId);
        }
    }
}
