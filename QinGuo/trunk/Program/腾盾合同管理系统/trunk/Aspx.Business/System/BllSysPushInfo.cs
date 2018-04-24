using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;

namespace QINGUO.Business
{
    public class BllSysPushInfo : BllBase<ModSysPushInfo>
    {
        private ISysPushInfo DAL = CreateDalFactory.CreateSysPushInfo();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
