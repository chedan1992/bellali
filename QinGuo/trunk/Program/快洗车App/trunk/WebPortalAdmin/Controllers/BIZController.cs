#region Version Info
/* ======================================================================== 
* 【本类功能概述】 电梯公司管理
* 
* 
* 修改者：青果科技 时间： 
* 修改说明： 
* ======================================================================== 
*/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Threading;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 电梯公司
    /// </summary>
    public class BIZController : BaseController<ModSysCompany>
    {
        BllSysCompany bll = new BllSysCompany();
        ModJsonResult json = new ModJsonResult();

        /// <summary>
        /// 视图
        /// </summary>
        /// <returns></returns>
        public ActionResult BIZ()
        {
            return View();
        }

        #region ===查询岗位管理 SearchData

        /// <summary>
        /// 查询岗位管理
        /// </summary>
        public void SearchData()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("Attribute=" + (int)CompanyType.汽配商管理);
                if (!CurrentMaster.IsMain)
                {
                    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                    {
                        search.AddCondition("CreaterUserId='" + CurrentMaster.Id + "'");//自己查看自己的
                    }
                }
                WriteJsonToPage(bll.SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
            }
        }
        #endregion

        #region ===保存表单

        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysCompany mod)
        {
            try
            {
                ModSysMaster master = new ModSysMaster();

                mod.CompLat = (mod.CompLat == null ? "" : mod.CompLat);
                mod.ComPLon = (mod.ComPLon == null ? "" : mod.ComPLon);
                mod.LinkUser = (mod.LinkUser == null ? "" : mod.LinkUser);
                mod.LegalPerson = (mod.LegalPerson == null ? "" : mod.LegalPerson);

                #region ===获取管理员信息
                string UserName = Request.Params["UserName"];
                string LoginName = Request.Params["LoginName"];
                string Pwd = Request.Params["Pwd"];
                string UserEmail = Request.Params["UserEmail"];
                string UserPhone = Request.Params["UserPhone"];
                Pwd = (string.IsNullOrEmpty(Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(Pwd));
                #endregion

                int result = 0;
                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    ModSysCompany model = bll.LoadData(mod.Id);
                    model.Name = mod.Name;//公司名称
                    model.LegalPerson = mod.LegalPerson;
                    model.LinkUser = mod.LinkUser;//联系人
                    model.Address = mod.Address;//公司地址
                    model.Code = mod.Code;
                    model.ReegistMoney = mod.ReegistMoney;//注册资金
                    model.Phone = mod.Phone;//公司电话
                    model.Email = mod.Email;//邮箱
                    model.Introduction = mod.Introduction;//公司介绍
                    model.CompLat = mod.CompLat;
                    model.ComPLon = mod.ComPLon;
                    model.Nature = mod.Nature;//公司分类
                    model.Type = mod.Type;//公司性质
                    model.LegalPerson = mod.LegalPerson;//法人
                    model.Pact = mod.Pact;//合同

                    model.Province = mod.Province;
                    model.CityId = mod.CityId;
                    model.AreaId = mod.AreaId;


                    result = bll.Update(model);
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
                    mod.Id = Guid.NewGuid().ToString();
                    master.Id = Guid.NewGuid().ToString();
                    master.Status = (int)StatusEnum.正常;
                    master.Pwd = Pwd;
                    master.IsMain = true;
                    master.Cid = mod.Id;
                    master.IsSystem = true;
                    master.CreaterId = CurrentMaster.Id;
                    master.UserName = UserName;
                    master.LoginName = LoginName;
                    master.Email = UserEmail;
                    master.Phone = UserPhone;
                    master.OrganizaId = "0";
                    master.Attribute = (int)AdminTypeEnum.汽配商管理员;
                    new BllSysMaster().ClearCache();
                     result = new BllSysMaster().Insert(master);
                     if (result <= 0)
                     {
                         json.success = false;
                         json.msg = " 保存失败,请稍后再操作!";
                     }
                     else
                     {
                         mod.Attribute = (int)CompanyType.汽配商管理;
                         mod.CreateCompanyId = CurrentMaster.Cid;
                         mod.CreateTime = DateTime.Now;
                         mod.CreaterUserId = CurrentMaster.Id;
                         mod.Status = (int)StatusEnum.正常;
                         mod.Path = "1," + CurrentMaster.Cid;
                         mod.ProPic = "/UploadFile/CompanyProPic/default_img_company.png";
                         mod.MasterId = master.Id;
                         result = bll.Insert(mod);
                         if (result <= 0)
                         {
                             json.success = false;
                             json.msg = " 保存失败,请稍后再操作!";
                         }
                     }
                }
              
            }
            catch (Exception)
            {
                json.msg = "保存失败！";
                json.success = false;
            }

            WriteJsonToPage(json.ToString());

        }
        #endregion

        #region ===根据id查询实体 GetData
        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetData(string id)
        {
            var mod = bll.LoadData(id);
            return Json(new { data = mod }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ===删除 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteData(string id)
        {
            var msg = new ModJsonResult();
            try
            {
                //判断岗位下是否还存在员工
                var mode = new BllSysMaster().Exists(" OrganizaId=" + id );
                if (mode ==true)
                {
                    msg.success = false;
                    msg.msg = "此岗位下还有员工,暂不能删除.";
                }
                else
                {

                    if (bll.DeleteStatus(id) > 0)
                    {
                        msg.success = true;
                    }
                    else
                    {
                        msg.success = false;
                        msg.msg = "操作失败";
                    }
                }
            }
            catch (Exception ex)
            {
                msg.msg = "操作失败：" + ex.Message;
            }
            WriteJsonToPage(msg.ToString());
        }
        #endregion

        #region ===禁用和启用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DisableUse(string id)
        {
            var msg = new ModJsonResult();
            if (!bll.UpdateStatue((int)StatusEnum.禁用, id))
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void EnableUse(string id)
        {
            var msg = new ModJsonResult();
            if (!bll.UpdateStatue((int)StatusEnum.正常, id))
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
        }

        #endregion

    }
}
