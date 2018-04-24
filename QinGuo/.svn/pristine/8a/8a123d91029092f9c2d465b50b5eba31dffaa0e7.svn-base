using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllSysMessageAuthCode : BllBase<ModSysMessageAuthCode>
    {
        private ISysMessageAuthCode DAL = CreateDalFactory.CreateSysMessageAuthCode();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

          /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysMessageAuthCode GetModelByWhere(string where)
        {
            return DAL.GetModelByWhere(where);
        }

         /// <summary>
        /// 修改验证码状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateStatus(int status, string key)
        {
            return DAL.UpdateStatus(status,key);
        }
    }
}
