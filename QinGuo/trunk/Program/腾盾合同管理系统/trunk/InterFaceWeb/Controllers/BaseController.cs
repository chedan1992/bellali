using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Aspx.Model;
using Aspx.Common;
using InterFaceWeb.Common;
using System.Text;

namespace InterFaceWeb.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 参数获取与拼装
        /// </summary>
        public Dictionary<string, string> parameter = new Dictionary<string, string>();
        private IKernel ninjectKernel = new StandardKernel();
        public ModJsonResult jsonResult = new ModJsonResult();
        public string Page_Size = "10";
        public string Page_Index = "1";
        public int Page_Count = 0;


        /// <summary>
        /// 初始化
        /// </summary>
        public BaseController()
        {
            ninjectKernel.Bind<ILogAction>().To<SharePresentationLog>();
            jsonResult.success = true;
            jsonResult.msg = "请求成功！";
            jsonResult.nowdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        //错误日志记录器
        public ILogAction LogErrorRecord
        {
            get
            {
                return ninjectKernel.Get<ILogAction>();
            }
        }

        /// <summary>
        /// 获取查询列表
        /// </summary>
        /// <returns></returns>
        protected Search GetSearch()
        {
            LogErrorRecord.InfoFormat("参数:PageIndex={0}&PageSize={1}", Request.Params["PageIndex"], Request["PageSize"]);

            int start = int.Parse(Request.Params["PageIndex"] == null ? "1" : Request.Params["PageIndex"]);
            int _pageSize = int.Parse(Request.Params["PageSize"] == null ? "20" : Request.Params["PageSize"]);

            var _currentPage = start;
            Search search = new Search
            {
                PageSize = _pageSize,
                CurrentPageIndex = _currentPage//当前页数
            };
            return search;
        }
        //检查接口请求参数
        public void CheckParams(string param, string returnmsg)
        {
            if (jsonResult.success && string.IsNullOrEmpty(param))
            {
                jsonResult.success = false;
                jsonResult.msg = returnmsg;
            }
        }

        /// <summary>
        /// 返回JsonResult.24 重新json        
        /// </summary>     
        /// <param name="data">数据</param>
        /// <param name="behavior">行为</param>
        /// <param name="format">json中dateTime类型的格式</param>
        /// <returns>Json</returns>
        protected JsonResult Json(object data, string ContentType, JsonRequestBehavior behavior, string format = "yyyy-MM-dd HH:mm")
        {
            return new CustomJsonResult
            {
                Data = data,
                JsonRequestBehavior = behavior,
                ContentType = ContentType,
                FormateStr = format
            };
        }
    }
}
