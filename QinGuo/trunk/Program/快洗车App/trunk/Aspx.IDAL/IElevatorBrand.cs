using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using Dapper;
using QINGUO.IDAL;
using QINGUO.ViewModel;
using QINGUO.Common;

namespace QINGUO.IDAL
{
    public interface IElevatorBrand : IBaseDAL<ModEElevatorBrand>
    {
        List<ModEElevatorBrand> GetSysIdList(string SysId, string where);

        /// <summary>
        /// 查询是否可以删除品牌
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataSet CanDelete(string BrandId);

        /// <summary>
        /// 查询品牌下电梯数量查询
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sysId"></param>
        /// <param name="SumCount"></param>
        /// <returns></returns>
        Page<ModChartElevatorBrandView> QueryPage(Search search, string sysId, ref int SumCount);

        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int InsertData(ModEElevatorBrand t);

        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int EditData(ModEElevatorBrand t);
    }
}
