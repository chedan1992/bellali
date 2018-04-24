using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.ViewModel;
using QINGUO.Common;
using Dapper;
using System.Data;

namespace QINGUO.DAL
{
    public class AdActiveDAL : BaseDAL<ModAdActive>, IAdActive
    {
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Cor_Ad_Active where Status!=" + (int)StatusEnum.删除 + "");
            sb.Append(" and " + where);
            List<ModAdActive> list = dabase.ReadDataBase.Query<ModAdActive>(sb.ToString()).ToList();
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModAdActiveView GetModelByWhere(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Cor_Ad_Active");
            sb.Append("  where 1=1 " + where);
            return dabase.ReadDataBase.SingleOrDefault<ModAdActiveView>(sb.ToString());
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <returns>ModSysUser</returns>
        public Page<ModAdActiveView> Search(Search search)
        {
            return dabase.ReadDataBase.Page<ModAdActiveView>(search.CurrentPageIndex, search.PageSize, search.SqlString);
        }

        /// <summary>
        /// 获取最新的广告列表
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
        public List<ModAdActive> QueryAll(int? top)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select top " + top + "  id,ActiveName,Img,ActionFormId,ActionType,CreateTime,case ActionType when 2 then info else '' end as info from Ad_Active where (getdate() between ActiveStartTime and ActiveEndTime and ShowType=2 or ShowType=1) and Status=" + (int)StatusEnum.正常 + " order by CreateTime desc");

            return dabase.ReadDataBase.Query<ModAdActive>(sb.ToString()).ToList();
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
            sb.Append("update Ad_Active set Status=@Status where Id=@Id;");//停用
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
        /// 广告过期
        /// </summary>
        /// <returns></returns>
        public int Expired()
        {
            var parameters = new DataParameters();

            StringBuilder sb = new StringBuilder();
            sb.Append("update Ad_Active set Status=-2 where ActiveEndTime<GETDATE() and Status=1");//过期
            try
            {
                int r = dabase.ExecuteNonQueryByText(sb.ToString(), parameters);
                dabase.CommitTransaction();
                return r;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 根据用户编号获取用户名称
        /// </summary>
        /// <param name="Idlist"></param>
        /// <returns></returns>
        public DataSet GetPersonalName(string Idlist)
        {
            string sql = "select UserName from Sys_Master where charindex(Id,'" + Idlist + "')>0";
            return dabase.ExecuteDataSet(sql);
        }
    }
}
