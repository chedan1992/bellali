using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysCompanyCognate : IBaseDAL<ModSysCompanyCognate>
    {
        /// <summary>
        /// 查询消防部门，维保单位，供应商下的单位列表
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<ModSysCompanyCognate> GetInCompany(string CId, int type);

        /// <summary>
        /// 根据单位id查询对应的消防部门，维保单位，供应商列表
        /// </summary>
        /// <param name="CId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<ModSysCompanyCognate> GetInEmployerId(string CId, int type);
    }
}
