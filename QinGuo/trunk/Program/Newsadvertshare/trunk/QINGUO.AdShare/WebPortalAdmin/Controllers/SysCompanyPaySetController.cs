using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using System.Data;
using QINGUO.Common;
using QINGUO.Business;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 公司支付配置
    /// </summary>
    public class SysCompanyPaySetController : BaseController<ModSysCompanyPaySet>
    {
        BllSysCompanyPaySet Bll = new BllSysCompanyPaySet();

        /// <summary>
        /// 获取全部支付类型
        /// </summary>
        [HttpPost]
        public void SearchAuthData()
        {
            string where = "";
            if (!String.IsNullOrEmpty(Request["Id"]))
            {
                where = " Id='" + Request["Id"] + "'";
            }
            else
            {
                where = " Status=" + (int)StatusEnum.正常 + " and Id not in (select payType from Sys_CompanyPaySet where CompanyID='"+CurrentMaster.Cid+"')";
            }
            DataSet ds = Bll.GetSelectAll(where);//获取所有支付方式
            var result = JsonHelper.DataTableToJson(ds.Tables[0]);
            WriteJsonToPage(result);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region ==保存表单 SaveData
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="t"></param>
        [HttpPost]
        [ValidateInput(false)]
        public void SaveData(ModSysCompanyPaySet t)
        {
            BllSysCompanyPaySet bll = new BllSysCompanyPaySet();
            ModJsonResult json = new ModJsonResult();

            t.CompanyID = CurrentMaster.Cid;

            if (!string.IsNullOrEmpty(Request["modify"])) //修改
            {
                var model = bll.LoadData(t.Id);
                if (model != null)
                {
                    t.Status = model.Status;
                }
                int result = bll.Update(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "修改失败,请稍后再操作!";
                }
            }
            else
            {
                t.Status = (int)StatusEnum.正常;
                t.Id = Guid.NewGuid().ToString();
                int result = bll.Insert(t);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
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
                int result = new BllSysCompanyPaySet().Delete(id);
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

        #region==页面列表 SearchData
        /// <summary>
        /// 页面列表
        /// </summary>
        public void SearchData()
        {
            var search = base.GetSearch();
            //查询自己所属公司
            search.AddCondition("CompanyID='" + CurrentMaster.Cid + "'");

            var jsonResult = new BllSysCompanyPaySet().SearchData(search);

            WriteJsonToPage(jsonResult);
        }

        #endregion
    }
}
