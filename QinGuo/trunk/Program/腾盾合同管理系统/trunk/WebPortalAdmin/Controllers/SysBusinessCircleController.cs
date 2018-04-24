using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Business;
using System.Data;
using WebPortalAdmin.Code;
using System.Web.Script.Serialization;
using QINGUO.Common;

namespace WebPortalAdmin.Controllers
{
    public class SysBusinessCircleController : BaseController<ModSysHatProvince>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SysBusinessCircle()
        {
            return View();
        }

        #region ==禁用状态 EnableUse
        /// <summary>
        /// 禁用状态
        /// </summary>
        public void EnableUse()
        {
            var msg = new ModJsonResult();

            string key = Request["id"];
            int result = new BllSysHatcity().UpdateStatus(1, key);
            if (result > 0)
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
            int result = new BllSysHatcity().UpdateStatus(0, key);
            if (result > 0)
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

        #region ===省份树形过滤条件查询 SearchProvinceByTree

        /// <summary>
        /// 省份树形过滤条件查询
        /// </summary>
        [HttpPost]
        public void SearchProvinceByTree()
        {
            string name = (Request["name"] == null ? "" : Request["name"]); //查询过滤条件
            string where = "";
            if (!string.IsNullOrEmpty(name.Trim()))
            {
                where += "(Name like '%" + name.Trim() + "%' or Code like '%" + name.Trim() + "%')";
            }
            DataSet ds = new BllSysHatProvince().GetTreeList(where);
            List<jsonSysBusinessCircle> list = new FunTreeCommon().GetJsonTree(ds);
            WriteJsonToPage(new JavaScriptSerializer().Serialize(list));
        }

        #endregion

        #region ===查询市区管理 SearchData
        /// <summary>
        /// 查询市区管理
        /// </summary>
        public void SearchData()
        {
            try
            {
                Search search = this.GetSearch();
                string ProvinceCode = (Request["ProvinceCode"] == null ? "" : Request["ProvinceCode"]);
                if (!string.IsNullOrEmpty(ProvinceCode) && ProvinceCode!="-1")
                {
                    search.AddCondition("ProvinceCode='" + ProvinceCode + "'");
                }
                WriteJsonToPage(new BllSysHatcity().SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
            }
        }
        #endregion

    }
}
