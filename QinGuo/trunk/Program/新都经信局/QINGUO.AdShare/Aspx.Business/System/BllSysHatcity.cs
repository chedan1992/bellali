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
    public class BllSysHatcity : BllBase<ModSysHatcity>
    {
        private ISysHatcity DAL = CreateDalFactory.CreateSysHatcity();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取省份下的城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModSysHatcity> QueryList(string ProviceId)
        {
            return DAL.QueryList(ProviceId);
        }

        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public ModSysHatcity QuerCityName(string cityName)
        {
            return DAL.QuerCityName(cityName);
        }

          /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            return DAL.UpdateStatus(flag,key);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"Sys_HatCity";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.AddCondition("Status!=" + (int)StatusEnum.删除);//过滤条件
            search.StatusDefaultCondition = "";
            search.SortField = "Id asc";
            return base.QueryPageToJson(search);
        }
    }
}
