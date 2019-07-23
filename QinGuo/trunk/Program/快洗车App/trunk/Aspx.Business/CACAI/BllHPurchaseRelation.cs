using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Factory;
using QINGUO.Model;

namespace QINGUO.Business
{
    public class BllHPurchaseRelation : BllBase<ModHPurchaseRelation>
    {
        private IHPurchaseRelation DAL = CreateDalFactory.CreateHPurchaseRelation();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
