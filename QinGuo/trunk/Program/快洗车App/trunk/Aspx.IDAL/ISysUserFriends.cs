using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysUserFriends : IBaseDAL<ModSysUserFriends>
    {
        Dapper.Page<ModSysMaster> GetFriends(Common.Search search);
    }
}
