using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.IDAL;
using System.Data;
using QINGUO.Common;

namespace QINGUO.DAL
{
    public class ContractInOutDAL : BaseDAL<ModContractInOut>, IContractInOut
    {

        /// <summary>
        /// 查询视图实体
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModSysUser</returns>
        public ModContractInOut GetModelByWhere(string where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from Con_ContractInOut");
            sb.Append("  where 1=1 " + where);
            return dabase.ReadDataBase.SingleOrDefault<ModContractInOut>(sb.ToString());
        }
        /// <summary>
        /// 导出获取列表数据
        /// </summary>
        /// <param name="search"></param>
        /// <param name="where"></param>
        /// <param name="ctype"></param>
        /// <returns></returns>
        public DataSet GetAllList(Search  search,string where,int ctype)
        {
            string sql = @"select "+search.SelectedColums+" from view_Con_ContractInOut where 1=1 and CType="+ctype;
            if (!string.IsNullOrEmpty(where))
            {
                sql += " and "+ where;
            }
            if (!string.IsNullOrEmpty(search.GetConditon()))
            {
                sql += " and " + search.GetConditon();
            }
            sql += " order by CreateTime asc";
            return dabase.ExecuteDataSet(sql);
        }

        /// <summary>
        /// 获取统计查询结果
        /// </summary>
        /// <param name="Ctype"></param>
        /// <returns></returns>
        public DataSet GetTotal(int Ctype, Search search)
        {
            string sql = "";
            switch (Ctype)
            { 
                case 1:
                      sql = @"select SUM(CAST(TotalContractAmount as money)) as TotalContractAmount,
SUM(CAST(ReceivablesAmount as money)) as ReceivablesAmount,
SUM(CAST(InvoiceValueHas as money)) as TotalInvoice,
SUM(CAST(ReceiptsTotalAmount as money)) as ReceiptsTotalAmount,
SUM(CAST(InvoiceValueBefore as money)) as InvoiceValueBefore,
SUM(CAST(cdefine1 as money)) as cdefine1,
SUM(CAST(cdefine2 as money)) as cdefine2,
SUM(CAST(cdefine3 as money)) as cdefine3

 from Con_ContractInOut where 1=1 and CType=" + Ctype;
                    break;
                case 2:
                case 5:
                case 6:
                    sql = @"select SUM(CAST(TotalContractAmount as money)) as TotalContractAmount,
SUM(CAST(ReceivablesAmount as money)) as ReceivablesAmount,
SUM(CAST(TotalInvoice as money)) as TotalInvoice,
SUM(CAST(ReceiptsTotalAmount as money)) as ReceiptsTotalAmount,
SUM(CAST(InvoiceValueBefore as money)) as InvoiceValueBefore,
SUM(CAST(InvoiceValueHas as money)) as InvoiceValueHas,
SUM(CAST(cdefine1 as money)) as cdefine1,
SUM(CAST(cdefine2 as money)) as cdefine2,
SUM(CAST(cdefine3 as money)) as cdefine3
 from Con_ContractInOut where 1=1 and CType=" + Ctype;
                    break;
                case 3:
                    sql = @"select SUM(CAST(TotalContractAmount as money)) as TotalContractAmount,
SUM(CAST(ReceivablesAmount as money)) as ReceivablesAmount,
SUM(CAST(TotalInvoice as money)) as TotalInvoice,
SUM(CAST(ReceiptsTotalAmount as money)) as ReceiptsTotalAmount,
SUM(CAST(InvoiceValueBefore as money)) as InvoiceValueBefore,
SUM(CAST(cdefine1 as money)) as cdefine1,
SUM(CAST(cdefine2 as money)) as cdefine2,
SUM(CAST(cdefine3 as money)) as cdefine3
 from Con_ContractInOut where 1=1 and CType=" + Ctype;
                    break;
                case 4:
                    break;
            }
            if (!string.IsNullOrEmpty(search.GetConditon()))
            {
                sql += " and " + search.GetConditon();
            }
            return dabase.ExecuteDataSet(sql);
        }
    }
}
