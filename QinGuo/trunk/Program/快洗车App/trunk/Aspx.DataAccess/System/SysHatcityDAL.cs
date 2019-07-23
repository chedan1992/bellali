using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;

namespace QINGUO.DAL
{
    public class SysHatcityDAL : BaseDAL<ModSysHatcity>, ISysHatcity
    {
        /// <summary>
        /// 获取省份下的城市
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ModSysHatcity> QueryList(string ProviceId)
        {
            string sql = "select * from Sys_Hatcity where 1=1";
            if (!string.IsNullOrEmpty(ProviceId))
            {
                sql += " and ProvinceCode='" + ProviceId + "'";
            }
            return dabase.ReadDataBase.Query<ModSysHatcity>(sql, null).ToList();
        }

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateStatus(int flag, string key)
        {
            var parameters = new DataParameters();
            parameters.Add("@Status", flag);
            parameters.Add("@Id", key);

            StringBuilder sb = new StringBuilder();
            sb.Append("update Sys_HatCity set Status=@Status where Id=@Id;");//停用
            try
            {
                dabase.ExecuteNonQueryByText(sb.ToString(), parameters);
                dabase.CommitTransaction();
                return 1;
            }
            catch (Exception ex)
            {
                dabase.RollbackTransaction();
                return 0;
            }
        }

        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public ModSysHatcity QuerCityName(string cityName)
        {
            string sql = "select * from Sys_Hatcity where name like'%"+cityName+"%'";
            return dabase.ReadDataBase.FirstOrDefault<ModSysHatcity>(sql);
        }

    }
}
