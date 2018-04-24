using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using QINGUO.Model;
using QINGUO.Common;
using QINGUO.ViewModel;
using QINGUO.Factory;
using QINGUO.IDAL;

namespace QINGUO.Business
{
    /// <summary>
    /// 权限设置--功能权限操作类
    /// </summary>
    public class BllSysRoleFun : BllBase<ModSysRoleFun>
    {
        private ISysRoleFun Dal = CreateDalFactory.CreateDalSysRoleFun();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = Dal;
        }


        /// <summary>
        /// 返回插入角色信息数据库
        /// </summary>
        /// <returns></returns>
        public string InsertRole(ModSysRoleFun model)
        {
            return Dal.InsertRole(model);
        }

        /// <summary>
        /// 保存用户角色菜单信息
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="sql">插入sql</param>
        /// <returns></returns>
        public int InsertRoleByRoleId(string roleId, string sql)
        {
            return Dal.InsertRoleByRoleId(roleId, sql);
        }


        /// <summary>
        /// 查询已选中的菜单节点
        /// </summary>
        /// <param name="funId">菜单主键</param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetSelectFun(string funId, string roleId)
        {
            return Dal.GetSelectFun(funId, roleId);
        }


        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddSysRoleFun(ModSysRoleFun model)
        {
            return Dal.AddSysRoleFun(model);
        }


        /// <summary>
        /// 获取子权限根据角色Id和权限父Id
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="parentFunID"></param>
        /// <returns></returns>
        public List<JsonRoleFunZtree> GetChildFunByRoleID(string roleID, string parentFunID, string companyID, bool isMultilRoleString = false)
        {
            DataSet ds = Dal.GetChildFunByRoleID(roleID, parentFunID, companyID, isMultilRoleString);
            List<JsonRoleFunZtree> jsonTree = FillModelList(ds);
            if (jsonTree != null)
            {
                foreach (var item in jsonTree)
                {
                    List<JsonRoleFunZtree> childList = GetChildFunByRoleID(roleID, item.id, companyID);
                    item.ParentID = parentFunID;
                    item.children = childList;
                }

            }
            return jsonTree;
        }

        /// <summary>
        /// 填充数据实体
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public List<JsonRoleFunZtree> FillModelList(DataSet ds)
        {
            List<JsonRoleFunZtree> list = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    list = new List<JsonRoleFunZtree>();
                    foreach (DataRow row in dt.Rows)
                    {
                        JsonRoleFunZtree ztree = new JsonRoleFunZtree();
                        ztree.id = row["Id"].ToString();
                        ztree.name = row["FunName"].ToString();
                        ztree.PageURL = row["PageUrl"].ToString();
                        ztree.IconClass = row["iconCls"].ToString();
                        list.Add(ztree);
                    }
                }

            }
            return list;
        }


        /// <summary>
        /// 删除角色所有的权限
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="cid"></param>
        /// <returns></returns>
        public bool DeleteRoleFun(string roleID, string cid)
        {
            return Dal.DeleteRoleFun(roleID, cid);
        }


        /// <summary>
        /// 删除角色权限,根据权限编号和公司编号
        /// </summary>
        /// <returns></returns>
        public bool DeleteRoleFunByFunIDCID(string funID, string Cid)
        {
            return Dal.DeleteRoleFunByFunIDCID(funID, Cid);
        }
        /// <summary>
        /// 获取角色权限根据权限Id和公司编号
        /// </summary>
        /// <returns></returns>
        public int GetRoleByFunIDCID(string funID, string Cid)
        {
            return Dal.GetRoleByFunIDCID(funID, Cid);
        }

        /// <summary>
        /// 根据公司id查询所有funID
        /// </summary>
        /// <param name="cid"></param>
        public List<ModSysRoleFun> GetFunIdByCID(string cid)
        {
            List<ModSysRoleFun> list = CommonFunction.DataSetToList<ModSysRoleFun>(Dal.GetFunIdByCID(cid), 0);
            return list;
        }

        /// <summary>
        /// 根据局角色和公司cid查询功能权限（公司接口端使用）
        /// </summary>
        /// <returns></returns>
        public List<ModSysRoleFun> GetRoleCidByList(string rold,string Cid) {
            return Dal.GetRoleCidByList(rold, Cid);
        }
    }
}
