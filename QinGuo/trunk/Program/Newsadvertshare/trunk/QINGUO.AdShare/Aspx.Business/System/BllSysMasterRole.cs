using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;
using QINGUO.Common;

namespace QINGUO.Business
{
    /// <summary>
    /// 管理员角色类
    /// </summary>
    public class BllSysMasterRole : BllBase<ModSysMasterRole>
    {
        ISysMasterRole dal = CreateDalFactory.CreateDalSysMasterRole();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        /// <summary>
        /// 删除管理员角色信息
        /// </summary>
        /// <param name="MasterId">管理员编号</param>
        /// <returns></returns>
        public int DeleteAllByMasterId(string MasterId)
        {
            return dal.DeleteAllByMasterId(MasterId);
        }

        /// <summary>
        /// 根据管理员编号获取它的角色信息
        /// </summary>
        /// <returns></returns>
        public string GetMasterRole(string masterId)
        {
            DataSet ds = dal.GetMasterRole(masterId);
            string result = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                result += "'" + ds.Tables[0].Rows[i]["RoleId"] + "',";
            }
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }

        /// <summary>
        /// 获取角色根据公司编号
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<ModSysRole> GetRoleByCompanyID(string companyID)
        {
            List<ModSysRole> roleList = null;
            DataSet ds = dal.GetRoleByCompanyID(companyID);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    ModelHandler<ModSysRole> handler = new ModelHandler<ModSysRole>();
                    roleList = handler.FillModel(dt);
                }
            }
            return roleList;

        }


        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        public bool DeleteMasterRole(string MasterID)
        {
            return dal.DeleteMasterRole(MasterID);
        }

        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="MasterID"></param>
        /// <returns></returns>
        public bool AddMasterRole(ModSysMasterRole masterRole)
        {
            return dal.AddMasterRole(masterRole.Id, masterRole.MasterId, masterRole.RoleId);
        }

        /// <summary>
        /// 根据角色编号删除管理员相关的角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool DeleteMasterRoleByRoleID(string roleID)
        {
            return dal.DeleteMasterRoleByRoleID(roleID);
        }
    }
}
