using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysCompanyPaySet : IBaseDAL<ModSysCompanyPaySet>
    {
        ModSysCompanyPaySet getByCId(string Cid);

        /// <summary>
        /// 获取付款方式类型
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        DataSet GetSelectAll(string where);
    }
}
