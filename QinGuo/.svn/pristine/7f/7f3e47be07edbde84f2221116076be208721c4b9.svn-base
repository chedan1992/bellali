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
            string sql = @"select NumCode,OldNumCode,ContraceName,NatureName,
                              InitiatorUser,InitiatorDept,Agent,Approver,ProjectName,Subsystem,ProductDescription,DeliveriesQuantities,
                               DeliveriesAddress,ProjectPhaseName,ContractStateName,ContractIimplementation,SigningDate,DeliverDate,ValidityDate,Currency,CurrencyUnit,TotalContractAmount,ReceivablesAmount,TotalInvoice,
                               ReceiptsTotalAmount,AgreedMoney1,AgreedTime1,AgreedMoney2,
                               AgreedTime2,AgreedMoney3,AgreedTime3,AgreedMoney4,AgreedTime4,
                               AgreedMoney5,AgreedTime5,FinancialAmount1,FinancialTime1,FinancialAmount2,FinancialTime2,FinancialAmount3,FinancialTime3,
                               FinancialAmount4,FinancialTime4,FinancialAmount5,FinancialTime5,TicketMoney1,
                               TicketTime1,TicketMoney2,TicketTime2,TicketMoney3,TicketTime3,
                                TicketMoney4,TicketTime4,TicketMoney5,TicketTime5,
                                UnitName,UnitAddress,OpeningBank,OpeningAccount,LinkUser,LinType,InvoiceTypeName,TaxRate,
                                InvoiceValueBe,InvoiceValueHas,InvoiceValueBefore,BudgetSituation,AccordingDocument,FilingSituation,
                                cdefine1,cdefine2,cdefine3,cdefine4,cdefine5,cdefine6,cdefine7,cdefine8,cdefine9,cdefine10
                            from view_Con_ContractInOut where 1=1 and CType="+ctype;
            if (!string.IsNullOrEmpty(where))
            {
                sql += " and "+ where;
            }
            if (!string.IsNullOrEmpty(search.GetConditon()))
            {
                sql += " and " + search.GetConditon();
            }
            return dabase.ExecuteDataSet(sql);
        }
    }
}
