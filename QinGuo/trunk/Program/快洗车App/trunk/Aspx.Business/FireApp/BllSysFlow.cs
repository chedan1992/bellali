using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QINGUO.Model;
using QINGUO.Factory;
using QINGUO.IDAL;
using QINGUO.Common;
using Dapper;

namespace QINGUO.Business
{
    public class BllSysFlow : BllBase<ModSysFlow>
    {
        private ISysFlow DAL = CreateDalFactory.CreateDalSysFlow();

        public override void SetCurrentReposiotry()
        {
            CurrentDAL = DAL;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override int Insert(ModSysFlow t)
        {
            try
            {
                int result = CurrentDAL.Insert(t);
                try
                {
                    #region 推送
                    if (result > 0)//发送推送消息
                    {
                        //获取公司id
                        string cid = t.CompanyId;
                        //获取公司管理员id
                        BllSysMaster bllSysMaster = new BllSysMaster();
                        var masterlist = bllSysMaster.GetCIdByList(cid);
                        if (masterlist != null)
                        {
                            var master = masterlist.FirstOrDefault(c => !string.IsNullOrEmpty(c.BDUserId) && (c.Attribute == (int)AdminTypeEnum.汽配商管理员 || c.Attribute == (int)AdminTypeEnum.维修厂管理员 || c.Attribute == (int)AdminTypeEnum.系统管理员));
                            if (master != null)
                            {

                                ModPushView view = new ModPushView();
                                view.PushType = 1;//推送类型，依次往后推(0:巡检通知,1:审核通知)
                                view.Id = Guid.NewGuid().ToString();
                                view.CreateTime = DateTime.Now;//纪录推送时间
                                view.Infomation = t.Title;
                                view.Correlation = "";
                                string custom_content = JsonHelper.ToJson(view);//自定义格式有

                                PushResultView response = GeTuiPush.PushMessage(master.PaltForm, master.BDUserId, master.MobileCode, "审核通知", t.Title, custom_content);

                                if (response.result == "ok")
                                {
                                    BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                                    ModSysPushMessage info = new ModSysPushMessage()
                                    {
                                        Id = view.Id,
                                        CreateTime = DateTime.Now,
                                        CorrelativeId = "",
                                        CompanyId = cid,
                                        UserId = master.Id,
                                        Info = view.Infomation,
                                        PTitle = "审核通知",
                                        Status = 1,
                                        Type = view.PushType
                                    };
                                    int a = bllSysPushMessage.Insert(info);
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {

                }

                return result;
            }
            catch (Exception e)
            {
                return 0;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<ModSysFlow> getListAll(int? top, string where)
        {
            return DAL.getListAll(top, where);
        }
        /// <summary>
        /// 查询所有审核列表
        /// </summary>
        /// <param name="where">条件( and field=value and field=value)</param>
        /// <returns>ModCompanyOrder</returns>
        public string SearchData(Search search)
        {
            search.TableName = @"View_SysFlow";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "ApprovalTime desc";//排序
            return base.QueryPageToJson(search);
        }

        /// <summary>
        /// 分页查询审核列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Page<ModSysFlow> GetFlowList(Search search)
        {
            search.TableName = @"View_SysFlow";//表名
            search.KeyFiled = "Id";//主键
            search.SelectedColums = "*";
            search.StatusDefaultCondition = "";
            search.SortField = "ApprovalTime desc";//排序
            return DAL.GetFlowList(search);
        }


        /// <summary>
        /// 查询是否在审核中
        /// </summary>
        /// <param name="FlowType"></param>
        /// <param name="FlowStatus"></param>
        /// <param name="AppointId"></param>
        /// <returns></returns>
        public bool Exists(int FlowType, int FlowStatus, string AppointId)
        {
            return DAL.Exists(FlowType, FlowStatus, AppointId);
        }

        /// <summary>
        /// 审核流程
        /// </summary>
        /// <param name="userid">审核人id</param>
        /// <param name="cid">审核人单位id</param>
        /// <param name="reamrk">描述</param>
        /// <param name="flowId">流程id</param>
        /// <param name="isResult">1:通过,2:不通过</param>
        /// <returns></returns>
        public bool WaitWork(string userid, string cid, string reamrk, string flowId, int isResult = 0)
        {
            var model = this.LoadData(flowId);
            StringBuilder sb = new StringBuilder();
            if (model.FlowType == 1)//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核)
            {
                if (isResult == 1)//通过
                {
                    sb.Append("update Sys_Company set Status=" + (int)StatusEnum.正常 + " where Id='" + model.CompanyId + "';");
                    sb.AppendLine();
                    sb.Append("update Sys_Master set Status=" + (int)StatusEnum.正常 + " where Cid='" + model.CompanyId + "' and IsMain=1;");//一个单位关联一个管理员
                    sb.AppendLine();
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.正常 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }
                else
                { //不通过
                    sb.Append("update Sys_Company set Status=" + (int)StatusEnum.删除 + " where Id='" + model.CompanyId + "';");
                    sb.AppendLine();
                    sb.Append("update Sys_Master set Status=" + (int)StatusEnum.删除 + " where Cid='" + model.CompanyId + "' and IsMain=1;");//一个单位关联一个管理员
                    sb.AppendLine();
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.删除 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }
            }
            else if (model.FlowType == 2)
            {
                if (isResult == 1)
                {
                    sb.Append("update Sys_Master set Status=" + (int)StatusEnum.正常 + " where Id='" + model.MasterId + "';");
                    sb.AppendLine();
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.正常 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }
                else
                {
                    sb.Append("update Sys_Master set Status=" + (int)StatusEnum.删除 + " where Id='" + model.MasterId + "';");
                    sb.AppendLine();
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.删除 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }

            }
            else if (model.FlowType == 3)
            {
                //上级单位审核
                if (isResult == 1)
                {
                    sb.Append("update Sys_CompanyCognate set Status=" + (int)StatusEnum.正常 + " where FlowId='" + flowId + "';");
                    sb.AppendLine();
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.正常 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }
                else
                {
                    sb.Append("update Sys_CompanyCognate set Status=" + (int)StatusEnum.删除 + " where FlowId='" + flowId + "';");
                    sb.AppendLine();
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.删除 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }
            }
            else if (model.FlowType == 4)
            {
                var r = "通过";
                if (isResult == 1)//通过
                {
                    sb.Append("update Sys_Appointed set ResponsibleId='" + model.ApprovalUser + "' where Id='" + model.MasterId + "';");
                    sb.AppendLine();
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.正常 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }
                else //不通过
                {
                    r = "不通过";
                    sb.Append("update Sys_Flow set Reamrk='" + reamrk + "',FlowStatus=" + (int)StatusEnum.删除 + ",AuditUser='" + userid + "',AuditTime=getdate() where Id='" + flowId + "';");
                }

                try
                {
                    #region 推送

                    BllSysMaster bllSysMaster = new BllSysMaster();

                    var master = bllSysMaster.LoadData(model.ApprovalUser);
                    if (master != null && !string.IsNullOrEmpty(master.BDUserId))
                    {

                        ModPushView view = new ModPushView();
                        view.PushType = 1;//推送类型，依次往后推(0:巡检通知,1:审核通知,2:变更责任人)
                        view.Id = Guid.NewGuid().ToString();
                        view.CreateTime = DateTime.Now;//纪录推送时间
                        view.Infomation = model.Title;
                        view.Correlation = "";
                        string custom_content = JsonHelper.ToJson(view);//自定义格式有

                        PushResultView response = GeTuiPush.PushMessage(master.PaltForm, master.BDUserId, master.MobileCode, "变更责任人:(" + r + ")", model.Title, custom_content);
                        if (response.result == "ok")
                        {
                            BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                            ModSysPushMessage info = new ModSysPushMessage()
                            {
                                Id = view.Id,
                                CreateTime = DateTime.Now,
                                CorrelativeId = "",
                                CompanyId = cid,
                                UserId = master.Id,
                                Info = view.Infomation,
                                PTitle = "变更责任人",
                                Status = 1,
                                Type = view.PushType
                            };
                            int a = bllSysPushMessage.Insert(info);
                        }
                    }
                    #endregion
                }
                catch (Exception)
                {

                }

            }
            int result = this.ExecuteNonQueryByText(sb.ToString());

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 待办消息数
        /// </summary>
        /// <param name="sysMaster"></param>
        /// <returns></returns>
        public int flowmsg(ModSysMaster sysMaster)
        {
            return DAL.flowmsg(sysMaster);
        }
    }
}
