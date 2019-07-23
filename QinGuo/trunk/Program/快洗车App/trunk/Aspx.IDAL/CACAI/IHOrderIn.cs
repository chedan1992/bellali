using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface IHOrderIn : IBaseDAL<ModHOrderIn>
    {
        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        DataSet GetAllList(string IdList);
    }
}
