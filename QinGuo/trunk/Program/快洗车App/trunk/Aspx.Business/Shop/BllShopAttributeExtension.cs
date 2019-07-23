using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllShopAttributeExtension : BllBase<ModShopAttributeExtension>
    {
        private IShopAttributeExtension DAL = CreateDalFactory.CreateShopAttributeExtension();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
