using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllSysHatarea : BllBase<ModSysHatArea>
    {
        private ISysHatarea DAL = CreateDalFactory.CreateSysHatarea();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获取省份下的城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModSysHatArea> QueryList(string CityId)
        {
            return DAL.QueryList(CityId);
        }
    }
}
