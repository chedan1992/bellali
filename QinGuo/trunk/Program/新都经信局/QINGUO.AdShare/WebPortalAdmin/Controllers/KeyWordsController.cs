using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;

namespace WebPortalAdmin.Controllers
{
    public class KeyWordsController : BaseController<ModAdActive>
    {
        string companyId = "C0A38317-59DA-4DE2-9428-E5B66EDBC2CF";
        /// <summary>
        /// 敏感词管理
        /// </summary>
        /// <returns></returns>
        public ActionResult KeyWords()
        {
            string content = new BllSysCompany().GetSetting(4);
            ViewData["KeyWords"] = content;
            return View();
        }

        /// <summary>
        /// 系统配置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Setting()
        {
            var company = new BllSysCompany().LoadData(companyId);
            ViewData["NameTitle"] = company.Name;
            ViewData["Address"] = company.Address;
            ViewData["Phone"] = company.Phone;
            //ViewData["Money"] = company.ReegistMoney;
            ViewData["LawyerName"] = company.LawyerName == null ? "0" : company.LawyerName;
            ViewData["LawyerPhone"] = company.LawyerPhone == null ? "0" : company.LawyerPhone;
            ViewData["LawFirm"] = company.LawFirm == null ? "0" : company.LawFirm;

            ViewData["LicenseNumber"] = company.LicenseNumber == null ? "0" : company.LicenseNumber;
            

            return View();
        }


        /// <summary>
        /// 保存敏感词管理
        /// </summary>
        [ValidateInput(false)]
        public void SavaKeyWords()
        {
            var msg = new ModJsonResult();
            try
            {
                string txteditor = Request["txteditor"];
                int result = new BllSysCompany().UpdateSetting(4, txteditor);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                }
            }
            catch (Exception a)
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
            WriteJsonToPage(msg.ToString());
        }

         /// <summary>
        /// 保存系统设置信息
        /// </summary>
        [ValidateInput(false)]
        public void SavaSetting()
        {
            var msg = new ModJsonResult();
            try
            {
                string NameTitle = Request["NameTitle"];
                string Address = Request["Address"];
                string Phone = Request["Phone"];
                //string ReegistMoney = (string.IsNullOrEmpty(Request["ReegistMoney"]) == true ?"0": Request["ReegistMoney"].ToString());
                //string LawyerName = Request["LawyerName"];//起步价
                //string LawyerPhone = Request["LawyerPhone"];//每公里
                //string LawFirm = Request["LawFirm"];//每分钟

                //string LicenseNumber = Request["LicenseNumber"] == null ? "0" : Request["LicenseNumber"];//倍数
                

                var company = new BllSysCompany().LoadData(companyId);
                company.Name = NameTitle;
                company.NameTitle = NameTitle;
                company.Address = Address;
                company.Phone = Phone;
                //company.ReegistMoney = Convert.ToDecimal(ReegistMoney);
                //company.LawyerName = LawyerName;
                //company.LawyerPhone = LawyerPhone;
                //company.LawFirm = LawFirm;

                //company.LicenseNumber = LicenseNumber;

                int result = new BllSysCompany().Update(company);
                if (result > 0)
                {
                    msg.success = true;
                }
                else
                {
                    msg.success = false;
                }
            }
            catch (Exception a)
            {
                msg.success = false;
                msg.msg = "操作失败";
            }
            WriteJsonToPage(msg.ToString());
        }
        
    }
}
