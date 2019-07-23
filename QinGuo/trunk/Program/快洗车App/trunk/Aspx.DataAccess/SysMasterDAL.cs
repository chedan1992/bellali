using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using QINGUO.ViewModel;
using Dapper;
using QINGUO.Common;

namespace QINGUO.DAL
{
    /// <summary>
    /// 后台用户管理员操作类
    /// </summary>
    public class SysMasterDAL : BaseDAL<ModSysMaster>, ISysMaster
    {
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Sys_Master where Status!=" + (int)StatusEnum.删除 + "");
            sb.Append(" and " + where);
            List<ModSysMaster> list = dabase.ReadDataBase.Query<ModSysMaster>(sb.ToString()).ToList();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysMaster GetModelByWhere(string where)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModSysMaster>("select * from Sys_Master where 1=1 " + where);
        }

        /// <summary>
        /// 修改用户登录信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateLogin(ModSysMaster model)
        {
            string sql = "update Sys_Master set LoginTime=@LoginTime,LoginNum=@LoginNum,PaltForm=@PaltForm ";
            DataParameters paras = new DataParameters();
            paras.Add("@Id", model.Id);
            paras.Add("@LoginTime", model.LoginTime);
            paras.Add("@LoginNum", model.LoginNum);
            paras.Add("@PaltForm", model.PaltForm);
            if (!string.IsNullOrEmpty(model.MobileCode))
            {
                sql += " ,MobileCode=@MobileCode";
            }
            if (!string.IsNullOrEmpty(model.BDUserId))
            {
                sql += " ,BDUserId=@BDUserId";
            }
            if (!string.IsNullOrEmpty(model.BDChannelId))
            {
                sql += " ,BDChannelId=@BDChannelId";
            }

            sql += " where Id=@Id ";

            int result = dabase.ExecuteNonQueryByText(sql, paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMaster(string UserId, string Column)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@UserId", UserId);

            int result = dabase.ExecuteNonQueryByText("update Sys_Master set " + Column + " where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 查询能接受推送的商户
        /// </summary>
        /// <param name="CategoryId">商户选择的分类</param>
        /// <returns></returns>
        public List<ModPushMsgCompany> GetPushCompany(string CategoryId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select [Id],[Name],[NameTitle],UserId,MobileCode from SysCompany_CompanyView where Id in (");
            sb.Append(" select CompanyId from Sys_CompanyCategory where CategoryId in ('" + CategoryId + "'))");
            sb.Append(" union");
            sb.Append(" select [Id],[Name],[NameTitle],UserId,MobileCode from SysCompany_CompanyView where Id not in ");
            sb.Append(" (select distinct(CompanyId) from Sys_CompanyCategory)");

            return dabase.ReadDataBase.Query<ModPushMsgCompany>(sb.ToString()).ToList();
        }

        /// <summary>
        /// 获取推送商户列表
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public List<ModPushUserView> GetPushMasterList(string CategoryId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select Id,[BDUserId],[BDChannelId],[PaltForm] from SysCompany_CompanyView where Id not in ");
            sb.Append(" (select distinct(CompanyId) from Sys_CompanyCategory)");
            sb.Append(" union");
            sb.Append(" select Id,[BDUserId],[BDChannelId],[PaltForm] from SysCompany_CompanyView where Id in ");
            sb.Append(" (select distinct(CompanyId) from Sys_CompanyCategory where CategoryId in('" + CategoryId + "'))");
            return dabase.ReadDataBase.Query<ModPushUserView>(sb.ToString()).ToList();
        }


        /// <summary>
        /// 验证后台用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        public DataSet ValidateLogin(string userName, string userPwd)
        {
            var parameters = new DataParameters();
            parameters.Add("@UserName", userName);
            parameters.Add("@PassWord", userPwd);
            return dabase.ExecuteDataSetByProc("Sys_Login", parameters);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="Id">用户主键</param>
        /// <param name="Pwd">密码</param>
        /// <returns></returns>
        public int ReSetPwd(string Id, string Pwd)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", Id);
            parameters.Add("@Pwd", Pwd);

            string sql = "update Sys_Master set Pwd=@Pwd where Id=@Id";
            try
            {
                dabase.ExecuteNonQueryByText(sql, parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 根据主键进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteSysMasterById(string id)
        {
            var parameters = new DataParameters();
            parameters.Add("@id", id);
            string sql = "delete from Sys_Master where Id=@id";
            int flag = 0;
            try
            {
                flag = dabase.ExecuteNonQueryByText(sql, parameters);
                dabase.CommitTransaction();

            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                flag = 0;
            }
            return flag;
        }


        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <param name="key">用户主键</param>
        /// <param name="userPwd">密码</param>
        /// <returns></returns>
        public int upPwd(string key, string userPwd)
        {
            var parameters = new DataParameters();
            parameters.Add("@Pwd", userPwd);
            parameters.Add("@Id", key);

            string sql = "update Sys_Master set Pwd=@Pwd where Id=@Id;";
            try
            {
                dabase.ExecuteNonQueryByText(sql, parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 登录成功,设置登录信息
        /// </summary>
        /// <param name="ip">用户ip地址</param>
        /// <param name="key">用户主键</param>
        /// <returns></returns>
        public int upLoginInfo(string ip, string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@LoginIp", ip);
            parameters.Add("@LoginTime", DateTime.Now.ToString());
            parameters.Add("@Id", key);

            string sql = "update Sys_Master set LoginIp=@LoginIp,LoginTime=@LoginTime,LoginNum=LoginNum+1 where Id=@Id";

            try
            {
                dabase.ExecuteNonQueryByText(sql, parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", flag);
            parameters.Add("@Id", key);

            StringBuilder sb = new StringBuilder();
            sb.Append("update Sys_Master set Status=@Status where Id=@Id;");
            //if (flag == (int)StatusEnum.禁用)
            //{
            //    SetStatusClose(key, ref sb);
            //}
            try
            {
                dabase.ExecuteNonQueryByText(sb.ToString(), parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 根据编号,设置管理员下的管理员信息
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="sb"></param>
        public void SetStatusClose(string Id, ref StringBuilder sb)
        {
            string sql = " select * from Sys_Master where CreaterId in ('" + Id + "');";
            DataSet ds = dabase.ExecuteDataSet(sql);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sb.AppendLine();
                sb.Append("update Sys_Master set Status=@Status where Id='" + ds.Tables[0].Rows[i]["Id"] + "';");
                sb.AppendLine();
                SetStatusClose(ds.Tables[0].Rows[i]["Id"].ToString(), ref sb);
            }
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int UpdateDate(ModSysMaster t)
        {
            t.Email = (t.Email == null ? "" : t.Email);
            t.Phone = (t.Phone == null ? "" : t.Phone);
            var parameters = new DataParameters();
            parameters.Add("@Id", t.Id);
            parameters.Add("@LoginName", t.LoginName);
            parameters.Add("@UserName", t.UserName);
            parameters.Add("@Pwd", t.Pwd);
            parameters.Add("@Sex", t.Sex);
            parameters.Add("@Email", t.Email);
            parameters.Add("@CardNum", t.CardNum);
            parameters.Add("@Phone", t.Phone);
            parameters.Add("@HeadImg", t.HeadImg);
            parameters.Add("@RoleName", t.RoleName);


            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sys_Master set LoginName=@LoginName,UserName=@UserName,Sex=@Sex,Email=@Email,Phone=@Phone,RoleName=@RoleName,Pwd=@Pwd,HeadImg=@HeadImg,CardNum=@CardNum");
            strSql.Append(" where Id=@Id");

            try
            {
                dabase.ExecuteNonQueryByText(strSql.ToString(), parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 页面按钮权限
        /// </summary>
        /// <param name="className">页面操作类</param>
        /// <returns></returns>
        public DataSet GetPageBtns(string className, int TypeID)
        {
            var parameters = new DataParameters();
            parameters.Add("@action", className);
            parameters.Add("@TypeID", TypeID);
            return dabase.ExecuteDataSetByProc("Sys_GetAuthorityBtn", parameters);
        }

        /// <summary>
        /// 获取角色访问模块树
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAuthByPage(string roleId, string where)
        {
            DataSet sysfun = new DataSet();

            //获取权限访问模块树
            string sql = "select distinct(FunId) from Sys_RoleFun where  RoleId in(" + roleId + ")";

            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("select * from (");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(" union ");
                    }
                    sb.Append("select Id as id,ParentID as parentId,FunName as text,[TypeID],[FunSort],[PageUrl],[ClassName],[CreateTime],[iconCls],[IsSystem] ,[Status],isChild,isCheckRole from Sys_Fun a , Uf_GetSysFunChildID('" + ds.Tables[0].Rows[i]["FunId"] + "') b where a.Id = b.IDKey ");
                }
                sb.Append(" ) as a where Status=" + (int)StatusEnum.正常 + " and " + where + " order by FunSort asc ");
                sysfun = dabase.ExecuteDataSet(sb.ToString());
            }
            return sysfun;
        }

        /// <summary>
        /// 获取页面角色按钮
        /// </summary>
        /// <param name="className">页面模块名称</param>
        /// <param name="roleId">用户角色</param>
        /// <returns></returns>
        public DataSet GetAuthByBtn(string className, string roleId, int Attribute)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select distinct(b.[Id]),b.[Name],b.[Value],b.[ActionName],b.[IConName],b.[FunSort] from (");
            sb.Append(" SELECT * FROM [Sys_RoleFun] where RoleId in(" + roleId + ")");
            sb.Append(" and FunId=(select top 1 Id from Sys_Fun where ClassName=@action and TypeID=@Attribute) and BtnId!=''");
            sb.Append(" ) as a inner join v_FunLinkBtnValue_BtnValue as b on a.FunId=b.FunId and a.BtnId=b.Id");
            sb.Append("   order by FunSort asc");

            var parameters = new DataParameters();
            parameters.Add("@action", className);
            parameters.Add("@Attribute", Attribute);

            return dabase.ExecuteDataSet(sb.ToString(), CommandType.Text, parameters);
        }


        /// <summary>
        /// 查询页面用户访问类型
        /// </summary>
        /// <param name="className">页面类名称</param>
        /// <param name="roleIdList">用户角色List</param>
        /// <param name="Attr">公司类型</param>
        /// <returns></returns>
        public int GetLookPower(string className, string roleIdList, int Attr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT top 1 * FROM [Sys_RoleRangeData] ");
            sb.Append("where RoleId in(" + roleIdList + ") ");
            sb.Append("and charindex((select Id from [Sys_Fun] where ClassName='" + className + "' and TypeID=" + Attr + " and Status=" + (int)StatusEnum.正常 + "),FunId)>0 ");
            sb.Append("order by lookPower desc");

            int lookPower = (int)LookPowerEnum.查看自建;
            DataSet ds = dabase.ExecuteDataSet(sb.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                lookPower = Convert.ToInt32(ds.Tables[0].Rows[0]["lookPower"].ToString());
            }
            //else { //没有授权方式，查看全部
            //    lookPower = (int)LookPowerEnum.无需授权;
            //}
            return lookPower;
        }

        /// <summary>
        /// 公司下面的管理员信息
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public ModSysMaster CompanyMaster(string cid)
        {
            string sql = "select * from Sys_Master where IsMain=1 and CID=@0 and IsSystem=1";

            return dabase.ReadDataBase.SingleOrDefault<ModSysMaster>(sql, cid);
        }

        /// <summary>
        /// 根据店铺Id和用户名进行用户名判断
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public int GetMasterExists(string UserName, string companyID)
        {
            string sql = "select count(*) from Sys_Master where LoginName=@0 and CID=@1";
            return dabase.ReadDataBase.ExecuteScalar<int>(sql, UserName, companyID);
        }

        /// <summary>
        /// 修改用户名是否存在相同
        /// </summary>
        /// <param name="CurrentUserName"></param>
        /// <param name="UpdateLoginName"></param>
        /// <returns></returns>
        public int UpdateExists(string CurrentUserName, string UpdateLoginName)
        {
            string sql = "select isnull(count(*),0) from Sys_Master where LoginName!=@0 and LoginName=@1";
            return dabase.ReadDataBase.ExecuteScalar<int>(sql, CurrentUserName, UpdateLoginName);
        }

        /// <summary>
        /// 获得系统用户树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id as id,UserName as text,Attribute,OrganizaId");
            strSql.Append(" FROM Sys_Master where Status=1 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            return dabase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 修改用户头像信息
        /// </summary>
        /// <param name="HeadImg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateHeadImg(string HeadImg, string userId)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", userId);
            paras.Add("@HeadImg", HeadImg);
            int result = dabase.ExecuteNonQueryByText("update Sys_Master set HeadImg=@HeadImg where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUser(string Userid, string Column)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", Userid);

            int result = dabase.ExecuteNonQueryByText("update Sys_Master set " + Column + " where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 收入排行榜
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="jd"></param>
        /// <returns></returns>
        public List<JsonRevenueRanking> RevenueRanking()
        {
            string sql = @"select  ISNULL(a.Money,0) as Money ,b.Id,b.UserName,b.HeadImg,(select d.Name from Sys_Company as d where d.Id=b.Cid) as CName,b.Cid
                        from 
                        (select SUM(MoneyNum) as Money,UserId from Order_UserMoneyRecord  where BankName<>'系统保证金' group by UserId) as a 
                        right join Sys_Master as b on (a.UserId=b.Id) where b.Attribute=1 and b.Status=1  order by a.Money desc";

            return dabase.ReadDataBase.Query<JsonRevenueRanking>(sql).ToList();
        }

        /// <summary>
        /// 工程师排行榜
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="jd"></param>
        /// <returns></returns>
        public List<JsonRevenueRanking> EngineerRanking()
        {
            string sql = @"select ISNULL(a.OrderCount,0) as OrderCount,b.Id,b.Cid,b.UserName,b.HeadImg,(select d.Name from Sys_Company as d where d.Id=b.Cid) as CName
                            from (
                            select SUM(1) as OrderCount,EngineerId from E_Task  
                            where Status>=" + (int)OrderStatusEnum.确认 + @"  group by EngineerId
                            ) as a right join Sys_Master as b on (a.EngineerId=b.Id) where b.Attribute=1 and b.status=1 order by a.OrderCount desc";

            return dabase.ReadDataBase.Query<JsonRevenueRanking>(sql).ToList();
        }

        /// <summary>
        /// 根据类型获取用户信息
        /// </summary>
        /// <param name="attribut"></param>
        /// <returns></returns>
        public List<ModSysMaster> GetListByAttribute(int attribut)
        {
            string sql = "select * from Sys_Master where status=1 and Attribute=@0";

            return dabase.ReadDataBase.Query<ModSysMaster>(sql, attribut).ToList();
        }

        /// <summary>
        /// 获取距离最近的工程师
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        public List<ModSysMaster> range(string CustomerId, string range)
        {
            string sql = @"select a.Id from Sys_Master as a left join Sys_Company as b on (b.CreateCompanyId=a.Cid)
                    where dbo.fnGetDistance(b.CompLat,b.ComPLon,a.Lae,a.Loe)<=" + range + @" and b.Id=@0
                    and a.Attribute=6 and a.Status=1";

            return dabase.ReadDataBase.Query<ModSysMaster>(sql, CustomerId).ToList();

        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <returns>ModCorporateApplyFor</returns>
        public Page<ModSysMaster> Search(Search search)
        {
            return dabase.ReadDataBase.Page<ModSysMaster>(search.CurrentPageIndex, search.PageSize, search.SqlString);
        }


        /// <summary>
        /// 根据cid 查询
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public List<ModSysMaster> GetOrganizaIdByList(string Cid)
        {
            string sql = @"select * from Sys_Master where OrganizaId=@0 and status=@1";
            return dabase.ReadDataBase.Query<ModSysMaster>(sql, Cid, (int)StatusEnum.正常).ToList();
        }



        /// <summary>
        /// 根据cid 查询单位员工
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public List<ModSysMaster> GetCIdByList(string Cid)
        {
            string sql = @"select * from Sys_Master where Cid=@0 and status=@1";
            return dabase.ReadDataBase.Query<ModSysMaster>(sql, Cid, (int)StatusEnum.正常).ToList();
        }
    }
}
