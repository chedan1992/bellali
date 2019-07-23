using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;

namespace QINGUO.Business
{
    public class BllSysCompanyCognate : BllBase<ModSysCompanyCognate>
    {
        ISysCompanyCognate DAL = CreateDalFactory.CreateSysCompanyCognate();
        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 查询消防部门，维保单位，供应商下的单位列表
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ModSysCompanyCognate> GetInCompany(string CId, int type)
        {
            return DAL.GetInCompany(CId, type);
        }

        /// <summary>
        /// 查询消防部门，维保单位，供应商下的单位列表
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetInCompanyResult(string CId, int type)
        {
            var r = DAL.GetInCompany(CId, type);
            StringBuilder sb = new StringBuilder();
            int i=1;
            foreach (var item in r)
            {
                if (i == r.Count) {
                    sb.Append("'" + item.EmployerId + "'");
                }else{
                    sb.Append("'" + item.EmployerId + "',");
                }
                i++;
            }
            return sb.ToString();
        }


        /// <summary>
        /// 根据单位id查询对应的消防部门，维保单位，供应商列表
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ModSysCompanyCognate> GetInEmployerId(string CId, int type)
        {
            return DAL.GetInEmployerId(CId, type);
        }

        /// <summary>
        /// 根据单位id查询对应的消防部门，维保单位，供应商列表
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetInEmployerIdResult(string CId, int type)
        {
            var r = DAL.GetInEmployerId(CId, type);
            StringBuilder sb = new StringBuilder();
            int i = 1;
            foreach (var item in r)
            {
                if (i == r.Count)
                {
                    sb.Append("'" + item.CId + "'");
                }
                else
                {
                    sb.Append("'" + item.CId + "',");
                }
                i++;
            }
            return sb.ToString();
        }


        /// <summary>
        /// 后台分页查询关联单位
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_SysCompanyCognate";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            //search.AddCondition("Status=" + (int)StatusEnum.禁用);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "ApprovalTime desc";
            return base.QueryPageToJson(search);
        }
    }
}
