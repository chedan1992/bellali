using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Model;
using QINGUO.Common;
using QINGUO.Business;

namespace WebPortalAdmin.Controllers
{
    public class SysAppointCheckNotesController : BaseController<ModSysAppointCheckNotes>
    {
        BllSysAppointCheckNotes Bll = new BllSysAppointCheckNotes();

        /// <summary>
        /// 我的设备巡检
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        #region ==根据加载数据 SearchData()
        /// <summary>
        /// 根据id 加载数据
        /// </summary>
        public void SearchData()
        {
            try
            {
                Search search = this.GetSearch();
                search.AddCondition("ResponsibleId='" + CurrentMaster.Id + "'");//自己查看自己的

                LogInsert(OperationTypeEnum.访问, "设备巡检模块", "访问页面成功.");

                WriteJsonToPage(new BllSysQRCode().SearchData(search));
            }
            catch (Exception ex)
            {
                LogErrorRecord.Info("查询列表:" + ex);
                LogInsert(OperationTypeEnum.异常, "设备巡检模块", "查询列表错误消息:" + ex.Message.ToString());
            }
        }
        #endregion
    }
}
