using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysUser : IBaseDAL<ModSysUser>
    {
         /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModSysUser GetModelByWhere(string where);
        
        /// <summary>
        /// 修改用户登录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateLogin(ModSysUser model);

         /// <summary>
        /// 修改用户头像信息
        /// </summary>
        /// <param name="HeadImg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UpdateHeadImg(string HeadImg, string userId);

         /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateUser(string Userid, string Column);

           /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int UpdateStatus(int flag, string key);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="Id">用户主键</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        int ReSetPwd(string Id, string Pwd);

         /// <summary>
        /// 获得系统用户树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        DataSet GetTreeList(string strWhere);

         /// <summary>
        /// 获取推送用户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        List<ModPushUserView> GetPushUserList(string userList);

         /// <summary>
        /// 根据通讯录查询不是Iwill用户
        /// </summary>
        /// <param name="list"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataSet GetIwillUser(string list, string userId);

         /// <summary>
        /// 查询用户详细信息
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModSysUser GetUserByTel(string Tel);
           /// <summary>
        /// 查询用户详细信息
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModSysUser GetUserById(string id);
    }
}
