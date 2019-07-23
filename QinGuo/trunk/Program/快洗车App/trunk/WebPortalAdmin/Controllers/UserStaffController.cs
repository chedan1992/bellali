using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Configuration;
using System.Data;
using System.Text;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 手机端员工管理
    /// </summary>
    public class UserStaffController : BaseController<ModSysMaster>
    {
        /// <summary>
        /// 电梯公司员工管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上级单位查询员工
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyIndex()
        {
            return View();
        }


        /// <summary>
        /// 员工审核
        /// </summary>
        /// <returns></returns>
        public ActionResult Audit()
        {
            return View();
        }

        #region==公司页面列表 SearchDataByCompany
        /// <summary>
        /// 公司页面列表
        /// </summary>
        public void SearchDataByCompany()
        {
            var search = base.GetSearch();
            switch (CurrentMaster.Attribute)
            { 
                case (int)AdminTypeEnum.消防部门管理员:
                    search.AddCondition("Attribute="+(int)AdminTypeEnum.消防用户);
                    break;
                case (int)AdminTypeEnum.维保公司管理员:
                    search.AddCondition("Attribute=" + (int)AdminTypeEnum.维保用户);
                    break;
                case (int)AdminTypeEnum.供应商管理员:
                    search.AddCondition("Attribute=" + (int)AdminTypeEnum.供应商用户);
                    break;
            }
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysMaster().SearchDataByCompany(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region==页面列表 SearchMasterGrid
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchMasterGrid()
        {
            var search = base.GetSearch();
            //查询自己所属公司
            search.AddCondition("Cid='" + CurrentMaster.Cid + "'");
            search.AddCondition("Attribute=" + (int)AdminTypeEnum.普通员工);
            search.AddCondition("Status=" + (int)StatusEnum.正常); //过滤条件
            var jsonResult = new BllSysMaster().SearchMasterGrid(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region==超级管理员查询页面列表 SearchData
        /// <summary>
        /// 超级管理员查询页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            string OrganizaId = Request["OrganizaId"];
            if (!string.IsNullOrEmpty(OrganizaId) && OrganizaId != "-1")
            {
                search.AddCondition("(OrganizaId='" + OrganizaId + "' or OrganizaId='' or OrganizaId in (select Id from GetCompanyByChildId('" + OrganizaId + "',1)))");
            }
            else { 
                //根据权限
                if (CurrentMaster.Attribute == (int)AdminTypeEnum.系统管理员)
                {
                  
                }
                else if (CurrentMaster.Attribute == (int)AdminTypeEnum.单位管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || CurrentMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                {
                    //查看自己单位下人员的审核
                    search.AddCondition("Cid='" + CurrentMaster.Cid + "'");
                }
            }
            search.AddCondition("(Attribute>=" + (int)AdminTypeEnum.单位用户 + " and Attribute<=" + (int)AdminTypeEnum.供应商用户+")");
            search.AddCondition("(Status!=" + (int)StatusEnum.删除 + " and Status!=-2)"); //过滤条件
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysMaster().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region==页面列表 SearchAuditData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchAuditData()
        {
            var search = base.GetSearch();
            //查询自己所属公司
            search.AddCondition("Cid='" + CurrentMaster.Cid + "'");
            search.AddCondition("Attribute=" + (int)AdminTypeEnum.普通员工);
            search.AddCondition("Status=-2"); //过滤条件
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterId='" + CurrentMaster.Id + "'");
                }
            }
            var jsonResult = new BllSysMaster().SearchData(search);
            WriteJsonToPage(jsonResult);
        }

        #endregion

        #region 查询当前注册普通员工是否存在 ExitsMaster
        /// <summary>
        /// 查询当前注册管理员是否存在
        /// </summary>
        public void ExitsMaster(string code, string key)
        {
            ModJsonResult json = new ModJsonResult();
            int count = 0;
            if (key != "" && key != null)
            {
                json.success = new BllSysMaster().Exists("Sys_Master", "  and Attribute=" + (int)AdminTypeEnum.普通员工 + " and LoginName='" + code + "'  and Id<>'" + key + "' and Status!=" + (int)StatusEnum.删除, out count);
            }
            else
            {
                json.success = new BllSysMaster().Exists("Sys_Master", " and Attribute=" + (int)AdminTypeEnum.普通员工 + " and LoginName='" + code + "'  and Status!=" + (int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(json.ToString());
        }

        #endregion

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysMaster t)
        {
            BllSysMaster bll = new BllSysMaster();
            ModJsonResult json = new ModJsonResult();

            string HPact = Request["HPact"] == null ? "" : Request["HPact"];//用户上传头像
            t.Pwd = (string.IsNullOrEmpty(t.Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(t.Pwd.Trim()));
            t.LoginName = t.LoginName.Trim();
            t.UserName = t.UserName.Trim();
            
            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                t.HeadImg = HPact;

                var model = bll.LoadData(t.Id);
                model.Pwd = t.Pwd;
                model.LoginName = t.LoginName;
                model.UserName = t.UserName;
                model.HeadImg = HPact;
                int result = bll.UpdateDate(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else
            {
                string OrganizaId=Request["OrganizaId"].ToString();//部门编号
                //根据部门编号查询单位信息
                var company = new BllSysCompany().LoadData(OrganizaId);
                if (company.Attribute == (int)CompanyType.单位 || company.Attribute == (int)CompanyType.部门)
                {
                    t.Attribute = (int)AdminTypeEnum.单位用户;//用户类型
                }
                else if (company.Attribute == (int)CompanyType.供应商)
                {
                    t.Attribute = (int)AdminTypeEnum.供应商用户;
                }
                else if (company.Attribute == (int)CompanyType.维保公司)
                {
                    t.Attribute = (int)AdminTypeEnum.维保用户;
                }
                else if (company.Attribute == (int)CompanyType.消防部门)
                {
                    t.Attribute = (int)AdminTypeEnum.消防用户;
                }
              

                t.Id = Guid.NewGuid().ToString();
                t.Status = (int)StatusEnum.正常;
                t.Cid = company.CreateCompanyId;
                t.OrganizaId = OrganizaId;//部门编号
                t.IsSystem = false;
                t.CreaterId = CurrentMaster.Id;
                t.CreateTime = DateTime.Now;
                t.HeadImg = HPact;
                t.IsMain = true;
                //t.Money = 1000;
                //DataSet ds = new BllSysHatProvince().GetArea(CurrentMaster.Cid);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    t.Detail = ds.Tables[0].Rows[0]["ProvinceName"].ToString() + " " + ds.Tables[0].Rows[0]["CityName"].ToString() + " " + ds.Tables[0].Rows[0]["AreaName"].ToString();
                //}
                t.CardNum = t.OperateNum;

                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
               
                new BllSysMaster().ClearCache();
            }

            WriteJsonToPage(json.ToString());
        }


        #endregion

        #region ==保存上级单位员工表单 SaveParentData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveParentData(ModSysMaster t)
        {
            BllSysMaster bll = new BllSysMaster();
            ModJsonResult json = new ModJsonResult();

            string HPact = Request["HPact"] == null ? "" : Request["HPact"];//用户上传头像
            t.Pwd = (string.IsNullOrEmpty(t.Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(t.Pwd.Trim()));
            t.LoginName = t.LoginName.Trim();
            t.UserName = t.UserName.Trim();

            switch (CurrentMaster.Attribute)
            {
                case (int)AdminTypeEnum.消防部门管理员:
                    t.Attribute = (int)AdminTypeEnum.消防用户;
                    break;
                case (int)AdminTypeEnum.维保公司管理员:
                    t.Attribute = (int)AdminTypeEnum.维保用户;
                    break;
                case (int)AdminTypeEnum.供应商管理员:
                    t.Attribute = (int)AdminTypeEnum.供应商用户;
                    break;
            }
            t.Id = Guid.NewGuid().ToString();
            t.Status = (int)StatusEnum.正常;
            t.Cid =CurrentMaster.Cid;
            t.OrganizaId ="0";//部门编号
            t.IsSystem = false;
            t.CreaterId = CurrentMaster.Id;
            t.CreateTime = DateTime.Now;
            t.HeadImg = HPact;
            t.IsMain = true;
            t.CardNum = t.OperateNum;
            int result = bll.Insert(t);
            if (result <= 0)
            {
                json.success = false;
                json.msg = " 保存失败,请稍后再操作!";
            }
            new BllSysMaster().ClearCache();
            WriteJsonToPage(json.ToString());
        }


        #endregion

        
        #region ==用户审核通过 GTPAudit
        /// <summary>
        /// 用户审核通过
        /// </summary>
        public void GTPAudit()
        {
            BllSysMaster Bll = new BllSysMaster();
            var msg = new ModJsonResult();
            string key = Request["id"];
            var model = Bll.LoadData(key);
            int result = Bll.UpdateStatus(1, key);
            if (result > 0)
            {
                msg.success = true;
                //发送短信
                WebService.SDKService msm = new WebService.SDKService();
                string sn = ConfigurationManager.AppSettings["sn"];
                string pwd = ConfigurationManager.AppSettings["pwd"];
                string smstext = "亲,恭喜您,您在电梯宝申请的账户信息审核已通过,第一次使用,系统赠送您【" + model.Money.ToString("#0.00") + "】元保证金,赶紧登录电梯宝,体验一下吧.";
                result = msm.sendSMS(sn.Trim().ToString(), pwd.Trim().ToString(), "", model.LoginName.Split(new char[] { ',' }), smstext, "", "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";
            }

            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==用户审核不通过 GTPAuditcancel
        /// <summary>
        /// 用户审核不通过
        /// </summary>
        public void GTPAuditcancel()
        {
            BllSysMaster Bll = new BllSysMaster();
            string Introduction = Request["Introduction"].ToString();
            var msg = new ModJsonResult();
            string key = Request["id"];
            var model = Bll.LoadData(key);
            int result = Bll.Delete(key);
            if (result > 0)
            {
                msg.success = true;
                //发送短信
                WebService.SDKService msm = new WebService.SDKService();
                string sn = ConfigurationManager.AppSettings["sn"];
                string pwd = ConfigurationManager.AppSettings["pwd"];
                string smstext = "您在电梯宝申请的账户信息审核没有通过,原因：【" + Introduction + "】,请麻烦您尽快重新申请.为您审批通过.";
                result = msm.sendSMS(sn.Trim().ToString(), pwd.Trim().ToString(), "", model.LoginName.Split(new char[] { ',' }), smstext, "", "GBK", 3, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";
            }

            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region 查询当前注册普通员工操作码否存在 ExitsOperateNum
        /// <summary>
        /// 查询当前注册普通员工操作码否存在
        /// </summary>
        public void ExitsOperateNum(string code, string key)
        {
            ModJsonResult json = new ModJsonResult();
            int count = 0;
            if (key != "" && key != null)
            {
                json.success = new BllSysMaster().Exists("Sys_Master", "  and Attribute=" + (int)AdminTypeEnum.普通员工 + " and OperateNum='" + code + "'  and Id<>'" + key + "' and Status!=" + (int)StatusEnum.删除, out count);
            }
            else
            {
                json.success = new BllSysMaster().Exists("Sys_Master", " and Attribute=" + (int)AdminTypeEnum.普通员工 + " and OperateNum='" + code + "'  and Status!=" + (int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(json.ToString());
        }

        #endregion

        #region ===获取责任人 下拉树 GetSysMaster
        /// <summary>
        /// 获取责任人
        /// </summary>
        [HttpPost]
        public void GetSysMaster()
        {
            string CompanyId = Request["CompanyId"].ToString().Trim();//
            string UserId = Request["UserId"];//一键变更责任人

            if (string.IsNullOrEmpty(CompanyId) || CompanyId == "0")
            {
                CompanyId = CurrentMaster.Cid;
            }
            if (string.IsNullOrEmpty(UserId))
            {
                List<ModSysMaster> list = new BllSysMaster().QueryToAll().Where(p => p.Cid == CompanyId && p.Attribute == (int)AdminTypeEnum.单位用户 && p.Status == (int)StatusEnum.正常).ToList();
                WriteJsonToPage(JsonHelper.ToJson(list));
            }
            else {
                List<ModSysMaster> list = new BllSysMaster().QueryToAll().Where(p => p.Cid == CompanyId && p.Attribute == (int)AdminTypeEnum.单位用户 && p.Status == (int)StatusEnum.正常 &&p.Id !=UserId).ToList();
                WriteJsonToPage(JsonHelper.ToJson(list));
            }
        }
        #endregion

        #region ===一键交接工作  ChangeUser
        /// <summary>
        /// 一键交接工作
        /// </summary>
        [HttpPost]
        public void ChangeUser()
        {
            var msg = new ModJsonResult();
            try
            {
                string UserId = Request["UserId"].ToString().Trim();//当前人
                string comResponsibleId = Request["comResponsibleId"].ToString();//一键变更责任人.

                StringBuilder sb = new StringBuilder();
                //设备信息
                sb.Append("update Sys_Appointed set ResponsibleId='" + comResponsibleId + "' where ResponsibleId='" + UserId + "';");
                sb.AppendLine();
                //巡检记录
                sb.Append("update Sys_AppointCheckNotes set ResponsibleId='" + comResponsibleId + "' where ResponsibleId='" + UserId + "';");
                sb.AppendLine();
                int result = new BllSysMaster().ExecuteNonQueryByText(sb.ToString());
                msg.success = true;
                msg.msg = "操作成功";
            }
            catch(Exception aa) {
                msg.success = false;
                msg.msg = "操作失败," + aa.Message;
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion
        
    }
}
