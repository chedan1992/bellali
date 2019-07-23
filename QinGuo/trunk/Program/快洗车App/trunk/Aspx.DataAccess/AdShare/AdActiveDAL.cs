using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.ViewModel;
using QINGUO.Common;
using Dapper;
using System.Data;

namespace QINGUO.DAL
{
    public class AAShareDAL : BaseDAL<ModAAShare>, IAAShare
    {
        public List<ModAAShare> getUserId(string userid)
        {
            return dabase.ReadDataBase.Query<ModAAShare>("select * from Ad_AShare where CreaterId=@0 and Status>=0 order by type asc", userid).ToList();
        }
    }
}
