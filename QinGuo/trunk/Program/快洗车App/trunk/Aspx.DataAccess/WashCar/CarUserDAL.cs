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
    public class CarUserDAL : BaseDAL<ModCarUser>, ICarUser
    {
        public ModCarUser GetCarUser(string uid, string cid)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModCarUser>("select * from W_CarUser where CreateId=@0 and CId=@1 and Status=1", uid, cid);
        }
    }
}
