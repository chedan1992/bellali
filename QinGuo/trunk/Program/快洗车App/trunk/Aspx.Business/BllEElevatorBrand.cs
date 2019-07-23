using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.IDAL;
using Dapper;
using QINGUO.Business;
using QINGUO.Factory;
using QINGUO.Common;
using QINGUO.ViewModel;
using QINGUO.Model;

namespace QINGUO.Business
{
    public class BllEElevatorBrand : BllBase<ModEElevatorBrand>
    {
        IElevatorBrand dal = CreateDalFactory.CreateElevatorBrand();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        public List<ModEElevatorBrand> GetSysIdList(string SysId, string where)
        {
            return dal.GetSysIdList(SysId, where);
        }

         /// <summary>
        /// 查询是否可以删除品牌
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet CanDelete(string BrandId)
        {
            return dal.CanDelete(BrandId);
        }

        ///// <summary>
        ///// 列表
        ///// </summary>
        ///// <param name="search">查询集合</param>
        ///// <returns></returns>
        //public string SearchData(Search search)
        //{
        //    search.TableName = @"Chart_ElevatorBrand";//表名
        //    search.KeyFiled = "Id";//主键
        //    search.SelectedColums = "*";
        //    search.StatusDefaultCondition = "";
        //    search.SortField = "CreateTime desc";
        //    return base.QueryPageToJson(search);
        //}

          //
          /// <summary>
        /// 查询品牌下电梯数量查询
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sysId"></param>
        /// <param name="SumCount"></param>
        /// <returns></returns>
        public string SearchData(Search search, string sysId)
        {
            int SumCount = 0;
            Page<ModChartElevatorBrandView> pageModel = dal.QueryPage(search, sysId, ref SumCount);
            if (pageModel != null)
            {
                List<ModChartElevatorBrandView> statisList = pageModel.Items;
                var data = new { total = SumCount, rows = statisList };
                return JsonHelper.ToJson(data);
            }
            else
            {
                return "";
            }
        }

          /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int InsertData(ModEElevatorBrand t)
        {
            return dal.InsertData(t);
        }


        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int EditData(ModEElevatorBrand t)
        {
            return dal.EditData(t);
        }
    }
}
