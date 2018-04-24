using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.ViewModel;

namespace QINGUO.IDAL
{
    public interface IEDynamic : IBaseDAL<ModEDynamic>
    {
        List<ModEDynamic> getListAll(int? top);

        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int UpdateStatus(int flag, string key);

        /// <summary>
        /// 获取文章分页数据
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        Dapper.Page<ModEDynamic> GetDynamic(Common.Search search);

         /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
         ModViewDynamic GetModelByWhere(string where);
    }
}
