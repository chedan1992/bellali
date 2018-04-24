using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysHatProvince : IBaseDAL<ModSysHatProvince>
    {
          /// <summary>
        /// 获取公司省市区
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        DataSet GetArea(string companyId);

          /// <summary>
        /// 获得启用的省份列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        DataSet GetTreeList(string strWhere);
    }
}
