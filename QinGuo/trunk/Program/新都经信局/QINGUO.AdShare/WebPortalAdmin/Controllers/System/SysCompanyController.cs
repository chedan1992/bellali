
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Business;
using QINGUO.Model;
using QINGUO.Common;
using System.Web.Script.Serialization;
using System.Collections;
using System.Data;
using System.Text;

namespace WebPortalAdmin.Controllers
{
    public class SysCompanyController : BaseController<ModSysCompany>
    {
        BllSysCompany bll = new BllSysCompany();
        /// <summary>
        /// 供应商 管理
        /// </summary>
        /// <returns></returns>
        public ActionResult SysCompany()
        {
            return View();
        }

        #region 分页列表查询 SearchData
        /// <summary>
        /// 查询分公司列表
        /// </summary>
        public void SearchData()
        {
            Search search = this.GetSearch();
            try
            {
                search.AddCondition("Attribute=" + (int)CompanyType.单位);
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        search.AddCondition("CreaterUserId='" + CurrentMaster.Id + "'");//自己查看自己的
                    }
                }
                LogInsert(OperationTypeEnum.访问, "单位管理模块", "访问页面成功.");
            }
            catch (Exception ex)
            {
                LogInsert(OperationTypeEnum.异常, "单位管理模块", "查询列表错误消息:" + ex.Message.ToString());
            }
            WriteJsonToPage(new BllSysCompany().SearchData(search));
        }
        #endregion

        #region 保存表单 SaveData

        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysCompany t)
        {
            ModJsonResult json = new ModJsonResult();

            ModSysMaster master = new ModSysMaster();

            #region ===获取管理员信息
            string UserName = Request.Params["UserName"];
            string LoginName = Request.Params["LoginName"];
            string Pwd = Request.Params["Pwd"];
            string UserEmail = Request.Params["UserEmail"];
            string UserPhone = Request.Params["UserPhone"];
             Pwd = (string.IsNullOrEmpty(Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(Pwd));
            #endregion


            t.LinkUser = (t.LinkUser == null ? "" : t.LinkUser);
            t.LegalPerson = (t.LegalPerson == null ? "" : t.LegalPerson);
            t.CompLat = (t.CompLat == null ? "" : t.CompLat);
            t.ComPLon = (t.ComPLon == null ? "" : t.ComPLon);

            string x = Request.Params["x"];
            string y = Request.Params["y"];


            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                ModSysCompany model = bll.LoadData(t.Id);
                model.Name = t.Name;
                model.LegalPerson = t.LegalPerson;
                model.LinkUser = t.LinkUser;
                model.Address = t.Address;
                model.CompLat = t.CompLat;
                model.ComPLon = t.ComPLon;
                model.Code = t.Code;
                model.ReegistMoney = t.ReegistMoney;
                model.Phone = t.Phone;
                model.Email = t.Email;
                model.Introduction = t.Introduction;
                model.LawyerPhone = t.LawyerPhone;
                model.LawyerName = t.LawyerName;
                model.CompLat = t.CompLat;
                model.ComPLon = t.ComPLon;
                model.Province = t.Province;
                model.CityId = t.CityId;
                model.AreaId = t.AreaId;
                model.Nature = t.Nature;//公司分类
                model.Type = t.Type;//公司性质
                model.RegisiTime = t.RegisiTime;

                int result = bll.Update(model);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
                else {
                    BllSysMaster newMaster = new BllSysMaster();
                    master = newMaster.LoadData(Request.Params["UID"]);
                    master.UserName = UserName;
                    master.Pwd = Pwd;
                    master.LoginName = LoginName;
                    master.Email = UserEmail;
                    master.Phone = UserPhone;
                    master.Id = Request.Params["UID"];
                    newMaster.Update(master);
                }
            }
            else
            {
                t.Id = Guid.NewGuid().ToString();
                master.Id = Guid.NewGuid().ToString();
                master.Status = (int)StatusEnum.正常;
                master.IsMain = true;
                master.Cid = t.Id;
                master.IsSystem = true;
                master.Pwd = Pwd;
                master.CreaterId = CurrentMaster.Id;
                master.UserName = UserName;
                master.LoginName = LoginName;
                master.Email = UserEmail;
                master.Phone = UserPhone;
                master.OrganizaId = "0";
                master.Attribute = (int)AdminTypeEnum.单位管理员;
                new BllSysMaster().ClearCache();
                int result = new BllSysMaster().Insert(master);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
                else {
                    t.Status = (int)StatusEnum.正常;
                    t.CreaterUserId = CurrentMaster.Id;
                    t.CreateTime = DateTime.Now;
                    t.Attribute = (int)CompanyType.单位;
                    t.Path = "1," + CurrentMaster.Cid;
                    t.ProPic = "/UploadFile/CompanyProPic/default_img_company.png";
                    t.MasterId = master.Id;
                    t.CreateCompanyId = "0";
                    t.RegisiTime = DateTime.Now;//公司注册时间

                    result = bll.Insert(t);
                    if (result <= 0)
                    {
                        new BllSysMaster().Delete(master.Id);
                    }
                }
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
                int result = bll.DeleteStatus(id);
                if (result > 0)
                {
                    msg.success = true;
                    //同步其他操作

                    StringBuilder sb = new StringBuilder();
                    //同步用户
                    sb.Append("update Sys_Master set Status=" + (int)StatusEnum.删除 + " where Cid='" + id + "';");
                    sb.AppendLine();
                    //同步设备
                    sb.Append("update Sys_Appointed set Status=" + (int)StatusEnum.删除 + " where Cid=''" + id + "';");
                    sb.AppendLine();
                    //同步巡检记录
                    sb.Append("delete from Sys_AppointCheckNotes where CId=''" + id + "';");
                    sb.AppendLine();
                    //同步关联单位
                    sb.Append("delete from Sys_CompanyCognate where CId=''" + id + "';");
                    sb.AppendLine();
                    bll.ExecuteNonQueryByText(sb.ToString());
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

        #region ==启用状态 EnableUse
        /// <summary>
        /// 启用状态
        /// </summary>
        public void EnableUse()
        {
            var msg = new ModJsonResult();
            string key = Request["id"].ToString();
            bool result =new BllSysCompany().UpdateStatue(1,key);
            if (result)
            {
                msg.success = true;
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";
            }

            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ==停用状态 DisableUse

        /// <summary>
        /// DisableUse
        /// </summary>
        public void DisableUse()
        {
            var msg = new ModJsonResult();

            string key = Request["id"];
            bool result = new BllSysCompany().UpdateStatue(0, key);
            if (result)
            {
                msg.success = true;
            }
            else
            {
                msg.success = false;
                msg.msg = "操作失败";
            }

            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===查询是否存在 SearchNameExits
        /// <summary>
        /// name查询是否存在
        /// </summary>
        public void SearchNameExits(string name, string key, string attr)
        {
            var msg = new ModJsonResult();
            int count = 0;
            if (key != "" && key != null)
            {
                msg.success = bll.Exists("Sys_Company", " and Attribute=" + attr + " and Name='" + name + "' and Id<>'" + key + "' and Status<>" + (int)StatusEnum.删除, out count);
            }
            else
            {
                msg.success = bll.Exists("Sys_Company", " and Attribute=" + attr + " and Name='" + name + "' and Status<>"+(int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion

        #region ===查询编码是否存在 SearchCodeExits
        /// <summary>
        /// 查询编码是否存在
        /// </summary>
        public void SearchCodeExits(string code, string key, string attr)
        {
            var msg = new ModJsonResult();
            int count = 0;
            if (key != "" && key != null)
            {
                msg.success = bll.Exists("Sys_Company", " and Attribute=" + attr + " and Code='" + code + "' and Id<>'" + key + "' and Status<>" + (int)StatusEnum.删除, out count);
            }
            else
            {
                msg.success = bll.Exists("Sys_Company", " and Attribute=" + attr + " and Code='" + code + "' and Status<>" + (int)StatusEnum.删除, out count);
            }
            WriteJsonToPage(msg.ToString());
        }

        #endregion


        /// <summary>
        /// 获取单位列表下拉
        /// </summary>
        public void GetCompanyList()
        {
            var data = bll.GetList("Sys_Company", " and Attribute=" + (int)CompanyType.单位 + " and Status="+(int)StatusEnum.正常+"", "ID,Name", 0);
            var result = JsonHelper.DataTableToJson(data.Tables[0]);
            WriteJsonToPage(result);
        }
        /// <summary>
        /// 获取部门列表下拉
        /// </summary>
        public void GetDeptList()
        {
            string OrgId = Request["OrgId"].ToString();
            var data = bll.GetList("Sys_Company", " and Attribute=" + (int)CompanyType.部门 + " and CreateCompanyId='" + OrgId + "' and Status=" + (int)StatusEnum.正常 + "", "ID,Name", 0);
            var result = JsonHelper.DataTableToJson(data.Tables[0]);
            WriteJsonToPage(result);
        }
          /// <summary>
        /// 获取单位管理员列表下拉
        /// </summary>
        public void GetCompanyByAttrList()
        {
            var data = bll.GetList("Sys_Company", " and Attribute=" + Request["attr"] + " and Status=" + (int)StatusEnum.正常 + "", "ID,Name", 0);
            var result = JsonHelper.DataTableToJson(data.Tables[0]);
            WriteJsonToPage(result);
        }

        #region ===公司选择树 GetTreeByMaster
        [HttpPost]
        public void GetTreeByCompany()
        {
            string Attribute = Request["Attribute"];//单位类型
            string selectUserkey = (Request["select"] == null ? "" : Request["select"]);//选中的编号

            string where = " and Attribute=" + Attribute + " and Status=" + (int)StatusEnum.正常;
            string name = (Request["MasterName"] == null ? "" : Request["MasterName"]); //查询过滤条件
            if (name != "")
            {
                where += " and Name like '%" + name + "%'";
            }
            var mytab = bll.GetList("Sys_Company", where, "ID,Name", 0);
            GetTreeNode(mytab.Tables[0], selectUserkey);
            Output = Output.Replace("check", "checked");
            WriteJsonToPage(Output);
        }

        /// <summary>
        /// 查询父节点
        /// </summary>
        public void GetTreeNode(DataTable dt, string selectlist)
        {
            List<Hashtable> hashList = new List<Hashtable>();

            foreach (DataRow myrow in dt.Rows)
            {
                Hashtable ht = new Hashtable();
                ht.Add("id", myrow["ID"].ToString());
                ht.Add("text", myrow["Name"].ToString());
                ht.Add("expanded", true);
                ht.Add("leaf", true);
                if (selectlist.Contains(myrow["ID"].ToString()))
                {
                    ht.Add("check", true);
                }
                else
                {
                    ht.Add("check", false);
                }
                hashList.Add(ht);
            }
            Output = new JavaScriptSerializer().Serialize(hashList);
        }
        #endregion


        #region 分页列表查询 SearchData

        /// <summary>
        /// 查询分公司列表
        /// </summary>
        public void SearchTopData()
        {
            Search search = this.GetSearch();
            search.AddCondition("Attribute=" + Request["Attribute"]);
            WriteJsonToPage(new BllSysCompany().SearchData(search));
        }
        #endregion
    }
}
