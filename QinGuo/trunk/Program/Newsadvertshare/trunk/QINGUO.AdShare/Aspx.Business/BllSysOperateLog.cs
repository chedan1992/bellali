using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using System.Data;
using System.Threading;

namespace QINGUO.Business
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public class BllSysOperateLog : BllBase<ModSysOperateLog>
    {
        private ISysOperateLog DAL = CreateDalFactory.CreateSysOperateLog();

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
            search.TableName = @"Sys_OperateLog";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }

         /// <summary>
        /// 导出日志数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet ExportOut(string where)
        {
            return DAL.ExportOut(where);
        }


         /// <summary>
        /// 清空
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int DeleteAll(string where)
        {
            return DAL.DeleteAll(where);
        }
    }
}
