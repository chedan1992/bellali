
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QINGUO.Model;
using QINGUO.Business;

namespace WebPortalAdmin.Controllers
{
    public class SysRoleRangeDataController : BaseController<ModSysRoleRangeData>
    {
        #region ==删除角色数据 DeleteData
        /// <summary>
        /// 删除
        /// </summary>
        [HttpPost]
        public void DeleteData()
        {
            var msg = new ModJsonResult();
            try
            {
                string id = Request["id"].ToString();
                int result = new BllSysRoleRangeData().DeleteData(id);
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