using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;

namespace QINGUO.IDAL
{
    public interface ISysRoleRangeData : IBaseDAL<ModSysRoleRangeData> 
    {
        /// <summary>
        /// 根据角色类型获取所有数据集合
        /// </summary>
        /// <param name="RoleId">角色类型编号</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        DataSet GetAllData(string RoleId, string where);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        int UpdateData(ModSysRoleRangeData model);

         /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        int DeleteData(string key);
    }
}
