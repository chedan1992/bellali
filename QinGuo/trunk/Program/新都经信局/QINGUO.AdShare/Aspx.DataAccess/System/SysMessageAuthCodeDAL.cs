using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;

namespace QINGUO.DAL
{
    public class SysMessageAuthCodeDAL : BaseDAL<ModSysMessageAuthCode>, ISysMessageAuthCode
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModSysMessageAuthCode GetModelByWhere(string where)
        {
            return dabase.ReadDataBase.SingleOrDefault<ModSysMessageAuthCode>("select * from Sys_MessageAuthCode where 1=1 " + where + " order by CreateTime desc");
        }

        /// <summary>
        /// 修改验证码状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateStatus(int status,string key)
        {
            DataParameters paras = new DataParameters();
            paras.Add("@Id", key);
            paras.Add("@MsgState", status);

            int result = dabase.ExecuteNonQueryByText("update Sys_MessageAuthCode set MsgState=@MsgState where Id=@Id", paras);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
