using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.ViewModel;
using Dapper;
using QINGUO.Common;

namespace QINGUO.DAL
{
    /// <summary>
    /// 朋友列表
    /// </summary>
    public class SysUserFriendsDAL : BaseDAL<ModSysUserFriends>, ISysUserFriends
    {

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <returns>ModSysUser</returns>
        public Page<ModSysMaster> GetFriends(Search search)
        {
            return dabase.ReadDataBase.Page<ModSysMaster>(search.CurrentPageIndex, search.PageSize, search.SqlString);
        }
    }
}
