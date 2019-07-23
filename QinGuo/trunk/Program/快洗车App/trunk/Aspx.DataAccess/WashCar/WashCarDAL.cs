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
    public class WashCarDAL : BaseDAL<ModWashCar>, IWashCar
    {

        public ModWashCar GetWashCar(string uid, string cid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return dabase.ReadDataBase.FirstOrDefault<ModWashCar>("select * from W_WashCar where CId=@0 and Status=1 order by CreateTime desc", cid);
            }
            else {
                return dabase.ReadDataBase.FirstOrDefault<ModWashCar>("select * from W_WashCar where CreateId=@0 and CId=@1 and Status=1", uid, cid);
            }
        }
    }
}
