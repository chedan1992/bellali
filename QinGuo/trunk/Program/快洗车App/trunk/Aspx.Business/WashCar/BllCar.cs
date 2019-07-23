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
    public class BllCar : BllBase<ModCar>
    {
        private ICar DAL = CreateDalFactory.CreateCar();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        public List<ModCar> getUserId(string userid)
        {
            return DAL.getUserId(userid);
        }

        public bool Exit(string LicensePlate)
        {
            return DAL.Exit(LicensePlate);
        }

        public ModCar getLicensePlate(string licensePlate)
        {
            return DAL.getLicensePlate(licensePlate);
        }
    }
}
