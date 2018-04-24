using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using QINGUO.Common;
using QINGUO.Business;
using QINGUO.Model;

namespace WebPortalAdmin.Controllers
{
    /// <summary>
    /// 省市区
    /// </summary>
    public class AreaController : BaseController<ModSysHatProvince>
    {

        #region 加载省市区 ===Cityarea
        /// <summary>
        /// 查询所属区
        /// </summary>
        [HttpPost]
        public void comProvince(string code)
        {
            //默认添加
            if (string.IsNullOrEmpty(code))
            {
                List<ModSysHatProvince> list = new BllSysHatProvince().QueryToAll();
                WriteJsonToPage(JsonHelper.ToJson(list));
            }
            else {
                List<ModSysHatProvince> list = new BllSysHatProvince().QueryToAll().Where(p => p.Code == code).ToList();
                WriteJsonToPage(JsonHelper.ToJson(list));
            }
        }
        /// <summary>
        /// 加载市区
        /// </summary>
        [HttpPost]
        public void comCity(string code)
        {
            List<ModSysHatcity> list = new BllSysHatcity().QueryToAll().Where(p=>p.ProvinceCode==code && p.Status==1).ToList();
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        /// <summary>
        /// 加载区
        /// </summary>
        [HttpPost]
        public void comArea(string code)
        {
            List<ModSysHatArea> list = new BllSysHatarea().QueryToAll().Where(p => p.CityCode == code).ToList();
            WriteJsonToPage(JsonHelper.ToJson(list));
        }
        #endregion
    }
}
