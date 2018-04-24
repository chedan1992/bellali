using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.ViewModel;
using System.Data;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysUser : BllBase<ModSysUser>
    {
        private ISysUser DAL = CreateDalFactory.CreateDalSysUser();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取管理员分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"Sys_User"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.AddCondition("Status!=" + (int)StatusEnum.删除); //过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "Status asc ,CreateTime desc";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysUser GetModelByWhere(string where)
        {
            return DAL.GetModelByWhere(where);
        }


        /// <summary>
        /// 修改用户登录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLogin(ModSysUser model)
        {
            return DAL.UpdateLogin(model);
        }

        /// <summary>
        /// 修改用户头像信息
        /// </summary>
        /// <param name="HeadImg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateHeadImg(string HeadImg, string userId)
        {
            return DAL.UpdateHeadImg(HeadImg, userId);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(string Userid, string Column)
        {
            return DAL.UpdateUser(Userid, Column);
        }

           /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            return DAL.UpdateStatus(flag, key);
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="Id">用户主键</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        public int ReSetPwd(string Id, string Pwd)
        {
            return DAL.ReSetPwd(Id, Pwd);
        }

         /// <summary>
        /// 获得系统用户树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            return DAL.GetTreeList(strWhere);
        }

        /// <summary>
        /// 获取推送用户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public List<ModPushUserView> GetPushUserList(string userList)
        {
            return DAL.GetPushUserList(userList);
        }

        /// <summary>
        /// 根据通讯录查询不是Iwill用户
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataSet GetIwillUser(string list, string userId)
        {
            return DAL.GetIwillUser(list, userId);
        }

         /// <summary>
        /// 查询用户详细信息
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysUser GetUserByTel(string Tel)
        {
            return DAL.GetUserByTel(Tel);
        }

           /// <summary>
        /// 查询用户详细信息
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysUser GetUserById(string id)
        {
            return DAL.GetUserById(id);
        }

    }
}
