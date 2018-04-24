using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPortalAdmin.Controllers;
using QINGUO.Model;
using QINGUO.Business;
using WebPortalAdmin.Code;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using System.Text;
using NPOI.HSSF.UserModel;
using System.Configuration;
using QINGUO.Common;
using System.Text.RegularExpressions;
using System.Drawing;
using AppLibrary.WriteExcel;
using System.Diagnostics;
using NPOI.XSSF.UserModel;


namespace WebPortalAdmin.Areas.Contract.Controllers
{
    /// <summary>
    /// 合同管理
    /// </summary>
    public class InComeController : BaseController<ModContractInOut>
    {
        /// <summary>
        /// 收入合同
        /// </summary>
        /// <returns></returns>
        public ActionResult IncomeIndex()
        {
            return View();
        }

        /// <summary>
        /// 支出合同
        /// </summary>
        /// <returns></returns>
        public ActionResult OutcomeIndex()
        {
            return View();
        }

        /// <summary>
        /// 经费合同
        /// </summary>
        /// <returns></returns>
        public ActionResult MoneycomeIndex()
        {
            return View();
        }

        /// <summary>
        /// 价格库合同
        /// </summary>
        /// <returns></returns>
        public ActionResult PricecomeIndex()
        {
            return View();
        }

        /// <summary>
        /// 导入收入/支出合同
        /// </summary>
        /// <returns></returns>
        public ActionResult Exportcome()
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
            if (!string.IsNullOrEmpty(Request["Nature"]))
            {
                search.AddCondition("Nature='" + Request["Nature"].ToString() + "'");
            }
            if (!string.IsNullOrEmpty(Request["Project"]))
            {
                search.AddCondition("Project='" + Request["Project"].ToString() + "'");
            }
            if (!string.IsNullOrEmpty(Request["ProjectPhase"]))
            {
                search.AddCondition("ProjectPhase='" + Request["ProjectPhase"].ToString() + "'");
            }
            if (!string.IsNullOrEmpty(Request["ContractState"]))
            {
                search.AddCondition("ContractState='" + Request["ContractState"].ToString() + "'");
            }
            if (!string.IsNullOrEmpty(Request["InvoiceType"]))
            {
                search.AddCondition("InvoiceType='" + Request["InvoiceType"].ToString() + "'");
            }
            //合同名称
            if (!string.IsNullOrEmpty(Request["ContraceName"]))
            {
                search.AddCondition("ContraceName like '%" + Request["ContraceName"].ToString().Trim() + "%'");
            }
            //发起人
            if (!string.IsNullOrEmpty(Request["InitiatorUser"]))
            {
                search.AddCondition("InitiatorUser like '%" + Request["InitiatorUser"].ToString().Trim() + "%'");
            }
            //发起部门
            if (!string.IsNullOrEmpty(Request["InitiatorDept"]))
            {
                search.AddCondition("InitiatorDept like '%" + Request["InitiatorDept"].ToString().Trim() + "%'");
            }
            //经办人
            if (!string.IsNullOrEmpty(Request["Agent"]))
            {
                search.AddCondition("Agent like '%" + Request["Agent"].ToString().Trim() + "%'");
            }
            //审批人
            if (!string.IsNullOrEmpty(Request["Approver"]))
            {
                search.AddCondition("Approver like '%" + Request["Approver"].ToString().Trim() + "%'");
            }
            //子系统名称
            if (!string.IsNullOrEmpty(Request["Subsystem"]))
            {
                search.AddCondition("Subsystem like '%" + Request["Subsystem"].ToString().Trim() + "%'");
            }
            //交付地点
            if (!string.IsNullOrEmpty(Request["DeliveriesAddress"]))
            {
                search.AddCondition("DeliveriesAddress like '%" + Request["DeliveriesAddress"].ToString().Trim() + "%'");
            }
            //合同执行情况
            if (!string.IsNullOrEmpty(Request["ContractIimplementation"]))
            {
                search.AddCondition("ContractIimplementation like '%" + Request["ContractIimplementation"].ToString().Trim() + "%'");
            }
            //有效期
            if (!string.IsNullOrEmpty(Request["ValidityDate"]))
            {
                search.AddCondition("ValidityDate like '%" + Request["ValidityDate"].ToString().Trim() + "%'");
            }
            //合同金额币种
            if (!string.IsNullOrEmpty(Request["Currency"]))
            {
                search.AddCondition("Currency like '%" + Request["Currency"].ToString().Trim() + "%'");
            }
            //发票税率
            if (!string.IsNullOrEmpty(Request["TaxRate"]))
            {
                search.AddCondition("TaxRate like '%" + Request["TaxRate"].ToString().Trim() + "%'");
            }
            //协作单位名
            if (!string.IsNullOrEmpty(Request["UnitName"]))
            {
                search.AddCondition("UnitName like '%" + Request["UnitName"].ToString().Trim() + "%'");
            }
            // 签订日期
            if (!string.IsNullOrEmpty(Request["BegSigningDate"]) || !string.IsNullOrEmpty(Request["EndSigningDate"]))
            {
                if (!string.IsNullOrEmpty(Request["BegSigningDate"]))
                {
                    search.AddCondition("SigningDate>='" + Convert.ToDateTime(Request["BegSigningDate"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndSigningDate"]))
                {
                    search.AddCondition("SigningDate<='" + Convert.ToDateTime(Request["EndSigningDate"]).AddDays(1) + "'");
                }
            }
            // 交付日期
            if (!string.IsNullOrEmpty(Request["BenDeliverDate"]) || !string.IsNullOrEmpty(Request["EndDeliverDate"]))
            {
                if (!string.IsNullOrEmpty(Request["BenDeliverDate"]))
                {
                    search.AddCondition("DeliverDate>='" + Convert.ToDateTime(Request["BenDeliverDate"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndDeliverDate"]))
                {
                    search.AddCondition("DeliverDate<='" + Convert.ToDateTime(Request["EndDeliverDate"]).AddDays(1) + "'");
                }
            }
            // 合同总金额
            if (!string.IsNullOrEmpty(Request["BegTotalContractAmount"]) || !string.IsNullOrEmpty(Request["EndTotalContractAmount"]))
            {
                if (!string.IsNullOrEmpty(Request["BegTotalContractAmount"]))
                {
                    search.AddCondition("TotalContractAmount>='" + Convert.ToDouble(Request["BegTotalContractAmount"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndTotalContractAmount"]))
                {
                    search.AddCondition("TotalContractAmount<='" + Convert.ToDouble(Request["EndTotalContractAmount"]) + "'");
                }
            }
            // 财务收款总额
            if (!string.IsNullOrEmpty(Request["BenReceiptsTotalAmount"]) || !string.IsNullOrEmpty(Request["EndReceiptsTotalAmount"]))
            {
                if (!string.IsNullOrEmpty(Request["BenReceiptsTotalAmount"]))
                {
                    search.AddCondition("ReceiptsTotalAmount>='" + Convert.ToDouble(Request["BenReceiptsTotalAmount"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndReceiptsTotalAmount"]))
                {
                    search.AddCondition("ReceiptsTotalAmount<='" + Convert.ToDouble(Request["EndReceiptsTotalAmount"]) + "'");
                }
            }
            // 待收款金额
            if (!string.IsNullOrEmpty(Request["BegReceivablesAmount"]) || !string.IsNullOrEmpty(Request["EndReceivablesAmount"]))
            {
                if (!string.IsNullOrEmpty(Request["BegReceivablesAmount"]))
                {
                    search.AddCondition("ReceivablesAmount>='" + Convert.ToDouble(Request["BegReceivablesAmount"]) + "'");
                }
                if (!string.IsNullOrEmpty(Request["EndReceivablesAmount"]))
                {
                    search.AddCondition("ReceivablesAmount<='" + Convert.ToDouble(Request["EndReceivablesAmount"]) + "'");
                }
            }
            #endregion
        }

        #region==页面列表 SearchData 收入合同
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            //CType：1收入合同 2支出合同
            search.AddCondition("CType=" + Request["CType"].ToString());
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }

            GetSearch(ref search);

            var jsonResult = new BllContractInOut().SearchInComeData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region ===保存表单
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModContractInOut mod)
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                BllContractInOut Bll = new BllContractInOut();

                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    ModContractInOut model = Bll.LoadData(mod.Id);
                    mod.CreateTime = model.CreateTime;
                    mod.Status = model.Status;
                    mod.CreaterId = model.CreaterId;
                    mod.CompanyId = model.CompanyId;
                    mod.CType = model.CType;
                    int result = Bll.Update(mod);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "收入合同", "收入合同修改失败.");
                    }
                }
                else
                {
                    mod.Id = Guid.NewGuid().ToString();
                    mod.CreateTime = DateTime.Now;
                    mod.Status = (int)StatusEnum.正常;
                    mod.CreaterId = CurrentMaster.Id;
                    mod.CompanyId = CurrentMaster.Cid;
                    int result = Bll.Insert(mod);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "收入合同", "收入合同保存失败.");
                    }
                    else
                    {
                        LogInsert(OperationTypeEnum.操作, "收入合同", "收入合同保存成功.");
                    }
                }
            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "收入合同", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());
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
                StringBuilder sb = new StringBuilder();
                sb.Append("delete from Sys_FileAttach where KeyId in(" + id + ");");
                sb.AppendLine();
                sb.Append("delete from Con_ContractInOut where Id in(" + id + ");");
                BllContractInOut Back = new BllContractInOut();
                int result = Back.ExecuteNonQueryByText(sb.ToString());
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==编辑加载数据 LoadData()
        /// <summary>
        /// 根据id 加载数据
        /// </summary>
        public void LoadData()
        {
            string Id = Request["Id"].ToString();
            var list = new BllContractInOut().GetModelByWhere(" and Id='" + Id + "'");
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        #endregion

        #region ==导入数据 ImportDate
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void ImportDate()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                //获取文件
                HttpPostedFileBase excel = Request.Files["excel"];
                
                DataTable errortab = null;
                string sql = @"insert into Con_ContractInOut(Id,CreateTime,CreaterId,Status,CompanyId
                                  ,NumCode
                                  ,OldNumCode
                                  ,ContraceName
                                  ,Nature,InitiatorUser,InitiatorDept,Agent,Approver,Project,Subsystem,ProductDescription
                                  ,DeliveriesQuantities
                                  ,DeliveriesAddress
                                  ,ProjectPhase
                                  ,ContractState
                                  ,ContractIimplementation
                                  ,SigningDate
                                  ,DeliverDate
                                  ,ValidityDate
                                  ,Currency
                                  ,CurrencyUnit
                                  ,TotalContractAmount
                                  ,TotalInvoice
                                  ,ReceivablesAmount
                                  ,ReceiptsTotalAmount
                                  ,AgreedMoney1
                                  ,AgreedTime1
                                  ,AgreedMoney2
                                  ,AgreedTime2
                                  ,AgreedMoney3
                                  ,AgreedTime3
                                  ,AgreedMoney4
                                  ,AgreedTime4
                                  ,AgreedMoney5
                                  ,AgreedTime5
                                  ,FinancialAmount1
                                  ,FinancialTime1
                                  ,FinancialAmount2
                                  ,FinancialTime2
                                  ,FinancialAmount3
                                  ,FinancialTime3
                                  ,FinancialAmount4
                                  ,FinancialTime4
                                  ,FinancialAmount5
                                  ,FinancialTime5
                                  ,TicketMoney1
                                  ,TicketTime1
                                  ,TicketMoney2
                                  ,TicketTime2
                                  ,TicketMoney3
                                  ,TicketTime3
                                  ,TicketMoney4
                                  ,TicketTime4
                                  ,TicketMoney5
                                  ,TicketTime5
                                  ,PlanTotalAmount
                                  ,HasFileAttach
                                  ,UnitName
                                  ,UnitAddress
                                  ,OpeningBank
                                  ,OpeningAccount
                                  ,LinkUser
                                  ,LinType
                                  ,InvoiceType
                                  ,TaxRate
                                  ,InvoiceValueBe
                                  ,InvoiceValueHas
                                  ,InvoiceValueBefore
                                  ,BudgetSituation
                                  ,AccordingDocument
                                  ,FilingSituation
                                  ,Remark
                                  ,CType
                                  ,cdefine1
                                  ,cdefine2
                                  ,cdefine3
                                  ,cdefine4
                                  ,cdefine5
                                  ,cdefine6
                                  ,cdefine7
                                  ,cdefine8
                                  ,cdefine9
                                  ,cdefine10)";
                int rowAffected = ExportData(excel.FileName,excel.InputStream,sql, out errortab, CurrentMaster);
                json.msg = "成功导入" + rowAffected + "条数据！";
                json.success = true;
                if (errortab != null)
                {
                    ExcelRender.RenderToExcel(errortab, Request.MapPath("/Project/Template/Error/" + CurrentMaster.LoginName + "Error.xls"));
                    json.data = "\"" + "/Project/Template/Error/" + CurrentMaster.LoginName + "Error.xls\"";
                    json.msg = "成功导入" + rowAffected + "条数据！错误" + errortab.Rows.Count + "条数据.";
                }
                LogInsert(OperationTypeEnum.操作, "设备导入", "导入操作成功.");

                WriteJsonToPage(json.ToString());
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备导入", "操作异常信息:" + ex);
            }
        }

        #endregion

        #region ===导入数据入口
        /// <summary>
        /// Excel文档导入到数据库
        /// 默认取Excel的第一个表
        /// 第一行必须为标题行
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <returns></returns>
        public int ExportData(string FileName,Stream excelFileStream, string insertSql, out DataTable tab, ModSysMaster CurrentMaster)
        {
            return RenderToDbCompany(FileName,excelFileStream, insertSql, 0, 0, out tab, CurrentMaster);
        }

        #region ===数据校验 RowDataVis
        /// <summary>
        /// 项目名称
        /// </summary>
        public static List<ModSysDirc> SysDirc0 = new List<ModSysDirc>();
        /// <summary>
        /// 合同性质
        /// </summary>
        public static List<ModSysDirc> SysDirc1 = new List<ModSysDirc>();
        /// <summary>
        /// 项目阶段
        /// </summary>
        public static List<ModSysDirc> SysDirc2 = new List<ModSysDirc>();
        /// <summary>
        /// 合同状态
        /// </summary>
        public static List<ModSysDirc> SysDirc3 = new List<ModSysDirc>();
        /// <summary>
        /// 发票类型
        /// </summary>
        public static List<ModSysDirc> SysDirc4 = new List<ModSysDirc>();


       /// <summary>
        /// 数据校验空
       /// </summary>
       /// <param name="CurrentMaster"></param>
        /// <param name="val1">合同名称</param>
        /// <param name="val2">合同性质</param>
        /// <param name="val3">项目</param>
        /// <param name="val4">项目阶段</param>
        /// <param name="val5">合同状态阶段</param>
        /// <param name="val6">发票类型</param>
       /// <param name="CompanyId"></param>
       /// <param name="CategoryId"></param>
       /// <param name="errorStr"></param>
       /// <returns></returns>
        public bool CheckData(ModSysMaster CurrentMaster, string val1, string val2, string val3, string val4, string val5, string val6, string CompanyId, ref string SysDirc0Id, ref string SysDirc1Id, ref string SysDirc2Id, ref string SysDirc3Id, ref string SysDirc4Id, ref string errorStr)
        {
            bool flag = false;
            bool AddSysDirc0 = false;
            bool AddSysDirc1 = false;
            bool AddSysDirc2 = false;
            bool AddSysDirc3 = false;
            bool AddSysDirc4 = false;

            #region ===校验合同名称
            if (val1.Length == 0 || string.IsNullOrEmpty(val1.Trim()) || val1.Length == 0 || string.IsNullOrEmpty(val1.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "合同名称不能为空,为必填项";
                return flag;
            }
            else
            {
                if (val1.Length > 50 || val1.Length > 50) //验证长度是否合理
                {
                    flag = true;
                    errorStr += "合同名称长度过长,有效范围1-100字符.";
                    return flag;
                }
            }
            #endregion

            #region ===校验合同性质
            if (val2.Length == 0 || string.IsNullOrEmpty(val2.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "合同性质不能为空,为必填项.";
                return flag;
            }
            else
            {
                AddSysDirc1 = true;
            }
            #endregion

            #region ===校验项目
            if (val3.Length == 0 || string.IsNullOrEmpty(val3.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "项目不能为空,为必填项.";
                return flag;
            }
            else
            {
                AddSysDirc0 = true;
            }
            #endregion

            #region ===校验项目阶段
            if (val4.Length == 0 || string.IsNullOrEmpty(val4.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "项目阶段不能为空,为必填项.";
                return flag;
            }
            else
            {
                AddSysDirc2 = true;
            }
            #endregion

            #region ===校验合同状态
            if (val5.Length == 0 || string.IsNullOrEmpty(val5.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "合同状态不能为空,为必填项.";
                return flag;
            }
            else
            {
                AddSysDirc3 = true;
            }
            #endregion

            #region ===校验发票类型
            if (val6.Length == 0 || string.IsNullOrEmpty(val6.Trim())) //验证长度是否合理
            {
                flag = true;
                errorStr += "发票类型不能为空,为必填项.";
                return flag;
            }
            else
            {
                AddSysDirc4 = true;
            }
            #endregion


            //添加合同性质
            if (AddSysDirc1 == true)
            {
                List<ModSysDirc> newCategory = new BllSysDirc().QueryToAll().Where(p => p.Type == 1 && p.Name.Trim() == val2.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    SysDirc1Id = AddSysDirc(1, val2, CurrentMaster, CompanyId);
                }
                else
                {
                    SysDirc1Id = newCategory[0].Id;
                }
            }

            //添加项目
            if (AddSysDirc0 == true)
            {
                List<ModSysDirc> newCategory = new BllSysDirc().QueryToAll().Where(p => p.Type == 0 && p.Name.Trim() == val3.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    SysDirc0Id = AddSysDirc(0, val3, CurrentMaster, CompanyId);
                }
                else
                {
                    SysDirc0Id = newCategory[0].Id;
                }
            }

            //添加项目阶段
            if (AddSysDirc2 == true)
            {
                List<ModSysDirc> newCategory = new BllSysDirc().QueryToAll().Where(p => p.Type == 2 && p.Name.Trim() == val4.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    SysDirc2Id = AddSysDirc(2, val4, CurrentMaster, CompanyId);
                }
                else
                {
                    SysDirc2Id = newCategory[0].Id;
                }
            }

            //添加合同状态
            if (AddSysDirc3 == true)
            {
                List<ModSysDirc> newCategory = new BllSysDirc().QueryToAll().Where(p => p.Type == 3 && p.Name.Trim() == val5.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    SysDirc3Id = AddSysDirc(3, val5, CurrentMaster, CompanyId);
                }
                else
                {
                    SysDirc3Id = newCategory[0].Id;
                }
            }

            //添加发票类型
            if (AddSysDirc4 == true)
            {
                List<ModSysDirc> newCategory = new BllSysDirc().QueryToAll().Where(p => p.Type == 4 && p.Name.Trim() == val6.Trim() && p.Status != (int)StatusEnum.删除).ToList();
                if (newCategory.Count <= 0)
                {
                    SysDirc4Id = AddSysDirc(4, val6, CurrentMaster, CompanyId);
                }
                else
                {
                    SysDirc4Id = newCategory[0].Id;
                }
            }
            return flag;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="CurrentMaster"></param>
        public string AddSysDirc(int type,string name, ModSysMaster CurrentMaster, string companyId)
        {
            ModSysDirc t = new ModSysDirc();
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.ParentId = "0";
            t.Name = name;
            t.CompanyId = companyId;
            t.Name = name;
            t.Type = type;
            new BllSysDirc().Insert(t);
            switch (type)
            {
                case 0: //项目名称
                    SysDirc0.Add(t);
                    break;
                case 1://合同性质
                    SysDirc1.Add(t);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
            return t.Id;
        }

        #endregion

        #region Excel文档导入到数据库
        /// <summary>
        /// Excel文档导入到数据库
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="insertSql">插入语句</param>
        /// <param name="dbAction">更新到数据库的方法</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <param name="shopTypeid">商家分类</param>
        /// <returns></returns>
        public int RenderToDbCompany(string FileName,Stream excelFileStream, string insertSql, int sheetIndex, int headerRowIndex, out DataTable tab, ModSysMaster CurrentMaster)
        {
            #region ===构建错误表头
            DataTable ErrorTab = new DataTable();
            DataColumn dc1 = new DataColumn("现行有效合同编号", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("原合同编号", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("合同名称", Type.GetType("System.String"));
            DataColumn dc4 = new DataColumn("合同性质", Type.GetType("System.String"));
            DataColumn dc5 = new DataColumn("发起人", Type.GetType("System.String"));
            DataColumn dc6 = new DataColumn("发起部门", Type.GetType("System.String"));
            DataColumn dc7 = new DataColumn("经办人", Type.GetType("System.String"));
            DataColumn dc8 = new DataColumn("审批人", Type.GetType("System.String"));
            DataColumn dc9 = new DataColumn("项目", Type.GetType("System.String"));
            DataColumn dc10 = new DataColumn("子系统名称", Type.GetType("System.String"));
            DataColumn dc11 = new DataColumn("产品说明", Type.GetType("System.String"));
            DataColumn dc12 = new DataColumn("交付物及数量", Type.GetType("System.String"));
            DataColumn dc13 = new DataColumn("交付地点", Type.GetType("System.String"));
            DataColumn dc14 = new DataColumn("项目阶段", Type.GetType("System.String"));
            DataColumn dc15 = new DataColumn("合同状态阶段", Type.GetType("System.String"));
            DataColumn dc16 = new DataColumn("合同执行情况", Type.GetType("System.String"));
            DataColumn dc17 = new DataColumn("签订日期", Type.GetType("System.String"));
            DataColumn dc18 = new DataColumn("交付日期", Type.GetType("System.String"));
            DataColumn dc19 = new DataColumn("有效期", Type.GetType("System.String"));
            DataColumn dc20 = new DataColumn("币种", Type.GetType("System.String"));
            DataColumn dc21 = new DataColumn("金额单位", Type.GetType("System.String"));
            DataColumn dc22 = new DataColumn("合同总金额", Type.GetType("System.String"));
            DataColumn dc23 = new DataColumn("待收款金额", Type.GetType("System.String"));
            DataColumn dc24 = new DataColumn("已开票总金额", Type.GetType("System.String"));
            DataColumn dc25 = new DataColumn("财务收款总金额", Type.GetType("System.String"));
            DataColumn dc26 = new DataColumn("约定收款金额1", Type.GetType("System.String"));
            DataColumn dc27 = new DataColumn("约定收款日期1", Type.GetType("System.String"));
            DataColumn dc28 = new DataColumn("约定收款金额2", Type.GetType("System.String"));
            DataColumn dc29 = new DataColumn("约定收款日期2", Type.GetType("System.String"));
            DataColumn dc30 = new DataColumn("约定收款金额3", Type.GetType("System.String"));
            DataColumn dc31 = new DataColumn("约定收款日期3", Type.GetType("System.String"));
            DataColumn dc32 = new DataColumn("约定收款金额4", Type.GetType("System.String"));
            DataColumn dc33 = new DataColumn("约定收款日期4", Type.GetType("System.String"));
            DataColumn dc34 = new DataColumn("约定收款金额5", Type.GetType("System.String"));
            DataColumn dc35 = new DataColumn("约定收款日期5", Type.GetType("System.String"));
            DataColumn dc36 = new DataColumn("财务实收金额1", Type.GetType("System.String"));
            DataColumn dc37 = new DataColumn("财务实收日期1", Type.GetType("System.String"));
            DataColumn dc38 = new DataColumn("财务实收金额2", Type.GetType("System.String"));
            DataColumn dc39 = new DataColumn("财务实收日期2", Type.GetType("System.String"));
            DataColumn dc40 = new DataColumn("财务实收金额3", Type.GetType("System.String"));
            DataColumn dc41 = new DataColumn("财务实收日期3", Type.GetType("System.String"));
            DataColumn dc42 = new DataColumn("财务实收金额4", Type.GetType("System.String"));
            DataColumn dc43 = new DataColumn("财务实收日期4", Type.GetType("System.String"));
            DataColumn dc44 = new DataColumn("财务实收金额5", Type.GetType("System.String"));
            DataColumn dc45 = new DataColumn("财务实收日期5", Type.GetType("System.String"));
            DataColumn dc46 = new DataColumn("开票节点金额1", Type.GetType("System.String"));
            DataColumn dc47 = new DataColumn("开票节点日期1", Type.GetType("System.String"));
            DataColumn dc48 = new DataColumn("开票节点金额2", Type.GetType("System.String"));
            DataColumn dc49 = new DataColumn("开票节点日期2", Type.GetType("System.String"));
            DataColumn dc50 = new DataColumn("开票节点金额3", Type.GetType("System.String"));
            DataColumn dc51 = new DataColumn("开票节点日期3", Type.GetType("System.String"));
            DataColumn dc52 = new DataColumn("开票节点金额4", Type.GetType("System.String"));
            DataColumn dc53 = new DataColumn("开票节点日期4", Type.GetType("System.String"));
            DataColumn dc54 = new DataColumn("开票节点金额5", Type.GetType("System.String"));
            DataColumn dc55 = new DataColumn("开票节点日期5", Type.GetType("System.String"));
            DataColumn dc56 = new DataColumn("协作单位", Type.GetType("System.String"));
            DataColumn dc57 = new DataColumn("协作单位地址", Type.GetType("System.String"));
            DataColumn dc58 = new DataColumn("开户行", Type.GetType("System.String"));
            DataColumn dc59 = new DataColumn("开户账号", Type.GetType("System.String"));
            DataColumn dc60 = new DataColumn("联系人", Type.GetType("System.String"));
            DataColumn dc61 = new DataColumn("联系方式", Type.GetType("System.String"));
            DataColumn dc62 = new DataColumn("发票类型", Type.GetType("System.String"));
            DataColumn dc63 = new DataColumn("税率", Type.GetType("System.String"));
            DataColumn dc64 = new DataColumn("应开发票", Type.GetType("System.String"));
            DataColumn dc65 = new DataColumn("已开发票", Type.GetType("System.String"));
            DataColumn dc66 = new DataColumn("待开发票", Type.GetType("System.String"));
            DataColumn dc67 = new DataColumn("预算情况", Type.GetType("System.String"));
            DataColumn dc68 = new DataColumn("依据文件", Type.GetType("System.String"));
            DataColumn dc69 = new DataColumn("归档情况", Type.GetType("System.String"));
            DataColumn dc70 = new DataColumn("备注1", Type.GetType("System.String"));
            DataColumn dc71 = new DataColumn("备注2", Type.GetType("System.String"));
            DataColumn dc72 = new DataColumn("备注3", Type.GetType("System.String"));
            DataColumn dc73 = new DataColumn("备注4", Type.GetType("System.String"));
            DataColumn dc74 = new DataColumn("备注5", Type.GetType("System.String"));
            DataColumn dc75 = new DataColumn("备注6", Type.GetType("System.String"));
            DataColumn dc76 = new DataColumn("备注7", Type.GetType("System.String"));
            DataColumn dc77 = new DataColumn("备注8", Type.GetType("System.String"));
            DataColumn dc78 = new DataColumn("备注9", Type.GetType("System.String"));
            DataColumn dc79 = new DataColumn("备注10", Type.GetType("System.String"));
            DataColumn dc80 = new DataColumn("错误原因", Type.GetType("System.String"));



            ErrorTab.Columns.Add(dc1);
            ErrorTab.Columns.Add(dc2);
            ErrorTab.Columns.Add(dc3);
            ErrorTab.Columns.Add(dc4);
            ErrorTab.Columns.Add(dc5);
            ErrorTab.Columns.Add(dc6);
            ErrorTab.Columns.Add(dc7);
            ErrorTab.Columns.Add(dc8);
            ErrorTab.Columns.Add(dc9);
            ErrorTab.Columns.Add(dc10);
            ErrorTab.Columns.Add(dc11);
            ErrorTab.Columns.Add(dc12);
            ErrorTab.Columns.Add(dc13);
            ErrorTab.Columns.Add(dc14);
            ErrorTab.Columns.Add(dc15);
            ErrorTab.Columns.Add(dc16);
            ErrorTab.Columns.Add(dc17);
            ErrorTab.Columns.Add(dc18);
            ErrorTab.Columns.Add(dc19);
            ErrorTab.Columns.Add(dc20);
            ErrorTab.Columns.Add(dc21);
            ErrorTab.Columns.Add(dc22);
            ErrorTab.Columns.Add(dc23);
            ErrorTab.Columns.Add(dc24);
            ErrorTab.Columns.Add(dc25);
            ErrorTab.Columns.Add(dc26);
            ErrorTab.Columns.Add(dc27);
            ErrorTab.Columns.Add(dc28);
            ErrorTab.Columns.Add(dc29);
            ErrorTab.Columns.Add(dc30);
            ErrorTab.Columns.Add(dc31);
            ErrorTab.Columns.Add(dc32);
            ErrorTab.Columns.Add(dc33);
            ErrorTab.Columns.Add(dc34);
            ErrorTab.Columns.Add(dc35);
            ErrorTab.Columns.Add(dc36);
            ErrorTab.Columns.Add(dc37);
            ErrorTab.Columns.Add(dc38);
            ErrorTab.Columns.Add(dc39);
            ErrorTab.Columns.Add(dc40);
            ErrorTab.Columns.Add(dc41);
            ErrorTab.Columns.Add(dc42);
            ErrorTab.Columns.Add(dc43);
            ErrorTab.Columns.Add(dc44);
            ErrorTab.Columns.Add(dc45);
            ErrorTab.Columns.Add(dc46);
            ErrorTab.Columns.Add(dc47);
            ErrorTab.Columns.Add(dc48);
            ErrorTab.Columns.Add(dc49);
            ErrorTab.Columns.Add(dc50);
            ErrorTab.Columns.Add(dc51);
            ErrorTab.Columns.Add(dc52);
            ErrorTab.Columns.Add(dc53);
            ErrorTab.Columns.Add(dc54);
            ErrorTab.Columns.Add(dc55);
            ErrorTab.Columns.Add(dc56);
            ErrorTab.Columns.Add(dc57);
            ErrorTab.Columns.Add(dc58);
            ErrorTab.Columns.Add(dc59);
            ErrorTab.Columns.Add(dc60);
            ErrorTab.Columns.Add(dc61);
            ErrorTab.Columns.Add(dc62);
            ErrorTab.Columns.Add(dc63);
            ErrorTab.Columns.Add(dc64);
            ErrorTab.Columns.Add(dc65);
            ErrorTab.Columns.Add(dc66);
            ErrorTab.Columns.Add(dc67);
            ErrorTab.Columns.Add(dc68);
            ErrorTab.Columns.Add(dc69);
            ErrorTab.Columns.Add(dc70);
            ErrorTab.Columns.Add(dc71);
            ErrorTab.Columns.Add(dc72);
            ErrorTab.Columns.Add(dc73);
            ErrorTab.Columns.Add(dc74);
            ErrorTab.Columns.Add(dc75);
            ErrorTab.Columns.Add(dc76);
            ErrorTab.Columns.Add(dc77);
            ErrorTab.Columns.Add(dc78);
            ErrorTab.Columns.Add(dc79);
            ErrorTab.Columns.Add(dc80);
            #endregion

            //当前用户公司编号
            string CompanyId =CurrentMaster.Cid;
          
            //获取所有列表
            SysDirc0 = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 0).ToList();
            SysDirc1 = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 1).ToList();
            SysDirc2 = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 2).ToList();
            SysDirc3 = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 3).ToList();
            SysDirc4 = new BllSysDirc().QueryToAll().Where(p => p.Status != (int)StatusEnum.删除 && p.Type == 4).ToList();

            int rowAffected = 0;
            using (excelFileStream)//读取流
            {
                //根据类型查询题库信息
             //   List<ModSysAppointed> list = new BllSysAppointed().GetListByWhere(" and Status!=" + (int)StatusEnum.删除);
                string sExtension =FileName.Substring(FileName.LastIndexOf('.'));//获取拓展名
                IWorkbook workbook=null;
                if (sExtension == ".xls")
                {
                    workbook = new HSSFWorkbook(excelFileStream);
                }
                else if (sExtension == ".xlsx")
                {
                    workbook = new XSSFWorkbook(excelFileStream);
                }
                 
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                StringBuilder builder = new StringBuilder();
                StringBuilder builderValue = new StringBuilder();
                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;//
                int rowCount = sheet.LastRowNum;//
                int Insertcount = 0;  //插入总行数
                //循环表格
                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    //判断空行
                    if (row != null)
                    {
                        #region  数据验证判断
                        var column0 = ExcelRender.GetCellValue(row.GetCell(0)).Replace("'", "''");//现行有效合同编号
                        var column1 = ExcelRender.GetCellValue(row.GetCell(1)).Replace("'", "''");//原合同编号
                        var column2 = ExcelRender.GetCellValue(row.GetCell(2)).Replace("'", "''");//合同名称
                        var column3 = ExcelRender.GetCellValue(row.GetCell(3)).Replace("'", "''");//合同性质
                        var column4 = ExcelRender.GetCellValue(row.GetCell(4)).Replace("'", "''");//发起人
                        var column5 = ExcelRender.GetCellValue(row.GetCell(5)).Replace("'", "''"); //发起部门
                        var column6 = ExcelRender.GetCellValue(row.GetCell(6)).Replace("'", "''");//经办人
                        var column7 = ExcelRender.GetCellValue(row.GetCell(7)).Replace("'", "''");///审批人
                        var column8 = ExcelRender.GetCellValue(row.GetCell(8)).Replace("'", "''");//项目
                        var column9 = ExcelRender.GetCellValue(row.GetCell(9)).Replace("'", "''");
                        var column10 = ExcelRender.GetCellValue(row.GetCell(10)).Replace("'", "''");
                        var column11 = ExcelRender.GetCellValue(row.GetCell(11)).Replace("'", "''");
                        var column12 = ExcelRender.GetCellValue(row.GetCell(12)).Replace("'", "''");
                        var column13 = ExcelRender.GetCellValue(row.GetCell(13)).Replace("'", "''");//项目阶段
                        var column14 = ExcelRender.GetCellValue(row.GetCell(14)).Replace("'", "''"); //合同状态阶段
                        var column15 = ExcelRender.GetCellValue(row.GetCell(15)).Replace("'", "''");//合同执行情况
                        var column16 = ExcelRender.GetCellValue(row.GetCell(16)).Replace("'", "''");
                        var column17 = ExcelRender.GetCellValue(row.GetCell(17)).Replace("'", "''");
                        var column18 = ExcelRender.GetCellValue(row.GetCell(18)).Replace("'", "''");//有效期
                        var column19 = ExcelRender.GetCellValue(row.GetCell(19)).Replace("'", "''");
                        var column20 = ExcelRender.GetCellValue(row.GetCell(20)).Replace("'", "''");
                        var column21 = ExcelRender.GetCellValue(row.GetCell(21)).Replace("'", "''");//合同总金额
                        var column22 = ExcelRender.GetCellValue(row.GetCell(22)).Replace("'", "''");
                        var column23 = ExcelRender.GetCellValue(row.GetCell(23)).Replace("'", "''");
                        var column24 = ExcelRender.GetCellValue(row.GetCell(24)).Replace("'", "''");
                        var column25 = ExcelRender.GetCellValue(row.GetCell(25)).Replace("'", "''");//约定收款金额1
                        var column26 = ExcelRender.GetCellValue(row.GetCell(26)).Replace("'", "''");//约定收款日期1
                        var column27 = ExcelRender.GetCellValue(row.GetCell(27)).Replace("'", "''");
                        var column28 = ExcelRender.GetCellValue(row.GetCell(28)).Replace("'", "''");
                        var column29 = ExcelRender.GetCellValue(row.GetCell(29)).Replace("'", "''");
                        var column30 = ExcelRender.GetCellValue(row.GetCell(30)).Replace("'", "''");
                        var column31 = ExcelRender.GetCellValue(row.GetCell(31)).Replace("'", "''");
                        var column32 = ExcelRender.GetCellValue(row.GetCell(32)).Replace("'", "''");
                        var column33 = ExcelRender.GetCellValue(row.GetCell(33)).Replace("'", "''");//约定收款金额5
                        var column34 = ExcelRender.GetCellValue(row.GetCell(34)).Replace("'", "''");//约定收款日期5
                        var column35 = ExcelRender.GetCellValue(row.GetCell(35)).Replace("'", "''");//财务实收金额1
                        var column36 = ExcelRender.GetCellValue(row.GetCell(36)).Replace("'", "''");//财务实收日期1
                        var column37 = ExcelRender.GetCellValue(row.GetCell(37)).Replace("'", "''");
                        var column38 = ExcelRender.GetCellValue(row.GetCell(38)).Replace("'", "''");
                        var column39 = ExcelRender.GetCellValue(row.GetCell(39)).Replace("'", "''");
                        var column40 = ExcelRender.GetCellValue(row.GetCell(40)).Replace("'", "''");
                        var column41 = ExcelRender.GetCellValue(row.GetCell(41)).Replace("'", "''");
                        var column42 = ExcelRender.GetCellValue(row.GetCell(42)).Replace("'", "''");
                        var column43 = ExcelRender.GetCellValue(row.GetCell(43)).Replace("'", "''");
                        var column44 = ExcelRender.GetCellValue(row.GetCell(44)).Replace("'", "''");//财务实收日期5
                        var column45 = ExcelRender.GetCellValue(row.GetCell(45)).Replace("'", "''");
                        var column46 = ExcelRender.GetCellValue(row.GetCell(46)).Replace("'", "''");//开票节点金额1
                        var column47 = ExcelRender.GetCellValue(row.GetCell(47)).Replace("'", "''");
                        var column48 = ExcelRender.GetCellValue(row.GetCell(48)).Replace("'", "''");
                        var column49 = ExcelRender.GetCellValue(row.GetCell(49)).Replace("'", "''");
                        var column50 = ExcelRender.GetCellValue(row.GetCell(50)).Replace("'", "''");
                        var column51 = ExcelRender.GetCellValue(row.GetCell(51)).Replace("'", "''");
                        var column52 = ExcelRender.GetCellValue(row.GetCell(52)).Replace("'", "''");
                        var column53 = ExcelRender.GetCellValue(row.GetCell(53)).Replace("'", "''");
                        var column54 = ExcelRender.GetCellValue(row.GetCell(54)).Replace("'", "''");//开票节点金额5
                        var column55 = ExcelRender.GetCellValue(row.GetCell(55)).Replace("'", "''");//协作单位
                        var column56 = ExcelRender.GetCellValue(row.GetCell(56)).Replace("'", "''");
                        var column57 = ExcelRender.GetCellValue(row.GetCell(57)).Replace("'", "''");
                        var column58 = ExcelRender.GetCellValue(row.GetCell(58)).Replace("'", "''");
                        var column59 = ExcelRender.GetCellValue(row.GetCell(59)).Replace("'", "''");
                        var column60 = ExcelRender.GetCellValue(row.GetCell(60)).Replace("'", "''");
                        var column61 = ExcelRender.GetCellValue(row.GetCell(61)).Replace("'", "''");  //发票类型
                        var column62 = ExcelRender.GetCellValue(row.GetCell(62)).Replace("'", "''");
                        var column63 = ExcelRender.GetCellValue(row.GetCell(63)).Replace("'", "''");
                        var column64 = ExcelRender.GetCellValue(row.GetCell(64)).Replace("'", "''");
                        var column65 = ExcelRender.GetCellValue(row.GetCell(65)).Replace("'", "''");
                        var column66 = ExcelRender.GetCellValue(row.GetCell(66)).Replace("'", "''");
                        var column67 = ExcelRender.GetCellValue(row.GetCell(67)).Replace("'", "''");
                        var column68 = ExcelRender.GetCellValue(row.GetCell(68)).Replace("'", "''"); //归档情况
                        var column69 = ExcelRender.GetCellValue(row.GetCell(69)).Replace("'", "''");
                        var column70 = ExcelRender.GetCellValue(row.GetCell(70)).Replace("'", "''");
                        var column71 = ExcelRender.GetCellValue(row.GetCell(71)).Replace("'", "''");
                        var column72 = ExcelRender.GetCellValue(row.GetCell(72)).Replace("'", "''");
                        var column73 = ExcelRender.GetCellValue(row.GetCell(73)).Replace("'", "''");
                        var column74 = ExcelRender.GetCellValue(row.GetCell(74)).Replace("'", "''");
                        var column75 = ExcelRender.GetCellValue(row.GetCell(75)).Replace("'", "''");
                        var column76 = ExcelRender.GetCellValue(row.GetCell(76)).Replace("'", "''");
                        var column77 = ExcelRender.GetCellValue(row.GetCell(77)).Replace("'", "''");
                        var column78 = ExcelRender.GetCellValue(row.GetCell(78)).Replace("'", "''");


                        string errorStr = "";
                        string SysDirc0Id = ""; //项目
                        string SysDirc1Id = "";//合同性质
                        string SysDirc2Id = "";//项目阶段
                        string SysDirc3Id = ""; //合同状态
                        string SysDirc4Id = "";//发票类型

                        //校验信息是否完整
                        if (CheckData(CurrentMaster, column2, column3, column8, column13, column14, column61, CompanyId, ref  SysDirc0Id, ref  SysDirc1Id, ref  SysDirc2Id, ref  SysDirc3Id, ref  SysDirc4Id, ref errorStr) == true)
                        {
                            //校验不通过
                            if (column2 == "" && column3 == "" && column8 == "" && column13 == "" && column14 == "" && column61 == "")
                            {
                                break;//遇到空行,直接跳转
                            }
                            else
                            {
                                #region
                                ////添加生产错误信息到excel
                                //string errorTime = DateTime.Now.ToString("yyyy年MM月");
                                //if (column7.Trim() != "")
                                //{
                                //    try
                                //    {
                                //        if (column7.IndexOf("年") > 0 || column7.IndexOf("月") > 0)
                                //        {
                                //            errorTime = Convert.ToDateTime(column7).ToString("yyyy年MM月");
                                //        }
                                //        else
                                //        {
                                //            string time = ToDateTimeValue(column7);
                                //            errorTime = Convert.ToDateTime(time).ToString("yyyy年MM月");
                                //        }
                                //    }
                                //    catch
                                //    {
                                //        errorTime = column7;
                                //    }
                                //}
                                //else
                                //{
                                //    errorTime = column7;
                                //}
                                ErrorTab.Rows.Add(column0, column1, column2, column3, column4, column5, column6, column7, column8, column9, column10,
                                    column11, column12, column13, column14, column15, column16, column17, column18, column19, column20,
                                     column21, column22, column23, column24, column25, column26, column27, column28, column29, column30,
                                     column31, column32, column33, column34, column35, column36, column37, column38, column39, column40,
                                     column41, column42, column43, column44, column45, column46, column47, column48, column49, column50,
                                     column51, column52, column53, column54, column55, column56, column57, column58, column59, column60,
                                     column61, column62, column63, column64, column65, column66, column67, column68, column69, column70,
                                     column71, column72, column73, column74, column75, column76, column77, column78,
                                    errorStr);
                                #endregion
                            }
                        }
                        else
                        {
                            string newId = Guid.NewGuid().ToString();//主键编号

                            //添加编号
                            builderValue.AppendFormat("'{0}',", newId);
                            builderValue.AppendFormat("'{0}',", DateTime.Now);
                            builderValue.AppendFormat("'{0}',", CurrentMaster.Id);
                            builderValue.AppendFormat("'{0}',", (int)StatusEnum.正常);
                            builderValue.AppendFormat("'{0}',", CurrentMaster.Cid);

                            #region ==构建sql语句
                            if (builderValue.ToString() != "")
                            {
                                #region ==添加信息
                                builder.Append(insertSql);
                                builder.Append(" values (");
                                //添加value赋值
                                builder.Append(builderValue.ToString());
                               
                                //现行有效合同编号
                                builder.AppendFormat("'{0}',", column0);
                                //原合同编号
                                builder.AppendFormat("'{0}',", column1);
                                //合同名称
                                builder.AppendFormat("'{0}',", column2);
                                //合同性质
                                builder.AppendFormat("'{0}',", SysDirc1Id);
                                //发起人
                                builder.AppendFormat("'{0}',", column4);
                                //发起部门
                                builder.AppendFormat("'{0}',", column5);
                                //经办人
                                builder.AppendFormat("'{0}',", column6);
                                //审批人
                                builder.AppendFormat("'{0}',", column7);
                                //项目
                                builder.AppendFormat("'{0}',", SysDirc0Id);
                                //子系统名称
                                builder.AppendFormat("'{0}',", column9);
                                //产品说明
                                builder.AppendFormat("'{0}',", column10);
                                //交付物及数量
                                builder.AppendFormat("'{0}',", column11);
                                //交付地点
                                builder.AppendFormat("'{0}',", column12);
                                //项目阶段
                                builder.AppendFormat("'{0}',", SysDirc2Id);
                                //合同状态阶段
                                builder.AppendFormat("'{0}',", SysDirc3Id);
                                //合同执行情况
                                builder.AppendFormat("'{0}',", column15);
                                //签订日期
                                string time = "";
                                if (!string.IsNullOrEmpty(column16))
                                {
                                    if (column16.IndexOf("年") > 0 || column16.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column16).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column16);
                                    }
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                //交付日期
                                if (!string.IsNullOrEmpty(column17))
                                {
                                    if (column17.IndexOf("年") > 0 || column17.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column17).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column17);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                //有效期
                                builder.AppendFormat("'{0}',", column18);
                                //币种
                                builder.AppendFormat("'{0}',", column19);
                                //金额单位
                                builder.AppendFormat("'{0}',", column20);
                                //合同总金额
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column21) ? "0" : column21);
                                //待收款金额
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column22) ? "0" : column22);
                                //已开票总金额
                                builder.AppendFormat("'{0}',", column23);
                                //财务收款总金额
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column24) ? "0" : column24 );
                                //约定收款金额1
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column25) ? "0" : column25 );
                                //约定收款日期1
                                if (!string.IsNullOrEmpty(column26))
                                {
                                    if (column26.IndexOf("年") > 0 || column26.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column26).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column26);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                //约定收款金额2
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column27) ? "0" : column27 );
                                //约定收款日期2
                                if (!string.IsNullOrEmpty(column28))
                                {
                                    if (column28.IndexOf("年") > 0 || column28.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column28).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column28);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                //约定收款金额3
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column29) ? "0" : column29 );
                                //约定收款日期3
                                if (!string.IsNullOrEmpty(column30))
                                {
                                    if (column30.IndexOf("年") > 0 || column30.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column30).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column30);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                //约定收款金额4
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column31) ? "0" : column31 );
                                //约定收款日期4
                                if (!string.IsNullOrEmpty(column32))
                                {
                                    if (column32.IndexOf("年") > 0 || column32.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column32).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column32);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                //约定收款金额5
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column33) ? "0" : column33 );
                                //约定收款日期5
                                if (!string.IsNullOrEmpty(column34))
                                {
                                    if (column34.IndexOf("年") > 0 || column34.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column34).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column34);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");

                                //财务实收金额1
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column35) ? "0" : column35 );
                                // 财务实收日期1
                                if (!string.IsNullOrEmpty(column36))
                                {
                                    if (column36.IndexOf("年") > 0 || column36.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column36).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column36);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                //  财务实收金额2
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column37) ? "0" : column37 );
                                //财务实收日期2
                                if (!string.IsNullOrEmpty(column38))
                                {
                                    if (column38.IndexOf("年") > 0 || column38.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column38).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column38);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");


                                //财务实收金额3
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column39) ? "0" : column39 );
                                //财务实收日期3
                                if (!string.IsNullOrEmpty(column40))
                                {
                                    if (column40.IndexOf("年") > 0 || column40.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column40).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column40);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");

                                //财务实收金额4
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column41) ? "0" : column41 );
                                //财务实收日期4
                                if (!string.IsNullOrEmpty(column42))
                                {
                                    if (column42.IndexOf("年") > 0 || column42.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column42).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column42);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");

                                //财务实收金额5
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column43) ? "0" : column43 );
                                //财务实收日期5
                                if (!string.IsNullOrEmpty(column44))
                                {
                                    if (column44.IndexOf("年") > 0 || column44.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column44).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column44);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                               
                                 //开票节点金额1
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column45) ? "0" : column45 );
                                //开票节点日期1
                                if (!string.IsNullOrEmpty(column46))
                                {
                                    if (column46.IndexOf("年") > 0 || column46.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column46).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column46);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                 //开票节点金额2
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column47) ? "0" : column47 );
                                //开票节点日期2
                                if (!string.IsNullOrEmpty(column48))
                                {
                                    if (column48.IndexOf("年") > 0 || column48.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column48).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column48);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                                  //开票节点金额3
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column49) ? "0" : column49 );
                                //开票节点日期3
                                if (!string.IsNullOrEmpty(column50))
                                {
                                    if (column50.IndexOf("年") > 0 || column50.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column50).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column50);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");
                               
                               
                                //开票节点金额4
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column51) ? "0" : column51 );
                                //开票节点日期4
                                if (!string.IsNullOrEmpty(column52))
                                {
                                    if (column52.IndexOf("年") > 0 || column52.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column52).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column52);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time) ? "null" : "'" + time + "'");

                              
                                //开票节点金额5
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column53) ? "0" : column53 );
                                //开票节点日期5
                                if (!string.IsNullOrEmpty(column54))
                                {
                                    if (column54.IndexOf("年") > 0 || column54.IndexOf("月") > 0)
                                    {
                                        time = Convert.ToDateTime(column54).ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        time = ToDateTimeValue(column54);
                                    }
                                }
                                else
                                {
                                    time = "";
                                }
                                builder.AppendFormat("{0},", string.IsNullOrEmpty(time)?"null":"'"+time+"'");
                                //计划收款付总金额
                                builder.AppendFormat("'{0}',", 0);
                                //HasFileAttach 是否有附件
                                builder.AppendFormat("'{0}',", 0);


                                //协作单位
                                builder.AppendFormat("'{0}',", column55);
                                //协作单位地址
                                builder.AppendFormat("'{0}',", column56);
                                //开户行
                                builder.AppendFormat("'{0}',", column57);
                                //开户账号
                                builder.AppendFormat("'{0}',", column58);
                                //联系人
                                builder.AppendFormat("'{0}',", column59);
                                //联系方式
                                builder.AppendFormat("'{0}',", column60);
                                //发票类型
                                builder.AppendFormat("'{0}',", SysDirc4Id);
                                //税率
                                builder.AppendFormat("'{0}',", column62);
                                //应开发票
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column63) ? "0" : column63 );
                                //已开发票
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column64) ? "0" : column64 );
                                //待开发票
                                builder.AppendFormat("'{0}',", string.IsNullOrEmpty(column65) ? "0" : column65 );
                                //预算情况
                                builder.AppendFormat("'{0}',", column66);
                                //依据文件
                                builder.AppendFormat("'{0}',", column67);
                                //归档情况
                                builder.AppendFormat("'{0}',", column68);
                                //Remark
                                builder.AppendFormat("'{0}',", 0);
                                //CType
                                builder.AppendFormat("'{0}',", Request["CType"].ToString());
                                //备注1
                                builder.AppendFormat("'{0}',", column69);
                                //备注2
                                builder.AppendFormat("'{0}',", column70);
                                //备注3
                                builder.AppendFormat("'{0}',", column71);
                                //备注4
                                builder.AppendFormat("'{0}',", column72);
                                //备注5
                                builder.AppendFormat("'{0}',", column73);
                                //备注6
                                builder.AppendFormat("'{0}',", column74);
                                //备注7
                                builder.AppendFormat("'{0}',", column75);
                                //备注8
                                builder.AppendFormat("'{0}',", column76);
                                //备注9
                                builder.AppendFormat("'{0}',", column77);
                                //备注10
                                builder.AppendFormat("'{0}',", column78);
                                //去掉最后一个“,”
                                builder.Length = builder.Length - 1;

                                builder.Append(");");
                                #endregion
                                Insertcount++;
                            }
                            #endregion
                        }
                        if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                        {
                            rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                            builder.Clear();
                            builder.Length = 0;
                            LogErrorRecord.Debug("导入设备执行返回记录:" + rowAffected);
                        }
                        #endregion
                    }
                }

                if (builder.Length > 0)
                {
                    rowAffected += new BllContractInOut().ExecuteNonQueryByText(builder.ToString());
                    builder.Clear();
                    builder.Length = 0;
                    LogErrorRecord.Debug("导入设备最后一次执行返回记录:" + rowAffected);
                }
                if (ErrorTab.Rows.Count >= 1)
                {
                    tab = ErrorTab;
                }
                else
                {
                    tab = null;
                }
                return Insertcount;
            }
        }

        /// <summary>
        /// 数字转换时间格式
        /// </summary>
        /// <param name="timeStr">数字,如:42095.7069444444/0.650694444444444</param>
        /// <returns>日期/时间格式</returns>
        private string ToDateTimeValue(string strNumber)
        {
            if (!string.IsNullOrWhiteSpace(strNumber))
            {
                Decimal tempValue;
                //先检查 是不是数字;
                if (Decimal.TryParse(strNumber, out tempValue))
                {
                    //天数,取整
                    int day = Convert.ToInt32(Math.Truncate(tempValue));
                    //这里也不知道为什么. 如果是小于32,则减1,否则减2
                    //日期从1900-01-01开始累加 
                    // day = day < 32 ? day - 1 : day - 2;
                    DateTime dt = new DateTime(1900, 1, 1).AddDays(day < 32 ? (day - 1) : (day - 2));

                    //小时:减掉天数,这个数字转换小时:(* 24) 
                    Decimal hourTemp = (tempValue - day) * 24;//获取小时数
                    //取整.小时数
                    int hour = Convert.ToInt32(Math.Truncate(hourTemp));
                    //分钟:减掉小时,( * 60)
                    //这里舍入,否则取值会有1分钟误差.
                    Decimal minuteTemp = Math.Round((hourTemp - hour) * 60, 2);//获取分钟数
                    int minute = Convert.ToInt32(Math.Truncate(minuteTemp));
                    //秒:减掉分钟,( * 60)
                    //这里舍入,否则取值会有1秒误差.
                    Decimal secondTemp = Math.Round((minuteTemp - minute) * 60, 2);//获取秒数
                    int second = Convert.ToInt32(Math.Truncate(secondTemp));
                    //时间格式:00:00:00
                    string resultTimes = string.Format("{0}:{1}:{2}",
                            (hour < 10 ? ("0" + hour) : hour.ToString()),
                            (minute < 10 ? ("0" + minute) : minute.ToString()),
                            (second < 10 ? ("0" + second) : second.ToString()));

                    if (day > 0)
                        return string.Format("{0} {1}", dt.ToString("yyyy-MM-dd"), resultTimes);
                    else
                        return resultTimes;
                }
            }
            return string.Empty;
        }
        #endregion



        #endregion


        #region ==导出数据ImportOut
        string[] title;  //导出的标题
        string[] field;  //导出对应字段
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="mod"></param>
        public void ImportOut()
        { 

            string hearder = @"现行有效合同编号,原合同编号,合同名称,合同性质,
                               发起人,发起部门,经办人,审批人,项目,子系统名称,产品说明,交付物及数量,
                               交付地点,项目阶段,合同状态阶段,合同执行情况,签订日期,交付日期,有效期,币种,金额单位,合同总金额,待收款金额,已开票总金额,
                                财务收款总金额,约定收款金额1,约定收款日期1,约定收款金额2,
                                约定收款日期2,约定收款金额3,约定收款日期3,约定收款金额4,约定收款日期4,
                                约定收款金额5,约定收款日期5,财务实收金额1,财务实收日期1,财务实收金额2,财务实收日期2,财务实收金额3,财务实收日期3,
                                财务实收金额4,财务实收日期4,财务实收金额5,财务实收日期5,开票节点金额1,
                                开票节点日期1,开票节点金额2,开票节点日期2,开票节点金额3,开票节点日期3,
                                开票节点金额4,开票节点日期4,开票节点金额5,开票节点日期5,
                                协作单位,协作单位地址,开户行,开户账号,联系人,联系方式,发票类型,税率,
                                应开发票,已开发票,待开发票,预算情况,依据文件,归档情况,
                                备注1,
                                备注2,
                                备注3,
                                备注4,
                                备注5,
                                备注6,
                                备注7,
                                备注8,
                                备注9,
                                备注10";
            string column = @"NumCode,OldNumCode,ContraceName,NatureName,
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
                                cdefine1,cdefine2,cdefine3,cdefine4,cdefine5,cdefine6,cdefine7,cdefine8,cdefine9,cdefine10";
            title = hearder.Split(',');  //导出的标题
            field = column.Split(',');  //导出对应字段
            var search = base.GetSearch();
            try
            {
                search.AddCondition("Status!=" + (int)StatusEnum.删除);
                GetSearch(ref search);
                DataSet ds = new BllContractInOut().GetAllList(search,"",1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ToExcel(ds.Tables[0],"收入合同");
                }
                LogInsert(OperationTypeEnum.操作, "收入合同批量导出", "收入合同导出成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "收入合同批量导出", "操作异常信息:" + ex);
            }
        }

        #region ===导出
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dt"></param>
        public void ToExcel(DataTable dt,string name)
        {
            AppLibrary.WriteExcel.XlsDocument doc = new AppLibrary.WriteExcel.XlsDocument();
            doc.FileName =name+ DateTime.Now.ToString("yyyyMMdd") + ".xls";
            string SheetName = name;
            //记录条数
            int mCount = dt.Rows.Count;
            Worksheet sheet = doc.Workbook.Worksheets.Add(SheetName);
            Cells cells = sheet.Cells;
            //第一行表头
            for (int i = 0; i < title.Length; i++)
            {
                cells.Add(1, i + 1, title[i].Trim());
            }
            for (int m = 0; m < mCount; m++)
            {
                for (int j = 0; j < title.Length; j++)
                {
                    cells.Add(m + 2, j + 1, dt.Rows[m][j].ToString());
                }
            }
            doc.Send();
            Response.End();
        }
        #endregion
        #endregion
    }
}
