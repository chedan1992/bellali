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
    public class BllHPurchase : BllBase<ModHPurchase>
    {
        private IHPurchase DAL = CreateDalFactory.CreateHPurchase();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"view_HPurchase"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.StatusDefaultCondition = "";
            search.AddCondition("Status!=" + (int)StatusEnum.删除); //过滤条件
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
