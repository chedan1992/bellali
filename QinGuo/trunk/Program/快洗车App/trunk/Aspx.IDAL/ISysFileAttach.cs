using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysFileAttach : IBaseDAL<ModSysFileAttach>
    {
          /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="KeyId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ModSysFileAttach> GetList(string where);
    }
}
