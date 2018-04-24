using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using System.Data;

namespace QINGUO.IDAL
{
    public interface ISysCollection : IBaseDAL<ModSysCollection>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Dapper.Page<ModEDynamic> getEDynamicSearch(Common.Search search);

        /// <summary>
        /// 删除用户收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <returns></returns>
        int DeleteCollid(string userid, string collid, CollectionEnum type);

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ModSysCollection Exit(string userid, string collid, CollectionEnum type);

        /// <summary>
        /// 统计用户收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int getNum(string userid, CollectionEnum type);
    }
}
