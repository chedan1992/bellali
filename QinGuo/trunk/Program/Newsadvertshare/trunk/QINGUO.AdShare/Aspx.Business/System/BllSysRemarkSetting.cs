using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllSysRemarkSetting : BllBase<ModSysRemarkSetting>
    {
        private ISysRemarkSetting DAL = CreateDalFactory.CreateSysRemarkSetting();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
