using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using Dapper;
using QINGUO.Common;
using QINGUO.ViewModel;

namespace QINGUO.DAL
{
    public class EDynamicDAL : BaseDAL<ModEDynamic>, IEDynamic
    {

        public List<ModEDynamic> getListAll(int? top)
        {
            if (top > 0)
            {
                string sql = "select top " + top + " * from E_Dynamic where Status=" + (int)StatusEnum.正常 + " order by CreateTime desc";
                return dabase.ReadDataBase.Query<ModEDynamic>(sql).ToList();
            }
            else
            {
                string sql = "select * from E_Dynamic where Status=" + (int)StatusEnum.正常 + " order by CreateTime desc";
                return dabase.ReadDataBase.Query<ModEDynamic>(sql).ToList();
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
            sb.Append("update E_Dynamic set Status=@Status where Id=@Id;");//停用
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
        /// 获取文章分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public Page<ModEDynamic> GetDynamic(Search search)
        {
            return dabase.ReadDataBase.Page<ModEDynamic>(search.CurrentPageIndex, search.PageSize, search.SqlString2);
        }

        /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModViewDynamic GetModelByWhere(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from View_Dynamic");
            sb.Append("  where 1=1 " + where);
            return dabase.ReadDataBase.SingleOrDefault<ModViewDynamic>(sb.ToString());
        }
    }
}
