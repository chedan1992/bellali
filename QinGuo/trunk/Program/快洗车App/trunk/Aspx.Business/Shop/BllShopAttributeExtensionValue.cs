using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllShopAttributeExtensionValue : BllBase<ModShopAttributeExtensionValue>
    {
        private IShopAttributeExtensionValue DAL = CreateDalFactory.CreateShopAttributeExtensionValue();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
