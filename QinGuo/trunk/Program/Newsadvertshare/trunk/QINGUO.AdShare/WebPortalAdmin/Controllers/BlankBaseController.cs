using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using QINGUO.Model;
using QINGUO.Common;
using QINGUO.Business;

namespace WebPortalAdmin.Controllers
{
    public class BlankBaseController : Controller
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected ModSysMaster CurrentMaster;
        private IKernel ninjectKernel = new StandardKernel();
        //错误日志记录器
        public ILogAction LogErrorRecord
        {
            get
            {
                return ninjectKernel.Get<ILogAction>();
            }
        }

        public BlankBaseController()
        {
            ninjectKernel.Bind<ILogAction>().To<SharePresentationLog>();

            var content = new MasterContext();
            if (!content.IsLogined)
            {
                return;
            }
            else
            {
                CurrentMaster = content.Master;
            }
        }
    }
}
