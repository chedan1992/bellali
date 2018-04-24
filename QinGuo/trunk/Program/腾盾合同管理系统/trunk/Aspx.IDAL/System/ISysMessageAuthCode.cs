using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    /// <summary>
    /// 短信验证码操作类
    /// </summary>
    public interface ISysMessageAuthCode : IBaseDAL<ModSysMessageAuthCode>
    {
          /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModSysMessageAuthCode GetModelByWhere(string where);

         /// <summary>
        /// 修改验证码状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateStatus(int status, string key);
    }
}
