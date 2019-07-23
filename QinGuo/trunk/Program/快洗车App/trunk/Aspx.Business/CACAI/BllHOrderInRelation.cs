using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllHOrderInRelation : BllBase<ModHOrderInRelation>
    {
        private IHOrderInRelation DAL = CreateDalFactory.CreateHOrderInRelation();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
