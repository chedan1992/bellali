using Ninject;
using QINGUO.Business;
using QINGUO.Common;
using QINGUO.Model;
using QINGUO.ViewModel;
using SwaggerApiDemo.Common;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApp.Controllers
{
    /// <summary>
    /// 集成
    /// </summary>
    public class WashCarApiController : ApiController
    {
        #region Comm
        private IKernel ninjectKernel = new StandardKernel();
        private JsonUser user = null;

        public WashCarApiController()
        {
            ninjectKernel.Bind<ILogAction>().To<SharePresentationLog>();
        }

        public JsonUser CurUser = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            var au = controllerContext.Request.Headers.Authorization;
            if (au != null && !string.IsNullOrEmpty(au.Scheme))
            {
                CurUser = UserTokenManager.GetCache(au.Scheme);
            }
        }


        //错误日志记录器
        public ILogAction LogErrorRecord
        {
            get
            {
                return ninjectKernel.Get<ILogAction>();
            }
        }

        /*短信账号*/
        public string serverIp = "api.ucpaas.com";
        //public string serverPort = "443";
        //public string account = "b5dbea6d198d79b7034319caa2cb33e3";    //用户sid
        //public string token = "b01f27d8e0f561e00dfab4fdaa5eb902";      //用户sid对应的token
        //public string appId = "ac74d8afac8a4902ab55d1ecba287bd2";      //对应的应用id，非测试应用需上线使用

        public string serverPort = "443";
        public string account = "c1ea643283b7b0f6340c09d9ed416428";    //用户sid
        public string token = "6b1a97cecd5ec2c9493f5c3ec01a45e5";      //用户sid对应的token
        public string appId = "6168e62cc7684630a0261c2e3bf9c53b";      //对应的应用id，非测试应用需上线使用

        #endregion

        #region 基础接口

        private JsonUser UserTo(ModSysMaster modsysuser)
        {
            JsonUser r = new JsonUser();
            r.id = modsysuser.Id;
            r.loginname = modsysuser.LoginName;
            r.name = modsysuser.UserName;
            r.phone = modsysuser.Phone;
            r.sex = modsysuser.Sex;
            r.headimg = modsysuser.HeadImg;
            r.cid = modsysuser.Cid;
            r.cname = modsysuser.UserName;
            r.createrId = modsysuser.CreaterId;//创建者id 汽配商id

            /*
            系统管理员 = 1,
            汽配商管理员 = 2,
            维修厂管理员 = 3,
            手机用户=4,
            普通用户=10,*/
            r.usertype = modsysuser.Attribute;
            r.token = r.id;
            return r;
        }

        /// <summary>
        /// 注册维修厂账号
        /// </summary>
        /// <param name="account">登录名</param>
        /// <param name="phone">联系电话</param>
        /// <param name="pwd">密码</param>
        /// <param name="CommpanyName">维修厂名</param>
        /// <param name="MobileCode">机器码</param>
        /// <param name="code">短信验证码(口令：102913)</param>
        /// <param name="PaltForm">注册平台(1:苹果 2:安卓 3:网页)</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/register/{account}/{pwd}/{phone}/{CommpanyName}")]
        public MyJsonResult<string> register(string account, string pwd, string phone, string CommpanyName, string MobileCode = "", string code = "102913", int PaltForm = 3)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                if (jsonResult.success && string.IsNullOrEmpty(account))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "登录名不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(phone))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "联系电话不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(CommpanyName))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "维修厂名不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(pwd))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户密码不能为空！";
                }
                /*
                if (pwd.Trim().Length < 6 || pwd.Trim().Length > 20)
                {
                    jsonResult.success = false;
                    jso
                    nResult.msg = "密码长度必须6至20个字符之间！";
                }*/
                if (jsonResult.success && string.IsNullOrEmpty(code))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "短信验证码不能为空！";
                }
                //Regex regex = new Regex("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
                //if (!string.IsNullOrEmpty(email) && !regex.IsMatch(email))
                //{
                //    jsonResult.success = false;
                //    jsonResult.msg = "请输入正确的邮箱地址！";
                //}

                if (jsonResult.success)
                {
                    //判断当前手机号是否已经存在
                    BllSysMaster bllmaster = new BllSysMaster();
                    var ModsysMasterView = bllmaster.GetModelByWhere(" and LoginName='" + account + "' and Status<>" + (int)StatusEnum.删除);
                    if (ModsysMasterView != null)
                    {
                        jsonResult.msg = "此账号信息已经注册,不能重复注册！";
                        jsonResult.success = false;
                    }

                    if (jsonResult.success)
                    {
                        ModSysCompany c = new ModSysCompany();
                        c.Id = Guid.NewGuid().ToString();
                        ModSysMaster sysMaster = new ModSysMaster();
                        sysMaster.Id = Guid.NewGuid().ToString();
                        sysMaster.LoginName = account;
                        sysMaster.Pwd = DESEncrypt.Encrypt(pwd);  //加密
                        sysMaster.Status = (int)StatusEnum.正常;
                        sysMaster.MobileCode = MobileCode; //机器码
                        sysMaster.PaltForm = PaltForm;
                        sysMaster.CreateTime = DateTime.Now;
                        sysMaster.UserName = CommpanyName;
                        sysMaster.Cid = c.Id;
                        sysMaster.Phone = phone;
                        sysMaster.CreaterId = CurUser.id;//汽配id
                        //sysMaster.Email = email;
                        sysMaster.HeadImg = "";//默认头像
                        sysMaster.Attribute = (int)AdminTypeEnum.维修厂管理员;
                        sysMaster.IsMain = true;

                        if (bllmaster.Insert(sysMaster) > 0)
                        {
                            c.Phone = phone;
                            c.Name = CommpanyName;
                            c.CreaterUserId = sysMaster.Id;
                            c.CreateTime = DateTime.Now;
                            BllSysCompany bllSysCompany = new BllSysCompany();

                            if (bllSysCompany.Insert(c) > 0)
                            {
                                jsonResult.msg = "添加成功！";
                                jsonResult.success = true;
                            }
                            else
                            {
                                bllmaster.Delete(sysMaster.Id);
                                jsonResult.msg = "添加失败！";
                                jsonResult.success = false;
                            }
                        }
                        else
                        {
                            jsonResult.msg = "添加失败！";
                            jsonResult.success = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 维修厂 汽配商 账号密码登录
        /// </summary>
        /// <param name="loginname">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="BDChannelId">百度推送id</param>
        /// <param name="BDUserId">百度推送用户id</param>
        /// <param name="MobileCode">机器码</param>
        /// <param name="PaltForm">登录平台(1:苹果 2:安卓 3:网页)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/login/{loginname}/{pwd}/{PaltForm}")]
        public MyJsonResult<JsonUser> login(string loginname, string pwd, string BDChannelId = "", string BDUserId = "", string MobileCode = "", int PaltForm = 3)
        {
            MyJsonResult<JsonUser> jsonResult = new MyJsonResult<JsonUser>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(loginname))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户名不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(pwd))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "密码不能为空！";
                }

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();

                    ModSysMaster modsysuser = bllmaster.GetModelByWhere("and LoginName='" + loginname + "' and Status<>" + (int)StatusEnum.删除);//登录名称
                    if (modsysuser != null)
                    {
                        if (!string.IsNullOrEmpty(modsysuser.Pwd) && DESEncrypt.md5(DESEncrypt.Decrypt(modsysuser.Pwd), 32) == pwd)
                        {
                            #region 其他扩展
                            try
                            {
                                //修改登录次数和时间
                                modsysuser.LoginTime = DateTime.Now;
                                modsysuser.LoginNum = modsysuser.LoginNum + 1;
                                modsysuser.MobileCode = MobileCode; //机器码
                                modsysuser.BDUserId = BDUserId;
                                modsysuser.BDChannelId = BDChannelId;
                                modsysuser.PaltForm = PaltForm;

                                bllmaster.UpdateLogin(modsysuser);
                            }
                            catch (Exception ex)
                            {
                                LogErrorRecord.ErrorFormat("其他扩展={0}", ex.Message);
                            }
                            #endregion

                            JsonUser r = UserTo(modsysuser);

                            UserTokenManager.Remove(r.token);
                            UserTokenManager.SetCache(r.token, r);
                            jsonResult.data = r;
                            jsonResult.msg = "登录成功！";
                            jsonResult.success = true;
                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "登录失败,密码输入错误！";
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "登录失败，该用户不存在！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 汽车用户手机验证码 登录
        /// </summary>
        /// <param name="loginname">手机号</param>
        /// <param name="code">短信验证码(口令：102913)</param>
        /// <param name="BDChannelId">百度推送id</param>
        /// <param name="BDUserId">百度推送用户id</param>
        /// <param name="MobileCode">机器码</param>
        /// <param name="PaltForm">登录平台(1:苹果 2:安卓 3:网页)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/loginE/{loginname}/{code}/{PaltForm}")]
        public MyJsonResult<JsonUser> loginE(string loginname, string code, string BDChannelId = "", string BDUserId = "", string MobileCode = "", int PaltForm = 3)
        {
            MyJsonResult<JsonUser> jsonResult = new MyJsonResult<JsonUser>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(loginname))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "手机号不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(code))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "短信验证码不能为空！";
                }

                BllSysMaster bllmaster = new BllSysMaster();
                ModSysMaster modsysuser = bllmaster.GetModelByWhere("and LoginName='" + loginname + "' and Status=" + (int)StatusEnum.正常);//登录名称

                if (modsysuser == null)
                {
                    modsysuser = new ModSysMaster();
                    modsysuser.Id = Guid.NewGuid().ToString();
                    modsysuser.LoginName = loginname;
                    modsysuser.Pwd = DESEncrypt.Encrypt("666666");  //加密
                    modsysuser.Status = (int)StatusEnum.正常;
                    modsysuser.Attribute = (int)AdminTypeEnum.手机用户;
                    modsysuser.IsMain = false;
                    if (bllmaster.Insert(modsysuser) > 0)
                    {
                        jsonResult.success = true;
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "登录失败，请稍后再试！";
                    }
                }

                if (jsonResult.success && code == "102913")
                {
                    jsonResult.success = true;
                }
                else if (jsonResult.success)
                {
                    //验证码是是否正确
                    BllSysMessageAuthCode regVBll = new BllSysMessageAuthCode();
                    ModSysMessageAuthCode modAuthcode = regVBll.GetModelByWhere("and  getdate() < EndTime and TypeInt=0 and Tel='" + loginname + "'");
                    if (jsonResult.success && modAuthcode != null)
                    {
                        if (modAuthcode.Code != code)
                        {
                            jsonResult.msg = "短信验证码错误！";
                            jsonResult.success = false;
                        }
                    }
                    else if (jsonResult.success && modAuthcode == null)
                    {
                        jsonResult.msg = "验证码已过期！";
                        jsonResult.success = false;
                    }
                }

                if (jsonResult.success)
                {
                    #region 其他扩展
                    try
                    {
                        //修改登录次数和时间
                        modsysuser.LoginTime = DateTime.Now;
                        modsysuser.LoginNum = modsysuser.LoginNum + 1;
                        modsysuser.MobileCode = MobileCode; //机器码
                        modsysuser.BDUserId = BDUserId;
                        modsysuser.BDChannelId = BDChannelId;
                        modsysuser.PaltForm = PaltForm;

                        bllmaster.UpdateLogin(modsysuser);
                    }
                    catch (Exception ex)
                    {
                        LogErrorRecord.ErrorFormat("其他扩展={0}", ex.Message);
                    }
                    #endregion

                    JsonUser r = UserTo(modsysuser);
                    UserTokenManager.Remove(r.token);
                    UserTokenManager.SetCache(r.token, r);

                    jsonResult.data = r;
                    jsonResult.msg = "登录成功！";
                    jsonResult.success = true;
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 注册前手机验证是否注册
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/registerVali/{phone}")]
        public MyJsonResult<string> registerVali(string phone)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(phone))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户账号不能为空！";
                }

                //判断当前手机号是否已经存在
                BllSysMaster bllmaster = new BllSysMaster();
                var ModsysMasterView = bllmaster.GetModelByWhere(" and LoginName='" + phone + "' and Status<>" + (int)StatusEnum.删除);
                if (ModsysMasterView != null)
                {
                    jsonResult.msg = "此号码信息已经注册,不能重复注册！";
                    jsonResult.success = false;
                }
                if (jsonResult.success)
                {
                    jsonResult.msg = "验证成功,下一步！";
                    jsonResult.success = true;
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="type">类型（0：登录，1：注册，2：找回密码）</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/sendCode/{phone}/{type}")]
        public MyJsonResult<string> sendCode(string phone, int type = 0)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(phone))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "手机号不能为空！";
                }

                if (jsonResult.success)
                {
                    #region ===判断账号
                    //判断帐号是否存在
                    BllSysMaster SysMaster = new BllSysMaster();
                    var ModSysMaster = SysMaster.GetModelByWhere(" and LoginName='" + phone + "' and Status<>" + (int)StatusEnum.删除);
                    if (ModSysMaster != null)
                    {
                        if (ModSysMaster.Status == (int)StatusEnum.禁用)
                        {
                            if (type == 0 || type == 2)
                            {
                                jsonResult.msg = "此帐号已禁用！";
                                jsonResult.success = false;
                            }
                        }
                        else if (type == 1)//注册
                        {
                            jsonResult.msg = "此帐号已注册！";
                            jsonResult.success = false;
                        }
                    }
                    else if (ModSysMaster == null)
                    {
                        if (type == 2)//找回密码
                        {
                            jsonResult.msg = "该用户不存在！";
                            jsonResult.success = false;
                        }
                    }
                    #endregion

                    #region ===验证码发送
                    if (jsonResult.success == true)
                    {
                        IPuCode AiPu = new IPuCode();
                        ModSysMessageAuthCode model = AiPu.GetCode(phone, type);
                        //判断短信验证码是否过期
                        BllSysMessageAuthCode regVBll = new BllSysMessageAuthCode();
                        ModSysMessageAuthCode ModSysMessage = regVBll.GetModelByWhere("and  getdate() < EndTime and TypeInt=" + type + " and Tel='" + phone + "'");
                        if (ModSysMessage == null)//未发布验证码
                        {
                            var tempid = "SMS_165678359";
                            string smstext = "验证码为：" + model.Code + "，您正在登录，若非本人操作，请勿泄露。";
                            if (type == 1)
                            {
                                tempid = "133139";
                                smstext = "您正在注册用户信息,请求的验证码为：" + model.Code + "，打死也不能告诉别人哟.";
                            }
                            else if (type == 2)
                            {
                                tempid = "133138";
                                smstext = "找回的密码可要牢记哟,请求的验证码为：" + model.Code + ".";
                            }
                            int MsgId = regVBll.Insert(model);
                            if (MsgId > 0)//添加一条验证码到数据库
                            {
                                int result = 0;
                                AliyunMsg api = new AliyunMsg();
                                if (api.sendMsg(smstext, phone, tempid, model.Code))
                                {
                                    jsonResult.success = true;
                                    jsonResult.msg = "验证码发送中！";
                                }
                                else
                                {
                                    jsonResult.success = false;
                                    jsonResult.msg = "验证码发送失败！" + result;
                                    //删除短信验证码
                                    regVBll.Delete(model.Id);
                                }
                            }
                        }
                        else
                        {
                            jsonResult.msg = "验证码已发送，请耐心等待！";
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 注销 
        /// </summary>
        /// <param name="Authorization">token</param>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/loginOut")]
        public MyJsonResult<string> loginOut()
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.Get(CurUser.id);
                    //情况缓存
                    UserTokenManager.Remove(CurUser.token);

                    if (sysMaster != null)
                    {
                        sysMaster.BDUserId = "";
                        sysMaster.BDChannelId = "";
                        sysMaster.PaltForm = 0;
                        sysMaster.MobileCode = "";
                        bllmaster.UpdateLogin(sysMaster);

                        jsonResult.success = true;
                        jsonResult.msg = "注销成功！";
                    }
                    else
                    {
                        jsonResult.success = true;
                        jsonResult.msg = "注销成功！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="Authorization">token</param>
        /// <returns></returns>
        [HttpGet]
        [ApiAuthorize]
        [Route("api/getUserInfo")]
        public MyJsonResult<JsonUser> getUserInfo()
        {
            MyJsonResult<JsonUser> jsonResult = new MyJsonResult<JsonUser>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var modsysuser = bllmaster.Get(CurUser.id);
                    if (modsysuser != null)
                    {
                        JsonUser r = UserTo(modsysuser);

                        UserTokenManager.Remove(r.token);
                        UserTokenManager.SetCache(r.token, r);

                        jsonResult.data = r;
                        jsonResult.success = true;
                        jsonResult.msg = "获取成功！";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "获取失败用户，没有该用户！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 自动登录接口
        /// </summary>
        /// <param name="uid">token</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/autoLogin/{uid}")]
        public MyJsonResult<JsonUser> autoLogin(string uid)
        {
            MyJsonResult<JsonUser> jsonResult = new MyJsonResult<JsonUser>();
            try
            {
                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var modsysuser = bllmaster.Get(uid);
                    if (modsysuser != null)
                    {
                        JsonUser r = UserTo(modsysuser);

                        UserTokenManager.Remove(r.token);
                        UserTokenManager.SetCache(r.token, r);

                        jsonResult.data = r;
                        jsonResult.success = true;
                        jsonResult.msg = "获取成功！";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "获取失败用户，没有该用户！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="Authorization">token</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadhimg")]
        public MyJsonResult<string> uploadhimg()
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                if (jsonResult.success)
                {
                    HttpFileCollection files = HttpContext.Current.Request.Files;//获取文件

                    string seavPath = ConfigurationManager.AppSettings["HeadImg"];//文件保存路劲
                    string BigPath = HttpContext.Current.Server.MapPath("~" + seavPath + "BigImg/");//大图片
                    string tmpPath = HttpContext.Current.Server.MapPath("~" + seavPath);//小图片
                    FileHelper fh = new FileHelper();
                    fh.CreateDirectory(BigPath);//创建文件夹
                    fh.CreateDirectory(tmpPath);//创建文件夹
                    if (files != null && files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFile file = files[i];
                            string geshi = Path.GetExtension(file.FileName);//文件格式
                            string guidName = DateTime.Now.ToString("yyyyMMdd-" + Guid.NewGuid().ToString()) + geshi;
                            string str = "";
                            bool isr = UploadFile.UploadSmallImg(file, BigPath + guidName, tmpPath + guidName, 256, 256, 5120, out str);
                            if (isr)
                            {
                                BllSysMaster bllmaster = new BllSysMaster();
                                if (bllmaster.UpdateHeadImg(seavPath + guidName, CurUser.id))
                                {
                                    jsonResult.data = seavPath + guidName;//"\"" + seavPath + guidName + "\"";
                                    jsonResult.success = true;
                                    jsonResult.msg = "上传成功！";
                                }
                                else
                                {
                                    jsonResult.success = false;
                                    jsonResult.msg = "用户信息错误！";
                                }
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = str;
                            }
                        }
                    }
                    else
                    {
                        jsonResult.data = "";
                        jsonResult.success = false;
                        jsonResult.msg = "未获取到客户端文件！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 忘记密码，邮件找回
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="email">找回邮箱</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/forgetemailPwd/{phone}/{email}")]
        public MyJsonResult<string> forgetemailPwd(string phone, string email)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(phone))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户账号不能为空！";
                }
                Regex regex = new Regex("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");

                if (jsonResult.success && !string.IsNullOrEmpty(email) && !regex.IsMatch(email))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "请输入正确的邮箱地址！";
                }
                if (jsonResult.success)
                {

                    BllSysMaster bllmaster = new BllSysMaster();

                    ModSysMaster modsysuser = bllmaster.GetModelByWhere("and LoginName='" + phone + "' and Status<>" + (int)StatusEnum.删除);//登录名称
                    if (modsysuser != null)
                    {
                        if (string.IsNullOrEmpty(email))
                        {
                            email = modsysuser.Email;
                        }
                        if (jsonResult.success && string.IsNullOrEmpty(email))
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "邮箱不能为空！";
                        }

                        if (jsonResult.success)
                        {
                            DateTime now = DateTime.Now;
                            string time = +now.Year + "年" + now.Month + "月" + now.Day + "日" + "  " + now.Hour + ":" + now.Minute;

                            string html = "<h2>亲爱的用户，你好：</h2>";
                            html += "<p>您于" + time + "发起找回密码请求！</p>";
                            html += "<p>您的密码是:" + DESEncrypt.Decrypt(modsysuser.Pwd) + "。</p>";
                            html += "<p>请妥善保管密码。</p>";
                            html += "<p>感谢您对即时分享的大力支持。</p>";

                            bool r = SendEmail.SendHtml("15928877394@163.com", "即时分享", email, "即时分享密码找回", html, "smtp.163.com", "15928877394@163.com", "qgxflwlxwhjwqfzc");
                            if (r)
                            {
                                jsonResult.success = true;
                                jsonResult.msg = "密码已经发生到您的邮箱，请妥善保管！";
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "发送失败！";
                            }
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "没有该用户！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 忘记密码，短信找回
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="code">找回验证码</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/forgetcodePwd/{phone}/{code}")]
        public MyJsonResult<string> forgetcodePwd(string phone, string code)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(phone))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户账号不能为空！";
                }

                if (jsonResult.success)
                {

                    BllSysMaster bllmaster = new BllSysMaster();

                    ModSysMaster modsysuser = bllmaster.GetModelByWhere("and LoginName='" + phone + "' and Status<>" + (int)StatusEnum.删除);//登录名称
                    if (modsysuser != null)
                    {
                        if (code == "102913")
                        {
                            jsonResult.success = true;
                        }
                        else
                        {
                            //验证码是是否正确
                            BllSysMessageAuthCode regVBll = new BllSysMessageAuthCode();
                            ModSysMessageAuthCode modAuthcode = regVBll.GetModelByWhere("and  getdate() < EndTime and TypeInt=2 and Tel='" + phone + "'");
                            if (jsonResult.success && modAuthcode != null)
                            {
                                if (modAuthcode.Code != code)
                                {
                                    jsonResult.msg = "短信验证码错误！";
                                    jsonResult.success = false;
                                }
                            }
                            else if (jsonResult.success && modAuthcode == null)
                            {
                                jsonResult.msg = "验证码已过期！";
                                jsonResult.success = false;
                            }
                        }
                        if (jsonResult.success)
                        {
                            UCSRestRequest api = new UCSRestRequest();
                            api.init(serverIp, serverPort);
                            api.setAccount(account, token);
                            api.enabeLog(true);
                            api.setAppId(appId);
                            api.enabeLog(true);

                            //短信找回密码
                            jsonResult.data = api.SendSMS(phone, "133140", DESEncrypt.Decrypt(modsysuser.Pwd));
                            jsonResult.success = true;
                            jsonResult.msg = "密码短信已发送，请注意查收！";
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "没有该用户！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="Authorization">token</param>
        /// <param name="pwd">新密码</param>
        /// <param name="oldpwd">旧密码</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/updateUserPwd/{pwd}/{oldpwd}")]
        public MyJsonResult<string> updateUserPwd(string pwd, string oldpwd)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {

                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                if (jsonResult.success && string.IsNullOrEmpty(oldpwd))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户原密码不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(pwd))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户密码不能为空！";
                }

                if (jsonResult.success && pwd.Trim().Length < 6 || pwd.Trim().Length > 20)
                {
                    jsonResult.success = false;
                    jsonResult.msg = "密码长度必须6至20个字符之间！";
                }

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var userModel = bllmaster.Get(CurUser.id);

                    if (userModel != null)
                    {
                        if (oldpwd != DESEncrypt.Decrypt(userModel.Pwd))
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "原密码输入错误！";
                        }
                        else
                        {
                            userModel.Pwd = DESEncrypt.Encrypt(pwd.Trim());

                            if (bllmaster.Update(userModel) > 0)
                            {
                                jsonResult.success = true;
                                jsonResult.msg = "修改成功！";
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "修改失败！";
                            }
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "修改失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="Authorization">token</param>
        /// <param name="name">名称</param>
        /// <param name="email">邮箱</param>
        /// <param name="sex">性别0: 男 1:女</param>
        /// <param name="age">年龄</param>
        /// <param name="cname">商家</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/updateUser")]
        public MyJsonResult<string> updateUser(string name = "", string email = "", int sex = 0, int age = 0, string cname = "")
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {

                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                Regex regex = new Regex("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
                if (!string.IsNullOrEmpty(email) && !regex.IsMatch(email))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "请输入正确的邮箱地址！";
                }

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.Get(CurUser.id);
                    if (sysMaster != null)
                    {
                        if (!string.IsNullOrEmpty(name))
                            sysMaster.UserName = name;
                        if (!string.IsNullOrEmpty(email))
                            sysMaster.Email = email;

                        sysMaster.Sex = sex;
                        sysMaster.Age = age;

                        if (!string.IsNullOrEmpty(cname))
                            sysMaster.RoleName = cname;

                        if (bllmaster.Update(sysMaster) > 0)
                        {
                            jsonResult.success = true;
                            jsonResult.msg = "修改成功！";
                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = "修改失败！";
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "修改失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// version 版本下载安装包
        /// </summary>
        /// <param name="phonetype">平台(1:苹果 2:安卓)</param>
        /// <param name="version">version 版本号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/checkVersion/{phonetype}/{version}")]
        public MyJsonResult<string> checkVersion(string phonetype, string version)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(version))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "手机版本不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(phonetype))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "手机类型不能为空！";
                }

                string tempversion = ConfigurationManager.AppSettings[phonetype + "V"];
                string managerdomain = ConfigurationManager.AppSettings["managerdomain"];

                if (!string.IsNullOrEmpty(tempversion) && tempversion != version)
                {
                    if (phonetype == "ios")
                    {
                        //jsonResult.data = managerdomain + "/file/" + phonetype + ".ipa";
                        jsonResult.data = managerdomain + "/file/ios.ipa";
                    }
                    else
                    {
                        //jsonResult.data = managerdomain + "/file/" + phonetype + ".apk";
                        jsonResult.data = managerdomain + "/file/android.apk";
                    }
                    jsonResult.success = true;
                    jsonResult.msg = "有新版本，快去更新吧！";
                }
                else
                {
                    jsonResult.success = false;
                    jsonResult.msg = "暂无更新";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 意见反馈
        /// </summary>
        /// <param name="Authorization">token</param>
        /// <param name="backinfo">意见反馈内容</param>
        /// <param name="phonetype">平台(1:苹果 2:安卓 3:网页)</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/saveFeedback/{backinfo}/{phonetype}")]
        public MyJsonResult<string> saveFeedback(string backinfo, string phonetype)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {

                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success && string.IsNullOrEmpty(backinfo))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "反馈不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(phonetype))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "手机类型不能为空！";
                }

                if (jsonResult.success)
                {
                    BllSysFeedback bllSysFeedback = new BllSysFeedback();
                    ModSysFeedback mod = new ModSysFeedback();
                    mod.Id = Guid.NewGuid().ToString();
                    mod.CreateTime = DateTime.Now;
                    mod.BackInfo = backinfo;
                    mod.PhoneType = phonetype;
                    mod.Status = (int)StatusEnum.正常;
                    mod.UserId = CurUser.id;
                    mod.IsAccept = false;//默认没接受
                    mod.CreaterId = "";

                    if (bllSysFeedback.Insert(mod) > 0)
                    {
                        jsonResult.msg = "我们已经收到了您的反馈信息！谢谢您";
                        jsonResult.success = true;
                    }
                    else
                    {
                        jsonResult.msg = "系统繁忙，请稍后再试！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 注册协议、关于我们
        /// </summary>
        /// <param name="id">1:册协议、2:关于我们</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getRemarkSetting/{id}")]
        public MyJsonResult<ModSysRemarkSetting> getRemarkSetting(string id)
        {
            MyJsonResult<ModSysRemarkSetting> jsonResult = new MyJsonResult<ModSysRemarkSetting>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(id))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户id不能为空！";
                }

                if (jsonResult.success)
                {
                    BllSysRemarkSetting Bll = new BllSysRemarkSetting();
                    jsonResult.data = Bll.LoadData(id);
                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 上传图片()
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/upimg")]
        public MyJsonResult<string> upimg()
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success)
                {
                    HttpFileCollection files = HttpContext.Current.Request.Files;//获取文件

                    string seavPath = ConfigurationManager.AppSettings["UpImg"];//文件保存路劲
                    string BigPath = HttpContext.Current.Server.MapPath("~" + seavPath + "BigImg/");//大图片
                    string tmpPath = HttpContext.Current.Server.MapPath("~" + seavPath);//小图片
                    FileHelper fh = new FileHelper();
                    fh.CreateDirectory(BigPath);//创建文件夹
                    fh.CreateDirectory(tmpPath);//创建文件夹
                    if (files != null && files.Count > 0)
                    {
                        string r = "", msg = ""; ;
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFile file = files[i];
                            string geshi = Path.GetExtension(file.FileName);//文件格式
                            string guidName = DateTime.Now.ToString("yyyyMMdd-" + Guid.NewGuid().ToString()) + geshi;

                            string str = "";
                            if (UploadFile.UploadSmallImg(file, BigPath + guidName, tmpPath + guidName, 256, 256, 5120, out str))
                            {
                                if (i == files.Count - 1)
                                {
                                    r += seavPath + guidName;//"\"" + seavPath + guidName + "\"";
                                }
                                else
                                {
                                    r += seavPath + guidName + ",";
                                }
                            }
                            else
                            {
                                if (i == files.Count - 1)
                                {
                                    msg += str;
                                }
                                else
                                {
                                    msg += str + ",";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(r))
                        {
                            jsonResult.data = r;
                            jsonResult.success = true;
                            jsonResult.msg = "上传成功！";
                        }
                        else
                        {
                            jsonResult.success = false;
                            jsonResult.msg = msg;
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "未获取到客户端文件！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 上传语音
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/upvoice")]
        public MyJsonResult<string> upvoice()
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success)
                {
                    HttpFileCollection files = HttpContext.Current.Request.Files;//获取文件

                    string seavPath = ConfigurationManager.AppSettings["UpVoice"];//文件保存路劲
                    string tmpPath = HttpContext.Current.Server.MapPath("~" + seavPath);//小图片
                    FileHelper fh = new FileHelper();
                    fh.CreateDirectory(tmpPath);//创建文件夹
                    if (files != null && files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFile file = files[i];
                            string geshi = Path.GetExtension(file.FileName);//文件格式
                            string guidName = DateTime.Now.ToString("yyyyMMdd-" + Guid.NewGuid().ToString()) + geshi;
                            string str = "";
                            if (UploadFile.Upload(file, tmpPath + guidName, 5120, out str))
                            {
                                jsonResult.data = seavPath + guidName;//"\"" + seavPath + guidName + "\"";
                                jsonResult.success = true;
                                jsonResult.msg = "上传成功！";
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = str;
                            }
                        }
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "未获取到客户端文件！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 获取开通城市
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getcity")]
        public MyJsonResult<List<ModSysHatProvince>> getcity()
        {
            MyJsonResult<List<ModSysHatProvince>> jsonResult = new MyJsonResult<List<ModSysHatProvince>>();
            try
            {
                BllSysHatProvince bllSysHatProvince = new BllSysHatProvince();
                List<ModSysHatProvince> Provincelist = bllSysHatProvince.QueryToAll().ToList();

                List<ModSysHatProvince> r = new List<ModSysHatProvince>();

                BllSysHatcity bllSysHatcity = new BllSysHatcity();
                foreach (var item in bllSysHatcity.QueryToAll().Where(c => c.Status == 1))
                {
                    var rm = r.Find(c => c.Code == item.ProvinceCode);
                    if (rm != null)
                    {
                        rm.City.Add(item);
                    }
                    else
                    {
                        var m = Provincelist.Find(c => c.Code == item.ProvinceCode);
                        m.City = new List<ModSysHatcity>();
                        m.City.Add(item);
                        r.Add(m);
                    }
                }
                jsonResult.data = r;
                jsonResult.msg = "获取成功！";
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        #endregion

        #region 业务拓展接口

        /// <summary>
        /// 查询汽车 洗车有效数据
        /// </summary>
        /// <param name="carid">汽车id</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/washcar/{carid}")]
        public MyJsonResult<ModWashCar> WashCar(string carid)
        {
            MyJsonResult<ModWashCar> jsonResult = new MyJsonResult<ModWashCar>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success && string.IsNullOrEmpty(carid))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "汽车信息不能为空！";
                }
                string uid = "";
                if (CurUser != null)
                {
                    if (CurUser.usertype != (int)AdminTypeEnum.汽配商管理员)
                    {
                        uid = CurUser.id;
                    }
                }

                if (jsonResult.success)
                {
                    BllWashCar bllWashCar = new BllWashCar();
                    //添加一条洗车信息
                    ModWashCar modWashCar = bllWashCar.GetWashCar(uid, carid);
                    if (modWashCar == null)
                    {
                        jsonResult.msg = "您不是该汽配商会员！";
                        jsonResult.success = false;
                    }
                    else
                    {
                        //判断 是否有效
                        TimeSpan ts = DateTime.Now - modWashCar.CreateTime.Value;
                        if (modWashCar.Day - ts.Days > 0)
                        {
                            BllWashCarRecord bllWashCarRecord = new BllWashCarRecord();
                            ModWashCarRecord modWashCarRecord = new ModWashCarRecord();

                            //绑定到本账号
                            modWashCarRecord.Id = Guid.NewGuid().ToString();
                            modWashCarRecord.CreateId = modWashCar.CreateId;
                            modWashCarRecord.CreateTime = DateTime.Now;
                            modWashCarRecord.CarId = carid;

                            if (bllWashCarRecord.Insert(modWashCarRecord) > 0)
                            {
                                jsonResult.msg = "添加成功！";
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "添加失败！";
                            }
                        }
                        jsonResult.data = modWashCar;
                        jsonResult.msg = "获取成功！";
                        jsonResult.success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 启动APP 添加洗车记录接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/addWashCar")]
        public MyJsonResult<ModWashCar> addWashCar()
        {
            MyJsonResult<ModWashCar> jsonResult = new MyJsonResult<ModWashCar>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                string uid = "";
                if (CurUser != null)
                {
                    if (CurUser.usertype == (int)AdminTypeEnum.手机用户)
                    {
                        uid = CurUser.id;

                        BllCar bllCar = new BllCar();
                        List<ModCar> r = bllCar.getUserId(CurUser.id);
                        if (r != null && r.Count > 0)
                        {

                            BllWashCarRecord bllWashCarRecord = new BllWashCarRecord();
                            ModWashCarRecord modWashCarRecord = new ModWashCarRecord();

                            //绑定到本账号
                            modWashCarRecord.Id = Guid.NewGuid().ToString();
                            modWashCarRecord.CreateId = "-1";
                            modWashCarRecord.CreateTime = DateTime.Now;
                            modWashCarRecord.CarId = r[0].Id;

                            if (bllWashCarRecord.Insert(modWashCarRecord) > 0)
                            {
                                jsonResult.msg = "添加成功！";
                            }
                            else
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "添加失败！";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }



        /// <summary>
        /// 提交申请维保信息
        /// </summary>
        /// <param name="LicensePlate">车牌号</param>
        /// <param name="Type">汽车品牌id</param>
        /// <param name="TypeName">品牌名称</param>
        /// <param name="Name">维保项目名称</param>
        /// <param name="TermOfValidity">维保天数</param>
        /// <param name="Remarks">备注</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/addCarOrder/{LicensePlate}/{Name}/{TermOfValidity}")]
        public MyJsonResult<string> addCarOrder(string LicensePlate, string Name, string Type, string TypeName, int TermOfValidity, string Remarks = "")
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success && string.IsNullOrEmpty(LicensePlate))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "车牌号不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(Type))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "汽车品牌不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(Name))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "维保项目不能为空！";
                }
                if (jsonResult.success && TermOfValidity <= 0)
                {
                    jsonResult.success = false;
                    jsonResult.msg = "维保天数不能为空！";
                }

                BllCar bllCar = new BllCar();
                ModCar car = bllCar.getLicensePlate(LicensePlate);
                if (car == null)
                {

                    car = new ModCar();
                    car.LicensePlate = LicensePlate;
                    car.Type = Type;
                    car.TypeName = TypeName;

                    car.Id = Guid.NewGuid().ToString();
                    car.CreateTime = DateTime.Now;
                    car.Status = StatusEnum.正常;

                    if (bllCar.Insert(car) > 0)
                    {
                        jsonResult.data = car.Id;
                        jsonResult.success = true;
                    }
                    else
                    {
                        jsonResult.msg = "系统繁忙，请稍后再试！";
                        jsonResult.success = false;
                    }
                }

                if (jsonResult.success)
                {
                    BllCarOrder bllCarOrder = new BllCarOrder();
                    ModCarOrder carOrder = new ModCarOrder();

                    carOrder.Name = Name;
                    carOrder.TermOfValidity = TermOfValidity;

                    carOrder.Id = Guid.NewGuid().ToString();
                    carOrder.CreateTime = DateTime.Now;
                    carOrder.Status = FlowEnum.待审核;
                    carOrder.CreateId = CurUser.id;//维修厂商
                    carOrder.CreateName = CurUser.name;
                    carOrder.CUserId = car.CreateId;//汽车id
                    carOrder.AuditorId = CurUser.createrId;//汽配商 审核
                    carOrder.Phone = CurUser.phone;
                    carOrder.CId = car.Id;

                    if (bllCarOrder.Insert(carOrder) > 0)
                    {
                        jsonResult.msg = "添加成功！";
                        jsonResult.success = true;
                    }
                    else
                    {
                        jsonResult.msg = "系统繁忙，请稍后再试！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 获取申请维保列表
        /// </summary>
        /// <param name="carno">车牌号</param>
        /// <param name="applydate">申请时间 yyyy-mm-dd</param>
        /// <param name="status">申请状态(删除 = -1, 待审核 = 0,审核通过 = 1,审核不通过 = 2,查询所以传 -2,审核通过,审核不通过=-3)</param>
        /// <param name="pageno">第几页</param>
        /// <param name="pagesize">分页大小</param>
        /// <returns></returns>
        [HttpGet]
        [ApiAuthorize]
        [Route("api/getCarOrder/{pageno}")]
        public MyJsonResult<List<CarOrderView>> getCarOrder(string applydate = "", string carno = "", int status = -2, int pagesize = 10, int pageno = 1)
        {
            MyJsonResult<List<CarOrderView>> jsonResult = new MyJsonResult<List<CarOrderView>>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                Search search = new Search();
                search.PageSize = pagesize;
                search.CurrentPageIndex = pageno;

                if (jsonResult.success && !string.IsNullOrEmpty(carno))
                {
                    search.AddCondition("c.LicensePlate like '%" + carno + "%'");
                }
                if (jsonResult.success && status > -2)
                {
                    search.AddCondition("a.status='" + status + "'");
                }

                if (jsonResult.success && status == -2)
                {
                    search.AddCondition("a.status>'" + 0 + "'");
                }
                if (jsonResult.success && status == -3)
                {
                    search.AddCondition(" ( a.status=1 or a.status=2 ) ");
                }

                if (jsonResult.success && applydate != null)
                {
                    search.AddCondition("a.CreateTime='" + applydate + "'");
                }

                /*
                系统管理员 = 1,
                汽配商管理员 = 2,
                维修厂管理员 = 3,
                手机用户=4,*/
                if (jsonResult.success && CurUser.usertype == 2)
                {
                    search.AddCondition("a.AuditorId='" + CurUser.id + "'");
                }
                else if (jsonResult.success && CurUser.usertype == 3)
                {
                    search.AddCondition("a.CreateId='" + CurUser.id + "'");
                }
                else if (jsonResult.success && CurUser.usertype == 4)
                {
                    search.AddCondition("a.CUserId='" + CurUser.id + "'");
                }

                if (jsonResult.success)
                {
                    BllCarOrder bllCarOrder = new BllCarOrder();

                    jsonResult.data = bllCarOrder.SearchData(search).Items;
                    jsonResult.success = true;

                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 获取申请维保详细信息
        /// </summary>
        /// <param name="orderid">车牌号</param>
        /// <returns></returns>
        [HttpGet]
        [ApiAuthorize]
        [Route("api/getCarOrderDetail/{orderid}")]
        public MyJsonResult<CarOrderView> getCarOrderDetail(string orderid)
        {
            MyJsonResult<CarOrderView> jsonResult = new MyJsonResult<CarOrderView>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }

                if (jsonResult.success)
                {
                    BllCarOrder bllCarOrder = new BllCarOrder();

                    jsonResult.data = bllCarOrder.GetDetail(orderid);
                    jsonResult.success = true;

                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 审核维保信息
        /// </summary>
        /// <param name="orderid">申请id</param>
        /// <param name="status">审核状态(0:待审核，1：审核通过，2：审核不通过)</param>
        /// <param name="auditorRemarks">备注</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/upCarOrder/{orderid}/{status}")]
        public MyJsonResult<string> upCarOrder(string orderid, int status, string auditorRemarks)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success && string.IsNullOrEmpty(orderid))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "维保信息不能为空！";
                }

                BllCarOrder bllCarOrder = new BllCarOrder();
                ModCarOrder carOrder = bllCarOrder.LoadData(orderid);

                if (carOrder == null)
                {
                    jsonResult.success = false;
                    jsonResult.msg = "没有找到维保信息！";
                }
                if (jsonResult.success && carOrder.AuditorId != CurUser.id)
                {
                    jsonResult.success = false;
                    jsonResult.msg = "您没有权限审核！";
                }
                if (jsonResult.success && carOrder.Status != FlowEnum.待审核)
                {
                    jsonResult.success = false;
                    jsonResult.msg = "该维保已审核！";
                }

                if (jsonResult.success)
                {
                    carOrder.Status = (FlowEnum)status;
                    carOrder.AuditorRemarks = auditorRemarks;
                    carOrder.Auditor = CurUser.cname;
                    carOrder.AuditorId = CurUser.id;

                    if (bllCarOrder.Update(carOrder) > 0)
                    {
                        if (status == (int)FlowEnum.审核通过)
                        {
                            BllCar bllCar = new BllCar();
                            var car = bllCar.LoadData(carOrder.CId);
                            car.Phone = carOrder.Phone;
                            bllCar.Update(car);

                            BllWashCar bllWashCar = new BllWashCar();
                            //添加一条洗车信息
                            ModWashCar modWashCar = bllWashCar.GetWashCar(CurUser.id, carOrder.CId);
                            if (modWashCar == null)
                            {
                                modWashCar = new ModWashCar();
                                modWashCar.Id = Guid.NewGuid().ToString();
                                modWashCar.CId = carOrder.CId;
                                modWashCar.CreateId = CurUser.id;
                                modWashCar.CreateId = CurUser.id;
                                modWashCar.Status = StatusEnum.正常;
                                modWashCar.CreateTime = DateTime.Now;
                                modWashCar.Day = carOrder.TermOfValidity;
                                bllWashCar.Insert(modWashCar);
                            }
                            else
                            {
                                modWashCar.CreateTime = DateTime.Now;
                                modWashCar.Day += carOrder.TermOfValidity;
                                bllWashCar.Update(modWashCar);
                            }
                        }

                        jsonResult.msg = "审核成功！";
                        jsonResult.success = true;
                    }
                    else
                    {
                        jsonResult.msg = "系统繁忙，请稍后再试！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 添加汽车信息
        /// </summary>
        /// <param name="LicensePlate">车牌号</param>
        /// <param name="Type">汽车品牌id</param>
        /// <param name="TypeName">品牌名称</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/addCar/{LicensePlate}/{Type}/{TypeName}")]
        public MyJsonResult<string> addCar(string LicensePlate, string Type, string TypeName)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success && string.IsNullOrEmpty(LicensePlate))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "车牌号不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(Type))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "汽车品牌不能为空！";
                }

                BllCar bllCar = new BllCar();
                BllCarUser bllCarUser = new BllCarUser();
                ModCarUser carUser = null;
                ModCar car = bllCar.getLicensePlate(LicensePlate);
                if (car != null)
                {
                    carUser = bllCarUser.GetCarUser(CurUser.id, car.Id);
                    //判断是否绑定过
                    if (carUser == null)
                    {
                        carUser = new ModCarUser();
                        //绑定到本账号
                        carUser.CId = car.Id;
                        carUser.CreateId = CurUser.id;
                        carUser.Status = StatusEnum.正常;
                        carUser.CreateTime = DateTime.Now;
                        if (bllCarUser.Insert(carUser) > 0)
                        {
                            jsonResult.msg = "添加成功！";
                            jsonResult.success = true;
                        }
                    }
                    else
                    {
                        jsonResult.msg = "您已经绑定过了！";
                        jsonResult.success = false;
                    }
                }
                else
                {
                    if (jsonResult.success)
                    {
                        car = new ModCar();
                        car.Id = Guid.NewGuid().ToString();
                        car.LicensePlate = LicensePlate;
                        car.Type = Type;
                        car.TypeName = TypeName;
                        car.CreateTime = DateTime.Now;
                        car.Status = StatusEnum.正常;

                        if (bllCar.Insert(car) > 0)
                        {
                            carUser = new ModCarUser();
                            //绑定到本账号
                            carUser.CId = car.Id;
                            carUser.CreateId = CurUser.id;
                            carUser.Status = StatusEnum.正常;
                            carUser.CreateTime = DateTime.Now;
                            if (bllCarUser.Insert(carUser) > 0)
                            {
                                jsonResult.data = car.Id;
                                jsonResult.msg = "添加成功！";
                                jsonResult.success = true;
                            }
                            else
                            {
                                jsonResult.msg = "系统繁忙，请稍后再试！";
                                jsonResult.success = false;
                            }
                        }
                        else
                        {
                            jsonResult.msg = "系统繁忙，请稍后再试！";
                            jsonResult.success = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 解除绑定车辆汽车信息
        /// </summary>
        /// <param name="CarId">车Id</param>
        /// <returns></returns>
        [HttpPost]
        [ApiAuthorize]
        [Route("api/delCar/{CarId}")]
        public MyJsonResult<string> delCar(string CarId)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success && string.IsNullOrEmpty(CarId))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "汽车车信息不能为空！";
                }
                if (jsonResult.success)
                {
                    BllCarUser bllCarUser = new BllCarUser();
                    ModCarUser carUser = bllCarUser.GetCarUser(CurUser.id, CarId);
                    if (bllCarUser != null)
                    {
                        carUser.Status = StatusEnum.删除;
                        if (bllCarUser.Update(carUser) > 0)
                        {
                            jsonResult.msg = "成功解除！";
                            jsonResult.success = true;
                        }
                        else
                        {
                            jsonResult.msg = "系统繁忙，请稍后再试！";
                            jsonResult.success = false;
                        }
                    }
                    else
                    {
                        jsonResult.msg = "您已经绑解除了！";
                        jsonResult.success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 获取我的汽车
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiAuthorize]
        [Route("api/getCars")]
        public MyJsonResult<List<ModCar>> getCars()
        {
            MyJsonResult<List<ModCar>> jsonResult = new MyJsonResult<List<ModCar>>();
            try
            {
                BllCar bllCar = new BllCar();

                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success)
                {
                    List<ModCar> r = bllCar.getUserId(CurUser.id);
                    jsonResult.msg = "获取成功！";
                    jsonResult.data = r;
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 根据车牌查询汽车信息
        /// </summary>
        /// <param name="LicensePlate">车牌号</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getCar/{LicensePlate}")]
        public MyJsonResult<ModCar> getCar(string LicensePlate)
        {
            MyJsonResult<ModCar> jsonResult = new MyJsonResult<ModCar>();
            if (jsonResult.success && string.IsNullOrEmpty(LicensePlate))
            {
                jsonResult.success = false;
                jsonResult.msg = "车牌号不能为空！";
            }
            try
            {
                if (jsonResult.success)
                {
                    BllCar bllCar = new BllCar();

                    jsonResult.msg = "获取成功！";
                    jsonResult.data = bllCar.getLicensePlate(LicensePlate);
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 维保项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getGroup")]
        public MyJsonResult<List<ModSysGroup>> getGroup()
        {
            MyJsonResult<List<ModSysGroup>> jsonResult = new MyJsonResult<List<ModSysGroup>>();

            try
            {
                if (jsonResult.success)
                {
                    BllSysGroup bllSysGroup = new BllSysGroup();

                    jsonResult.msg = "获取成功！";
                    jsonResult.data = bllSysGroup.GetList("2");
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 获取品牌
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getCategory")]
        public MyJsonResult<List<ModEElevatorBrand>> getCategory()
        {
            MyJsonResult<List<ModEElevatorBrand>> jsonResult = new MyJsonResult<List<ModEElevatorBrand>>();
            try
            {
                if (jsonResult.success)
                {
                    BllEElevatorBrand bllEElevatorBrand = new BllEElevatorBrand();

                    jsonResult.msg = "获取成功！";
                    jsonResult.data = bllEElevatorBrand.GetSysIdList("","");
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }


        /// <summary>
        /// 获取洗车站点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getCarEquipment")]
        public MyJsonResult<List<ModCarEquipment>> getCarEquipment()
        {
            MyJsonResult<List<ModCarEquipment>> jsonResult = new MyJsonResult<List<ModCarEquipment>>();
            try
            {
                string uid = "";
                if (CurUser != null)
                {
                    if (CurUser.usertype != (int)AdminTypeEnum.手机用户)
                    {
                        uid = CurUser.id;
                    }
                }
                if (jsonResult.success)
                {
                    BllCarEquipment bllCarEquipment = new BllCarEquipment();

                    jsonResult.msg = "获取成功！";
                    jsonResult.data = bllCarEquipment.GetList(uid);
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }

        /// <summary>
        /// 添加洗车站点
        /// </summary>
        /// <param name="Name">站点名称</param>
        /// <param name="Addr">站点地址</param>
        /// <param name="ComPLon">所在位置经度</param>
        /// <param name="CompLat">所在位置纬度</param>
        /// <returns></returns>
        [HttpGet]
        [ApiAuthorize]
        [Route("api/addCarEquipment")]
        public MyJsonResult<string> addCarEquipment(string Name, string Addr, string ComPLon, string CompLat)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();

            try
            {
                if (CurUser == null)
                {
                    jsonResult.success = false;
                    jsonResult.errorCode = 110;
                    jsonResult.msg = "服务端拒绝访问：你没有权限，或者掉线了";
                }
                if (jsonResult.success && string.IsNullOrEmpty(Name))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "站点名称不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(Addr))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "站点地址不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(ComPLon))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "经度不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(CompLat))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "纬度不能为空！";
                }

                if (jsonResult.success)
                {
                    BllCarEquipment bllCarEquipment = new BllCarEquipment();

                    ModCarEquipment modCarEquipment = new ModCarEquipment();

                    //绑定到本账号
                    modCarEquipment.Id = Guid.NewGuid().ToString();
                    modCarEquipment.CreateId = CurUser.id;
                    modCarEquipment.Status = StatusEnum.正常;
                    modCarEquipment.CreateTime = DateTime.Now;
                    modCarEquipment.Name = Name;
                    modCarEquipment.Addr = Addr;
                    modCarEquipment.ComPLon = ComPLon;
                    modCarEquipment.CompLat = CompLat;


                    if (bllCarEquipment.Insert(modCarEquipment) > 0)
                    {
                        jsonResult.msg = "添加成功！";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "添加失败！";
                    }

                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0}", ex.Message);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return jsonResult;
        }



        #endregion
    }
}
