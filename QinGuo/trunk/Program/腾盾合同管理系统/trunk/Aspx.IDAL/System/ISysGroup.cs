using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysGroup : IBaseDAL<ModSysGroup>
    {
          /// <summary>
        /// 获得系统班级树形列表
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        DataSet GetTreeList(string strWhere);

        List<ModSysGroup> GetGroupList(string cid);
    }
}
