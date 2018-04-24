using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using System.Data;

namespace QINGUO.Business
{
    public class BllSysHatProvince : BllBase<ModSysHatProvince>
    {
        private ISysHatProvince DAL = CreateDalFactory.CreateSysHatProvince();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

          /// <summary>
        /// 获取公司省市区
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataSet GetArea(string companyId)
        {
            return DAL.GetArea(companyId);
        }

          /// <summary>
        /// 获得启用的省份列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            return DAL.GetTreeList(strWhere);
        }
    }
}
