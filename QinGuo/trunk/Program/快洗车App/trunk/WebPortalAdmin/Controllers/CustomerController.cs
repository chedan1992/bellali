using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using QINGUO.Common;
using System.Data;
using System.Collections;

namespace WebPortalAdmin.Controllers
{
    public class CustomerController : BaseController<ModSysCompany>
    {
        BllSysCompany bll = new BllSysCompany();
        ModJsonResult json = new ModJsonResult();
        /// <summary>
        /// 供应商管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region ===查询供应商公司管理 SearchData

        /// <summary>
        /// 查询供应商公司管理
        /// </summary>
        public void SearchData()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("Attribute=" + (int)CompanyType.供应商);
                //search.AddCondition("CreateCompanyId='" + CurrentMaster.Cid + "'");
                //if (!CurrentMaster.IsMain)
                //{
                //    if (CurrentMaster.LookPower == (int)LookPowerEnum.查看自建)
                //    {
                //        search.AddCondition("CreaterUserId='" + CurrentMaster.Id + "'");//自己查看自己的
                //    }
                //}
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

                //#region ===获取管理员信息
                //string UserName = Request.Params["UserName"];
                //string LoginName = Request.Params["LoginName"];
                //string Pwd = Request.Params["Pwd"];
                //string UserEmail = Request.Params["UserEmail"];
                //string UserPhone = Request.Params["UserPhone"];
                //Pwd = (string.IsNullOrEmpty(Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(Pwd));
                //#endregion

                //mod.CompLat = (mod.CompLat == null ? "" : mod.CompLat);
                //mod.ComPLon = (mod.ComPLon == null ? "" : mod.ComPLon);

                int result = 0;
                if (!string.IsNullOrEmpty(Request["modify"])) //修改
                {
                    ModSysCompany model = bll.LoadData(mod.Id);
                    model.Name = mod.Name;//公司名称
                    model.Code = mod.Code;
                    model.Tel = mod.Tel;
                    model.AccountName = mod.AccountName;
                    model.AccountNum = mod.AccountNum;
                    model.CheckoutType =int.Parse(Request["ComCheckoutType"]);
                    model.PaymentType = int.Parse(Request["ComPaymentType"]);
                    model.Type = mod.Type;

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
                    model.LicenseNumber = mod.LicenseNumber;
                    model.Province = mod.Province;
                    model.CityId = mod.CityId;
                    model.AreaId = mod.AreaId;

                    result = bll.Update(model);
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = "修改失败,请稍后再操作!";
                    }
                    //else
                    //{
                    //    BllSysMaster newMaster = new BllSysMaster();
                    //    master = newMaster.LoadData(Request.Params["UID"]);
                    //    master.UserName = UserName;
                    //    master.LoginName = LoginName;
                    //    master.Email = UserEmail;
                    //    master.Phone = UserPhone;
                    //    master.Id = Request.Params["UID"];
                    //    master.Pwd = Pwd;
                    //    newMaster.Update(master);
                    //}
                }
                else
                {
                    mod.Id = Guid.NewGuid().ToString();
                   // master.Pwd = Pwd;
                    master.Id = Guid.NewGuid().ToString();
                    master.Status = (int)StatusEnum.正常;
                    master.IsMain = true;
                    master.Cid = mod.Id;
                    master.IsSystem = true;
                    master.CreaterId = CurrentMaster.Id;
                    //master.UserName = UserName;
                    //master.LoginName = LoginName;
                    //master.Email = UserEmail;
                    //master.Phone = UserPhone;
                    master.OrganizaId = "0";
                    master.Attribute = (int)AdminTypeEnum.供应商管理员;
                    new BllSysMaster().ClearCache();
                    result = new BllSysMaster().Insert(master);
                    if (result > 0)
                    {
                        mod.Attribute = (int)CompanyType.供应商;
                        mod.CreateCompanyId = CurrentMaster.Cid;
                        mod.CreateTime = DateTime.Now;
                        mod.CreaterUserId = CurrentMaster.Id;

                        mod.Status = (int)StatusEnum.正常;
                        mod.Path = "1," + CurrentMaster.Company.CreateCompanyId + "," + CurrentMaster.Cid;
                        mod.ProPic = "/UploadFile/CompanyProPic/default_img_company.png";
                        mod.MasterId = master.Id;
                        result = bll.Insert(mod);
                        if (result <= 0)
                        {
                            new BllSysMaster().Delete(master.Id);
                        }
                    }
                    if (result <= 0)
                    {
                        json.success = false;
                        json.msg = " 保存失败,请稍后再操作!";
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

        #region ===批量保存表单 SaveDataAll

        /// <summary>
        /// 批量保存表单
        /// </summary>
        /// <param name="mod"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveDataAll()
        {
            var msg = new ModJsonResult();
            try
            {
                string IdList = Request["IdList"].ToString();//主键

                string CheckoutType = Request["ComCheckoutType"].ToString();//结账方式：0未设置 1月结 2日结
                string PaymentType = Request["ComPaymentType"].ToString();//付款方式：1支付宝 2工行  3农行
                string AccountName = Request["AccountName"].ToString();//名字
                string AccountNum = Request["AccountNum"].ToString();//账号

                string sql = "update Sys_Company set CheckoutType=" + CheckoutType + ",PaymentType=" + PaymentType + ",AccountName='" + AccountName + "',AccountNum='" + AccountNum + "' where Id in ("+IdList+")";
                int result = bll.ExecuteNonQueryByText(sql);
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
                string sql = "update Sys_Company set Status=" + (int)StatusEnum.删除 + " where Id in(" + id + ")";
                if (bll.ExecuteNonQueryByText(sql) > 0)
                {
                    msg.success = true;
                    LogInsert(OperationTypeEnum.操作, "供应商删除", "供应商删除成功.");
                }
                else
                {
                    msg.success = false;
                    msg.msg = "操作失败";
                    LogInsert(OperationTypeEnum.操作, "供应商删除", "供应商删除失败.");
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

        #region===获取省市区 GetArea
        /// <summary>
        /// 获取公司省市区 GetArea
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetArea(string companyId)
        {
            Hashtable table = new Hashtable();
            DataSet ds = new BllSysHatProvince().GetArea(companyId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                table.Add("Province", ds.Tables[0].Rows[0]["Province"].ToString());
                table.Add("CityId", ds.Tables[0].Rows[0]["CityId"].ToString());
                table.Add("AreaId", ds.Tables[0].Rows[0]["AreaId"].ToString());
                table.Add("ProvinceName", ds.Tables[0].Rows[0]["ProvinceName"].ToString());
                table.Add("CityName", ds.Tables[0].Rows[0]["CityName"].ToString());
                table.Add("AreaName", ds.Tables[0].Rows[0]["AreaName"].ToString());
            }
            return Json(new { data = table }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
