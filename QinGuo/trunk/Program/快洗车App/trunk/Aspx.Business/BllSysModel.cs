using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysModel : BllBase<ModSysModel>
    {
        private ISysModel DAL = CreateDalFactory.CreateSysModel();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"Sys_Model";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.AddCondition("Status!=" + (int)StatusEnum.删除);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "Sort asc";
            return base.QueryPageToJson(search);
        }
    }
}
