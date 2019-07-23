using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPortalAdmin.Controllers;
using QINGUO.Model;
using QINGUO.Business;
using System.Text;
using QINGUO.Common;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Data;
using Aspose.Cells;

namespace WebPortalAdmin.Areas.CACAI.Controllers
{
    /// <summary>
    /// 财务
    /// </summary>
    public class FinanceController : BaseController<ModHFinance>
    {

        string[] title;  //导出的标题
        string[] field;  //导出对应字段

        /// <summary>
        /// 财务单据池
        /// </summary>
        /// <returns></returns>
        public ActionResult Pool()
        {
            return View();
        }

        /// <summary>
        /// 财务付款单
        /// </summary>
        /// <returns></returns>
        public ActionResult PayBill()
        {
            return View();
        }

        #region ===Pool 财务池

        /// <summary>
        /// 获取组合查询条件
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public void GetPoolSearch(ref Search search)
        {
            #region ===面板查询
            //入库单号
            if (!string.IsNullOrEmpty(Request["InNumber"]))
            {
                search.AddCondition("InNumber like '%" + Request["InNumber"].ToString() + "%'");
            }
            //采购单号
            if (!string.IsNullOrEmpty(Request["GetNumber"]))
            {
                search.AddCondition("GetNumber like '%" + Request["GetNumber"].ToString().Trim() + "%'");
            }
            //供应商
            if (!string.IsNullOrEmpty(Request["CusterName"]))
            {
                search.AddCondition("CusterName like '%" + Request["CusterName"].ToString().Trim() + "%'");
            }
            //备注
            if (!string.IsNullOrEmpty(Request["Remark"]))
            {
                search.AddCondition("Remark like '%" + Request["Remark"].ToString().Trim() + "%'");
            }
            //付款方式
            if (!string.IsNullOrEmpty(Request["PaymentType"]))
            {
                if (Request["PaymentType"].ToString() != "-1")
                {
                    search.AddCondition("PaymentType=" + Request["PaymentType"].ToString());
                }
            }
            //结账方式
            if (!string.IsNullOrEmpty(Request["CheckoutType"]))
            {
                if (Request["CheckoutType"].ToString() != "-1")
                {
                    search.AddCondition("CheckoutType=" + Request["CheckoutType"].ToString());
                }
            }
            // 单据日期
            if (!string.IsNullOrEmpty(Request["BegBillDate"]) || !string.IsNullOrEmpty(Request["EndBillDate"]))
            {
                if (!string.IsNullOrEmpty(Request["BegBillDate"]))
                {
                    search.AddCondition("BillDate>='" + Convert.ToDateTime(Request["BegBillDate"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndBillDate"]))
                {
                    search.AddCondition("BillDate<='" + Convert.ToDateTime(Request["EndBillDate"]).AddDays(1) + "'");
                }
            }
            // 提交时间
            if (!string.IsNullOrEmpty(Request["BegCreateTime"]) || !string.IsNullOrEmpty(Request["EndCreateTime"]))
            {
                if (!string.IsNullOrEmpty(Request["BegCreateTime"]))
                {
                    search.AddCondition("CreateTime>='" + Convert.ToDateTime(Request["BegCreateTime"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndCreateTime"]))
                {
                    search.AddCondition("CreateTime<='" + Convert.ToDateTime(Request["EndCreateTime"]).AddDays(1) + "'");
                }
            }
            // 供应商金额
            if (!string.IsNullOrEmpty(Request["BegTotalPrice"]) || !string.IsNullOrEmpty(Request["EndTotalPrice"]))
            {
                if (!string.IsNullOrEmpty(Request["BegTotalPrice"]))
                {
                    search.AddCondition("TotalPrice>='" + Convert.ToDouble(Request["BegTotalPrice"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndTotalPrice"]))
                {
                    search.AddCondition("TotalPrice<='" + Convert.ToDouble(Request["EndTotalPrice"]) + "'");
                }
            }
            //盈亏金额
            if (!string.IsNullOrEmpty(Request["BegLossPrice"]) || !string.IsNullOrEmpty(Request["EndLossPrice"]))
            {
                if (!string.IsNullOrEmpty(Request["BegLossPrice"]))
                {
                    search.AddCondition("LossPrice>='" + Convert.ToDouble(Request["BegLossPrice"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndLossPrice"]))
                {
                    search.AddCondition("LossPrice<='" + Convert.ToDouble(Request["EndLossPrice"]) + "'");
                }
            }
            //显示在途 IsOnLine： -1：全部 1：在途 0：未在途
            if (!string.IsNullOrEmpty(Request["IsOnLine"]))
            {
                //查询全部
                if (Request["IsOnLine"].ToString() != "-1")
                {
                    //在途
                    if (Request["IsOnLine"].ToString() == "1")
                    {
                        //有区间的
                        string whereOther = "";
                        if (!string.IsNullOrEmpty(Request["BegOnLine"]) || !string.IsNullOrEmpty(Request["EndOnLine"]))
                        {
                            if (!string.IsNullOrEmpty(Request["BegOnLine"]))
                            {
                               whereOther="BillDate>='" + Convert.ToDateTime(Request["BegOnLine"]) + "'";
                            }
                            if (!string.IsNullOrEmpty(Request["EndOnLine"]))
                            {
                                if (string.IsNullOrEmpty(whereOther))
                                {
                                    whereOther = "BillDate<='" + Convert.ToDateTime(Request["EndOnLine"]).AddDays(1) + "'";
                                }
                                else
                                {
                                    whereOther += " and BillDate<='" + Convert.ToDateTime(Request["EndOnLine"]).AddDays(1) + "'";
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(whereOther))
                        {
                            search.AddCondition(@"CusterId in(
select CusterId from view_HPurchaseRelation  where OnLineCount>0 and FinancialState!=3
group by CusterId
                            )");
                        }
                        else
                        {
                            search.AddCondition(@"CusterId in(
select CusterId from view_HPurchaseRelation where CusterId in(select CusterId from H_Purchase(nolock) where (FinancialState=-1 or FinancialState=-2 or FinancialState=1) and "+whereOther+" and CusterId=view_HPurchaseRelation.CusterId and Status!=-2) and FinancialState!=3 group by CusterId)");
                        }
                    }
                    else {
                        search.AddCondition(@"CusterId in(
select CusterId from view_HPurchaseRelation  where OnLineCount=0 and FinancialState!=3
group by CusterId
                            )");
                    }
                }
            }
            
            #endregion
        }

        #region==页面主表列表 SearchHOrderInRelation
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchHOrderInRelation()
        {
            var search = base.GetSearch();
            GetPoolSearch(ref search);
            search.AddCondition("PoolStatus!=2");//扣减状态 1：未完成 2：已完成
            var jsonResult = new BllHFinance().SearchHOrderInRelation(search);
            LogInsert(OperationTypeEnum.访问, "财务池入库单查询", CurrentMaster.UserName + "页面访问正常.");
            WriteJsonToPage(jsonResult);
        }
        #endregion

        #region==页面子表列表 SearchHPurchaseRelation
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchHPurchaseRelation()
        {
            var search = base.GetSearch();
            string CusterId = Request["CusterId"].ToString();
            if (!string.IsNullOrEmpty(Request["CusterId"]))
            {
                search.AddCondition("CusterId='" + CusterId + "'");
            }

            string column = "*,OnLine=-1";
            //显示在途 IsOnLine： -1：全部 1：在途 0：未在途
            if (!string.IsNullOrEmpty(Request["IsOnLine"]))
            {
                //查询全部
                if (Request["IsOnLine"].ToString() != "-1")
                {
                    //在途
                    if (Request["IsOnLine"].ToString() == "1")
                    {
                        //有区间的
                        string whereOther = "";
                        if (!string.IsNullOrEmpty(Request["BegOnLine"]) || !string.IsNullOrEmpty(Request["EndOnLine"]))
                        {
                            if (!string.IsNullOrEmpty(Request["BegOnLine"]))
                            {
                                whereOther = "BillDate>='" + Convert.ToDateTime(Request["BegOnLine"]) + "'";
                            }
                            if (!string.IsNullOrEmpty(Request["EndOnLine"]))
                            {
                                if (string.IsNullOrEmpty(whereOther))
                                {
                                    whereOther = "BillDate<='" + Convert.ToDateTime(Request["EndOnLine"]).AddDays(1) + "'";
                                }
                                else
                                {
                                    whereOther += " and BillDate<='" + Convert.ToDateTime(Request["EndOnLine"]).AddDays(1) + "'";
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(whereOther))
                        {
                            search.AddCondition(@"CusterId in(
select CusterId from  H_Purchase(nolock) where (FinancialState=-1 or FinancialState=-2 or FinancialState=1) and Status!=-2
group by CusterId
                            )");
                            column = "*,OnLine=(select count(CusterId) from H_Purchase(nolock) where (FinancialState=-1 or FinancialState=-2 or FinancialState=1) and CusterId=view_HPurchaseRelation.CusterId and Status!=-2)";
                            
                        }
                        else
                        {
                            search.AddCondition(@"CusterId in(
select CusterId from  H_Purchase(nolock) where (FinancialState=-1 or FinancialState=-2 or FinancialState=1) and "+whereOther+" and Status!=-2 group by CusterId)");
                            column = "*,OnLine=(select count(CusterId) from H_Purchase(nolock) where (FinancialState=-1 or FinancialState=-2 or FinancialState=1) and CusterId=view_HPurchaseRelation.CusterId and " + whereOther + "  and Status!=-2)";
                        }
                    }
                    else
                    {
                        search.AddCondition(@"CusterId in(
select CusterId from view_HPurchaseRelation  where OnLineCount=0 and FinancialState!=3
group by CusterId
                            )");
                    }
                }
            }

            search.AddCondition("PoolStatus!=3");//扣减状态 1：未完成 2：部分完成 3：已完成
            search.SelectedColums = column; //查询列
            var jsonResult = new BllHFinance().SearchHPurchaseRelation(search);
            WriteJsonToPage(jsonResult);
        }
        #endregion

        #region ==驳回主表 BtnBackMain
        /// <summary>
        /// 驳回主表
        /// </summary>
        public void BtnBackMain(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from H_OrderInRelation where OrderInId in(" + id + ");");
                sb.AppendLine();
                sb.Append("update H_OrderIn set FinancialState=-1,FinancialDT=getdate() where Id in(" + id + ");");
                sb.AppendLine();
                if (new BllHOrderIn().ExecuteNonQueryByText(sb.ToString()) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "财务单据池", "驳回成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "财务单据池", "驳回失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "财务单据池", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==驳回子表 BtnBackDetail
        /// <summary>
        /// 驳回子表
        /// </summary>
        public void BtnBackDetail(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from H_PurchaseRelation where PurchaseId in(" + id + ");");
                sb.AppendLine();
                sb.Append("update H_Purchase set FinancialState=-1,FinancialDT=getdate() where Id in(" + id + ");");
                sb.AppendLine();
                if (new BllHPurchase().ExecuteNonQueryByText(sb.ToString()) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "财务单据池", "驳回成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "财务单据池", "驳回失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "财务单据池", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===价格拆分 BtnCut
        /// <summary>
        /// 价格拆分
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void BtnCut()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                string id = Request["id"].ToString();

                var model = new BllHPurchaseRelation().LoadData(id);
                if (model != null)
                {
                    StringBuilder sb = new StringBuilder();
                    //更改原单据价格
                    //sb.Append("update H_Purchase set TotalPrice=(TotalPrice-" + Convert.ToDecimal(Request["DecomposePrice"].ToString()) + ") where Id='" + id + "';");
                    //更改原单据拆分状态
                    sb.AppendLine();
                    sb.Append("update H_PurchaseRelation set Price=Price-" + Convert.ToDecimal(Request["DecomposePrice"].ToString()) + ",IsDecompose=1,DecomposePrice+=" + Convert.ToDecimal(Request["DecomposePrice"].ToString()) + " where Id='" + model.Id + "' ");
                    sb.AppendLine();
                    //添加拆分单据
                    sb.Append("insert into H_PurchaseRelation( Id,PurchaseId,Status,Remark,IsDecompose,DecomposePrice,OutNumberS,Price)values (");
                    sb.Append("'" + Guid.NewGuid().ToString() + "',");
                    sb.Append("'" + model.PurchaseId + "',");
                    sb.Append("1,");
                    sb.Append("'',");
                    sb.Append("0,");
                    sb.Append("0,");
                    sb.Append("'" + model.OutNumberS + "," + model .Id+ "',");
                    sb.Append("" + Convert.ToDecimal(Request["DecomposePrice"].ToString()) + "");
                    sb.Append(")");
                    sb.AppendLine();
                    if (new BllHOrderIn().ExecuteNonQueryByText(sb.ToString()) > 0)
                    {
                        json.success = true;
                        LogInsert(OperationTypeEnum.操作, "价格拆分", "价格拆分保存失败.");
                    }
                    else
                    {
                        json.success = false;
                        json.msg = "操作失败";
                        LogInsert(OperationTypeEnum.操作, "价格拆分", "价格拆分保存成功.");
                    }
                }
            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "价格拆分", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region===提交财务 SubFinance
        /// <summary>
        /// 提交财务
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SubFinance()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                //主表ID
                string MainId = Request["MainId"].ToString();
                StringBuilder sb = new StringBuilder();
                bool IsAll = Convert.ToBoolean(Request["IsAll"].ToString());//是否批量操作
                if (IsAll == true)
                {
                    BllHOrderIn Biz = new BllHOrderIn();
                    //根据单据日期升序获取ID
                    string serarch = "select * from view_HOrderInRelation where relationId in(" + MainId + ") and Status!="+(int)StatusEnum.删除+" order by BillDate asc";
                    DataSet dds = new BllHOrderInRelation().ExecuteDataSet(serarch);
                    if (dds.Tables[0].Rows.Count > 0)
                    {
                        //供应商信息
                        for (int i = 0; i < dds.Tables[0].Rows.Count; i++)
                        {
                            var main = new BllHOrderInRelation().LoadData(dds.Tables[0].Rows[i]["relationId"]); //关系表
                            var model = Biz.LoadData(main.OrderInId);//入库表

                            if (model != null)
                            {
                                #region ===
                                string CusterId = model.CusterId;//供应商ID
                                var TotalPrice = model.TotalPrice;//供应商金额
                                var CusterModel = new BllSysCompany().LoadData(CusterId);

                                //获取任务池中明细
                                string sql = "select * from view_HPurchaseRelation(nolock) where CusterId='" + CusterId + "' and len(PID)=0 and PoolStatus!=3 and Status!=" + (int)StatusEnum.删除 + " order by BillDate asc;";
                                DataSet Relation = Biz.ExecuteDataSet(sql);
                                if (Relation.Tables[0].Rows.Count > 0)
                                {
                                    //统计明细金额，和主表金额对比
                                    decimal decimalCount = 0;
                                    var index = 0;
                                    var index2 = 0;
                                    for (var a = 0; a < Relation.Tables[0].Rows.Count; a++)
                                    {
                                        index++;
                                        decimalCount += Convert.ToDecimal(Relation.Tables[0].Rows[a]["PoolPrice"].ToString());

                                        if (decimalCount < TotalPrice && index == Relation.Tables[0].Rows.Count)
                                        {
                                            #region===更改原主表单据状态
                                            sb.Append("update H_OrderInRelation set Status=2 where Id='" + main.Id + "';");
                                            sb.AppendLine();
                                            #endregion

                                            #region===添加财务付款单据
                                            string FinanceId = Guid.NewGuid().ToString();
                                            sb.Append(@"insert into H_Finance(Id
                                         ,CreateTime
                                          ,CreaterId
                                          ,Status
                                          ,ReturnAmount
                                          ,ProfitTotal
                                          ,PaymentAmmount
                                          ,PayStatus
                                          ,FinanceRemark
                                          ,RelationId
                                          ,OrderInId,CheckoutType,PaymentType,AccountName,AccountNum) values (");
                                            sb.Append("'" + FinanceId + "',");
                                            sb.Append("getdate(),");
                                            sb.Append("'" + CurrentMaster.Id + "',");
                                            sb.Append("1,");
                                            sb.Append("" + model.TotalPrice + ",");//剩余退货金额
                                            sb.Append("" + decimalCount + ",");//退款金额
                                            sb.Append("" + (model.TotalPrice - decimalCount) + ",");//实际付款金额
                                            sb.Append("0,");//PayStatus
                                            sb.Append("'',"); //FinanceRemark
                                            sb.Append("'" + main.Id + "',");
                                            sb.Append("'" + model.Id + "',");
                                            sb.Append("" + CusterModel.CheckoutType + ",");
                                            sb.Append("" + CusterModel.PaymentType + ",");
                                            sb.Append("'" + CusterModel.AccountName + "',");
                                            sb.Append("'" + CusterModel.AccountNum + "'");
                                            sb.Append(")");
                                            sb.AppendLine();
                                            #endregion

                                            #region ===添加明细
                                            for (var q = 0; q < Relation.Tables[0].Rows.Count; q++)
                                            {

                                                //9:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                                                if (Convert.ToBoolean(Relation.Tables[0].Rows[q]["IsDecompose"]) == true)
                                                {
                                                    sb.Append("update H_PurchaseRelation set Status=3,DecomposePrice+=" + Convert.ToDecimal(Relation.Tables[0].Rows[q]["PoolPrice"]) + ",Price=0 where Id='" + Relation.Tables[0].Rows[q]["RelationId"] + "';");
                                                    sb.AppendLine();
                                                }
                                                else {
                                                    sb.Append("update H_PurchaseRelation set Status=3,Price=0 where Id='" + Relation.Tables[0].Rows[q]["RelationId"] + "';");
                                                    sb.AppendLine();
                                                }

                                                //拆分金额
                                                string RelationId = Relation.Tables[0].Rows[q]["RelationId"].ToString();
                                                var IsDecom = new BllHPurchaseRelation().LoadData(RelationId);
                                                if (IsDecom != null)
                                                {
                                                    if (IsDecom.IsDecompose == true)
                                                    {
                                                        var DecomposePrice =Convert.ToDecimal(Relation.Tables[0].Rows[q]["PoolPrice"]);
                                                        RelationId = Guid.NewGuid().ToString();
                                                        sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                                        sb.Append("'" + RelationId + "',");
                                                        sb.Append("'" + IsDecom.PurchaseId + "',");
                                                        sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                                        sb.Append("'',");
                                                        sb.Append("0,");//Price 剩余金额
                                                        sb.Append("1,"); //是否拆分
                                                        sb.Append("" + DecomposePrice + ",");
                                                        sb.Append("'',");
                                                        sb.Append("'" + Relation.Tables[0].Rows[q]["RelationId"] + "'");
                                                        sb.Append(");");
                                                        sb.AppendLine();
                                                    }
                                                }

                                                sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                                sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                                sb.Append("getdate(),");
                                                sb.Append("'" + CurrentMaster.Id + "',");
                                                sb.Append("3,");
                                                sb.Append("'" + Relation.Tables[0].Rows[q]["PoolPrice"] + "',");//剩余退货金额
                                                sb.Append("0,");//退货盈亏
                                                sb.Append("'',");
                                                sb.Append("'" + RelationId + "',");
                                                sb.Append("'" + Relation.Tables[0].Rows[q]["Id"] + "',");
                                                sb.Append("'" + FinanceId + "'");
                                                sb.Append(");");
                                                sb.AppendLine();



                                                //3更改明细主键的采购编码
                                                sb.Append(@"update H_Purchase set GetNumber='" + model.GetNumber + "' where Id='" + Relation.Tables[0].Rows[a]["Id"].ToString() + "';");
                                                sb.AppendLine();
                                            }
                                            #endregion

                                            break;
                                        }
                                        else if (decimalCount == TotalPrice)
                                        {
                                            #region===更改原单据状态
                                            sb.Append("update H_OrderInRelation set Status=2 where Id='" + main.Id + "';");
                                            sb.AppendLine();
                                            #endregion

                                            #region===添加财务付款单据
                                            string FinanceId = Guid.NewGuid().ToString();
                                            sb.Append(@"insert into H_Finance(Id
                                         ,CreateTime
                                          ,CreaterId
                                          ,Status
                                          ,ReturnAmount
                                          ,ProfitTotal
                                          ,PaymentAmmount
                                          ,PayStatus
                                          ,FinanceRemark
                                          ,RelationId
                                          ,OrderInId,CheckoutType,PaymentType,AccountName,AccountNum) values (");
                                            sb.Append("'" + FinanceId + "',");
                                            sb.Append("getdate(),");
                                            sb.Append("'" + CurrentMaster.Id + "',");
                                            sb.Append("1,");
                                            sb.Append("" + model.TotalPrice + ",");//剩余退货金额
                                            sb.Append("" + decimalCount + ",");//退款金额
                                            sb.Append("" + (model.TotalPrice - decimalCount) + ",");//实际付款金额
                                            sb.Append("0,");//PayStatus
                                            sb.Append("'',"); //FinanceRemark
                                            sb.Append("'" + main.Id + "',");
                                            sb.Append("'" + model.Id + "',");
                                            sb.Append("" + CusterModel.CheckoutType + ",");
                                            sb.Append("" + CusterModel.PaymentType + ",");
                                            sb.Append("'" + CusterModel.AccountName + "',");
                                            sb.Append("'" + CusterModel.AccountNum + "'");
                                            sb.Append(")");
                                            sb.AppendLine();
                                            #endregion

                                            #region ===添加明细
                                            for (var q = 0; q <= index2; q++)
                                            {
                                                //9:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                                                if (Convert.ToBoolean(Relation.Tables[0].Rows[q]["IsDecompose"]) == true)
                                                {
                                                    sb.Append("update H_PurchaseRelation set Status=3,DecomposePrice+=" + Convert.ToDecimal(Relation.Tables[0].Rows[q]["PoolPrice"]) + ",Price=0 where Id='" + Relation.Tables[0].Rows[q]["RelationId"] + "';");
                                                    sb.AppendLine();
                                                }
                                                else
                                                {
                                                    sb.Append("update H_PurchaseRelation set Status=3,Price=0 where Id='" + Relation.Tables[0].Rows[q]["RelationId"] + "';");
                                                    sb.AppendLine();
                                                }

                                                //拆分金额
                                                string RelationId = Relation.Tables[0].Rows[q]["RelationId"].ToString();
                                                var IsDecom = new BllHPurchaseRelation().LoadData(RelationId);
                                                if (IsDecom != null)
                                                {
                                                    if (IsDecom.IsDecompose == true)
                                                    {
                                                        var DecomposePrice= Convert.ToDecimal(Relation.Tables[0].Rows[q]["PoolPrice"]) ;
                                                        RelationId = Guid.NewGuid().ToString();
                                                        sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                                        sb.Append("'" + RelationId + "',");
                                                        sb.Append("'" + IsDecom.PurchaseId + "',");
                                                        sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                                        sb.Append("'',");
                                                        sb.Append("0,");//Price 剩余金额
                                                        sb.Append("1,"); //是否拆分
                                                        sb.Append("" + DecomposePrice + ",");
                                                        sb.Append("'',");
                                                        sb.Append("'" + Relation.Tables[0].Rows[q]["RelationId"] + "'");
                                                        sb.Append(");");
                                                        sb.AppendLine();
                                                    }
                                                }

                                                sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                                sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                                sb.Append("getdate(),");
                                                sb.Append("'" + CurrentMaster.Id + "',");
                                                sb.Append("3,");
                                                sb.Append("'" + Relation.Tables[0].Rows[q]["PoolPrice"] + "',");//剩余退货金额
                                                sb.Append("0,");//退货盈亏
                                                sb.Append("'',");
                                                sb.Append("'" + RelationId + "',");
                                                sb.Append("'" + Relation.Tables[0].Rows[q]["Id"] + "',");
                                                sb.Append("'" + FinanceId + "'");
                                                sb.Append(");");
                                                sb.AppendLine();

                                                //3更改明细主键的采购编码
                                                sb.Append(@"update H_Purchase set GetNumber='" + model.GetNumber + "' where Id='" + Relation.Tables[0].Rows[a]["Id"].ToString() + "';");
                                                sb.AppendLine();
                                            }
                                            #endregion

                                            break;
                                        }
                                        else if (decimalCount > TotalPrice)
                                        {
                                            //当前剩余金额
                                            var nowPrice = Convert.ToDecimal(Relation.Tables[0].Rows[a]["PoolPrice"].ToString()); //50
                                            //需要拆分价格
                                            var needPrice = decimalCount - TotalPrice;//主表需要拆分的金额 ((100+50)-120)=30
                                            //
                                            var LossPrice = nowPrice - needPrice;//拆分已完成金额

                                            string newid = Guid.NewGuid().ToString();
                                            string FinanceId = Guid.NewGuid().ToString();

                                            #region===
                                            //1:更改源单据为拆分状态，留在池子
                                            string nowsql = "update H_PurchaseRelation set [Status]=2,IsDecompose=1,Price-=" + LossPrice + ",DecomposePrice+=" + LossPrice + " where Id='" + Relation.Tables[0].Rows[a]["RelationId"].ToString() + "';";
                                            int res = new BllHOrderIn().ExecuteNonQueryByText(nowsql);

                                            //2:满足的直接设置状态为完成
                                            string newid2 = Guid.NewGuid().ToString();
                                            sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                            sb.Append("'" + newid2 + "',");
                                            sb.Append("'" + Relation.Tables[0].Rows[a]["Id"].ToString() + "',");
                                            sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                            sb.Append("'',");
                                            sb.Append("" + (nowPrice - LossPrice) + ",");//Price
                                            sb.Append("1,");
                                            sb.Append("" + LossPrice + ",");
                                            sb.Append("'',");
                                            sb.Append("'" + Relation.Tables[0].Rows[a]["RelationId"].ToString() + "'");
                                            sb.Append(");");
                                            sb.AppendLine();

                                            //
                                            //sb.Append("update H_PurchaseRelation set DecomposePrice=(select DecomposePrice from H_PurchaseRelation where Id='" + Relation.Tables[0].Rows[a]["RelationId"].ToString() + "') where Id='" + newid2 + "';");
                                            //sb.AppendLine();

                                            sb.Append("update H_OrderInRelation set Status=2 where Id='" + main.Id + "';");
                                            sb.AppendLine();

                                            //4：添加主单据到付款单
                                            sb.Append(@"insert into H_Finance(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,PaymentAmmount,PayStatus,FinanceRemark,RelationId,OrderInId,CheckoutType,PaymentType,AccountName,AccountNum) values(");
                                            sb.Append("'" + FinanceId + "',");
                                            sb.Append("getdate(),");
                                            sb.Append("'" + CurrentMaster.Id + "',");
                                            sb.Append("1,");
                                            sb.Append("" + TotalPrice + ",");//剩余退货金额
                                            sb.Append("" + TotalPrice + ",");//退款金额
                                            sb.Append("" + 0 + ",");//实际付款金额
                                            sb.Append("0,");//PayStatus
                                            sb.Append("'',");
                                            sb.Append("'" + main.Id + "',");
                                            sb.Append("'" + main.OrderInId + "',");//入库单主键
                                            sb.Append("" + CusterModel.CheckoutType + ",");
                                            sb.Append("" + CusterModel.PaymentType + ",");
                                            sb.Append("'" + CusterModel.AccountName + "',");
                                            sb.Append("'" + CusterModel.AccountNum + "'");
                                            sb.Append(");");
                                            sb.AppendLine();

                                            sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                            sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                            sb.Append("getdate(),");
                                            sb.Append("'" + CurrentMaster.Id + "',");
                                            sb.Append("3,");
                                            sb.Append("'" + LossPrice + "',");//退货金额
                                            sb.Append("0,");//退货盈亏
                                            sb.Append("'',");
                                            sb.Append("'" + newid2 + "',");
                                            sb.Append("'" + Relation.Tables[0].Rows[a]["Id"] + "',");
                                            sb.Append("'" + FinanceId + "'");
                                            sb.Append(");");
                                            sb.AppendLine();

                                            //9:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                                            //sb.Append("update H_PurchaseRelation set Status=3 where Id='" + Relation.Tables[0].Rows[a]["RelationId"] + "';");
                                            //sb.AppendLine();
                                            #region ===添加明细
                                            for (var q = 0; q < index2; q++)
                                            {
                                                //9:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                                                if (Convert.ToBoolean(Relation.Tables[0].Rows[q]["IsDecompose"]) == true)
                                                {
                                                    sb.Append("update H_PurchaseRelation set Status=3,DecomposePrice+=" + Convert.ToDecimal(Relation.Tables[0].Rows[q]["PoolPrice"]) + ",Price=0 where Id='" + Relation.Tables[0].Rows[q]["RelationId"] + "';");
                                                    sb.AppendLine();
                                                }
                                                else
                                                {
                                                    sb.Append("update H_PurchaseRelation set Status=3,Price=0 where Id='" + Relation.Tables[0].Rows[q]["RelationId"] + "';");
                                                    sb.AppendLine();
                                                }

                                                string RelationId = Relation.Tables[0].Rows[q]["RelationId"].ToString();
                                                var IsDecom = new BllHPurchaseRelation().LoadData(RelationId);
                                                if (IsDecom != null)
                                                {
                                                    if (IsDecom.IsDecompose == true)
                                                    {
                                                        var DecomposePrice = Convert.ToDecimal(Relation.Tables[0].Rows[q]["PoolPrice"]);
                                                        RelationId = Guid.NewGuid().ToString();
                                                        sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                                        sb.Append("'" + RelationId + "',");
                                                        sb.Append("'" + IsDecom.PurchaseId + "',");
                                                        sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                                        sb.Append("'',");
                                                        sb.Append("0,");//Price 剩余金额
                                                        sb.Append("1,"); //是否拆分
                                                        sb.Append("" + DecomposePrice + ",");
                                                        sb.Append("'',");
                                                        sb.Append("'" + Relation.Tables[0].Rows[q]["RelationId"] + "'");
                                                        sb.Append(");");
                                                        sb.AppendLine();
                                                    }
                                                }

                                                sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                                sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                                sb.Append("getdate(),");
                                                sb.Append("'" + CurrentMaster.Id + "',");
                                                sb.Append("3,");
                                                sb.Append("'" + Relation.Tables[0].Rows[q]["PoolPrice"] + "',");//剩余退货金额
                                                sb.Append("0,");//退货盈亏
                                                sb.Append("'',");
                                                sb.Append("'" + RelationId + "',");
                                                sb.Append("'" + Relation.Tables[0].Rows[q]["Id"] + "',");
                                                sb.Append("'" + FinanceId + "'");
                                                sb.Append(");");
                                                sb.AppendLine();

                                                //3更改明细主键的采购编码
                                                sb.Append(@"update H_Purchase set GetNumber='" + model.GetNumber + "' where Id='" + Relation.Tables[0].Rows[a]["Id"].ToString() + "';");
                                                sb.AppendLine();
                                            }
                                            #endregion


                                            #endregion
                                            break;
                                        }
                                        index2++;
                                    }
                                }
                                else
                                {
                                    #region===不存在明细,直接添加付款单中

                                    #region===更改原单据状态
                                    sb.Append("update H_OrderInRelation set Status=2 where Id='" + main.Id + "';");
                                    sb.AppendLine();
                                    #endregion

                                    #region===添加财务付款单据
                                    string FinanceId = Guid.NewGuid().ToString();
                                    sb.Append(@"insert into H_Finance(Id
                                         ,CreateTime
                                          ,CreaterId
                                          ,Status
                                          ,ReturnAmount
                                          ,ProfitTotal
                                          ,PaymentAmmount
                                          ,PayStatus
                                          ,FinanceRemark
                                          ,RelationId
                                          ,OrderInId,CheckoutType,PaymentType,AccountName,AccountNum) values (");
                                    sb.Append("'" + FinanceId + "',");
                                    sb.Append("getdate(),");
                                    sb.Append("'" + CurrentMaster.Id + "',");
                                    sb.Append("1,");
                                    sb.Append("" + model.TotalPrice + ",");//剩余退货金额
                                    sb.Append("" + 0 + ",");//退款金额
                                    sb.Append("" + model.TotalPrice + ",");//实际付款金额
                                    sb.Append("0,");//PayStatus
                                    sb.Append("'',"); //FinanceRemark
                                    sb.Append("'" + main.Id + "',");
                                    sb.Append("'" + model.Id + "',");
                                    sb.Append("" + CusterModel.CheckoutType + ",");
                                    sb.Append("" + CusterModel.PaymentType + ",");
                                    sb.Append("'" + CusterModel.AccountName + "',");
                                    sb.Append("'" + CusterModel.AccountNum + "'");
                                    sb.Append(")");
                                    sb.AppendLine();
                                    #endregion

                                    #endregion
                                }

                                #endregion

                            }
                            if (new BllHOrderIn().ExecuteNonQueryByText(sb.ToString()) > 0)
                            {
                                sb.Clear();
                                json.success = true;
                            }
                            else
                            {
                                json.success = false;
                                json.msg = "操作失败";
                            }
                        }
                    }
                }
                else
                {
                    //供应商ID
                    string CusterId = Request["CusterId"].ToString();
                    //采购单号
                    string GetNumber = Request["GetNumber"].ToString();
                    //明细表ID
                    string[] MainDetailId = Request["MainDetailId"].ToString().Split(',');
                    //供应商金额
                    decimal MainMoney = Convert.ToDecimal(Request["MainMoney"].ToString());
                    //退款金额
                    decimal MainsMoney = Convert.ToDecimal(Request["MainsMoney"].ToString());
                    if (MainDetailId.Length == 0)
                    {
                        MainsMoney = 0;
                    }
                    var CusterModel = new BllSysCompany().LoadData(CusterId);
                    if (CusterModel != null)
                    {
                        var main = new BllHOrderInRelation().LoadData(MainId);
                        string FinanceId = Guid.NewGuid().ToString();

                        //1：更改原主表单据状态
                        sb.Append("update H_OrderInRelation set Status=2 where Id='" + MainId + "';");
                        sb.AppendLine();

                        var ProfitTotal = (MainsMoney > MainMoney == true ? MainMoney : MainsMoney);
                        //2：添加主单据到付款单
                        sb.Append(@"insert into H_Finance(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,PaymentAmmount,PayStatus,FinanceRemark,RelationId,OrderInId,CheckoutType,PaymentType,AccountName,AccountNum) values(");
                        sb.Append("'" + FinanceId + "',");
                        sb.Append("getdate(),");
                        sb.Append("'" + CurrentMaster.Id + "',");
                        sb.Append("1,");
                        sb.Append("" + MainMoney + ",");//剩余退货金额
                        sb.Append("" + ProfitTotal + ",");//退款金额
                        sb.Append("" + (MainMoney - ProfitTotal) + ",");//实际付款金额
                        sb.Append("0,");//PayStatus
                        sb.Append("'',");
                        sb.Append("'" + main.Id + "',");
                        sb.Append("'" + main.OrderInId + "',");//入库单主键
                        sb.Append("" + CusterModel.CheckoutType + ",");
                        sb.Append("" + CusterModel.PaymentType + ",");
                        sb.Append("'" + CusterModel.AccountName + "',");
                        sb.Append("'" + CusterModel.AccountNum + "'");
                        sb.Append(");");
                        sb.AppendLine();

                        //根据明细ID获取所有明细信息
                        if (MainsMoney > MainMoney)
                        {
                            #region===
                            //自动拆分
                            string str = "";
                            for (var i = 0; i < MainDetailId.Length; i++)
                            {
                                str += "'" + MainDetailId[i] + "',";
                            }
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str.Substring(0, str.Length - 1);
                            }
                            string sql = "select * from view_HPurchaseRelation(nolock) where RelationId in(" + str + ") and PoolStatus!=3 order by FinancialDT asc;";
                            DataSet ds = new BllHOrderInRelation().ExecuteDataSet(sql);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //统计明细金额，和主表金额对比
                                decimal decimalCount = 0;
                                for (int a = 0; a < ds.Tables[0].Rows.Count; a++)
                                {
                                    decimalCount += Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"].ToString());

                                    #region===判断当前单据金额是否满足主表金额
                                    if (decimalCount < MainMoney)
                                    {
                                        #region===
                                        if (Convert.ToBoolean(ds.Tables[0].Rows[a]["IsDecompose"]) == true)
                                        {
                                            //1:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                                            sb.Append("update H_PurchaseRelation set Status=3,Price=0,DecomposePrice+=" + Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"]) + " where Id='" + ds.Tables[0].Rows[a]["RelationId"] + "';");
                                            sb.AppendLine();
                                        }
                                        else
                                        {
                                            sb.Append("update H_PurchaseRelation set Status=3,Price=0 where Id='" + ds.Tables[0].Rows[a]["RelationId"] + "';");
                                            sb.AppendLine();
                                        }
                                        //拆分金额
                                        string RelationId = ds.Tables[0].Rows[a]["RelationId"].ToString();
                                        var IsDecom = new BllHPurchaseRelation().LoadData(RelationId);
                                        if (IsDecom != null)
                                        {
                                            if (IsDecom.IsDecompose == true)
                                            {
                                                var DecomposePrice =  Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"]);
                                                RelationId = Guid.NewGuid().ToString();
                                                sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                                sb.Append("'" + RelationId + "',");
                                                sb.Append("'" + IsDecom.PurchaseId + "',");
                                                sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                                sb.Append("'',");
                                                sb.Append("0,");//Price 剩余金额
                                                sb.Append("1,"); //是否拆分
                                                sb.Append("" + DecomposePrice + ",");
                                                sb.Append("'',");
                                                sb.Append("'" + ds.Tables[0].Rows[a]["RelationId"].ToString() + "'");
                                                sb.Append(");");
                                                sb.AppendLine();
                                            }
                                        }
                                      
                                        //2:更改明细主键的采购编码
                                        sb.Append(@"update H_Purchase set FinancialState=2,GetNumber='" + GetNumber + "' where Id='" + ds.Tables[0].Rows[a]["Id"] + "';");
                                        sb.AppendLine();
                                        //3：添加退货单到付款单
                                        sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                        sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                        sb.Append("getdate(),");
                                        sb.Append("'" + CurrentMaster.Id + "',");
                                        sb.Append("1,");
                                        sb.Append("'" + Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"].ToString()) + "',");//剩余退货金额
                                        sb.Append("0,");//退货盈亏
                                        sb.Append("'',");
                                        sb.Append("'" + RelationId + "',");
                                        sb.Append("'" + ds.Tables[0].Rows[a]["Id"] + "',");
                                        sb.Append("'" + FinanceId + "'");
                                        sb.Append(");");
                                        sb.AppendLine();
                                        #endregion
                                    }
                                    else if (decimalCount == MainMoney)
                                    {
                                        #region===
                                        if (Convert.ToBoolean(ds.Tables[0].Rows[a]["IsDecompose"]) == true)
                                        {
                                            //1:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                                            sb.Append("update H_PurchaseRelation set Status=3,Price=0,DecomposePrice+=" + Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"]) + " where Id='" + ds.Tables[0].Rows[a]["RelationId"] + "';");
                                            sb.AppendLine();
                                        }
                                        else
                                        {
                                            sb.Append("update H_PurchaseRelation set Status=3,Price=0 where Id='" + ds.Tables[0].Rows[a]["RelationId"] + "';");
                                            sb.AppendLine();
                                        }
                                        //拆分金额
                                        string RelationId = ds.Tables[0].Rows[a]["RelationId"].ToString();
                                        var IsDecom = new BllHPurchaseRelation().LoadData(RelationId);
                                        if (IsDecom != null)
                                        {
                                            if (IsDecom.IsDecompose == true)
                                            {
                                                var DecomposePrice = Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"]); 
                                                RelationId = Guid.NewGuid().ToString();
                                                sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                                sb.Append("'" + RelationId + "',");
                                                sb.Append("'" + IsDecom.PurchaseId + "',");
                                                sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                                sb.Append("'',");
                                                sb.Append("0,");//Price 剩余金额
                                                sb.Append("1,"); //是否拆分
                                                sb.Append("" + DecomposePrice + ",");
                                                sb.Append("'',");
                                                sb.Append("'" + ds.Tables[0].Rows[a]["RelationId"].ToString() + "'");
                                                sb.Append(");");
                                                sb.AppendLine();
                                            }
                                        }
                                        
                                        //2:更改明细主键的采购编码
                                        sb.Append(@"update H_Purchase set FinancialState=2,GetNumber='" + GetNumber + "' where Id='" + ds.Tables[0].Rows[a]["Id"] + "';");
                                        sb.AppendLine();

                                        //4：添加退货单到付款单
                                        sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                        sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                        sb.Append("getdate(),");
                                        sb.Append("'" + CurrentMaster.Id + "',");
                                        sb.Append("1,");
                                        sb.Append("'" + Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"].ToString()) + "',");//剩余退货金额
                                        sb.Append("0,");//退货盈亏
                                        sb.Append("'',");
                                        sb.Append("'" + ds.Tables[0].Rows[a]["RelationId"] + "',");
                                        sb.Append("'" + ds.Tables[0].Rows[a]["Id"] + "',");
                                        sb.Append("'" + FinanceId + "'");
                                        sb.Append(");");
                                        sb.AppendLine();
                                        #endregion
                                        break;
                                    }
                                    else
                                    {
                                        //需要拆分价格
                                        var needPrice = decimalCount - MainMoney;//主表需要拆分的金额60 (150-100)
                                        //当前金额
                                        var nowPrice = Convert.ToDecimal(ds.Tables[0].Rows[a]["PoolPrice"].ToString()); //60
                                        //完成金额
                                        var FinshPrice = nowPrice - needPrice;

                                        #region===
                                        string newid = Guid.NewGuid().ToString();
                                        //1:更改源单据为拆分状态，留在池子
                                        sb.Append(@"update H_PurchaseRelation set [Status]=2,IsDecompose=1,Price-=" + FinshPrice + ",DecomposePrice+=" + FinshPrice + " where Id='" + ds.Tables[0].Rows[a]["RelationId"].ToString() + "';");
                                        sb.AppendLine();
                                        //2:满足的直接设置状态为完成
                                        string newid2 = Guid.NewGuid().ToString();
                                        sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                        sb.Append("'" + newid2 + "',");
                                        sb.Append("'" + ds.Tables[0].Rows[a]["Id"].ToString() + "',");
                                        sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                        sb.Append("'',");
                                        sb.Append("" + (nowPrice - FinshPrice) + ",");//Price 剩余金额
                                        sb.Append("1,"); //是否拆分
                                        sb.Append("" + FinshPrice + ",");
                                        sb.Append("'',");
                                        sb.Append("'" + ds.Tables[0].Rows[a]["RelationId"].ToString() + "'");
                                        sb.Append(");");
                                        sb.AppendLine();

                                        //
                                        //sb.Append("update H_PurchaseRelation set DecomposePrice=(select DecomposePrice from H_PurchaseRelation where Id='" + ds.Tables[0].Rows[a]["RelationId"].ToString() + "') where Id='" + newid2 + "';");
                                        //sb.AppendLine();

                                        //3：更改主单据状态为完成状态
                                        sb.Append("update H_Purchase set FinancialState=2,GetNumber='" + GetNumber + "' where Id='" + ds.Tables[0].Rows[a]["Id"] + "';");
                                        sb.AppendLine();
                                      
                                        //5：添加退货单到付款单
                                        sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                        sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                        sb.Append("getdate(),");
                                        sb.Append("'" + CurrentMaster.Id + "',");
                                        sb.Append("1,");
                                        sb.Append("" + FinshPrice + ",");//退货金额
                                        sb.Append("0,");//退货盈亏
                                        sb.Append("'',");
                                        sb.Append("'" + newid2 + "',");
                                        sb.Append("'" + ds.Tables[0].Rows[a]["Id"] + "',");
                                        sb.Append("'" + FinanceId + "'");
                                        sb.Append(");");
                                        sb.AppendLine();
                                        #endregion
                                        break;
                                    }
                                    #endregion

                                }
                            }
                            #endregion
                        }
                        else
                        {
                            var PayMoney = MainMoney - MainsMoney;
                            #region===添加付款单明细
                            if (MainDetailId.Length > 0)
                            {
                                for (var a = 0; a < MainDetailId.Length; a++)
                                {
                                    if (!string.IsNullOrEmpty(MainDetailId[a]))
                                    {
                                        //4:添加先前已有的明细到付款单
                                        var model = new BllHPurchaseRelation().LoadData(MainDetailId[a]);
                                        if (model != null)
                                        {
                                            //清空母记录信息
                                            if (model.IsDecompose == true)
                                            {
                                                sb.Append("update H_PurchaseRelation set Status=3,Price=0,DecomposePrice+=" + model.Price + " where Id='" + MainDetailId[a] + "';");
                                                sb.AppendLine();
                                            }
                                            else {
                                                sb.Append("update H_PurchaseRelation set Status=3,Price=0 where Id='" + MainDetailId[a] + "';");
                                                sb.AppendLine();
                                            }
                                            //拆分金额
                                            string RelationId = model.Id;
                                            if (model.IsDecompose == true)
                                            {
                                                var DecomposePrice =model.Price;
                                                RelationId = Guid.NewGuid().ToString();
                                                sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS,PID)values(");
                                                sb.Append("'" + RelationId + "',");
                                                sb.Append("'" + model.PurchaseId + "',");
                                                sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                                                sb.Append("'',");
                                                sb.Append("0,");//Price 剩余金额
                                                sb.Append("1,"); //是否拆分
                                                sb.Append("" + DecomposePrice + ",");
                                                sb.Append("'',");
                                                sb.Append("'" + MainDetailId[a] + "'");
                                                sb.Append(");");
                                                sb.AppendLine();
                                            }

                                            sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                                            sb.Append("'" + Guid.NewGuid().ToString() + "',");
                                            sb.Append("getdate(),");
                                            sb.Append("'" + CurrentMaster.Id + "',");
                                            sb.Append("1,");
                                            sb.Append("'" + model.Price + "',");//剩余退货金额
                                            sb.Append("0,");//退货盈亏
                                            sb.Append("'',");
                                            sb.Append("'" + RelationId + "',");
                                            sb.Append("'" + model.PurchaseId + "',");
                                            sb.Append("'" + FinanceId + "'");
                                            sb.Append(");");
                                            sb.AppendLine();

                                            //6更改明细主键的采购编码
                                            sb.Append(@"update H_Purchase set GetNumber='" + GetNumber + "' where Id='" + model.PurchaseId + "';");
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    if (new BllHOrderIn().ExecuteNonQueryByText(sb.ToString()) > 0)
                    {
                        sb.Clear();
                        json.success = true;
                        LogInsert(OperationTypeEnum.操作, "提交财务", "保存失败.");
                    }
                    else
                    {
                        json.success = false;
                        json.msg = "操作失败";
                        LogInsert(OperationTypeEnum.操作, "提交财务", "保存成功.");
                    }
                }
              
            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "提交财务", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ===批量备注主表  EditRemark
        /// <summary>
        /// 批量备注主表
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void EditRemark()
        {
            var msg = new ModJsonResult();
            try
            {
                string id = Request["ids"].ToString();
                string Remark = Request["Remark"].ToString();
                string sql = "update H_OrderIn set Remark=CAST(Remark AS VARCHAR)+'" + Remark + "' where Id in(" + id + ")";
                int result = new BllHFinance().ExecuteNonQueryByText(sql);
                if (result <= 0)
                {
                    msg.success = false;
                    msg.msg = "修改失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "财务池备注批量修改", "备注修改失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "财务池备注批量修改", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #endregion


        /// <summary>
        /// 获取组合查询条件
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public void GetSearch(ref Search search)
        {
            #region ===面板查询
            //财务状态
            if (!string.IsNullOrEmpty(Request["FinancialState"]))
            {
                if (Request["FinancialState"].ToString() != "-1")
                {
                    search.AddCondition("PayStatus=" + Request["FinancialState"].ToString());
                }
            }
            //付款方式
            if (!string.IsNullOrEmpty(Request["PaymentType"]))
            {
                if (Request["PaymentType"].ToString() != "-1")
                {
                    search.AddCondition("PaymentType=" + Request["PaymentType"].ToString());
                }
            }
            //结账方式
            if (!string.IsNullOrEmpty(Request["CheckoutType"]))
            {
                if (Request["CheckoutType"].ToString() != "-1")
                {
                    search.AddCondition("CheckoutType=" + Request["CheckoutType"].ToString());
                }
            }
            //入库单号
            if (!string.IsNullOrEmpty(Request["InNumber"]))
            {
                search.AddCondition("InNumber like '%" + Request["InNumber"].ToString() + "%'");
            }
            //供应商
            if (!string.IsNullOrEmpty(Request["CusterName"]))
            {
                search.AddCondition("CusterName like '%" + Request["CusterName"].ToString().Trim() + "%'");
            }
            //供应商Id
            if (!string.IsNullOrEmpty(Request["CusterId"]))
            {
                search.AddCondition("CusterId='" + Request["CusterId"].ToString().Trim() + "'");
            }
            //备注
            if (!string.IsNullOrEmpty(Request["FinanceRemark"]))
            {
                search.AddCondition("FinanceRemark like '%" + Request["FinanceRemark"].ToString().Trim() + "%'");
            }
            // 单据日期
            if (!string.IsNullOrEmpty(Request["BegBillDate"]) || !string.IsNullOrEmpty(Request["EndBillDate"]))
            {
                if (!string.IsNullOrEmpty(Request["BegBillDate"]))
                {
                    search.AddCondition("BillDate>'" + Convert.ToDateTime(Request["BegBillDate"]).AddDays(-1).ToShortDateString() + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndBillDate"]))
                {
                    search.AddCondition("BillDate<='" + Convert.ToDateTime(Request["EndBillDate"]).ToShortDateString() + "'");
                }
            }
            // 付款金额
            if (!string.IsNullOrEmpty(Request["BegPaymentAmmount"]) || !string.IsNullOrEmpty(Request["EndPaymentAmmount"]))
            {
                if (!string.IsNullOrEmpty(Request["BegPaymentAmmount"]))
                {
                    search.AddCondition("PaymentAmmount>='" + Convert.ToDouble(Request["BegPaymentAmmount"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndPaymentAmmount"]))
                {
                    search.AddCondition("PaymentAmmount<='" + Convert.ToDouble(Request["EndPaymentAmmount"]) + "'");
                }
            }
            #endregion
        }

        #region ===Finance 财务付款单

        #region==页面主表列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            GetSearch(ref search);
            var jsonResult = new BllHFinance().SearchData(search);
            LogInsert(OperationTypeEnum.访问, "财务付款单", CurrentMaster.UserName + "页面访问正常.");
            WriteJsonToPage(jsonResult);
        }
        #endregion

        #region==页面子表列表 SearchDataDetail
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchDataDetail()
        {
            string maid = Request["MainId"].ToString();
            var search = base.GetSearch();

            search.AddCondition("FinanceId='" + maid + "'");
            var jsonResult = new BllHFinanceDetail().SearchData(search);
            WriteJsonToPage(jsonResult);
        }
        #endregion


       

        #endregion

        #region===确认支付/取消支付  SureGive
        /// <summary>
        /// 确认支付/取消支付
        /// </summary>
        public void SureGive(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                int PayStatus = int.Parse(Request["Status"].ToString());
                StringBuilder sb = new StringBuilder();
                sb.Append("update H_Finance set PayStatus=" + PayStatus + ",PayTime=getdate() where Id in(" + id + ")");
                sb.AppendLine();

                //取消支付
                if (PayStatus == 0)
                {
                    //更改主表状态
                    sb.Append("update H_OrderIn set FinancialState=2 where Id in(select OrderInId from view_HFinance where Id in(" + id + "))");
                    sb.AppendLine();

                    //获取主表下所有的退货单
                    sb.Append(@"update H_Purchase set FinancialState=2 where Id in(
  select view_HFinanceDetail.PurchaseId from view_HFinanceDetail
  inner join H_PurchaseRelation
  on view_HFinanceDetail.RelationId=H_PurchaseRelation.Id
   where view_HFinanceDetail.FinanceId in(" + id + ") and H_PurchaseRelation.Status=3 and Price=0)");
                    sb.AppendLine();
                    if (new BllHPurchase().ExecuteNonQueryByText(sb.ToString()) > 0)
                    {
                        msg.success = true;
                        LogInsert(OperationTypeEnum.操作, "财务付款单取消支付", "取消支付成功.");
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
                        LogInsert(OperationTypeEnum.操作, "财务付款单取消支付", "取消支付失败.");
                    }
                }
                //已支付
                else
                {
                    //更改主表状态
                    sb.Append("update H_OrderIn set FinancialState=3 where Id in(select OrderInId from view_HFinance where Id in(" + id + "))");
                    sb.AppendLine();

                    //获取主表下所有的退货单
                    sb.Append(@"update H_Purchase set FinancialState=3 where Id in(
  select view_HFinanceDetail.PurchaseId from view_HFinanceDetail
  inner join H_PurchaseRelation
  on view_HFinanceDetail.RelationId=H_PurchaseRelation.Id
   where view_HFinanceDetail.FinanceId in(" + id + ") and H_PurchaseRelation.Status=3 and Price=0)");
                    sb.AppendLine();
                    sb.AppendLine();
                    if (new BllHPurchase().ExecuteNonQueryByText(sb.ToString()) > 0)
                    {
                        msg.success = true;
                        LogInsert(OperationTypeEnum.操作, "财务付款单确认支付", "确认支付成功.");
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
                        LogInsert(OperationTypeEnum.操作, "财务付款单确认支付", "确认支付失败.");
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "财务付款单支付", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region===付款单驳回  AuditBack
        /// <summary>
        /// 付款单驳回
        /// </summary>
        public void AuditBack(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                string[] str = id.Split(',');
                StringBuilder sb = new StringBuilder();
                BllHFinance Bll = new BllHFinance();

                for (var i = 0; i < str.Length; i++)
                {
                    //查询付款单主表信息
                    string sql = "select * from view_HFinance where Id=" + str[i] + ";";
                    sql += " select * from view_HFinanceDetail where FinanceId=" + str[i] + ";";
                    DataSet result = Bll.ExecuteDataSet(sql);

                    DataTable ds = result.Tables[0];//主表
                    DataTable Detail = result.Tables[1];//明细表

                    //驳回任务池主单据
                    sb.Append("update H_OrderInRelation set Status=-1 where Id='" + ds.Rows[0]["RelationId"].ToString() + "';");
                    sb.AppendLine();

                    for (var a = 0; a < Detail.Rows.Count; a++)
                    {
                        var mode = new BllHPurchaseRelation().LoadData(Detail.Rows[a]["RelationId"].ToString());
                        if (mode != null)
                        {
                            string RelationId = Detail.Rows[a]["RelationId"].ToString();
                            //拆分过
                            if (mode.IsDecompose == true)
                            {
                                //判断总记录是否已经审核
                                //查找母节点
                                string searchSql = "select * from H_PurchaseRelation where PurchaseId='" + mode.PurchaseId + "';";
                                DataSet sear = new BllHPurchaseRelation().ExecuteDataSet(searchSql);
                                //过滤出PID是母节点的
                                DataView dv = sear.Tables[0].DefaultView;
                                dv.RowFilter = "PID=''";
                                DataTable dvv = dv.ToTable();

                                if (dvv.Rows.Count > 0)
                                {
                                    RelationId = dvv.Rows[0]["Id"].ToString();
                                    if (sear.Tables[0].Rows.Count == 2)
                                    {
                                        sb.Append("update H_PurchaseRelation set IsDecompose=0,Status=-1,DecomposePrice-=" + Convert.ToDecimal(Detail.Rows[a]["ReturnAmount"].ToString()) + ",Price+=" + Convert.ToDecimal(Detail.Rows[a]["ReturnAmount"].ToString()) + " where Id='" + RelationId + "';");
                                        sb.AppendLine();
                                    }
                                    else
                                    {
                                        sb.Append("update H_PurchaseRelation set Status=-1,DecomposePrice-=" + Convert.ToDecimal(Detail.Rows[a]["ReturnAmount"].ToString()) + ",Price+=" + Convert.ToDecimal(Detail.Rows[a]["ReturnAmount"].ToString()) + " where Id='" + RelationId + "';");
                                        sb.AppendLine();
                                    }

                                    //删除现有分解的单据
                                    sb.Append("delete from H_PurchaseRelation where Id='" + Detail.Rows[a]["RelationId"].ToString() + "';");
                                    sb.AppendLine();
                                }
                            }
                            else
                            {
                                sb.Append("update H_PurchaseRelation set Status=-1,Price+=" + Convert.ToDecimal(Detail.Rows[a]["ReturnAmount"].ToString()) + " where Id='" + RelationId + "';");
                                sb.AppendLine();
                            }
                        }
                    }
                    //删除付款单主表
                    sb.Append("delete from H_Finance where Id=" + str[i] + ";");
                    sb.AppendLine();
                    //删除付款单子表
                    sb.Append("delete from H_FinanceDetail where FinanceId=" + str[i] + ";");
                    sb.AppendLine();

                    int re=new BllHPurchase().ExecuteNonQueryByText(sb.ToString());
                    if (re > 0)
                    {
                        sb.Clear();
                    }
                }
                msg.success = true;
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "财务付款单驳回", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData()
        {
            var msg = new ModJsonResult();
            try
            {
                string id = Request["id"].ToString();
                string Remark = Request["Remark"].ToString();
                ModHFinance model = new BllHFinance().LoadData(id);
                model.FinanceRemark = Remark;
                int result = new BllHFinance().Update(model);
                if (result <= 0)
                {
                    msg.success = false;
                    msg.msg = "修改失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "财务付款单备注修改", "备注修改失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "财务付款单备注修改", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #region ==导出数据ImportOut
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="mod"></param>
        public ActionResult ImportOut()
        {
            var search = base.GetSearch();
            var fileTypeName = "跨区域涉税事项报告表台账";
            var fileName = fileTypeName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            try
            {
                string IdList = Request["IdList"].ToString();
                DataTable dt = new BllHFinance().GetAllList(IdList).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RCol(dt);
                    Workbook w = new Workbook();
                    Worksheet ws = w.Worksheets[0];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        var col = dt.Columns[i];
                        ws.Cells[0, i].PutValue(col.ColumnName);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var row = dt.Rows[i];
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            //格式化状态栏
                            if (dt.Columns[j].ColumnName.Trim() == "结账方式")
                            {
                                string EmergencyState = "";
                                switch (row[j].ToString())
                                {
                                    case "0":
                                        EmergencyState = "未设置";
                                        break;
                                    case "1":
                                        EmergencyState = "月结";
                                        break;
                                    default:
                                        EmergencyState = "日结";
                                        break;
                                }
                                ws.Cells[i + 1, j].PutValue(EmergencyState);
                            }
                            else if (dt.Columns[j].ColumnName.Trim() == "付款方式")
                            {
                                string EmergencyState = "";
                                switch (row[j].ToString())
                                {
                                    case "0":
                                        EmergencyState = "未设置";
                                        break;
                                    case "1":
                                        EmergencyState = "支付宝";
                                        break;
                                    case "2":
                                        EmergencyState = "工行";
                                        break;
                                    default:
                                        EmergencyState = "农行";
                                        break;
                                }
                                ws.Cells[i + 1, j].PutValue(EmergencyState);
                            }
                            else
                            {
                                ws.Cells[i + 1, j].PutValue(row[j]);
                            }
                        }
                    }
                    if (!System.IO.Directory.Exists(Server.MapPath("~/ExportFile/")))
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/ExportFile/"));
                    w.Save(Server.MapPath("~/ExportFile/") + fileName);
                }
                LogInsert(OperationTypeEnum.操作, "财务付款单批量导出", "导出成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "财务付款单批量导出", "操作异常信息:" + ex);
            }
            return Json(new { success = true, msg = "http://" + Request.Headers["host"] + "/ExportFile/" + fileName });
        }

        private static void RCol(DataTable dt)
        {
            List<DataColumn> removeCols = new List<DataColumn>();
            foreach (DataColumn col in dt.Columns)
            {
                switch (col.ColumnName)
                {
                    case "CusterName":
                        col.ColumnName = "供应商";
                        break;
                    case "ReturnAmount":
                        col.ColumnName = "供应商金额";
                        break;
                    case "ProfitTotal":
                        col.ColumnName = "退款金额";
                        break;
                    case "PaymentAmmount":
                        col.ColumnName = "付款金额";
                        break;
                    case "CheckoutType":
                        col.ColumnName = "结账方式";
                        break;
                    case "PaymentType":
                        col.ColumnName = "付款方式";
                        break;
                    case "AccountName":
                        col.ColumnName = "名字";
                        break;
                    case "AccountNum":
                        col.ColumnName = "账号";
                        break;
                    default:
                        removeCols.Add(col);
                        break;

                }
            }
            foreach (var item in removeCols)
            {
                dt.Columns.Remove(item.ColumnName);
            }
        }

        // <summary>
        /// 图片转换成字节流
        /// </summary>
        /// <param name="img">要转换的Image对象</param>
        /// <returns>转换后返回的字节流</returns>
        public byte[] ImgToByt(Image img)
        {
            MemoryStream ms = new MemoryStream();
            byte[] imagedata = null;
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            imagedata = ms.GetBuffer();
            return imagedata;
        }
        /// <summary>
        /// 字节流转换成图片
        /// </summary>
        /// <param name="byt">要转换的字节流</param>
        /// <returns>转换得到的Image对象</returns>
        public Image BytToImg(byte[] byt)
        {
            MemoryStream ms = new MemoryStream(byt);
            Image img = Image.FromStream(ms);
            return img;
        }
        //
        /// <summary>
        /// 根据图片路径返回图片的字节流byte[]
        /// </summary>
        /// <param name="imagePath">图片路径</param>
        /// <returns>返回的字节流</returns>
        public byte[] getImageByte(string imagePath)
        {
            FileStream files = new FileStream(imagePath, FileMode.Open);
            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }
        ///// <summary>
        ///// 导出一般数据
        ///// </summary>
        ///// <param name="dt"></param>
        //public void ToExcel1(DataTable dt)
        //{
        //    AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
        //    doc.FileName = DateTime.Now.ToString("yyyyMMdd") + "财务付款单批量导出.xls";
        //    string SheetName = "Sheet1";
        //    //记录条数
        //    int mCount = dt.Rows.Count;
        //    Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
        //    Cells cells = sheet.Cells;

        //    //单元格格式设置
        //    AppLibrary.WriteExcel.XF XFstyle = doc.NewXF();
        //    XFstyle.HorizontalAlignment = AppLibrary.WriteExcel.HorizontalAlignments.Centered;   //设置左右对齐
        //    XFstyle.Font.FontName = "宋体";  //字体
        //    XFstyle.UseMisc = true;
        //    XFstyle.Font.Bold = true;   //字体加宽
        //    XFstyle.VerticalAlignment = VerticalAlignments.Centered;  //设置上下对齐
        //    XFstyle.Pattern = 1;  //背景色(与下共用)
        //    XFstyle.PatternColor = Colors.Silver; //背景色(与上共用)

        //    //第一行表头
        //    for (int i = 0; i < title.Length; i++)
        //    {
        //        cells.Add(1, i + 1, title[i].Trim());
        //    }
        //    for (int m = 0; m < mCount; m++)
        //    {
        //        for (int j = 0; j < title.Length; j++)
        //        {

        //            //时间类型
        //            if (dt.Columns[j].DataType == (new DateTime()).GetType())
        //            {
        //                if (!string.IsNullOrEmpty(dt.Rows[m][j].ToString()))
        //                {
        //                    cells.Add(m + 2, j + 1, Convert.ToDateTime(dt.Rows[m][j].ToString()).ToString("yyyy年MM月dd日"));
        //                }
        //                else
        //                {
        //                    cells.Add(m + 2, j + 1, dt.Rows[m][j].ToString());
        //                }
        //            }
        //            if (dt.Columns[j].DataType == typeof(float) || dt.Columns[j].DataType == typeof(double) || dt.Columns[j].DataType == typeof(decimal))
        //            {
        //                cells.Add(m + 2, j + 1, Convert.ToDouble(dt.Rows[m][j].ToString()), XFstyle);
        //            }
        //            else
        //            {
        //                if (title[j] == "结账方式")
        //                {
        //                    if (dt.Rows[m][j].ToString() == "0")
        //                    {
        //                        cells.Add(m + 2, j + 1, "未设置");
        //                    }
        //                    else if (dt.Rows[m][j].ToString() == "1")
        //                    {
        //                        cells.Add(m + 2, j + 1, "月结");
        //                    }
        //                    else
        //                    {
        //                        cells.Add(m + 2, j + 1, "日结");
        //                    }
        //                }
        //                else if (title[j] == "付款方式")
        //                {
        //                    if (dt.Rows[m][j].ToString() == "0")
        //                    {
        //                        cells.Add(m + 2, j + 1, "未设置");
        //                    }
        //                    else if (dt.Rows[m][j].ToString() == "1")
        //                    {
        //                        cells.Add(m + 2, j + 1, "支付宝");
        //                    }
        //                    else if (dt.Rows[m][j].ToString() == "2")
        //                    {
        //                        cells.Add(m + 2, j + 1, "工行");
        //                    }
        //                    else
        //                    {
        //                        cells.Add(m + 2, j + 1, "农行");
        //                    }
        //                }
        //                else
        //                {
        //                    cells.Add(m + 2, j + 1, dt.Rows[m][j].ToString());
        //                }
        //            }
        //        }
        //    }
        //    doc.Send();
        //    Response.End();
        //}

        //#region 导出带图片数据
        //protected void ToExcel(DataTable dt)
        //{
        //    if (dt != null)
        //    {
        //        #region 操作excel
        //        Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        //        Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
        //        xlWorkBook = new Microsoft.Office.Interop.Excel.Application().Workbooks.Add(Type.Missing);
        //        xlWorkBook.Application.Visible = false;
        //        xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[1];
        //        //设置标题
        //        xlWorkSheet.Cells[1, 1] = "二维码编号";
        //        xlWorkSheet.Cells[1, 2] = "图片";
        //        xlWorkSheet.Cells[1, 3] = "设备名称";
        //        xlWorkSheet.Cells[1, 4] = "设备规格";
        //        xlWorkSheet.Cells[1, 5] = "设备型号";
        //        xlWorkSheet.Cells[1, 6] = "设备位置";
        //        xlWorkSheet.Cells[1, 7] = "生产日期";
        //        xlWorkSheet.Cells[1, 8] = "过期日期";
        //        xlWorkSheet.Cells[1, 9] = "责任人";
        //        xlWorkSheet.Cells[1, 10] = "责任部门";
        //        xlWorkSheet.Cells[1, 11] = "电话";
        //        xlWorkSheet.Cells[1, 12] = "所属分类";
        //        xlWorkSheet.Cells[1, 13] = "添加时间";
        //        //设置宽度            
        //        ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1]).ColumnWidth = 35;//图片的宽度
        //        ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 2]).ColumnWidth = 15;//图片的宽度
        //        ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 7]).ColumnWidth = 20;//图片的宽度
        //        ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 8]).ColumnWidth = 20;//图片的宽度
        //        ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 11]).ColumnWidth = 20;//图片的宽度
        //        ((Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 13]).ColumnWidth = 20;//图片的宽度
        //        //设置字体
        //        xlWorkSheet.Cells.Font.Size = 12;
        //        xlWorkSheet.Cells.Rows.RowHeight = 100;

        //        #region 为excel赋值

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            //为单元格赋值。
        //            xlWorkSheet.Cells[i + 2, 1] = dt.Rows[i]["QRCode"].ToString();
        //            xlWorkSheet.Cells[i + 2, 2] = "";
        //            xlWorkSheet.Cells[i + 2, 3] = dt.Rows[i]["Name"].ToString();
        //            xlWorkSheet.Cells[i + 2, 4] = dt.Rows[i]["Specifications"].ToString();
        //            xlWorkSheet.Cells[i + 2, 5] = dt.Rows[i]["Model"].ToString();
        //            xlWorkSheet.Cells[i + 2, 6] = dt.Rows[i]["PlacesName"].ToString();
        //            xlWorkSheet.Cells[i + 2, 7] = Convert.ToDateTime(dt.Rows[i]["ProductionDate"].ToString()).ToString("yyyy-MM-dd");
        //            xlWorkSheet.Cells[i + 2, 8] = Convert.ToDateTime(dt.Rows[i]["LostTime"].ToString()).ToString("yyyy-MM-dd");
        //            xlWorkSheet.Cells[i + 2, 9] = dt.Rows[i]["Responsible"].ToString();
        //            xlWorkSheet.Cells[i + 2, 10] = dt.Rows[i]["DeptName"].ToString();
        //            xlWorkSheet.Cells[i + 2, 11] = dt.Rows[i]["LoginName"].ToString();
        //            xlWorkSheet.Cells[i + 2, 12] = dt.Rows[i]["GroupName"].ToString();
        //            xlWorkSheet.Cells[i + 2, 13] = Convert.ToDateTime(dt.Rows[i]["CreateTime"].ToString()).ToString("yyyy-MM-dd");
        //            string filename = Server.MapPath("~/" + dt.Rows[i]["Img"].ToString());
        //            if (System.IO.File.Exists(filename))
        //            {
        //                //声明一个pictures对象,用来存放sheet的图片
        //                //xlWorkSheet.Shapes.AddPicture(filename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue,220,100+ i * 100, 100, 100);
        //            }
        //        }
        //        #endregion

        //        #region 保存excel文件
        //        string filePath = Server.MapPath("/UploadFile/QrExport/");
        //        new FileHelper().CreateDirectory(filePath);
        //        filePath += DateTime.Now.ToString("yyyymmddHHmmss") + "财务付款单批量导出.xls";
        //        try
        //        {
        //            xlWorkBook.SaveAs(filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, null, null, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, null, null, null, null, null);
        //        }
        //        catch { }
        //        xlWorkBook.Application.Quit();
        //        xlWorkSheet = null;
        //        xlWorkBook = null;
        //        GC.Collect();
        //        System.GC.WaitForPendingFinalizers();
        //        #endregion
        //        #endregion
        //        #region 导出到客户端
        //        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        //        Response.AppendHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode("财务付款单批量导出", System.Text.Encoding.UTF8) + ".xls");
        //        Response.ContentType = "Application/excel";
        //        Response.WriteFile(filePath);
        //        Response.End();
        //        #endregion
        //        KillProcessexcel("EXCEL");
        //    }
        //}
        //#endregion

        //#region 杀死进程
        //private void KillProcessexcel(string processName)
        //{ //获得进程对象，以用来操作
        //    System.Diagnostics.Process myproc = new System.Diagnostics.Process();
        //    //得到所有打开的进程
        //    try
        //    {
        //        //获得需要杀死的进程名
        //        foreach (Process thisproc in Process.GetProcessesByName(processName))
        //        { //立即杀死进程
        //            thisproc.Kill();
        //        }
        //    }
        //    catch (Exception Exc)
        //    {
        //        throw new Exception("", Exc);
        //    }
        //}
        //#endregion


        #endregion

        #region ===批量备注明细表  EditRemark
        /// <summary>
        /// 批量备注主表
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void EditDetailRemark()
        {
            var msg = new ModJsonResult();
            try
            {
                string id = Request["ids"].ToString();
                string Remark = Request["Remark"].ToString();
                string sql = "update H_Purchase set Remark=CAST(Remark AS VARCHAR)+'" + Remark + "' where Id in(" + id + ")";
                int result = new BllHFinance().ExecuteNonQueryByText(sql);
                if (result <= 0)
                {
                    msg.success = false;
                    msg.msg = "修改失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "财务池备注批量修改", "备注修改失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "财务池备注批量修改", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

    }
}
