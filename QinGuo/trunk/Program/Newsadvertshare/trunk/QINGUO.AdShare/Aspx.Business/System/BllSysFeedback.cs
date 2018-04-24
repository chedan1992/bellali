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
    public class BllSysFeedback : BllBase<ModSysFeedback>
    {
        private ISysFeedback DAL = CreateDalFactory.CreateSysFeedback();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取意见反馈分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"v_Feedback_User";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";
            return base.QueryPageToJson(search);
        }
    }
}
