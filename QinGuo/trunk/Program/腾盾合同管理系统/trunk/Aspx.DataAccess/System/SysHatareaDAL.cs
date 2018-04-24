using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;

namespace QINGUO.DAL
{
    public class SysHatareaDAL : BaseDAL<ModSysHatArea>, ISysHatarea
    {
        /// <summary>
        /// 获取省份下的城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModSysHatArea> QueryList(string CityId)
        {
            string sql = "select * from Sys_HatArea where 1=1";
            if (!string.IsNullOrEmpty(CityId))
            {
                sql += " and CityCode='" + CityId + "'";
            }
            return dabase.ReadDataBase.Query<ModSysHatArea>(sql, null).ToList();
        }

    }
}
