using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysHatcity : IBaseDAL<ModSysHatcity>
    {
        /// <summary>
        /// 获取省份下的城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ModSysHatcity> QueryList(string ProviceId);

          /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int UpdateStatus(int flag, string key);

        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        ModSysHatcity QuerCityName(string cityName);
    }
}
