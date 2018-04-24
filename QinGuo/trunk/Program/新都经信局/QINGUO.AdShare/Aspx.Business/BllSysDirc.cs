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
    public class BllSysDirc : BllBase<ModSysDirc>
    {
        private ISysDirc DAL = CreateDalFactory.CreateSysDirc();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取类型分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"Sys_Dirc";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
           // search.SortField = "OrderNum asc";//排序
            return base.QueryPageToJson(search);
        }

          /// <summary>
        /// 获得系统树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            return DAL.GetTreeList(strWhere);
        }
        /// <summary>
        /// 获取类型列表
        /// </summary>
        /// <returns></returns>
        public List<ModSysGroup> GetGroupList()
        {
            return DAL.GetGroupList();
        }
    }
}
