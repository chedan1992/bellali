using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;

namespace QINGUO.Business
{
    public class BllSysPushUser : BllBase<ModSysPushUser>
    {
        private ISysPushUser DAL = CreateDalFactory.CreateSysPushUser();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
