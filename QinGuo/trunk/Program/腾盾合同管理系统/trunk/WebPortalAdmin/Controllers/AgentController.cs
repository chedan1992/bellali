using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers.Agent
{
    public class AgentController : BaseController<ModSysCompany>
    {
        BllSysCompany bll = new BllSysCompany();

        public ActionResult Agent()
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
            search.AddCondition("Attribute=" + (int)CompanyType.维保公司);
            if (!CurrentMaster.IsMain)
            {
                if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                {
                    search.AddCondition("CreaterUserId='" + CurrentMaster.Id + "'");//自己查看自己的
                }
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
                model.Province = t.Province;
                model.CityId = t.CityId;
                model.AreaId = t.AreaId;
                model.LicenseNumber = t.LicenseNumber;

                int result = bll.Update(model);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
                else
                {
                    BllSysMaster newMaster = new BllSysMaster();
                    master = newMaster.LoadData(Request.Params["UID"]);
                    master.UserName = UserName;
                    master.LoginName = LoginName;
                    master.Email = UserEmail;
                    master.Phone = UserPhone;
                    master.Id = Request.Params["UID"];
                    master.Pwd = Pwd;
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
                master.Attribute = (int)AdminTypeEnum.维保公司管理员;
                new BllSysMaster().ClearCache();
                int result = new BllSysMaster().Insert(master);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
                else
                {
                    t.Status = (int)StatusEnum.正常;
                    t.CreaterUserId = CurrentMaster.Id;
                    t.CreateTime = DateTime.Now;
                    t.Attribute = (int)CompanyType.维保公司;
                    t.CreateCompanyId = CurrentMaster.Cid;
                    t.Path = "1," + CurrentMaster.Cid;
                    t.ProPic = "/UploadFile/CompanyProPic/default_img_company.png";
                    t.MasterId = master.Id;
                    t.RegisiTime = DateTime.Now;//公司注册时间
                    result = bll.Insert(t);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
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
                int result =bll.DeleteStatus(id);
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
    }
}
