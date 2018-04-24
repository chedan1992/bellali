using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllSysImgPic : BllBase<ModSysImgPic>
    {
        private ISysImgPic DAL = CreateDalFactory.CreateSysImgPicDAL();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OtherKeyId"></param>
        /// <returns></returns>
        public List<ModSysImgPic> GetList(string OtherKeyId)
        {
            return DAL.GetList(OtherKeyId);
        }
    }
}