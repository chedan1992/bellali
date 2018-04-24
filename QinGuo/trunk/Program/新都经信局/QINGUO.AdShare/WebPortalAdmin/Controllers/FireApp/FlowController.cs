using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using System.Text;
using System.Collections;

namespace WebPortalAdmin.Controllers
{
    public class FlowController : BaseController<ModSysFlow>
    {
        BllSysFlow Bll = new BllSysFlow();

        /// <summary>
        /// 待办事项
        /// </summary>
        /// <returns></returns>
        public ActionResult WaitWork()
        {
            return View();
        }

        /// <summary>
        /// 已办事项
        /// </summary>
        /// <returns></returns>
        public ActionResult DoWork()
        {
            return View();
        }

        /// <summary>
        /// 我发起的
        /// </summary>
        /// <returns></returns>
        public ActionResult MySend()
        {
            return View();
        }

        #region==待办事项页面列表 SearchWaitWorkData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchWaitWorkData()
        {
            var search = base.GetSearch();
            try
            {
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                    search.AddCondition("FlowStatus=0");//审批状态(-1:未通过 0:待审核 1:已通过)  超级管理员的都可以看到
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位下人员的审核
                    search.AddCondition("FlowStatus=0");
                    search.AddCondition("ApprovalUser!='" + CurrentMaster.Id + "'");
                    search.AddCondition("(ApprovalUser in(select Id from Sys_Master where Cid='" + CurrentMaster.Cid + "') or CompanyId='" + CurrentMaster.Cid + "')");
                }
                else
                {
                    search.AddCondition("FlowStatus=0");
                    search.AddCondition("AuditUser='" + CurrentMaster.Id + "'");
                }
                LogInsert(OperationTypeEnum.访问, "待办任务模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "待办任务模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(Bll.SearchData(search));
        }
        #endregion

        #region==已办事项页面列表 SearchDoWorkData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchDoWorkData()
        {
            var search = base.GetSearch();
            try
            {
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                    search.AddCondition("FlowStatus!=0");//审批状态(-1:未通过 0:待审核 1:已通过)  超级管理员的都可以看到
                    search.AddCondition("AuditUser='" + CurrentMaster.Id + "'");
                    //search.AddCondition("ApprovalUser in(select Id from Sys_Master where Cid='" + CurrentMaster.Cid + "')");
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位下人员的审核
                    search.AddCondition("FlowStatus!=0");
                    search.AddCondition("(AuditUser='" + CurrentMaster.Id + "' or CompanyId='" + CurrentMaster.Cid + "')");
                    //search.AddCondition("ApprovalUser in(select Id from Sys_Master where Cid='" + CurrentMaster.Cid + "')");
                }
                else
                {
                    search.AddCondition("FlowStatus!=0");
                    search.AddCondition("AuditUser='" + CurrentMaster.Id + "'");
                }
                LogInsert(OperationTypeEnum.访问, "已办任务模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "已办任务模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(Bll.SearchData(search));
        }
        #endregion

        #region==我发起的页面列表 SearchMySendData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchMySendData()
        {
            var search = base.GetSearch();
            try
            {
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                    search.AddCondition("ApprovalUser='" + CurrentMaster.Id + "'");
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位下人员的审核
                    search.AddCondition("ApprovalUser='" + CurrentMaster.Id + "'");
                }
                else
                {
                    search.AddCondition("ApprovalUser='" + CurrentMaster.Id + "'");
                }
                LogInsert(OperationTypeEnum.访问, "我发起的模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "我发起的模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(Bll.SearchData(search));
        }
        #endregion


        #region ==单位审核 CompanyWaitWork
        /// <summary>
        /// 单位审核
        /// </summary>
        public void CompanyWaitWork()
        {
            var msg = new ModJsonResult();

            string key = Request["id"];
            string Introduction = Request["Introduction"].ToString();
            string Attr = Request["Attribute"].ToString();//类型 1:通过  2:不通过
            var model = Bll.LoadData(key);
            try
            {
                BllSysFlow bllsyscompany = new BllSysFlow();

                if (bllsyscompany.WaitWork(CurrentMaster.Id, CurrentMaster.Cid, Introduction, key, int.Parse(Attr)))
                {
                    msg.success = true;
                    //发送短信
                    WebService.SDKService msm = new WebService.SDKService();
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                }
                LogInsert(OperationTypeEnum.访问, "待办任务", "审核操作成功.");
                WriteJsonToPage(msg.ToString());
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "待办任务", "审核操作异常消息:" + ex.Message.ToString());
            }
        }

        #endregion

        #region ==流程撤销 DeleteData
        /// <summary>
        /// 流程撤销
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                var FlowModel = Bll.LoadData(id);
                if (FlowModel.FlowType == 3)
                {
                    StringBuilder sb = new StringBuilder();
                    //撤销流程
                    sb.Append("delete from Sys_Flow where Id='" + id + "';");
                    sb.AppendLine();
                    sb.Append("delete from Sys_CompanyCognate where FlowId='" + id + "';");
                    int result = Bll.ExecuteNonQueryByText(sb.ToString());
                    if (result <= 0)
                    {
                        msg.success = false;
                        msg.msg = " 撤销失败,请稍后再操作!";
                    }
                    else
                    {
                        msg.success = true;
                    }
                }
                else
                {
                    int result = Bll.DeleteStatus(id);
                    if (result > 0)
                    {
                        msg.success = true;
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "删除失败.";
                    }
                }
                LogInsert(OperationTypeEnum.操作, "待办撤销", "任务撤销操作操作成功.");
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "待办撤销", "任务撤销操作异常消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion


        #region ===根据流程Id查询单位和管理员信息 GetCompanyByFlow
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetCompanyByFlow(string Id)
        {
            var FlowModel = Bll.LoadData(Id);
            var companyModel = new ModSysCompany();
            var MasterModel = new ModSysMaster();
            if (FlowModel.FlowType != 3)
            {
                companyModel = new BllSysCompany().LoadData(FlowModel.CompanyId);
                MasterModel = new BllSysMaster().Get(FlowModel.MasterId);
            }
            else
            {
                MasterModel = new BllSysMaster().Get(FlowModel.ApprovalUser);//申请人编号
                companyModel = new BllSysCompany().LoadData(MasterModel.Cid);
            }
            Hashtable ht = new Hashtable();
            ht.Add("Introduction", FlowModel.Reamrk);
            //公司信息
            if (companyModel != null)
            {
                ht.Add("Name", companyModel.Name);//公司名称
                ht.Add("Address", companyModel.Address);//地址
                ht.Add("LinkUser", companyModel.LinkUser);//联系 人
                ht.Add("Phone", companyModel.Phone);//联系电话
                ht.Add("Attribute", companyModel.Attribute);
            }
            if (MasterModel != null)
            {
                ht.Add("LoginName", MasterModel.LoginName);
            }
            return Json(new { data = ht }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //#region ===根据流程中用户编号查询单位和管理员信息 GetCompanyByUserId
        // <summary>
        // 查询实体
        // </summary>
        // <param name="mod"></param>
        //[HttpPost]
        //[ValidateInput(false)]
        //public JsonResult GetCompanyByUserId(string Id)
        //{
        //    var FlowModel = Bll.LoadData(Id);
        //    var MasterModel = new BllSysMaster().Get(FlowModel.ApprovalUser);//申请人编号
        //    var companyModel = new BllSysCompany().LoadData(MasterModel.Cid);
        //    Hashtable ht = new Hashtable();
        //    ht.Add("Introduction", FlowModel.Reamrk);
        //    公司信息
        //    if (companyModel != null)
        //    {
        //        ht.Add("Name", companyModel.Name);//公司名称
        //        ht.Add("Address", companyModel.Address);//地址
        //        ht.Add("LinkUser", companyModel.LinkUser);//联系 人
        //        ht.Add("Phone", companyModel.Phone);//联系电话
        //        ht.Add("Attribute", companyModel.Attribute);
        //    }
        //    if (MasterModel != null)
        //    {
        //        ht.Add("LoginName", MasterModel.LoginName);
        //    }
        //    return Json(new { data = ht }, JsonRequestBehavior.AllowGet);
        //}
        //#endregion
    }
}
