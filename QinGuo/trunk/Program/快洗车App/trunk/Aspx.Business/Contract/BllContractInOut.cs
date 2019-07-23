using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;
using QINGUO.Common;
using System.Data;

namespace QINGUO.Business
{
    public class BllContractInOut : BllBase<ModContractInOut>
    {
        private IContractInOut DAL = CreateDalFactory.CreateContractInOut();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModCompanyOrder</returns>
        public string SearchInComeData(Search search)
        {
            //查询条件
            search.TableName = @"view_Con_ContractInOut";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModContractInOut GetModelByWhere(string where)
        {
            return DAL.GetModelByWhere(where);
        }
        /// <summary>
        /// 导出获取列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="where"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public DataSet GetAllList(Search search, string where, int ctype)
        {
            return DAL.GetAllList(search, where, ctype);
        }

         /// <summary>
        /// 获取统计查询结果
        /// </summary>
        /// <param name="Ctype"></param>
        /// <returns></returns>
        public DataSet GetTotal(int Ctype, Search search)
        {
            return DAL.GetTotal(Ctype, search);
        }
    }
}
