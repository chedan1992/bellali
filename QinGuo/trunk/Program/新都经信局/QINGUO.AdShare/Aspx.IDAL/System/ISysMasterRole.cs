using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysMasterRole : IBaseDAL<ModSysMasterRole>
    {
        /// <summary>
        /// 根据管理员编号获取它的角色信息
        /// </summary>
        /// <returns></returns>
        DataSet GetMasterRole(string masterId);

        /// <summary>
        /// 获取角色根据公司编号
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        DataSet GetRoleByCompanyID(string companyID);


        /// <summary>
        /// 获取用户的
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="masterID"></param>
        /// <returns></returns>
        DataSet GetMasterRoleByCompanyID(string companyID, string masterID);


        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        bool DeleteMasterRole(string MasterID);

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        bool AddMasterRole(string ID, string MasterID, string roleID);

        /// <summary>
        /// 根据角色编号删除管理员相关的角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        bool DeleteMasterRoleByRoleID(string roleID);


        /// <summary>
        /// 获取角色所在的用户的个数
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        bool GetCountByRoleId(string roleID);

        /// <summary>
        /// 删除管理员角色信息
        /// </summary>
        /// <param name="MasterId">管理员编号</param>
        /// <returns></returns>
        int DeleteAllByMasterId(string MasterId);
    }
}
