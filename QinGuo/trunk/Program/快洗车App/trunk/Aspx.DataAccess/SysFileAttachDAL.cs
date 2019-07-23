using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.IDAL;
using QINGUO.Model;

namespace QINGUO.DAL
{
    public class SysFileAttachDAL : BaseDAL<ModSysFileAttach>, ISysFileAttach
    {

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ModSysFileAttach> GetList(string where)
        {
            string sql = "select * from Sys_FileAttach where Status=" + (int)StatusEnum.正常;// SysId=@0 and
            if (where != "")
            {
                sql += " and "+where;
            }
            sql += " order by CreateTime desc";
            return dabase.ReadDataBase.Query<ModSysFileAttach>(sql).ToList();
        }
    }
}
