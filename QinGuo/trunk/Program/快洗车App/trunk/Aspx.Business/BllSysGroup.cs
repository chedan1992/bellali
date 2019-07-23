using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using System.Data;

namespace QINGUO.Business
{
    public class BllSysGroup : BllBase<ModSysGroup>
    {
        private ISysGroup DAL = CreateDalFactory.CreateSysGroup();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取类型分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_SysGroup";//表名
            search.SelectedColums = "*";//查询列
            search.KeyFiled = "Id";//主键
            search.AddCondition("Status!=" + (int)StatusEnum.删除);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";//排序
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            return DAL.UpdateStatus(flag, key);
        }

          /// <summary>
        /// 获得系统班级树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            return DAL.GetTreeList(strWhere);
        }
        /// <summary>
        /// 获取类型列表
        /// </summary>
        /// <returns></returns>
        public List<ModSysGroup> GetGroupList(string cid)
        {
            return DAL.GetGroupList(cid);
        }

        public List<ModSysGroup> GetList(string type)
        {
            return DAL.GetList(type);
        }
    }
}
