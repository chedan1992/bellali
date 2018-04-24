using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using System.Collections;
using System.Data;
using QINGUO.Business;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 统计
    /// </summary>
    public class ChartController : BaseController<ModSysAppointed>
    {
        /// <summary>
        /// 设备图表统计
        /// </summary>
        /// <returns></returns>
        public ActionResult ChartIndex()
        {
            return View();
        }
        /// <summary>
        /// 单位设备图表统计
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyChart()
        {
            return View();
        }
        /// <summary>
        /// 超期未巡检列表统计
        /// </summary>
        /// <returns></returns>
        public ActionResult MaintenanceDate()
        {
            return View();
        }
        #region ==根据单位统计设备信息 ChartPart
        /// <summary>
        /// 已过期的数量（红色），异常状态设备数量（黄色），半年内即将过期的数量（蓝色），其他正常数量（绿色）
        /// </summary>
        public void ChartPart()
        {
            BllSysAppointed Bll = new BllSysAppointed();
            var msg = new ModJsonResult();
            List<Hashtable> info = new List<Hashtable>();
            try
            {
                string CompanyId = Request["CompanyId"].ToString() == "" ? CurrentMaster.Cid : Request["CompanyId"].ToString();
                DataSet ds = Bll.ChartAppointed(CompanyId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Hashtable table = new Hashtable();
                        table.Add("已过期数量", ds.Tables[0].Rows[i]["Count1"].ToString());
                        table.Add("异常状态设备量", ds.Tables[0].Rows[i]["Count2"].ToString());
                        table.Add("半年内即将过期数量", ds.Tables[0].Rows[i]["Count3"].ToString());
                        table.Add("正常数量", ds.Tables[0].Rows[i]["Count4"].ToString());
                        info.Add(table);
                    }
                }
                msg.data = JsonHelper.ToJson(info);
                LogInsert(OperationTypeEnum.访问, "设备统计模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "设备统计模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===查询图表链接设备 SearchData
        /// <summary>
        /// 查询超级管理设备
        /// </summary>
        public void SearchData()
        {
            try
            {
                Search search = this.GetSearch();
                string TypeShow = Request["TypeShow"].ToString();
                string CID = Request["CID"].ToString() == "" ? CurrentMaster.Cid : Request["CID"].ToString();
             
                string ResponsibleId = Request["ResponsibleId"].ToString();//责任人编号
                switch (int.Parse(TypeShow))
                {
                    case 1://已过期数量
                        search.AddCondition("MaintenanceStatus=-1");
                        if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.单位用户)
                        {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        else if (CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防用户)
                        {
                            search.AddCondition("Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)");
                        }
                        else
                        {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        break;
                    case 2://异常状态设备量
                        search.AddCondition("MaintenanceStatus=1");
                        if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.单位用户)
                        {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        else if (CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防用户)
                        {
                            search.AddCondition("Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)");
                        }
                        else
                        {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        break;
                    case 3://半年内即将过期数量
                        search.AddCondition("DATEADD(mm,-6,LostTime)<getDate() and MaintenanceStatus=0 and MaintenanceStatus!=-1");
                        if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.单位用户)
                        {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        else if (CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防用户)
                        {
                            search.AddCondition("Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)");
                        }
                        else {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        break;
                    case 4://正常数量
                        search.AddCondition("MaintenanceStatus=0");
                        if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.单位用户)
                        {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        else if (CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保用户 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防用户)
                        {
                            search.AddCondition("Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)");
                        }
                        else
                        {
                            search.AddCondition("Cid='" + CID + "'");
                        }
                        break;
                    case 5://我管理的设备
                        search.AddCondition("ResponsibleId='" + ResponsibleId + "'");
                        break;
                    case 6://部门下的设备
                        search.AddCondition("DeptId='" + ResponsibleId + "'");
                        break;
                    case 7://箱子下的设备信息
                        search.AddCondition("Places='" + ResponsibleId + "'");
                        break;
                }
                WriteJsonToPage(new BllSysAppointed().SearchData(search));
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        /// <summary>
        /// 超期未巡检列表统计
        /// </summary>
        public void SuperParentData()
        {
            try
            {
                BllSysAppointed Bll = new BllSysAppointed();
                Search search = this.GetSearch();
                if (Request["Id"].ToString() != "-1")
                {
                    search.AddCondition("Cid='" + Request["Id"].ToString() + "'");
                }
                else
                {
                    search.AddCondition("Cid in (select EmployerId from Sys_CompanyCognate where Cid='" + CurrentMaster.Cid + "' and Status=1 and SelectType=2)");
                }

                string searchType = Request["searchType"].ToString();
                switch (int.Parse(searchType))
                { 
                    case 1://全部
                        break;
                    case 2://本月
                        search.AddCondition("datediff(month,MaintenanceDate,getdate())=0");
                        break;
                    case 3://上月
                        search.AddCondition("datediff(month,MaintenanceDate,getdate())=1");
                        break;
                }
                search.AddCondition("MaintenanceDate<getDate()");

                LogInsert(OperationTypeEnum.访问, "超期未巡检列表统计模块", "访问页面成功.");
                WriteJsonToPage(Bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
                LogInsert(OperationTypeEnum.异常, "超期未巡检列表统计模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
    }
}
