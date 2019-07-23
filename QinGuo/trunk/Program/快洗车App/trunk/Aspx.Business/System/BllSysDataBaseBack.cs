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
    public class BllSysDataBaseBack : BllBase<ModSysDataBaseBack>
    {
        private ISysDataBaseBack DAL = CreateDalFactory.CreateSysDataBaseBack();

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
            search.TableName = @"Sys_DataBaseBack";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status=" + (int)StatusEnum.正常);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }

          /// <summary>
        /// 数据库备份
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int BackDataBase(string path)
        {
            return DAL.BackDataBase(path);
        }

          /// <summary>
        /// 数据库还原
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public int Rollback(string path)
        {
            return DAL.Rollback(path);
        }
    }
}
