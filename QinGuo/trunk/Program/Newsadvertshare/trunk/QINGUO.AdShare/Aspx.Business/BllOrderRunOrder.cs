using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Factory;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.ViewModel;
using QINGUO.Common;
using System.Data;

namespace QINGUO.Business
{
    public class BllOrderRunOrder : BllBase<ModOrderRunOrder>
    {
        private IOrderRunOrder DAL = CreateDalFactory.CreateOrderRunOrder();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
          /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModviewOrderList GetModelByWhere(string where)
        {
            return DAL.GetModelByWhere(where);
        }

        /// <summary>
        /// 查询所有取消订单列表
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModCompanyOrder</returns>
        public string GetCancelSearchData(Search search)
        {
            search.TableName = @"view_CancelOrderList";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }


        /// <summary>
        /// 查询所有订单列表
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModCompanyOrder</returns>
        public string GetAllList(Search search)
        {
            search.TableName = @"view_OrderList as a inner join Sys_User as b on a.OrderUserId=b.Id";//表名
            search.KeyFiled = "a.Id";//主键
            search.SelectedColums = "a.*,b.Nickname as OrderName,b.Tel";
            search.AddCondition("FlowStatus>0");//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }

        /// <summary>
        ///  获取司机接的最新订单信息
        /// </summary>
        /// <param name="search">查询数据</param>
        /// <param name="total">总条数</param>
        /// <param name="top">最新的第几条</param>
        /// <param name="UserId">用户编号</param>
        /// <returns></returns>
        public List<ModviewOrderList> GetOrderList(Search search, out int total, int? top, string UserId)
        {
            if (top > 0)
            {
                total = 0;
                List<ModviewOrderList> model = DAL.GetOrderList(search,UserId, top);
                return model;
            }
            else
            {
                List<ModviewOrderList> model = SearchData(search, out total);
                return model;
            }
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModCompanyOrder</returns>
        public List<ModviewOrderList> SearchData(Search search, out int total)
        {
            search.TableName = @"view_OrderList";//表名
            search.KeyFiled = "Id";//主键
            DataSet ds = QueryPageToDataSet(out total, search);
            List<ModviewOrderList> model = CommonFunction.DataSetToList<ModviewOrderList>(ds, 0);
            return model;
        }


          /// <summary>
        /// 查询当前用户是否还有未支付的订单，如果没有，就不能再次订单添加
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int IsNoPayOrder(string UserId)
        {
            return DAL.IsNoPayOrder(UserId);
        }

          /// <summary>
        /// 订单取消
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int CancelOrder(string keyId)
        {
            return DAL.CancelOrder(keyId);
        }
    }
}
