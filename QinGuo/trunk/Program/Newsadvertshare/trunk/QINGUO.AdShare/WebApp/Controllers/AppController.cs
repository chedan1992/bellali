using SwaggerApiDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApp.Controllers
{
    /// <summary>
    /// App接口集成
    /// </summary>
    public class AppController : ApiController
    {
        private List<App> GetApps()
        {
            List<App> list = new List<App>();
            list.Add(new App() { Id = 1, Name = "WeChat", Remark = "WeChat", Date = DateTime.Now });
            list.Add(new App() { Id = 2, Name = "FaceBook", Remark = "FaceBook" });
            list.Add(new App() { Id = 3, Name = "Google", Remark = "Google" });
            list.Add(new App() { Id = 4, Name = "QQ", Remark = "QQ" });
            return list;
        }
        /*
        /// <summary>
        /// 获取授权token
        /// </summary>
        /// <returns>token</returns>
        [HttpGet]
        [Route("App/GetAuthorizeToken")]
        public JsonResult<string> GetAuthorizeToken()
        {
            return new JsonResult<string>() { data = "1", msg = "获取token成功", success = true };
        }

        /// <summary>
        /// 获取所有APP
        /// </summary>
        /// <returns>所有APP集合</returns>
        [HttpGet]
        [Route("App/Get/{token}")]
        [TokenAuthorize]
        public HttpResponseMessage Get()
        {
            return MyJson.ObjectToJson(GetApps());
        }

        /// <summary>
        /// 获取指定APP
        /// </summary>
        /// <param name="id">需要获取APP的id</param>
        /// <returns>返回指定APP</returns>
        [HttpGet]
        [Route("App/GetId")]
        [TokenAuthorize]
        public ModJsonResult<App> GetId(string token, int id)
        {
            var app = GetApps().Where(m => m.Id.Equals(id)).FirstOrDefault();
            return new ModJsonResult<App>() { data = app, msg = "获取成功", success = true };
        }

        /// <summary>
        /// 增加App信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("App/Insert")]
        [TokenAuthorize]
        public HttpResponseMessage Insert([FromBody]App value)
        {
            ResultJson json = new ResultJson() { Code = 200, Message = "Ok" };
            return MyJson.ObjectToJson(json);
        }

        /// <summary>
        /// 更新APP信息
        /// </summary>
        /// <param name="value">APP信息</param>
        /// <returns>更新结果</returns>
        [HttpPost]
        [Route("App/UpdateApp")]
        [TokenAuthorize]
        public HttpResponseMessage UpdateApp([FromBody]App value)
        {
            ResultJson json = new ResultJson() { Code = 200, Message = "Ok" };
            return MyJson.ObjectToJson(json);
        }

        /// <summary>
        /// 删除APP信息
        /// </summary>
        /// <param name="id">APP编号</param>
        /// <returns>删除结果</returns>
        [HttpPost]
        [Route("App/DeleteApp")]
        [TokenAuthorize]
        public HttpResponseMessage DeleteApp(int id)
        {
            ResultJson json = new ResultJson() { Code = 200, Message = "Ok" };
            return MyJson.ObjectToJson(json);
        }*/
    }
}
