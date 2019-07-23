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
    public class BllCarUser : BllBase<ModCarUser>
    {
        private ICarUser DAL = CreateDalFactory.CreateCarUser();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 根据 汽车车主id和汽车id查询
        /// </summary>
        /// <param name="uid">汽车车主id</param>
        /// <param name="cid">汽车id</param>
        /// <returns></returns>
        public ModCarUser GetCarUser(string uid, string cid)
        {
            return DAL.GetCarUser(uid, cid);
        }
    }
}
