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
    public class CarDAL : BaseDAL<ModCar>, ICar
    {
        public List<ModCar> getUserId(string userid)
        {
            return dabase.ReadDataBase.Query<ModCar>("select c.* from W_CarUser as a inner join W_Car as c on c.id=a.cid  where a.CreateId=@0 and a.Status=1 order by CreateTime asc", userid).ToList();
        }
        public bool Exit(string LicensePlate)
        {
            return dabase.ReadDataBase.Exists<ModCar>("select * from W_Car where LicensePlate=@0 and Status=1", LicensePlate);
        }

        public ModCar getLicensePlate(string LicensePlate)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModCar>("select * from W_Car where LicensePlate=@0 and Status=1", LicensePlate);
        }
    }
}
