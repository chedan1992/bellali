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
    public class BllWashCar : BllBase<ModWashCar>
    {
        private IWashCar DAL = CreateDalFactory.CreateWashCar();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 根据 汽配商id和汽车id查询
        /// </summary>
        /// <param name="uid">汽配商id</param>
        /// <param name="cid">汽车id</param>
        /// <returns></returns>
        public ModWashCar GetWashCar(string uid, string cid)
        {
            return DAL.GetWashCar(uid, cid);
        }
    }
}
