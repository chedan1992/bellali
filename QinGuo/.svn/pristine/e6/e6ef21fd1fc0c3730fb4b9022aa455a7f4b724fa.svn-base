using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;
using System.Data;

namespace QINGUO.Business
{
    public class BllSysCompanyPaySet : BllBase<ModSysCompanyPaySet>
    {
        private ISysCompanyPaySet DAL = CreateDalFactory.CreateSysCompanyPaySet();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
        /// <summary>
        /// 根据公司id查询支付信息
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public ModSysCompanyPaySet getByCId(string Cid)
        {
            return DAL.getByCId(Cid);
        }

        /// <summary>
        /// 获取付款方式类型
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetSelectAll(string where)
        {
            return DAL.GetSelectAll(where);
        }

        /// <summary>
        /// 获取按钮分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"v_SysCompanyPaySet";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常 + "");//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "AddTime asc";//排序
            return base.QueryPageToJson(search);
        }
    }
}
