using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Common;
using QINGUO.ViewModel;
using System.Data;

namespace QINGUO.Business
{
    public class BllCarEquipment : BllBase<ModCarEquipment>
    {
        private ICarEquipment DAL = CreateDalFactory.CreateCarEquipment();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        public List<ModCarEquipment> GetList(string uid)
        {
            return DAL.GetList(uid);
        }
    }
}
