using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.Factory;

namespace QINGUO.Business
{
    public class BllSysRoleRangeData : BllBase<ModSysRoleRangeData> 
    {
        ISysRoleRangeData DAL = CreateDalFactory.CreateDalSysRoleRangeData();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }

        /// <summary>
        /// 根据角色类型获取所有数据集合
        /// </summary>
        /// <param name="RoleId">角色类型编号</param>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public DataSet GetAllData(string RoleId, string where)
        {
            return DAL.GetAllData(RoleId, where);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">实体类</param>
        public int UpdateData(ModSysRoleRangeData model)
        {
            return DAL.UpdateData(model);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public int DeleteData(string key)
        {
            return DAL.DeleteData(key);
        }
    }
}
