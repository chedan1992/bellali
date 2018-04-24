using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.IDAL
{
    public interface IAdActive : IBaseDAL<ModAdActive>
    {
         /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Exists(string where);
          /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        ModAdActiveView GetModelByWhere(string where);
         /// <summary>
        /// 获取最新的广告列表
        /// </summary>
        /// <param name="IwillId"></param>
        /// <returns></returns>
        List<ModAdActive> QueryAll(int? top);
        /// <summary>
        /// 启用/停用
        /// </summary>
        /// <param name="flag">状态</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int UpdateStatus(int flag, string key);

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Dapper.Page<ModAdActiveView> Search(Common.Search search);

        /// <summary>
        /// 广告过期
        /// </summary>
        /// <returns></returns>
        int Expired();
         /// <summary>
        /// 根据用户编号获取用户名称
        /// </summary>
        /// <param name="Idlist"></param>
        /// <returns></returns>
        DataSet GetPersonalName(string Idlist);
    }
}
