using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Common;
using QINGUO.Business;
using System.Text;

namespace WebPortalAdmin.Controllers
{
    public class SysCompanyCognateController : BaseController<ModSysCompanyCognate>
    {
        BllSysCompanyCognate Bll = new BllSysCompanyCognate();

        /// <summary>
        /// 社会单位 关联单位
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 上级单位 关联单位
        /// </summary>
        /// <returns></returns>
        public ActionResult ParentIndex()
        {
            return View();
        }

        #region ===查询社会单位,我发起的 SearchIndex
        /// <summary>
        /// 查询社会单位
        /// </summary>
        public void SearchIndex()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("EmployerId='" + CurrentMaster.Cid + "'");
                search.AddCondition("SelectType=1");//查询单位管理的
                LogInsert(OperationTypeEnum.访问, "关联单位模块", "访问页面成功.");

                WriteJsonToPage(Bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "关联单位模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion

        #region ===查询上级单位,我发起的 SearchParentIndex
        /// <summary>
        /// 查询上级单位
        /// </summary>
        public void SearchParentIndex()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("CId='" + CurrentMaster.Cid + "'");
                search.AddCondition("SelectType=2");//上级单位管理的

                LogInsert(OperationTypeEnum.访问, "关联单位模块", "访问页面成功.");
                WriteJsonToPage(Bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
                LogInsert(OperationTypeEnum.异常, "关联单位模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion

        #region ===撤销社会单位申请 DeleteIndex
        /// <summary>
        /// 撤销社会单位申请
        /// </summary>
        public void DeleteIndex(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                StringBuilder sb = new StringBuilder();
                //撤销流程
                sb.Append("delete from Sys_Flow where Id=(select FlowId from Sys_CompanyCognate where Id='" + id + "' );");
                sb.AppendLine();
                sb.Append("delete from Sys_CompanyCognate where Id='" + id + "';");
                int result = Bll.ExecuteNonQueryByText(sb.ToString());
                if (result <= 0)
                {
                    msg.success = false;
                    msg.msg = " 撤销失败,请稍后再操作!";
                    LogInsert(OperationTypeEnum.操作, "单位申请撤销", "撤销失败.");
                }
                else
                {
                    LogInsert(OperationTypeEnum.操作, "单位申请撤销", "撤销成功.");
                    msg.success = true;
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
                LogInsert(OperationTypeEnum.异常, "单位申请撤销", "操作异常信息:" + ex);
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #region ===获取当前社会单位选择的上级单位 getSelected
        /// <summary>
        /// 获取当前社会单位选择的上级单位
        /// </summary>
        public void getSelected()
        {
            var data = Bll.GetList("View_SysCompanyCognate", " and EmployerId='" + CurrentMaster.Cid + "' and SelectType=1", "CId,CName", 0);
            var result = JsonHelper.DataTableToJson(data.Tables[0]);
            WriteJsonToPage(result);
        }

        #endregion


        #region ===获取当前上级单位选择的社会单位 getSelected
        /// <summary>
        /// 获取当前社会单位选择的上级单位
        /// </summary>
        public void getSelectedParent()
        {
            var data = Bll.GetList("View_SysCompanyCognate", " and CId='" + CurrentMaster.Cid + "' and SelectType=2", "EmployerId,EmployerName", 0);
            var result = JsonHelper.DataTableToJson(data.Tables[0]);
            WriteJsonToPage(result);
        }

        #endregion

        #region ===保存社会单位选择的单位信息 SaveIndex
        /// <summary>
        /// 保存社会单位选择的单位信息
        /// </summary>
        /// <param name="mod"></param>
        public void SaveIndex()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                string[] IdList = Request["IdList"].ToString().Split(',');
                if (IdList.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < IdList.Length; i++)
                    {
                        string FlowId = Guid.NewGuid().ToString();
                        sb.Append(" insert into Sys_CompanyCognate(Id,EmployerId,CId,Status,FlowId,SelectType)");
                        sb.Append( " values(");
                        sb.Append("'"+Guid.NewGuid().ToString()+"',");
                        sb.Append("'"+CurrentMaster.Cid+"',");//社会单位编号
                        sb.Append("'" + IdList[i] + "',");//上级单位编号
                        sb.Append("'0',");
                        sb.Append("'" + FlowId + "',");
                        sb.Append("'1'");
                        sb.Append(");");
                        sb.AppendLine();

                        //添加流程审核模块
                        sb.Append("insert into Sys_Flow(Id,Title,FlowType,FlowStatus,Reamrk,ApprovalUser,ApprovalTime,CompanyId,MasterId)");
                        sb.Append(" values(");
                        sb.Append("'" + FlowId + "',");
                        sb.Append("'社会单位" + CurrentMaster.Company.Name + "向您提交审核申请',");
                        sb.Append("'" +3+ "',");//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核)
                        sb.Append("'" + 0 + "',");
                        sb.Append("'',");
                        sb.Append("'" + CurrentMaster.Id + "',");
                        sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        sb.Append("'" + IdList[i] + "',");//审核单位编号
                        sb.Append("'" + CurrentMaster.Id + "'");
                        sb.Append(")");
                        sb.AppendLine();
                    }
                    int result = Bll.ExecuteNonQueryByText(sb.ToString());
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "保存单位关联", "保存失败.");
                    }
                    else {
                        LogInsert(OperationTypeEnum.操作, "保存单位关联", "保存成功.");
                    }
                }
               
            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "保存单位关联", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());

        }
        #endregion

        #region ===保存上级单位选择的社会单位信息 SaveParentIndex
        /// <summary>
        /// 保存上级单位选择的社会单位信息
        /// </summary>
        /// <param name="mod"></param>
        public void SaveParentIndex()
        {
            ModJsonResult json = new ModJsonResult();
            try
            {
                string[] IdList = Request["IdList"].ToString().Split(',');
                if (IdList.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < IdList.Length; i++)
                    {
                        string FlowId = Guid.NewGuid().ToString();
                        sb.Append(" insert into Sys_CompanyCognate(Id,EmployerId,CId,Status,FlowId,SelectType)");
                        sb.Append(" values(");
                        sb.Append("'" + Guid.NewGuid().ToString() + "',");
                        sb.Append("'" + IdList[i] + "',");//社会单位编号
                        sb.Append("'" +CurrentMaster.Cid + "',");//上级单位编号
                        sb.Append("'0',");
                        sb.Append("'" + FlowId + "',");
                        sb.Append("'2'");
                        sb.Append(");");
                        sb.AppendLine();

                        //添加流程审核模块
                        sb.Append("insert into Sys_Flow(Id,Title,FlowType,FlowStatus,Reamrk,ApprovalUser,ApprovalTime,CompanyId,MasterId)");
                        sb.Append(" values(");
                        sb.Append("'" + FlowId + "',");
                        sb.Append("'" + CurrentMaster.Company.Name + "向您提交查看单位信息申请',");
                        sb.Append("'" + 3 + "',");//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核)
                        sb.Append("'" + 0 + "',");
                        sb.Append("'',");
                        sb.Append("'" + CurrentMaster.Id + "',");
                        sb.Append("'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',");
                        sb.Append("'" + IdList[i] + "',");//审核单位编号
                        sb.Append("'" + CurrentMaster.Id + "'");
                        sb.Append(")");
                        sb.AppendLine();
                    }
                    int result = Bll.ExecuteNonQueryByText(sb.ToString());
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
                        LogInsert(OperationTypeEnum.操作, "保存单位关联", "保存失败.");
                    }
                    else
                    {
                        LogInsert(OperationTypeEnum.操作, "保存单位关联", "保存成功.");
                    }
                }

            }
            catch (Exception ex)
            {
                json.msg = "保存失败！";
                json.success = false;
                LogInsert(OperationTypeEnum.异常, "保存单位关联", "操作异常信息:" + ex);
            }
            WriteJsonToPage(json.ToString());

        }
        #endregion
    }
}
