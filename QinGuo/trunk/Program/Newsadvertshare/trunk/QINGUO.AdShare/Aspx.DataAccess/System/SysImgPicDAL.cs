using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;

namespace QINGUO.DAL
{
    public class SysImgPicDAL : BaseDAL<ModSysImgPic>, ISysImgPic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OtherKeyId"></param>
        /// <returns></returns>
        public List<ModSysImgPic> GetList(string OtherKeyId)
        {
            return dabase.ReadDataBase.Query<ModSysImgPic>("select * from Sys_ImgPic where OtherKeyId=@0 order by Sort asc", OtherKeyId).ToList();
        }
    }
}
