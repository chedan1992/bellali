using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.IDAL;
using Dapper;
using QINGUO.Common;

namespace QINGUO.DAL
{
    public class SysCollectionDAL : BaseDAL<ModSysCollection>, ISysCollection
    {

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModEDynamic> getEDynamicSearch(Search search)
        {
            return dabase.ReadDataBase.Page<ModEDynamic>(search.CurrentPageIndex, search.PageSize, search.SqlString2);
        }



        /// <summary>
        /// 删除用户收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <returns></returns>
        public int DeleteCollid(string userid, string collid, CollectionEnum type)
        {
            return dabase.WriteDataBase.Execute("DELETE FROM Sys_Collection WHERE CreaterId=@0 and CollId=@1 and Type=@2", userid, collid, (int)type);
        }

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ModSysCollection Exit(string userid, string collid, CollectionEnum type)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModSysCollection>("select * FROM Sys_Collection WHERE CreaterId=@0 and CollId=@1 and Type=@2", userid, collid, (int)type);
        }

        /// <summary>
        /// 统计用户收藏
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="collid"></param>
        /// <returns></returns>
        public int getNum(string userid, CollectionEnum type)
        {
            return dabase.ReadDataBase.ExecuteScalar<int>("select count(Id) FROM Sys_Collection WHERE CreaterId=@0 and Type=@1", userid, (int)type);
        }
    }
}
