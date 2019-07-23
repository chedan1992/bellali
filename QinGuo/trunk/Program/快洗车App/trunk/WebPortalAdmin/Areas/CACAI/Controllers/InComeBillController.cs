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
using System.Data;
using System.IO;
using Aspose.Cells;

namespace WebPortalAdmin.Areas.CACAI.Controllers
{
    /// <summary>
    ///进仓单据核对凭证 
    /// </summary>
    public class InComeBillController : BaseController<ModHOrderIn>
    {

        string[] title;  //导出的标题
        string[] field;  //导出对应字段

        /// <summary>
        /// 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 凭证审核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult InComeAudit()
        {
            return View();
        }
       
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
                string conditon = "(";
                string[] FinancialState = Request["FinancialState"].ToString().Split(',');
                for (var i = 0; i < FinancialState.Length; i++)
                {
                    if (i == FinancialState.Length - 1)
                    {
                        conditon += " FinancialState=" + FinancialState[i];
                    }
                    else
                    {
                        conditon += " FinancialState=" + FinancialState[i] + " or ";
                    }
                }
                conditon += ")";
                search.AddCondition(conditon);
            }
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
            //商品名称
            if (!string.IsNullOrEmpty(Request["GoodName"]))
            {
                //根据商品名称查询ID
                search.AddCondition("Id in ( select MainId from H_OrderInDetail where GoodName like '%" + Request["GoodName"].ToString().Trim() + "%')");
            }
            //根据明细备注
            if (!string.IsNullOrEmpty(Request["DetailRemark"]))
            {
                search.AddCondition("Id in ( select MainId from H_OrderInDetail where Remark like '%" + Request["DetailRemark"].ToString().Trim() + "%')");
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
            //数量差
            if (!string.IsNullOrEmpty(Request["CountDel"]))
            {
                var CountDel = Convert.ToBoolean(Request["CountDel"]);
                if (CountDel == true)
                {
                    search.AddCondition("Id in ( select MainId from H_OrderInDetail where BillNum-[Count]!=0)");
                }
            }
            //价格差
            if (!string.IsNullOrEmpty(Request["PirceDel"]))
            {
                var PirceDel = Convert.ToBoolean(Request["PirceDel"]);
                if (PirceDel == true)
                {
                    search.AddCondition("Id in ( select MainId from H_OrderInDetail where BillPrice-[Price]!=0)");
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
          
            #endregion
        }
        #region===单据管理

        #region==页面主表列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            GetSearch(ref search);

            //if (!CurrentMaster.IsMain)
            //{
            //    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
            //    {
            //        search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");//自己查看自己的
            //    }
            //}
            var jsonResult = new BllHOrderIn().SearchData(search);
            LogInsert(OperationTypeEnum.访问, "进仓单据核对凭证", CurrentMaster.UserName + "页面访问正常.");
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

            search.PageSize = 1000;

            search.AddCondition("MainId='" + maid + "'");
            var jsonResult = new BllHOrderInDetail().SearchData(search);
            WriteJsonToPage(jsonResult);
        }
        #endregion

        #region ==删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                string sql = "update H_OrderIn set Status=" + (int)StatusEnum.删除 + " where Id in(" + id + ")";
                if (new BllHOrderIn().ExecuteNonQueryByText(sql) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "采购入库单删除", "采购入库单删除成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "采购入库单删除", "采购入库单删除失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "采购入库单删除", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===清空 DeleteAll
        /// <summary>
        /// 清空
        /// </summary>
        public void DeleteAll()
        {
            var msg = new ModJsonResult();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from H_OrderIn;");
                sb.AppendLine();
                sb.Append("delete from H_OrderInDetail;");
                new BllHOrderIn().ExecuteNonQueryByText(sb.ToString());
                msg.success = true;
                LogInsert(OperationTypeEnum.操作, "入库单清空", "入库单清空成功.");
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "入库单清空", "操作异常信息:" + ex);
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

                ModHOrderIn model = new BllHOrderIn().LoadData(id);
                model.Remark = Remark;
                int result = new BllHOrderIn().Update(model);
                if (result <= 0)
                {
                    msg.success = false;
                    msg.msg = "修改失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "进仓单据核对凭证修改", "进仓单据核对凭证修改失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "进仓单据核对凭证修改", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #region ===保存明细表单 SaveDataDetail
        /// <summary>
        /// 保存明细表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveDataDetail()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                BllHOrderInDetail Bll = new BllHOrderInDetail();
                string id = Request["id"].ToString();
                string BillNum = (string.IsNullOrEmpty(Request["BillNum"].ToString()) == true ? "0" : Request["BillNum"].ToString());
                string BillPrice = (string.IsNullOrEmpty(Request["BillPrice"].ToString()) == true ? "0" : Request["BillPrice"].ToString());
                string BillMoney = (string.IsNullOrEmpty(Request["BillMoney"].ToString()) == true ? "0" : Request["BillMoney"].ToString());
                string CheckNum = (string.IsNullOrEmpty(Request["CheckNum"].ToString()) == true ? "0" : Request["CheckNum"].ToString());
                string CheckPrice = (string.IsNullOrEmpty(Request["CheckPrice"].ToString()) == true ? "0" : Request["CheckPrice"].ToString());
                string LosstPrice = (string.IsNullOrEmpty(Request["LosstPrice"].ToString()) == true ? "0" : Request["LosstPrice"].ToString());
                string Remark = Request["Remark"].ToString();

                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    ModHOrderInDetail model = Bll.LoadData(id);
                    model.BillNum = int.Parse(BillNum);
                    model.BillPrice = Convert.ToDecimal(BillPrice);
                    model.BillMoney = Convert.ToDecimal(BillMoney);
                    model.CheckNum = int.Parse(CheckNum);
                    model.CheckPrice = Convert.ToDecimal(CheckPrice);
                    model.LosstPrice = Convert.ToDecimal(LosstPrice);
                    model.Remark = Remark;

                    int result = Bll.Update(model);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "进仓单据核对凭证明细修改", "进仓单据核对凭证明细修改失败.");
                    }
                    else
                    {
                        //统计盈亏金额
                        string sql = "update H_OrderIn set oldPrice=(select SUM(Money) from H_OrderInDetail where MainId='" + model.MainId + "' and Status!=" + (int)StatusEnum.删除 + "),LossPrice=(select SUM(LosstPrice) from H_OrderInDetail where MainId='" + model.MainId + "' and Status!=" + (int)StatusEnum.删除 + "),TotalPrice=(select SUM(BillMoney) from H_OrderInDetail where MainId='" + model.MainId + "' and Status!=" + (int)StatusEnum.删除 + ") where Id='" + model.MainId + "';";
                        int retu = Bll.ExecuteNonQueryByText(sql);
                        if (retu > 0)
                        {
                            var OrderIn = new BllHOrderIn().LoadData(model.MainId);
                            json.msg = OrderIn.LossPrice.ToString() + "," + OrderIn.TotalPrice.ToString();//返回盈亏金额 +供应商金额
                        }
                        else
                        {
                            json.msg = "0";
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "进仓单据核对凭证明细保存", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
        }
        #endregion

        #region ===保存明细表单表单 SaveDetail
        /// <summary>
        /// 保存明细表单表单  SaveDetail
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveDetail(ModHOrderInDetail mod)
        {
            var msg = new ModJsonResult();
            try
            {
                BllHOrderInDetail bll = new BllHOrderInDetail();
                int result = 0;
                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    ModHOrderInDetail model = bll.LoadData(mod.Id);
                    mod.Id = model.Id;
                    mod.Status = model.Status;
                    mod.CreateTime = model.CreateTime;
                    mod.CreaterId = model.CreaterId;
                    mod.MainId = model.MainId;

                    result = bll.Update(mod);
                    if (result <= 0)
                    {
                        msg.success = false;
                        msg.msg = "修改失败,请稍后再操作!";
                    }
                }
                else
                {
                    mod.Id = Guid.NewGuid().ToString();
                    mod.Status = (int)StatusEnum.正常;
                    mod.CreaterId = CurrentMaster.Id;
                    mod.CreateTime = DateTime.Now;
                    result = bll.Insert(mod);
                    if (result > 0)
                    {
                        msg.success = true;
                        msg.msg = " 保存成功!";
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = " 保存失败,请稍后再操作!";
                    }
                }

                if (msg.success == true)
                {
                    StringBuilder sb = new StringBuilder();
                    //统计盈亏金额
                    sb.Append("update H_OrderIn set LossPrice=(select SUM(LosstPrice) from H_OrderInDetail where MainId='" + mod.MainId + "' and Status!=" + (int)StatusEnum.删除 + "),TotalPrice=(select SUM(BillMoney) from H_OrderInDetail where MainId='" + mod.MainId + "' and Status!=" + (int)StatusEnum.删除 + ") where Id='" + mod.MainId + "';");
                    int retu = bll.ExecuteNonQueryByText(sb.ToString());
                    if (retu > 0)
                    {
                        var OrderIn = new BllHOrderIn().LoadData(mod.MainId);
                        msg.success = true;
                        msg.msg = OrderIn.LossPrice.ToString() + "," + OrderIn.TotalPrice.ToString();//返回盈亏金额 +供应商金额
                    }
                }

            }
            catch (Exception)
            {
                msg.msg = "保存失败！";
                msg.success = false;
            }

            WriteJsonToPage(msg.ToString());

        }
        #endregion

        #region ==删除明细 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteDetail(string id, string key)
        {
            var msg = new ModJsonResult();
            try
            {
                string sql = "delete from H_OrderInDetail where Id in(" + id + ")";
                if (new BllHOrderIn().ExecuteNonQueryByText(sql) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "采购入库单明细删除", "明细删除成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "采购入库单明细删除", "明细删除失败.");
                }

                if (msg.success == true)
                {
                    StringBuilder sb = new StringBuilder();
                    //统计盈亏金额
                    sb.Append("update H_OrderIn set LossPrice=(select SUM(LosstPrice) from H_OrderInDetail where MainId='" + key + "' and Status!=" + (int)StatusEnum.删除 + "),TotalPrice=(select SUM(BillMoney) from H_OrderInDetail where MainId='" + key + "' and Status!=" + (int)StatusEnum.删除 + ") where Id='" + key + "';");
                    int retu = new BllHOrderIn().ExecuteNonQueryByText(sb.ToString());
                    if (retu > 0)
                    {
                        var OrderIn = new BllHOrderIn().LoadData(key);
                        msg.success = true;
                        msg.msg = OrderIn.LossPrice.ToString() + "," + OrderIn.TotalPrice.ToString();//返回盈亏金额 +供应商金额
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "采购入库单明细删除", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==合并明细 Combine
        /// <summary>
        /// 合并明细
        /// </summary>
        public void Combine(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllHOrderInDetail biz = new BllHOrderInDetail();
                var model = biz.LoadData(id);
                StringBuilder sb = new StringBuilder();
                if (model != null)
                {
                    //查询与他相同商品信息
                    string sql = "select * from H_OrderInDetail where StyleNum='" + model.StyleNum + "' and GoodName='" + model.GoodName + "' and FreightNum='" + model.FreightNum + "' and Price='" + model.Price + "' and MainId='" + model.MainId + "' and Id !='" + model.Id + "' AND Status!="+(int)StatusEnum.删除+";";
                    DataSet ds = biz.ExecuteDataSet(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (model.Code.IndexOf(ds.Tables[0].Rows[i]["Code"].ToString()) == -1)
                            {
                                model.Code += "," + ds.Tables[0].Rows[i]["Code"].ToString();//商品编码
                            }
                            if (model.GoodUnit.IndexOf(ds.Tables[0].Rows[i]["GoodUnit"].ToString()) == -1)
                            {
                                model.GoodUnit += "," + ds.Tables[0].Rows[i]["GoodUnit"].ToString();//颜色及规格
                            }
                            model.Count += int.Parse(ds.Tables[0].Rows[i]["Count"].ToString());
                            model.Money += Convert.ToDecimal(ds.Tables[0].Rows[i]["Money"].ToString());
                            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Remark"].ToString()))
                            {
                                model.Remark += ds.Tables[0].Rows[i]["Remark"].ToString();
                            }
                            //删除明细
                            sb.Append("update H_OrderInDetail set Status=" + (int)StatusEnum.删除 + " where Id='" + ds.Tables[0].Rows[i]["Id"].ToString() + "';");
                            sb.AppendLine();
                        }
                    }

                    model.BillNum = model.Count;
                    model.BillPrice = model.Price;
                    model.BillMoney = model.Money;

                    model.CheckNum = 0;
                    model.CheckPrice = 0;
                    model.LosstPrice = 0;

                    //更改主单据金额
                    int result = biz.Update(model);
                    if (result > 0)
                    {
                        //统计盈亏金额
                        sb.Append("update H_OrderIn set oldPrice=(select SUM(Money) from H_OrderInDetail where MainId='" + model.MainId + "' and  Status!=" + (int)StatusEnum.删除 + "),LossPrice=(select SUM(LosstPrice) from H_OrderInDetail where MainId='" + model.MainId + "' and Status!=" + (int)StatusEnum.删除 + "),TotalPrice=(select SUM(BillMoney) from H_OrderInDetail where MainId='" + model.MainId + "' and Status!=" + (int)StatusEnum.删除 + ") where Id='" + model.MainId + "';");
                        int retu = biz.ExecuteNonQueryByText(sb.ToString());
                        if (retu > 0)
                        {
                            var OrderIn = new BllHOrderIn().LoadData(model.MainId);
                            msg.success = true;
                            msg.msg = OrderIn.LossPrice.ToString() + "," + OrderIn.TotalPrice.ToString() + "," + ds.Tables[0].Rows.Count.ToString();//返回盈亏金额 +供应商金额+合并条数
                        }
                        else
                        {
                            msg.success = false;
                            msg.msg = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "采购入库单明细删除", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==合并全部明细 CombineAll
        /// <summary>
        /// 合并全部明细
        /// </summary>
        public void CombineAll(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllHOrderInDetail biz = new BllHOrderInDetail();
                //查询与他相同商品信息
                string sql = "select IsEdit=0,* from H_OrderInDetail where MainId ='" + id + "' and Status!=" + (int)StatusEnum.删除 + ";";
                DataSet ds = biz.ExecuteDataSet(sql);
                StringBuilder sb = new StringBuilder();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    var CombinNum = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToInt32(ds.Tables[0].Rows[i]["IsEdit"].ToString()) == 0)
                        {
                            string Id = ds.Tables[0].Rows[i]["Id"].ToString();
                            string Code = ds.Tables[0].Rows[i]["Code"].ToString();//商品编码
                            string GoodUnit = ds.Tables[0].Rows[i]["GoodUnit"].ToString();//颜色及规格
                            string GoodName = ds.Tables[0].Rows[i]["GoodName"].ToString();//商品名称
                            string Remark = ds.Tables[0].Rows[i]["Remark"].ToString();
                            decimal Price = Convert.ToDecimal(ds.Tables[0].Rows[i]["Price"].ToString());//单价
                            int Count = Convert.ToInt32(ds.Tables[0].Rows[i]["Count"].ToString());//数量
                            decimal Money = Convert.ToDecimal(ds.Tables[0].Rows[i]["Money"].ToString());//金额

                            var Combin = false;
                            //遍历循环ds里面和数据相同的
                            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                            {
                                if (ds.Tables[0].Rows[j]["Id"].ToString() != Id && Convert.ToInt32(ds.Tables[0].Rows[j]["IsEdit"].ToString()) == 0)
                                {
                                    if (ds.Tables[0].Rows[j]["StyleNum"].ToString() == ds.Tables[0].Rows[i]["StyleNum"].ToString() && ds.Tables[0].Rows[j]["FreightNum"].ToString() == ds.Tables[0].Rows[i]["FreightNum"].ToString() && Convert.ToDecimal(ds.Tables[0].Rows[j]["Price"].ToString()) == Convert.ToDecimal(ds.Tables[0].Rows[i]["Price"].ToString()))
                                    {
                                        Combin = true;//有合并的
                                        CombinNum++;
                                        ds.Tables[0].Rows[j]["IsEdit"] = "1";
                                        if (Code.IndexOf(ds.Tables[0].Rows[j]["Code"].ToString()) == -1)
                                        {
                                            Code += "," + ds.Tables[0].Rows[j]["Code"].ToString();
                                        }
                                        if (GoodUnit.IndexOf(ds.Tables[0].Rows[j]["GoodUnit"].ToString()) == -1)
                                        {
                                            GoodUnit += "," + ds.Tables[0].Rows[j]["GoodUnit"].ToString();
                                        }
                                        if (GoodName.IndexOf(ds.Tables[0].Rows[j]["GoodName"].ToString()) == -1)
                                        {
                                            GoodName += "," + ds.Tables[0].Rows[j]["GoodName"].ToString();
                                        }
                                        Money += Convert.ToDecimal(ds.Tables[0].Rows[j]["Money"].ToString());
                                        Count += Convert.ToInt32(ds.Tables[0].Rows[j]["Count"].ToString());

                                        if (!string.IsNullOrEmpty(ds.Tables[0].Rows[j]["Remark"].ToString()))
                                        {
                                            Remark += "," + ds.Tables[0].Rows[j]["Remark"].ToString();
                                        }
                                        //删除明细
                                        //sb.Append("delete from H_OrderInDetail where Id='" + ds.Tables[0].Rows[j]["Id"].ToString() + "';");
                                        sb.Append("update H_OrderInDetail set Status=" + (int)StatusEnum.删除 + " where Id='" + ds.Tables[0].Rows[j]["Id"].ToString() + "';");
                                        sb.AppendLine();
                                    }
                                }
                            }
                            if (Combin == true)
                            {
                                sb.Append(@"update H_OrderInDetail set Code='" + Code + "',GoodUnit='" + GoodUnit + "',GoodName='" + GoodName + "',Remark='" + Remark + "',Count=" + Count + ",BillNum=" + Count + ",BillPrice=" + Price + ",Money=" + Money + ",BillMoney=" + Money + ",CheckNum=0,CheckPrice=0,LosstPrice=0 where Id='" + ds.Tables[0].Rows[i]["Id"].ToString() + "';");
                                sb.AppendLine();
                            }
                        }
                    }

                    //统计盈亏金额
                    sb.Append("update H_OrderIn set oldPrice=(select SUM(BillMoney) from H_OrderInDetail where MainId='" + id + "' and  Status!=" + (int)StatusEnum.删除 + "),LossPrice=(select SUM(LosstPrice) from H_OrderInDetail where MainId='" + id + "' and  Status!=" + (int)StatusEnum.删除 + "),TotalPrice=(select SUM(BillMoney) from H_OrderInDetail where MainId='" + id + "' and  Status!=" + (int)StatusEnum.删除 + ") where Id='" + id + "';");
                    sb.AppendLine();
                    int retu = biz.ExecuteNonQueryByText(sb.ToString());
                    if (retu > 0)
                    {
                        var OrderIn = new BllHOrderIn().LoadData(id);
                        msg.success = true;
                        msg.msg = OrderIn.LossPrice.ToString() + "," + OrderIn.TotalPrice.ToString() + "," + CombinNum;//返回盈亏金额 +供应商金额+合并条数
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "入库单明细合并", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region===提交审核  SubmitAudit
        /// <summary>
        /// 提交财务
        /// </summary>
        public void SubmitAudit(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update H_OrderIn set FinancialState=1,FinancialDT=getdate() where Id in(" + id + ")");
                sb.AppendLine();
                if (new BllHOrderIn().ExecuteNonQueryByText(sb.ToString()) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "入库单提交审核", "提交审核成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "入库单提交审核", "提交审核失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "入库单提交审核", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==导出主表数据ImportOut
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="mod"></param>
        public ActionResult ImportOut()
        {
            var fileTypeName = "进仓入库单批量导出";
            var fileName = fileTypeName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            try
            {
                string IdList = Request["IdList"].ToString();
                DataTable dt = new BllHOrderIn().GetAllList(IdList).Tables[0];
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
                            //时间类型
                            if (dt.Columns[j].DataType == (new DateTime()).GetType())
                            {
                                if (!string.IsNullOrEmpty(row[j].ToString()))
                                {
                                    ws.Cells[i + 1, j].PutValue(Convert.ToDateTime(row[j].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                                }
                                else
                                {
                                    ws.Cells[i + 1, j].PutValue(row[j]);
                                }
                            }
                            else {
                                ws.Cells[i + 1, j].PutValue(row[j]);
                            }
                            //if (dt.Columns[i].ColumnName == "单据日期")
                            //{
                            //    Style style = ws.Cells[i + 1, j].GetStyle();
                            //    style.Custom = "yyyy-mm-dd hh:mm";
                            //}
                        }
                    }
                    if (!System.IO.Directory.Exists(Server.MapPath("~/ExportFile/")))
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/ExportFile/"));
                    w.Save(Server.MapPath("~/ExportFile/") + fileName);
                }
                LogInsert(OperationTypeEnum.操作, "进仓入库单批量导出", "导出成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "进仓入库单批量导出", "操作异常信息:" + ex);
                return Json(new { success = false, msg = "操作异常信息" + ex });
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
                    case "InNumber":
                        col.ColumnName = "入库单号";
                        break;
                    case "StoreName":
                        col.ColumnName = "仓库";
                        break;
                    case "CusterName":
                        col.ColumnName = "供应商";
                        break;
                    case "GetNumber":
                        col.ColumnName = "采购单号";
                        break;
                    case "BillDate":
                        col.ColumnName = "单据日期";
                        break;
                    case "InStatus":
                        col.ColumnName = "状态";
                        break;
                    case "MainRemark":
                        col.ColumnName = "备注";
                        break;
                    case "LogisticsNumber":
                        col.ColumnName = "物流单号";
                        break;
                    case "Code":
                        col.ColumnName = "商品编码";
                        break;
                    case "GoodName":
                        col.ColumnName = "商品名称";
                        break;
                    case "GoodUnit":
                        col.ColumnName = "颜色及规格";
                        break;
                    case "count":
                        col.ColumnName = "数量";
                        break;
                    case "Price":
                        col.ColumnName = "单价";
                        break;
                    case "Money":
                        col.ColumnName = "金额";
                        break;
                    case "Batch":
                        col.ColumnName = "批次号";
                        break;
                    case "Remark":
                        col.ColumnName = "入库明细备注";
                        break;
                    case "StyleNum":
                        col.ColumnName = "款式编号";
                        break;
                    case "FreightNum":
                        col.ColumnName = "供应商货号";
                        break;
                    case "MakerName":
                        col.ColumnName = "制单人";
                        break;
                    case "JudgerName":
                        col.ColumnName = "财审人";
                        break;
                    case "JudgeDate":
                        col.ColumnName = "财审日期";
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
        ///// <summary>
        ///// 导出
        ///// </summary>
        ///// <param name="dt"></param>
        //public void ToExcel1(DataTable dt)
        //{
        //    AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
        //    doc.FileName = DateTime.Now.ToString("yyyyMMdd") + "进仓入库单批量导出.xls";
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
        //                cells.Add(m + 2, j + 1, Convert.ToDouble(string.IsNullOrEmpty(dt.Rows[m][j].ToString()) == true ? "0" : dt.Rows[m][j].ToString()), XFstyle);
        //            }
        //            else
        //            {
        //                cells.Add(m + 2, j + 1, dt.Rows[m][j].ToString());
        //            }
        //        }
        //    }
        //    doc.Send();
        //    Response.End();
        //}
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
                    LogInsert(OperationTypeEnum.操作, "入库单备注批量修改", "备注修改失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "入库单备注批量修改", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #endregion 
        
        #region ===单据审核管理

        #region==页面主表审核列表 SearchAuditData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchAuditData()
        {
            var search = base.GetSearch();
            GetSearch(ref search);

            search.AddCondition("(FinancialState=1 or FinancialState=-1)");//已提交的 或驳回的

            var jsonResult = new BllHOrderIn().SearchData(search);
            LogInsert(OperationTypeEnum.访问, "进仓单据核对凭证审核", CurrentMaster.UserName + "页面访问正常.");
            WriteJsonToPage(jsonResult);
        }
        #endregion

                                                                                                                                                                                                                                                                                                                                                                                                                                                                      #region===驳回审核  CancelAudit
        /// <summary>
        /// 驳回审核
        /// </summary>
        public void CancelAudit(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("update H_OrderIn set FinancialState=-2,FinancialDT=getdate() where Id in(" + id + ")");
                sb.AppendLine();
                if (new BllHOrderIn().ExecuteNonQueryByText(sb.ToString()) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "入库单驳回审核", "驳回审核成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "入库单驳回审核", "驳回审核失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "入库单驳回审核", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==审核通过，提交财务 SubmitData
        /// <summary>
        /// 审核通过，提交财务
        /// </summary>
        public void SubmitData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                BllHOrderIn Biz = new BllHOrderIn();
                StringBuilder sb = new StringBuilder();
                string[] str = id.Split(',');
                for (int i = 0; i < str.Length; i++)
                {
                    var model = Biz.LoadData(str[i]);
                    if (model != null)
                    {
                        //#region ===
                        //string CusterId = model.CusterId;//供应商ID
                        //var TotalPrice = model.TotalPrice;//供应商金额
                        //var CusterModel = new BllSysCompany().LoadData(CusterId);
                        ////获取任务池中明细金额
                        //string sql = "select top 1 * from view_HPurchaseRelation(nolock) where CusterId='" + CusterId + "' and PoolStatus!=3 order by FinancialDT asc;";
                        //DataSet Relation = Biz.ExecuteDataSet(sql);
                        //if (Relation.Tables[0].Rows.Count > 0)
                        //{
                        //    //统计明细金额，和主表金额对比
                        //    decimal decimalCount = 0;
                        //    var index = 0;
                        //    for (var a = 0; a < Relation.Tables[0].Rows.Count; a++)
                        //    {
                        //        decimalCount += Convert.ToDecimal(Relation.Tables[0].Rows[a]["TotalPrice"].ToString());
                        //        if (decimalCount < TotalPrice)
                        //        {
                        //            //继续累加
                        //        }
                        //        else if (decimalCount == TotalPrice)
                        //        {
                        //            #region===判断明细金额等于主表金额
                        //            //1：设置任务池为完成状态
                        //            string newid = Guid.NewGuid().ToString();
                        //            sb.Append(@"insert into H_OrderInRelation(Id,OrderInId,Status,Remark,IsDecompose,DecomposePrice)values(");
                        //            sb.Append("'" + newid + "',");
                        //            sb.Append("'" + model.Id + "',");
                        //            sb.Append("2,");//1:未完成 2：已完成
                        //            sb.Append("'',");
                        //            sb.Append("0,");
                        //            sb.Append("0");
                        //            sb.Append(");");
                        //            sb.AppendLine();

                        //            //2：更改入库单状态为审核通过
                        //            sb.Append("update H_OrderIn set FinancialState=2 where Id='" + model.Id + "';");
                        //            sb.AppendLine();

                        //            //3：添加主单据到付款单
                        //            string FinanceId = Guid.NewGuid().ToString();
                        //            sb.Append(@"insert into H_Finance(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,PaymentAmmount,PayStatus,FinanceRemark,RelationId,OrderInId,CheckoutType,PaymentType,AccountName,AccountNum) values(");
                        //            sb.Append("'" + FinanceId + "',");
                        //            sb.Append("getdate(),");
                        //            sb.Append("'" + CurrentMaster.Id + "',");
                        //            sb.Append("1,");
                        //            sb.Append("0,");//剩余退货金额
                        //            sb.Append("0,");//总盈亏
                        //            sb.Append("'" + TotalPrice + "',");//实际付款金额
                        //            sb.Append("0,");//PayStatus
                        //            sb.Append("'',");
                        //            sb.Append("'" + newid + "',");
                        //            sb.Append("'" + model.Id + "',");//入库单主键
                        //            sb.Append("" + (CusterModel.CheckoutType == null ? 0 : CusterModel.CheckoutType) + ",");
                        //            sb.Append("" + (CusterModel.PaymentType == null ? 0 : CusterModel.PaymentType) + ",");
                        //            sb.Append("'" + CusterModel.AccountName + "',");
                        //            sb.Append("'" + CusterModel.AccountNum + "'");
                        //            sb.Append(");");
                        //            sb.AppendLine();

                        //            for (var b = 0; b <= index; b++)
                        //            {
                        //                //4:添加先前已有的明细到付款单
                        //                sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                        //                sb.Append("'" + Guid.NewGuid().ToString() + "',");
                        //                sb.Append("getdate(),");
                        //                sb.Append("'" + CurrentMaster.Id + "',");
                        //                sb.Append("1,");
                        //                sb.Append("'" + Relation.Tables[0].Rows[b]["TotalPrice"] + "',");//剩余退货金额
                        //                sb.Append("0,");//退货盈亏
                        //                sb.Append("'',");
                        //                sb.Append("'" + Relation.Tables[0].Rows[b]["RelationId"] + "',");
                        //                sb.Append("'" + Relation.Tables[0].Rows[b]["Id"] + "',");
                        //                sb.Append("'" + FinanceId + "'");
                        //                sb.Append(");");
                        //                sb.AppendLine();

                        //                //5:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                        //                sb.Append("update H_PurchaseRelation set Status=3 where Id='" + Relation.Tables[0].Rows[b]["RelationId"] + "';");
                        //                sb.AppendLine();

                        //                //6更改明细主键的采购编码
                        //                sb.Append(@"update H_Purchase set GetNumber='" + model.GetNumber + "' where Id='" + Relation.Tables[0].Rows[a]["Id"].ToString() + "';");
                        //            }
                        //            #endregion  
                        //            break;
                        //        }
                        //        else if (decimalCount > TotalPrice)
                        //        {
                        //            //进项拆分价格
                        //            var nowPrice = Convert.ToDecimal(Relation.Tables[0].Rows[a]["TotalPrice"].ToString()); //50
                        //            var needPrice = decimalCount - TotalPrice;//主表需要拆分的金额 ((100+50)-120)=30
                        //            var LossPrice = nowPrice - needPrice;//拆分已完成金额

                        //            string newid = Guid.NewGuid().ToString();

                        //            //1:把拆分的留在池子
                        //            sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS)values(");
                        //            sb.Append("'" + newid + "',");
                        //            sb.Append("'" + Relation.Tables[0].Rows[a]["Id"].ToString() + "',");
                        //            sb.Append("2,");//扣减状态 1:未完成 2：部分完成 3：已完成
                        //            sb.Append("'',");
                        //            sb.Append("" + needPrice + ",");//Price
                        //            sb.Append("1,");
                        //            sb.Append("" + TotalPrice + ",");
                        //            sb.Append("''");
                        //            sb.Append(");");
                        //            sb.AppendLine();
                        //            //2:满足的直接设置状态为完成
                        //            string newid2 = Guid.NewGuid().ToString();
                        //            sb.Append(@"insert into H_PurchaseRelation(Id,PurchaseId,Status,Remark,Price,IsDecompose,DecomposePrice,OutNumberS)values(");
                        //            sb.Append("'" + newid2 + "',");
                        //            sb.Append("'" + Relation.Tables[0].Rows[a]["Id"].ToString() + "',");
                        //            sb.Append("3,");//扣减状态 1:未完成 2：部分完成 3：已完成
                        //            sb.Append("'',");
                        //            sb.Append("" + LossPrice + ",");//Price
                        //            sb.Append("0,");
                        //            sb.Append("0,");
                        //            sb.Append("''");
                        //            sb.Append(");");
                        //            sb.AppendLine();

                        //            //3更改明细主键的采购编码
                        //            sb.Append(@"update H_Purchase set GetNumber='" + model.GetNumber + "' where Id='" + Relation.Tables[0].Rows[a]["Id"].ToString() + "';");

                        //            //4:添加主表入库单关系
                        //            string RelationId = Guid.NewGuid().ToString();
                        //            sb.Append(@"insert into H_OrderInRelation(Id,OrderInId,Status,Remark,IsDecompose,DecomposePrice)values(");
                        //            sb.Append("'" + RelationId + "',");
                        //            sb.Append("'" + model.Id + "',");
                        //            sb.Append("2,");//1:未完成 2：已完成
                        //            sb.Append("'',");
                        //            sb.Append("0,");
                        //            sb.Append("0");
                        //            sb.Append(");");
                        //            sb.AppendLine();

                        //            //5：更改入库单状态为审核通过
                        //            sb.Append("update H_OrderIn set FinancialState=2 where Id='" + model.Id + "';");
                        //            sb.AppendLine();

                        //            //6：添加主单据到付款单
                        //            string FinanceId = Guid.NewGuid().ToString();
                        //            sb.Append(@"insert into H_Finance(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,PaymentAmmount,PayStatus,FinanceRemark,RelationId,OrderInId,CheckoutType,PaymentType,AccountName,AccountNum) values(");
                        //            sb.Append("'" + FinanceId + "',");
                        //            sb.Append("getdate(),");
                        //            sb.Append("'" + CurrentMaster.Id + "',");
                        //            sb.Append("1,");
                        //            sb.Append("0,");//剩余退货金额
                        //            sb.Append("0,");//总盈亏
                        //            sb.Append("'" + TotalPrice + "',");//实际付款金额
                        //            sb.Append("0,");//PayStatus
                        //            sb.Append("'',");
                        //            sb.Append("'" + RelationId + "',");
                        //            sb.Append("'" + model.Id + "',");//入库单主键
                        //            sb.Append("" + (CusterModel.CheckoutType == null ? 0 : CusterModel.CheckoutType) + ",");
                        //            sb.Append("" + (CusterModel.PaymentType == null ? 0 : CusterModel.PaymentType) + ",");
                        //            sb.Append("'" + CusterModel.AccountName + "',");
                        //            sb.Append("'" + CusterModel.AccountNum + "'");
                        //            sb.Append(");");
                        //            sb.AppendLine();

                        //            //7：添加退货单到付款单
                        //            sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                        //            sb.Append("'" + Guid.NewGuid().ToString() + "',");
                        //            sb.Append("getdate(),");
                        //            sb.Append("'" + CurrentMaster.Id + "',");
                        //            sb.Append("1,");
                        //            sb.Append("'" + TotalPrice + "',");//剩余退货金额
                        //            sb.Append("0,");//退货盈亏
                        //            sb.Append("'',");
                        //            sb.Append("'" + newid2 + "',");
                        //            sb.Append("'" + Relation.Tables[0].Rows[a]["Id"].ToString() + "',");
                        //            sb.Append("'" + FinanceId + "'");
                        //            sb.Append(");");
                        //            sb.AppendLine();

                        //            //8:添加以前的到付款单
                        //            for (var q = 0; q <= index; q++)
                        //            {
                        //                sb.Append(@"insert into H_FinanceDetail(Id,CreateTime,CreaterId,Status,ReturnAmount,ProfitTotal,DetailRemark,RelationId,PurchaseId,FinanceId) values(");
                        //                sb.Append("'" + Guid.NewGuid().ToString() + "',");
                        //                sb.Append("getdate(),");
                        //                sb.Append("'" + CurrentMaster.Id + "',");
                        //                sb.Append("3,");
                        //                sb.Append("'" + Relation.Tables[0].Rows[q]["TotalPrice"] + "',");//剩余退货金额
                        //                sb.Append("0,");//退货盈亏
                        //                sb.Append("'',");
                        //                sb.Append("'" + Relation.Tables[0].Rows[q]["RelationId"] + "',");
                        //                sb.Append("'" + Relation.Tables[0].Rows[q]["Id"] + "',");
                        //                sb.Append("'" + FinanceId + "'");
                        //                sb.Append(");");
                        //                sb.AppendLine();

                        //                //9:更改以前的扣减状态 1:未完成 2：部分完成 3：已完成
                        //                sb.Append("update H_PurchaseRelation set Status=3 where Id='" + Relation.Tables[0].Rows[q]["RelationId"] + "';");
                        //                sb.AppendLine();
                        //            }
                        //            break;
                        //        }
                        //        index++;
                        //    }
                        //}
                        //else
                        //{
                        //    #region===不存在明细,直接添加到任务池中
                        //    string newid = Guid.NewGuid().ToString();
                        //    sb.Append(@"insert into H_OrderInRelation(Id,OrderInId,Status,Remark,IsDecompose,DecomposePrice)values(");
                        //    sb.Append("'" + newid + "',");
                        //    sb.Append("'" + model.Id + "',");
                        //    sb.Append("1,");//1:未完成 2：已完成
                        //    sb.Append("'',");
                        //    sb.Append("0,");
                        //    sb.Append("0");
                        //    sb.Append(");");
                        //    sb.AppendLine();

                        //    //2：更改入库单状态为审核通过
                        //    sb.Append("update H_OrderIn set FinancialState=2 where Id='" + model.Id + "';");
                        //    sb.AppendLine();
                        //    #endregion
                        //}

                        //#endregion

                        #region===直接添加到任务池中
                        string newid = Guid.NewGuid().ToString();
                        sb.Append(@"insert into H_OrderInRelation(Id,OrderInId,Status,Remark,IsDecompose,DecomposePrice)values(");
                        sb.Append("'" + newid + "',");
                        sb.Append("'" + model.Id + "',");
                        sb.Append("1,");//1:未完成 2：已完成
                        sb.Append("'',");
                        sb.Append("0,");
                        sb.Append("0");
                        sb.Append(");");
                        sb.AppendLine();

                        //2：更改入库单状态为审核通过
                        sb.Append("update H_OrderIn set FinancialState=2 where Id='" + model.Id + "';");
                        sb.AppendLine();
                        #endregion
                    }
                }
                if (new BllHOrderIn().ExecuteNonQueryByText(sb.ToString()) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "提交财务", "提交财务成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "提交财务", "提交财务失败.");
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "提交财务", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #endregion


    }
}
