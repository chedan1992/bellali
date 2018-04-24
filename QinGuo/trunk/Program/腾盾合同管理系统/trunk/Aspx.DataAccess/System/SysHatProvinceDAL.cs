using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using System.Data;

namespace QINGUO.DAL
{
    public class SysHatProvinceDAL : BaseDAL<ModSysHatProvince>, ISysHatProvince
    {
        /// <summary>
        /// 获取公司省市区
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public DataSet GetArea(string companyId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.Province,a.CityId,a.AreaId,b.Name as ProvinceName,c.Name as CityName,d.Name as AreaName from Sys_Company as a");
            sb.Append(" left join Sys_HatProvince as b on a.Province=b.Code left join Sys_HatCity as c on a.CityId=c.Code");
            sb.Append(" left join Sys_HatArea as d on a.AreaId=d.Code where a.Id='" + companyId + "'");
            return dabase.ExecuteDataSet(sb.ToString());
        }

        /// <summary>
        /// 获得启用的省份列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataSet GetTreeList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id as id,Name as text,leaf='true',expanded='true',Code");
            strSql.Append(" FROM Sys_HatProvince where Status=" + (int)MasterStatusEnum.启用);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            return dabase.ExecuteDataSet(strSql.ToString());
        }
    }
}
