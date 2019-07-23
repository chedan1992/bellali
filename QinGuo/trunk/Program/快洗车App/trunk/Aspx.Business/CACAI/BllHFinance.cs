using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using System.Data;

namespace QINGUO.Business
{
    public class BllHFinance : BllBase<ModHFinance>
    {
        private IHFinance DAL = CreateDalFactory.CreateHFinance();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
        #region ===Pool 财务池
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchHOrderInRelation(Search search)
        {
            search.TableName = @"view_HOrderInRelation"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.StatusDefaultCondition = "";
            //search.SortField = "FinancialState asc,Status asc ,CreateTime desc";
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchHPurchaseRelation(Search search)
        {
            search.TableName = @"view_HPurchaseRelation"; //表名
        
            search.KeyFiled = "Id"; //主键
            search.StatusDefaultCondition = "";
            //search.SortField = "FinancialState asc,Status asc ,CreateTime desc";
            return base.QueryPageToJson(search);
        }
        #endregion

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"view_HFinance"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.AddCondition("Status!=" + (int)StatusEnum.删除); //过滤条件
            search.StatusDefaultCondition = "";
            //search.SortField = "FinancialState asc,Status asc ,CreateTime desc";
            return base.QueryPageToJson(search);
        }

         /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAllList(string IdList)
        {
            return DAL.GetAllList(IdList);
        }
    }
}
