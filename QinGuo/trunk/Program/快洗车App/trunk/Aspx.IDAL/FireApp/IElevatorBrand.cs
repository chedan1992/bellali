using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Model;
using QINGUO.ViewModel;
using QINGUO.Common;
using Dapper;

namespace QINGUO.IDAL
{
    public interface ISysQRCode : IBaseDAL<ModSysQRCode>
    {
        List<ModSysQRCode> GetSysIdList(string SysId, string where);
         /// <summary>
        /// 查询是否可以删除品牌
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        DataSet CanDelete(string BrandId);

        //
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
        int InsertData(ModSysQRCode t);
    

        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        int EditData(ModSysQRCode t);

        /// <summary>
        /// 修改二维码状态（根据QRCode）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        int EditStatus(string QRCode);

        /// <summary>
        /// 根据二维码或者编号查询
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        ModSysQRCode GetQRCodeOrName(string p);
    }
}
