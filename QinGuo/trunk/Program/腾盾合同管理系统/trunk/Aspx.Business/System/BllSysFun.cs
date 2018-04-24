using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllSysFun : BllBase<ModSysFun>
    {
        private ISysFun DAL = CreateDalFactory.CreateDalSysFun();


        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 获得数据列表　GetTreeList
        /// </summary>
        public DataSet GetTreeList(string strWhere)
        {
            return DAL.GetTreeList(strWhere);
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int UpdateData(ModSysFun t)
        {
            try
            {
                DAL.UpdateDate(t);
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int UpdateIsStatus(int status, string key)
        {
            return DAL.UpdateIsStatus(status, key);
        }

        /// <summary>
        /// 根据主键软删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int DeleteDate(string key)
        {
            return DAL.DeleteDate(key);
        }
    }
}
