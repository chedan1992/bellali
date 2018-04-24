using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using QINGUO.DataAccessBase;
using Dapper;
using QINGUO.Common;
using QINGUO.ViewModel;

namespace QINGUO.DAL
{
    public class SysQRCodeDAL : BaseDAL<ModSysQRCode>, ISysQRCode
    {
        public List<ModSysQRCode> GetSysIdList(string SysId, string where)
        {
            string sql = "select * from E_ElevatorBrand where Status=" + (int)StatusEnum.正常;// SysId=@0 and
            if (where != "")
            {
                sql += where;
            }
            sql += " order by CreateTime desc";
            return dabase.ReadDataBase.Query<ModSysQRCode>(sql).ToList();
        }


        /// <summary>
        /// 查询是否可以删除品牌
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet CanDelete(string BrandId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select");
            sb.Append(" DocumentCount=(select count(Id) from E_Document where BrandId='" + BrandId + "' and Status!=-1),");
            sb.Append(" ElevatorCount=(select COUNT(Id) from E_Elevator where Brand='" + BrandId + "' and Status!=-1),");
            sb.Append(" CompanyCount=(select COUNT(Id) from Sys_Company where CHARINDEX ('" + BrandId + "',LawyerPhone,0)>0)");
            return dabase.ExecuteDataSet(sb.ToString());
        }

        //
        /// <summary>
        /// 后台统计--查询品牌下电梯数量查询
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sysId"></param>
        /// <param name="SumCount"></param>
        /// <returns></returns>
        public Page<ModChartElevatorBrandView> QueryPage(Search search, string sysId, ref int SumCount)
        {
            search.SelectedColums = @"*";//列名
            search.KeyFiled = "Id";//主键  
            search.StatusDefaultCondition = "";

            string sql = search.SqlString;

            if (string.IsNullOrEmpty(search.GetConditon()))
            {
                sql += " 1=1";
            }
            sql += " order by CountNum desc";

            string sqlCount = "select count(1) from " + search.TableName;
            if (!string.IsNullOrEmpty(search.GetConditon()))
            {
                sqlCount += " where " + search.GetConditon();
            }
            SumCount = Convert.ToInt32(dabase.ExecuteScalar(sqlCount));

            return dabase.ReadDataBase.Page<ModChartElevatorBrandView>(search.CurrentPageIndex, search.PageSize, sql);
        }

        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int InsertData(ModSysQRCode t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into E_ElevatorBrand(Id,Name,Status,SysId,CreateTime,Img,Jianpin)");
            sb.Append("  values(");
            sb.Append("'" + t.Id + "',");
            sb.Append("'" + t.Name + "',");
            sb.Append("'" + t.Status + "',");
            sb.Append("'" + t.SysId + "',");
            sb.Append("'" + t.CreateTime + "',");
            sb.Append("'" + t.Img + "',");
            sb.Append("dbo.f_GetPY('" + t.Name + "')");
            sb.Append(")");
            return dabase.ExecuteNonQuery(sb.ToString());
        }


        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int EditData(ModSysQRCode t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("update E_ElevatorBrand set");
            sb.Append(" Name='" + t.Name + "',");
            sb.Append(" Img='" + t.Img + "',");
            sb.Append(" Jianpin=dbo.f_GetPY('" + t.Name + "')");
            sb.Append(" where Id='" + t.Id + "'");
            return dabase.ExecuteNonQuery(sb.ToString());
        }


        /// <summary>
        /// 修改二维码状态（根据QRCode）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int EditStatus(string QRCode)
        {
            return dabase.WriteDataBase.Execute("update Sys_QRCode set Status=1 where QrCode=@0", QRCode);
        }

        /// <summary>
        /// 根据二维码或者编号查询
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public ModSysQRCode GetQRCodeOrName(string p)
        {
            return dabase.ReadDataBase.FirstOrDefault<ModSysQRCode>("select * from Sys_QRCode where Name=@0 or 'XF'+QrCode+'S'=@1", p, p);
        }
    }
}
