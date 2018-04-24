using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysFileAttach : BllBase<ModSysFileAttach>
    {
        private ISysFileAttach DAL = CreateDalFactory.CreateSysFileAttach();

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
            search.TableName = @"Sys_FileAttach"; //表名
            search.SelectedColums = "*"; //查询列
            search.KeyFiled = "Id"; //主键
            search.AddCondition("Status!=" + (int)StatusEnum.删除); //过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "Status asc ,CreateTime desc";
            return base.QueryPageToJson(search);
        }
    }
}
