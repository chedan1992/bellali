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
    public class CarEquipmentDAL : BaseDAL<ModCarEquipment>, ICarEquipment
    {

        public List<ModCarEquipment> GetList(string uid)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return dabase.ReadDataBase.Query<ModCarEquipment>("select * from W_Equipment where Status=1 order by CreateTime desc").ToList();
            }
            else
            {
                return dabase.ReadDataBase.Query<ModCarEquipment>("select * from W_Equipment where CreateId=@0 and Status=1 order by CreateTime desc", uid).ToList();
            }
        }
    }
}
