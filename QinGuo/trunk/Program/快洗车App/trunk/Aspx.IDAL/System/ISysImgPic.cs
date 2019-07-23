using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysImgPic : IBaseDAL<ModSysImgPic>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OtherKeyId"></param>
        /// <returns></returns>
        List<ModSysImgPic> GetList(string OtherKeyId);
    }
}
