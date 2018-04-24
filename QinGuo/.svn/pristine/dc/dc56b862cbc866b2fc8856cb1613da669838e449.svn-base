using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysMaster : IBaseDAL<ModSysMaster>
    {
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Exists(string where);
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModSysMaster GetModelByWhere(string where);
        /// <summary>
        /// 修改用户登录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateLogin(ModSysMaster model);

        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateMaster(string UserId, string Column);

        /// <summary>
        /// 查询能接受推送的商户
        /// </summary>
        /// <param name="CategoryId">商户选择的分类</param>
        /// <returns></returns>
        List<ModPushMsgCompany> GetPushCompany(string CategoryId);

        /// <summary>
        /// 获取推送商户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        List<ModPushUserView> GetPushMasterList(string CategoryId);
        /// <summary>
        /// 根据主键进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteSysMasterById(string id);
        /// <summary>
        /// 验证后台用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        DataSet ValidateLogin(string userName, string userPwd);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="Id">用户主键</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        int ReSetPwd(string Id, string Pwd);
        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <param name="key">用户主键</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        int upPwd(string key, string userPwd);
        /// <summary>
        /// 登录成功,设置登录信息
        /// </summary>
        /// <param name="ip">用户ip地址</param>
        /// <param name="key">用户主键</param>
        /// <returns></returns>
        int upLoginInfo(string ip, string key);
       
        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int UpdateStatus(int flag, string key);
        /// <summary>
        /// 根据编号,设置管理员下的管理员信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="sb"></param>
        void SetStatusClose(string ID, ref StringBuilder sb);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        int UpdateDate(ModSysMaster t);
        /// <summary>
        /// 页面按钮权限
        /// </summary>
        /// <param name="className">页面操作类</param>
        /// <returns></returns>
        DataSet GetPageBtns(string className, int TypeID);

        /// <summary>
        /// 获取公司超级管理员信息
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        ModSysMaster CompanyMaster(string cid);

        /// <summary>
        /// 获取角色访问模块树
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        DataSet GetAuthByPage(string roleId, string where);


        /// <summary>
        /// 获取页面角色按钮
        /// </summary>
        /// <param name="className">页面模块名称</param>
        /// <param name="roleId">用户角色</param>
        /// <param name="Attribute">用户类型</param>
        /// <returns></returns>
        DataSet GetAuthByBtn(string className, string roleId, int Attribute);

        /// <summary>
        /// 查询页面用户访问类型
        /// </summary>
        /// <param name="className">页面类名称</param>
        /// <param name="roleIdList">用户角色List</param>
        /// <param name="Attr">公司类型</param>
        /// <returns></returns>
        int GetLookPower(string className, string roleIdList, int Attr);


        /// <summary>
        /// 根据店铺ID和用户名进行用户名判断
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        int GetMasterExists(string UserName, string companyID);


        /// <summary>
        /// 修改用户名是否存在相同
        /// </summary>
        /// <param name="CurrentUserName"></param>
        /// <param name="UpdateLoginName"></param>
        /// <returns></returns>
        int UpdateExists(string CurrentUserName, string UpdateLoginName);

         /// <summary>
        /// 获得系统用户树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        DataSet GetTreeList(string strWhere);

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
        /// <param name="Userid"></param>
        /// <param name="Column"></param>
        /// <returns></returns>
        bool UpdateUser(string Userid, string Column);
        /// <summary>
        /// 收入排行榜
        /// </summary>
        /// <returns></returns>
        List<JsonRevenueRanking> RevenueRanking();

        /// <summary>
        /// 工程师排行榜
        /// </summary>
        /// <returns></returns>
        List<JsonRevenueRanking> EngineerRanking();

        List<ModSysMaster> GetListByAttribute(int attribut);

        List<ModSysMaster> range(string CustomerId, string range);

        Dapper.Page<ModSysMaster> Search(Common.Search search);

        List<ModSysMaster> GetOrganizaIdByList(string Cid);

        List<ModSysMaster> GetCIdByList(string Cid);
    }
}
