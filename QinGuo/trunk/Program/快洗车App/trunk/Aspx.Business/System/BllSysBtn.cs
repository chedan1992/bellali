using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Common;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllSysBtn : BllBase<ModSysBtnValue>
    {
        ISysBtnValue dal = CreateDalFactory.CreateDalSysBtnValue();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        /// <summary>
        /// 获取按钮分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"Sys_BtnValue";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Sys_BtnValue.Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime asc";//排序
            return base.QueryPageToJson(search);
        }


        /// <summary>
        /// 获得按钮选择左边列表
        /// </summary>
        /// <param name="Id">页面导航id</param>
        /// <returns></returns>
        public string GetBtnLeftSelect(string Id)
        {
            return JsonHelper.DataTableToJsonForGridDataSuorce(dal.GetBtnLeftSelect(Id).Tables[0]);
        }

        /// <summary>
        /// 获得按钮选择右边列表
        /// </summary>
        /// <param name="Id">页面导航id</param>
        /// <returns></returns>
        public string GetBtnRightSelect(string Id)
        {
            return JsonHelper.DataTableToJsonForGridDataSuorce(dal.GetBtnRightSelect(Id).Tables[0]);
        }

        /// <summary>
        /// 根据页面主键,获取页面的按钮
        /// </summary>
        /// <param name="key">页面主键</param>
        /// <returns></returns>
        public DataSet GetBtnByPage(string key)
        {
            return dal.GetBtnByPage(key);
        }

        /// <summary>
        /// 根据主键软删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int DeleteDate(string key)
        {
            return dal.DeleteDate(key);
        }
    }
}
