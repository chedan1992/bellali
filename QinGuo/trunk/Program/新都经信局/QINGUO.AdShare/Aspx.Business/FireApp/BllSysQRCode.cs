using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using QINGUO.Common;
using QINGUO.IDAL;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.ViewModel;
using Dapper;

namespace QINGUO.Business
{
    public class BllSysQRCode : BllBase<ModSysQRCode>
    {
        ISysQRCode dal = CreateDalFactory.CreateDalSysQRCode();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = dal;
        }

        public List<ModSysQRCode> GetSysIdList(string SysId, string where)
        {
            return dal.GetSysIdList(SysId, where);
        }

         /// <summary>
        /// 查询是否可以删除品牌
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet CanDelete(string BrandId)
        {
            return dal.CanDelete(BrandId);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search">查询集合</param>
        /// <returns></returns>
        public string SearchData(Search search)
        {
            search.TableName = @"Sys_QRCode";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "CreateTime desc";
            return base.QueryPageToJson(search);
        }

          //
          /// <summary>
        /// 查询品牌下电梯数量查询
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sysId"></param>
        /// <param name="SumCount"></param>
        /// <returns></returns>
        public string SearchData(Search search, string sysId)
        {
            int SumCount = 0;
            Page<ModChartElevatorBrandView> pageModel = dal.QueryPage(search, sysId, ref SumCount);
            if (pageModel != null)
            {
                List<ModChartElevatorBrandView> statisList = pageModel.Items;
                var data = new { total = SumCount, rows = statisList };
                return JsonHelper.ToJson(data);
            }
            else
            {
                return "";
            }
        }

          /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int InsertData(ModSysQRCode t)
        {
            return dal.InsertData(t);
        }


        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int EditData(ModSysQRCode t)
        {
            return dal.EditData(t);
        }

        /// <summary>
        /// 修改二维码状态（根据QRCode）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int EditStatus(string QRCode)
        {
            return dal.EditStatus(QRCode);
        }

        /// <summary>
        /// 根据二维码或者编号查询
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ModSysQRCode GetQRCodeOrName(string p)
        {
            return dal.GetQRCodeOrName(p);
        }

    }
}
