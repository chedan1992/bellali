#region Version Info
/* ======================================================================== 
* 【本类功能概述】 产品分类数据访问层
* 
* 作者：张建 时间：2013/12/31 15:34:14 
* 文件名：DalShopCategory 
* 版本：V1.0.1 
* 
* 修改者： 时间： 
* 修改说明： 
* ======================================================================== 
*/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using QINGUO.Model;

namespace QINGUO.DAL
{
    public class ShopCategoryDAL : BaseDAL<ModShopCategory>, IShopCategory
    {

        /// <summary>
        /// 根据Id获取下一级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModShopCategory> GetChaildList(string id)
        {
            return dabase.ReadDataBase.Query<ModShopCategory>("select * from Shop_Category where Status=1 and ParentCategoryId=@0 Order by Code", id).ToList();
        }

        /// <summary>
        /// 查询类别根节点
        /// </summary>
        /// <param name="parentCategoryId">父节点</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet SearchData(string parentCategoryId, string where)
        {
            var parameters = new DataParameters();
            parameters.Add("@ParentCategoryId", parentCategoryId);

            string sql = "select * from Shop_Category where Status!=" + (int)StatusEnum.删除 + " and ParentCategoryId=@ParentCategoryId";
            if (where != "")
            {
                sql += " and " + where;
            }
            sql += " order by OrderNum asc";
            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 获得树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id as id,ParentCategoryId as parentId,Name as text,HasChild");
            strSql.Append(" FROM Shop_Category where Status!=" + (int)StatusEnum.删除);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            strSql.Append(" order by OrderNum asc");
            return dabase.ExecuteDataSet(strSql.ToString());
        }

        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<ModShopCategory> SearchDataLikeName(string name)
        {
            string sql = "";
            if (name != "")
            {
                sql = "select * from Shop_Category where Status!=@0  and Name like '%" + name + "%' order by OrderNum asc";
            }
            else
            {
                sql = "select * from Shop_Category where Status!=@0 order by OrderNum asc";
            }
            return dabase.ReadDataBase.Query<ModShopCategory>(sql, (int)StatusEnum.删除).ToList();
        }


        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateIsStatus(int status, string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", status);
            parameters.Add("@Id", key);

            StringBuilder sb = new StringBuilder();
            if (status == (int)StatusEnum.正常)//启用
            {
                sb.Append("update Shop_Category set Status=@Status where Id in(select Id from GetShopCategoryByChild(@Id)) or Id in(select Id from GetShopCategoryByParent(@Id))");
            }
            else//禁用
            {
                sb.Append("update Shop_Category set Status=@Status where Id in(select Id from GetShopCategoryByChild(@Id))");
            }
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
        /// 删除商品类别及子类别
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int DeleteDate(string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", (int)StatusEnum.删除);
            parameters.Add("@Id", key);
            StringBuilder sb = new StringBuilder();
            sb.Append("update Shop_Category set Status=@Status where Id in(select Id from GetShopCategoryByChild(@Id))");

            //类别下是否还有兄弟节点
            string sql = " select * from Shop_Category where ParentCategoryId in(select ParentCategoryId from Shop_Category where Id='" + key + "') and Status!=" + (int)StatusEnum.删除 + " and Id!='" + key + "';";
            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                sb.AppendLine();
                sb.Append("update Shop_Category set HasChild=0 where Id=(select ParentCategoryId from Shop_Category where Id=@Id);");
            }

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
        /// 编辑类别信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int UpdateDate(ModShopCategory t)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(t.ParentCategoryId))
            {
                //类别是否还有兄弟节点
                string sql = "select * from Shop_Category where ParentCategoryId in(select ParentCategoryId from Shop_Category where Id='" + t.Id + "') and Status!=" + (int)StatusEnum.删除 + " and Id!='" + t.Id + "';";
                DataSet ds = dabase.ExecuteDataSet(sql);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    sb.Append("update Shop_Category set HasChild=0 where Id=(select ParentCategoryId from Shop_Category where Id='" + t.Id + "');");
                    sb.AppendLine();

                }
            }

            sb.Append("update Shop_Category set Name='" + t.Name + "',OrderNum=" + t.OrderNum + ",ParentCategoryId='" + t.ParentCategoryId + "',Path='" + t.Path + "',PicUrl='" + t.PicUrl + "',Remark='" + t.Remark + "' where Id='" + t.Id + "';");
            sb.AppendLine();
            sb.Append("update Shop_Category set HasChild=1 where Id='" + t.ParentCategoryId + "';");
            try
            {
                dabase.ExecuteNonQuery(sb.ToString());
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
        /// 新增类别信息
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public int InsertDate(ModShopCategory t)
        {
            var parameters = new DataParameters();
            parameters.Add("@Id", t.Id);
            parameters.Add("@Name", t.Name);
            parameters.Add("@OrderNum", t.OrderNum);
            parameters.Add("@ParentCategoryId", t.ParentCategoryId);
            parameters.Add("@Status", t.Status);
            parameters.Add("@Depth", t.Depth);
            parameters.Add("@Path", t.Path);
            parameters.Add("@HasChild", t.HasChild);
            parameters.Add("@PicUrl", t.PicUrl);
            parameters.Add("@IsSystem", t.IsSystem);
            parameters.Add("@Remark", t.Remark);
            parameters.Add("@CreaterId", t.CreaterId);
            parameters.Add("@CreaterName", t.CreaterName);


            StringBuilder sb = new StringBuilder();
            sb.Append("insert into Shop_Category(Id,Name,OrderNum,ParentCategoryId,Status,Depth,Path,HasChild,PicUrl,IsSystem,Remark,CreaterId,CreaterName)");
            sb.Append(" values(@Id,@Name,@OrderNum,@ParentCategoryId,@Status,@Depth,@Path,@HasChild,@PicUrl,@IsSystem,@Remark,@CreaterId,@CreaterName);");
            if (!string.IsNullOrEmpty(t.ParentCategoryId))
            {
                sb.AppendLine();
                sb.Append("update Shop_Category set HasChild=1 where Id=@ParentCategoryId");
            }

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
        /// 查询公司行业和标签
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="ParentCategoryId"></param>
        /// <returns></returns>
        public List<string> SearchByCompany(string comId, string ParentCategoryId)
        {
            if (ParentCategoryId == "0")
            {
                ParentCategoryId = "='0'";
            }
            else if (ParentCategoryId == "1")
            {
                ParentCategoryId = "<>'0'";
            }
            string sql = @"select a.Name from Shop_Category as a inner join Sys_CompanySellProType as b on b.ShopTypeId=a.Id
             where a.Status=@0  and a.ParentCategoryId " + ParentCategoryId + " and b.CompanyID=@1 order by OrderNum asc";

            return dabase.ReadDataBase.Query<string>(sql, (int)StatusEnum.正常, comId).ToList();
        }

        public DataSet SearchByCompanys(string comId, string ParentCategoryId)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", (int)StatusEnum.正常);
            parameters.Add("@comId", comId);
            if (ParentCategoryId == "0")
            {
                ParentCategoryId = "='0'";
            }
            else if (ParentCategoryId == "1")
            {
                ParentCategoryId = "<>'0'";
            }
            parameters.Add("@Depth", ParentCategoryId);

            string sql = @"select a.Name from Shop_Category as a inner join Sys_CompanySellProType as b on b.ShopTypeId=a.Id
             where a.Status=@Status  and a.ParentCategoryId " + ParentCategoryId + " and b.CompanyID=@comId order by OrderNum asc";

            return dabase.ExecuteDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 获取公司商品分类
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<string> GetCompanySellProType(string cid)
        {
            string sql = @"select ShopTypeID from Sys_CompanySellProType where CompanyID=@0";
            return dabase.ReadDataBase.Query<string>(sql, cid).ToList();
        }


        /// <summary>
        /// 更改商品类别,同步分公司开通管理类别信息
        /// </summary>
        /// <param name="NodeId"></param>
        public int UpdateSynchro(string NodeId)
        {
            int result = 0;
            //查询当前哪些公司需要添加类别
            string sql = "select distinct(CompanyID) CompanyID from Sys_CompanySellProType where CompanyID in (select CompanyID from Sys_CompanySellProType where ShopTypeID='" + NodeId + "') and ShopTypeID not in (select Id from dbo.GetShopCategoryByParent('" + NodeId + "') where Id!='" + NodeId + "' and Id!='0')";
            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string treeNode = "select Id from dbo.GetShopCategoryByParent('" + NodeId + "') where Id!='" + NodeId + "' and Id!='0'";
                DataSet Nodeds = dabase.ExecuteDataSet(treeNode);

                StringBuilder strSql = new StringBuilder();

                if (Nodeds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < Nodeds.Tables[0].Rows.Count; j++)
                        {
                            strSql.Append("insert into Sys_CompanySellProType values(newid(),'" + ds.Tables[0].Rows[i]["CompanyID"].ToString() + "','" + Nodeds.Tables[0].Rows[j]["Id"] + "');");
                            strSql.AppendLine();
                        }
                    }
                }
                try
                {
                    if (strSql.ToString().Trim() != "")
                    {
                        dabase.ExecuteNonQuery(strSql.ToString());
                        dabase.CommitTransaction();
                    }
                    result = 1;
                }
                catch (Exception ex)
                {
                    dabase.RollbackTransaction();
                }
            }
            return result;
        }


        /// <summary>
        /// 获取全部正常分类
        /// </summary>
        public List<ModShopCategory> GetCategory()
        {
            string sql = @"select * from Shop_Category where Status=@0";
            return dabase.ReadDataBase.Query<ModShopCategory>(sql, (int)StatusEnum.正常).ToList();
        }
    }
}
