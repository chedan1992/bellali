using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysRoleFun : IBaseDAL<ModSysRoleFun>
    {
        /// <summary>
        /// 返回插入角色信息数据库
        /// </summary>
        /// <returns></returns>
        string InsertRole(ModSysRoleFun model);

        /// <summary>
        /// 保存用户角色菜单信息
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="sql">插入sql</param>
        /// <returns></returns>
        int InsertRoleByRoleId(string roleId, string sql);
        /// <summary>
        /// 查询已选中的菜单节点
        /// </summary>
        /// <param name="funId">菜单主键</param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        DataTable GetSelectFun(string funId, string roleId);

        /// <summary>
        /// 获取权限根据角色ID和父权限为0
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        DataSet GetFunByRoleIDParentIDZero(string roleID, string companyID, bool isMultilRoleString = false);
        /// <summary>
        /// 获取子权限根据角色ID和权限父ID
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="parentFunID"></param>
        /// <returns></returns>
        DataSet GetChildFunByRoleID(string roleID, string parentFunID, string companyID, bool isMultilRoleString = false);


        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddSysRoleFun(ModSysRoleFun model);


        /// <summary>
        /// 删除角色所有的权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        bool DeleteRoleFun(string roleID, string cid);

        /// <summary>
        /// 获取角色权限根据角色编号和权限编号
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="Cid"></param>
        /// <returns></returns>
        bool GetFunByRoleID(string roleID, string Cid);


        /// <summary>
        /// 删除角色权限,根据权限编号和公司编号
        /// </summary>
        /// <returns></returns>
        bool DeleteRoleFunByFunIDCID(string funID, string Cid);


        /// <summary>
        /// 获取角色权限根据权限ID和公司编号
        /// </summary>
        /// <returns></returns>
        int GetRoleByFunIDCID(string funID, string Cid);
        /// <summary>
        /// 根据公司id查询所有funID
        /// </summary>
        /// <param name="cid"></param>
        DataSet GetFunIdByCID(string cid);
        /// <summary>
        /// 根据局角色和商家cid查询功能权限（商家接口端使用）
        /// </summary>
        /// <returns></returns>
        List<ModSysRoleFun> GetRoleCidByList(string rold, string Cid);
    }
}
