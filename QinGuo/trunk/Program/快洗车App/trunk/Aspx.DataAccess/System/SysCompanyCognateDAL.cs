using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.DataAccessBase;
using QINGUO.IDAL;

namespace QINGUO.DAL
{
    public class SysCompanyCognateDAL : BaseDAL<ModSysCompanyCognate>, ISysCompanyCognate
    {

        /// <summary>
        /// 查询消防部门，维保单位，供应商下的单位列表
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ModSysCompanyCognate> GetInCompany(string CId, int type)
        {
            return dabase.ReadDataBase.Query<ModSysCompanyCognate>("select * from Sys_CompanyCognate where CId=@0 and Status=1", CId).ToList();
        }

        /// <summary>
        /// 根据单位id查询对应的消防部门，维保单位，供应商列表
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<ModSysCompanyCognate> GetInEmployerId(string CId, int type)
        {
            return dabase.ReadDataBase.Query<ModSysCompanyCognate>("select * from Sys_CompanyCognate where EmployerId=@0 and Status=1", CId).ToList();
        }
    }
}
