using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.ViewModel;
using System.Web.Security;
using System.Web;
using System.Data;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysMaster : BllBase<ModSysMaster>
    {
        private ISysMaster DAL = CreateDalFactory.CreateSysMaster();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            return DAL.Exists(where);
        }
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysMaster GetModelByWhere(string where)
        {
            return DAL.GetModelByWhere(where);
        }

        /// <summary>
        /// 修改用户登录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLogin(ModSysMaster model)
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
        /// 修改商户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMaster(string UserId, string Column)
        {
            return DAL.UpdateMaster(UserId, Column);
        }

        /// <summary>
        /// 查询能接受推送的商户
        /// </summary>
        /// <param name="CategoryId">商户选择的分类</param>
        /// <returns></returns>
        public List<ModPushMsgCompany> GetPushCompany(string CategoryId)
        {
            return DAL.GetPushCompany(CategoryId);
        }

        /// <summary>
        /// 获取推送商户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public List<ModPushUserView> GetPushMasterList(string CategoryId)
        {
            return DAL.GetPushMasterList(CategoryId);
        }




        /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public ModSysMaster Get(string primaryKey)
        {
            return base.LoadData(primaryKey);
        }

        /// <summary>
        /// 根据用户编号,获取用户详细信息
        /// </summary>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public ModSysMaster LoadMasterWithPower(string masterID)
        {
            var master = LoadData(masterID);
            LoadMasterPower(master);
            return master;
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
        /// 根据用户编号,获取用户详细信息
        /// </summary>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public override ModSysMaster LoadData(object masterID)
        {
            var dic = GetMasterDic();
            var p = dic.Values.Where(c => c.Id == masterID.ToString()).ToList();
            return p.Any() ? p.First() : null;
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
        /// 根据主键进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteSysMasterById(string id)
        {
            return DAL.DeleteSysMasterById(id) > 0;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, ModSysMaster> GetMasterDic()
        {
            DataSet ds = base.GetList("v_Master_Company", " and Status!=" + (int)StatusEnum.删除 + "  and CompanyStatus!=" + (int)StatusEnum.删除, "", 0);
            var list = CommonFunction.DataSetToList<ModSysMaster>(ds, 0).Where(c => !string.IsNullOrEmpty(c.LoginName));// base.QueryToAll();
            var dic = list.ToDictionary(c => c.Id, c => c);
            return dic;
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="master"></param>
        public void LoadMasterPower(ModSysMaster master)
        {
            if (master == null) return;

            //获取用户角色信息
            master.RoleIdList = new BllSysMasterRole().GetMasterRole(master.Id);

            //获取所属公司
            master.Company = new BllSysCompany().LoadData(master.Cid);
        }


        /// <summary>
        /// 后台用户登录
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public LoginEnum Login(string loginName, string pwd, string ip)
        {
            var list = QueryToAll();
            loginName = loginName.ToLower().Trim();
            var p = list.Where(c => c.LoginName.Trim().ToLower() == loginName && c.Attribute != (int)AdminTypeEnum.普通用户).ToList();
            if (p.Any())
            {
                p = p.Where(c => DESEncrypt.Decrypt(c.Pwd) == pwd).ToList();
                if (p.Any())
                {
                    if (p.First().ComStatus == (int)StatusEnum.禁用)
                    {
                        return LoginEnum.公司帐号被禁用;
                    }
                    else if (p.First().Status == (int)StatusEnum.禁用)
                    {
                        return LoginEnum.帐号被禁用;
                    }
                    else if (p.First().Status == (int)StatusEnum.未激活)
                    {
                        return LoginEnum.账户未激活;
                    }
                    else
                    {
                        var master = p.First();
                        //缓存
                        HttpContext.Current.Session[BllStaticStr.SessionMaster] = master;
                        //更改用户登录信息
                        new BllSysMaster().upLoginInfo(ip, master.Id);
                        //获取用户权限
                        LoadMasterPower(master);

                        return LoginEnum.登录成功;
                    }
                }
                else
                {
                    return LoginEnum.密码错误;
                }
            }
            else
            {
                return LoginEnum.帐号不存在;
            }
        }


        /// <summary>
        /// 前台用户登录
        /// </summary>
        /// <param name="loginName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public LoginEnum LoginCustomer(string loginName, string pwd, string ip)
        {
            var list = QueryToAll();
            loginName = loginName.ToLower().Trim();
            var p = list.Where(c => c.LoginName.Trim().ToLower() == loginName && c.Attribute == (int)AdminTypeEnum.汽配商管理员).ToList();
            if (p.Any())
            {
                p = p.Where(c => DESEncrypt.Decrypt(c.Pwd) == pwd).ToList();
                if (p.Any())
                {
                    if (p.First().Status == (int)StatusEnum.禁用)
                    {
                        return LoginEnum.帐号被禁用;
                    }
                    else if (p.First().Status == (int)StatusEnum.禁用)
                    {
                        return LoginEnum.帐号被禁用;
                    }
                    else
                    {
                        var master = p.First();
                        //缓存
                        HttpContext.Current.Session[BllStaticStr.SessionMaster] = master;
                        //更改用户登录信息
                        new BllSysMaster().upLoginInfo(ip, master.Id);
                        //获取用户权限
                        LoadMasterPower(master);

                        return LoginEnum.登录成功;
                    }
                }
                else
                {
                    return LoginEnum.密码错误;
                }
            }
            else
            {
                return LoginEnum.帐号不存在;
            }
        }

        /// <summary>
        /// 获取所有管理员列表
        /// </summary>
        /// <returns></returns>
        public override List<ModSysMaster> QueryToAll()
        {
            var dic = GetMasterDic();
            return dic.Values.ToList();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void ClearCache()
        {
            CacheHelper.Remove(BllStaticStr.CacheMaster);
        }

        /// <summary>
        /// 获取接单工程师分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchMasterGrid(Search search)
        {
            search.TableName = @"View_Total_SysMaster"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime asc";
            return base.QueryPageToJson(search);
        }


        /// <summary>
        /// 获取管理员分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_Total_SysMaster"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.StatusDefaultCondition = "";
            search.SortField = "Status desc ,CreateTime asc";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 超级管理员获取管理员分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchDataByCompany(Search search)
        {
            search.TableName = @"View_Total_SysMaster"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.AddCondition("(Status!=" + (int)StatusEnum.删除 + " and Status!=-2)"); //过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "Status asc ,CreateTime asc";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <param name="key">用户主键</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        public int upPwd(string key, string userPwd)
        {
            return DAL.upPwd(key, userPwd);
        }


        /// <summary>
        /// 登录成功后,设置登录Ip,登录次数等信息
        /// </summary>
        /// <param name="ip">用户ip地址</param>
        /// <param name="key">用户主键</param>
        /// <returns></returns>
        public int upLoginInfo(string ip, string key)
        {
            return DAL.upLoginInfo(ip, key);
        }

        /// <summary>
        /// 系统注销
        /// </summary>
        public void LoginOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.RemoveAll();
        }

        /// <summary>
        /// 获取公司超级管理员信息
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public ModSysMaster CompanyMaster(string cid)
        {
            return DAL.CompanyMaster(cid);
        }

        /// <summary>
        /// 修改用户名是否存在相同
        /// </summary>
        /// <param name="CurrentUserName"></param>
        /// <param name="UpdateLoginName"></param>
        /// <returns></returns>
        public int UpdateExists(string CurrentUserName, string UpdateLoginName)
        {
            return DAL.UpdateExists(CurrentUserName, UpdateLoginName);
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
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(string Userid, string Column)
        {
            return DAL.UpdateUser(Userid, Column);
        }


        #region ===修改数据 UpdateDate
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int UpdateDate(ModSysMaster t)
        {
            return DAL.UpdateDate(t);
        }
        #endregion

        #region ===页面按钮权限 GetPageBtns
        /// <summary>
        /// 页面按钮权限
        /// </summary>
        /// <param name="className">页面操作类</param>
        /// <param name="TypeID">公司类型（）</param>
        /// <returns></returns>
        public DataSet GetPageBtns(string className, int TypeID)
        {
            return DAL.GetPageBtns(className, TypeID);
        }

        #endregion

        #region ===获取角色访问模块树 GetAuthByPage
        /// <summary>
        /// 获取角色访问模块树
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAuthByPage(string roleId, string where)
        {
            return DAL.GetAuthByPage(roleId, where);
        }
        #endregion

        #region ===获取页面角色按钮 GetAuthByBtn
        /// <summary>
        /// 获取页面角色按钮
        /// </summary>
        /// <param name="className">页面模块名称</param>
        /// <param name="roleId">用户角色</param>
        /// <param name="Attribute">用户类型</param>
        /// <returns></returns>
        public DataSet GetAuthByBtn(string className, string roleId, int Attribute)
        {
            return DAL.GetAuthByBtn(className, roleId, Attribute);
        }

        #endregion

        #region ===查询页面用户访问类型 GetLookPower
        /// <summary>
        /// 查询页面用户访问类型
        /// </summary>
        /// <param name="className">页面类名称</param>
        /// <param name="roleIdList">用户角色List</param>
        /// <param name="Attr">公司类型</param>
        /// <returns></returns>
        public int GetLookPower(string className, string roleIdList, int Attr)
        {
            return DAL.GetLookPower(className, roleIdList, Attr);
        }

        #endregion

        /// <summary>
        /// 收入排行榜
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="jd"></param>
        /// <returns></returns>
        public List<JsonRevenueRanking> RevenueRanking()
        {
            return DAL.RevenueRanking();
        }

        /// <summary>
        /// 工程师排行榜
        /// </summary>
        /// <returns></returns>
        public List<JsonRevenueRanking> EngineerRanking()
        {
            return DAL.EngineerRanking();
        }
        /// <summary>
        /// 根据类型获取用户信息
        /// </summary>
        /// <param name="attribut"></param>
        /// <returns></returns>
        public List<ModSysMaster> GetListByAttribute(int attribut)
        {
            return DAL.GetListByAttribute(attribut);
        }
        /// <summary>
        /// 获取距离最近的工程师
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<ModSysMaster> range(string CustomerId, string range)
        {
            return DAL.range(CustomerId,range);
        }
        /// <summary>
        /// 员工列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Dapper.Page<ModSysMaster> Search(Search search)
        {
            search.TableName = @"Sys_Master";//表名
            search.KeyFiled = "Id";//主键
            search.StatusDefaultCondition = "Status!=" + (int)StatusEnum.删除;
            search.SortField = "CreateTime desc";//排序
            return DAL.Search(search);
        }

        /// <summary>
        /// 根据cid 查询部门员工
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public List<ModSysMaster> GetOrganizaIdByList(string Cid)
        {
            return DAL.GetOrganizaIdByList(Cid);
        }


        /// <summary>
        /// 根据cid 查询单位员工
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public List<ModSysMaster> GetCIdByList(string Cid)
        {
            return DAL.GetCIdByList(Cid);
        }
    }
}
