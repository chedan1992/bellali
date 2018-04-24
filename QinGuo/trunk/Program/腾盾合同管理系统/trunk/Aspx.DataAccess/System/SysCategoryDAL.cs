using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.IDAL;

namespace QINGUO.DAL
{
    public class SysCategoryDAL : BaseDAL<ModSysCategory>, ISysCategory
    {

        /// <summary>
        /// 根据Id获取下一级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModSysCategory> GetChaildList(string id)
        {
            return dabase.ReadDataBase.Query<ModSysCategory>("select * from Sys_Category where Status=1 and ParentCategoryId=@0 Order by OrderNum", id).ToList();
        }

        /// <summary>
        /// 查询类别根节点
        /// </summary>
        /// <param name="parentCategoryId">父节点</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet SearchData(string parentCategoryId,string where)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", (int)StatusEnum.正常);
            parameters.Add("@ParentCategoryId", parentCategoryId);

            string sql = "select * from Sys_Category where (Status=@Status or Status!=-1) and ParentCategoryId=@ParentCategoryId";
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
            strSql.Append("select Id as id,ParentCategoryId as parentId,Name as text,HasChild,Depth");
            strSql.Append(" FROM Sys_Category where Status!=" + (int)StatusEnum.删除);
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
        public List<ModSysCategory> SearchDataLikeName(string name)
        {
            string sql = "";
            if (name != "")
            {
                sql = "select * from Sys_Category where Status=@0  and Name like '%" + name + "%' order by OrderNum asc";
            } 
            else
            {
                sql = "select * from Sys_Category where Status=@0 order by OrderNum asc";
            }
            return dabase.ReadDataBase.Query<ModSysCategory>(sql, (int)StatusEnum.正常).ToList();
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
                sb.Append("update Sys_Category set Status=@Status where Id in(select Id from GetSysCategoryByParent(@Id));");
             
                //sb.Append("update Sys_Category set Status=@Status where Id in(select Id from GetSysCategoryByChild(@Id)) or Id in(select Id from GetSysCategoryByParent(@Id))");
            }
            else//禁用
            {
                sb.Append("update Sys_Category set Status=@Status where Id in(select Id from GetSysCategoryByChild(@Id));");

                sb.AppendLine();
                sb.Append("update bs_TestStore set Status=@Status where Id in( select Id from bs_TestStore");
                sb.Append(" where (Type1 in(select Id from GetSysCategoryByChild('" + key + "'))");
                sb.Append(" or Type2 in(select Id from GetSysCategoryByChild('" + key + "'))");
                sb.Append(" or Type3 in(select Id from GetSysCategoryByChild('" + key + "'))");
                sb.Append(" or Type4 in(select Id from GetSysCategoryByChild('" + key + "'))");
                sb.Append(" ) and Status=1 and Id in ");
                sb.Append(" (select TestStoreId from bs_PapersTest where PapersId in (select Id from bs_Papers where Status=1)))");
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
            sb.Append("update Sys_Category set Status=@Status where Id in(select Id from GetSysCategoryByChild(@Id))");

            //类别下是否还有兄弟节点
            string sql = " select * from Sys_Category where ParentCategoryId in(select ParentCategoryId from Sys_Category where Id='" + key + "') and Status!=" + (int)StatusEnum.删除 + " and Id!='" + key + "';";
            DataSet ds = dabase.ExecuteDataSet(sql);
            if (ds.Tables[0].Rows.Count <=0)
            {
                sb.AppendLine();
                sb.Append("update Sys_Category set HasChild=0 where Id=(select ParentCategoryId from Sys_Category where Id=@Id);");
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
        public int UpdateDate(ModSysCategory t)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(t.ParentCategoryId))
            {
                //类别是否还有兄弟节点
                string sql = "select * from Sys_Category where ParentCategoryId in(select ParentCategoryId from Sys_Category where Id='" + t.Id + "') and Status!=" + (int)StatusEnum.删除 + " and Id!='" + t.Id + "';";
                DataSet ds = dabase.ExecuteDataSet(sql);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    sb.Append("update Sys_Category set HasChild=0 where Id=(select ParentCategoryId from Sys_Category where Id='" + t.Id + "');");
                    sb.AppendLine();

                }           
            }
            
            sb.Append("update Sys_Category set Name='"+t.Name+"',OrderNum="+t.OrderNum+",ParentCategoryId='"+t.ParentCategoryId+"',Path='"+t.Path+"',PicUrl='"+t.PicUrl+"',Remark='"+t.Remark+"' where Id='"+t.Id+"';");
            sb.AppendLine();
            sb.Append("update Sys_Category set HasChild=1 where Id='" + t.ParentCategoryId + "';");
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
        public int InsertDate(ModSysCategory t)
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
            sb.Append("insert into Sys_Category(Id,Name,OrderNum,ParentCategoryId,Status,Depth,Path,HasChild,PicUrl,IsSystem,Remark,CreaterId,CreaterName)");
            sb.Append(" values(@Id,@Name,@OrderNum,@ParentCategoryId,@Status,@Depth,@Path,@HasChild,@PicUrl,@IsSystem,@Remark,@CreaterId,@CreaterName);");
            if (!string.IsNullOrEmpty(t.ParentCategoryId))
            {
                sb.AppendLine();
                sb.Append("update Sys_Category set HasChild=1 where Id=@ParentCategoryId");
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
    }
}
