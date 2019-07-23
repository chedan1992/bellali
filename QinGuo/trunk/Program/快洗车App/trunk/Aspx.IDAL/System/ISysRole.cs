using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysRole : IBaseDAL<ModSysRole>
    {
        /// <summary>
        /// 查询角色列表信息
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        DataSet GetAllRole(string where);

        /// <summary>
        /// 获取选中的角色列表
        /// </summary>
        /// <param name="masterId">管理员编号</param>
        /// <returns></returns>
        DataSet GetCheckRoleList(string masterId);

        /// <summary>
        /// 根据主键进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteRoleById(string id);

        /// <summary>
        /// 角色下创建的其它角色
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        DataSet SearchClidRole(string roleId);

         /// <summary>
        /// 删除总角色下子角色不存在的链接树
        /// </summary>
        /// <param name="PraentRoleId"></param>
        /// <param name="ChildroleId"></param>
        /// <returns></returns>
        int DelteClidRole(string PraentRoleId, string ChildroleId);

        /// <summary>
        /// 删除角色下拥有的管理员的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int DeleteRoleInMaster(string roleId);

        /// <summary>
        /// 根据角色.获取角色下的管理员
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="sb"></param>
        void GetMasterRole(string roleId, ref StringBuilder sb);
        /// <summary>
        /// 获取用户模块下的隐藏列信息
        /// </summary>
        /// <param name="moudelId">模块ID</param>
        /// <param name="UserId">用户主键</param>
        /// <returns></returns>
        DataSet GetGridColumn(string moudelId, string UserId);
    }
}
