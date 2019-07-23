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
    public class BllCarProject : BllBase<ModCarProject>
    {
        private ICarProject DAL = CreateDalFactory.CreateCarProject();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        public List<ModCarProject> GetList()
        {
            return DAL.GetList();
        }
    }
}
