using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllShopAttributeType : BllBase<ModShopAttributeType>
    {
        private IShopAttributeType DAL = CreateDalFactory.CreateShopAttributeType();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
