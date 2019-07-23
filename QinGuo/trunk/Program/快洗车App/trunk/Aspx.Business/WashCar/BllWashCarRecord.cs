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
    public class BllWashCarRecord : BllBase<ModWashCarRecord>
    {
        private IWashCarRecord DAL = CreateDalFactory.CreateWashCarRecord();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }
    }
}
