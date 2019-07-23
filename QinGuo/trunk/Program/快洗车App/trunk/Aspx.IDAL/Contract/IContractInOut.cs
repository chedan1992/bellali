using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Common;
using System.Data;

namespace QINGUO.IDAL
{
    public interface IContractInOut : IBaseDAL<ModContractInOut>
    {
           /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModContractInOut GetModelByWhere(string where);
        /// <summary>
        /// 导出获取列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="where"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        DataSet GetAllList(Search search, string where, int ctype);

         /// <summary>
        /// 获取统计查询结果
        /// </summary>
        /// <param name="Ctype"></param>
        /// <returns></returns>
        DataSet GetTotal(int Ctype, Search search);
    }
}
