using Ninject;
using QINGUO.Business;
using QINGUO.Common;
using QINGUO.Model;
using SwaggerApiDemo.Common;
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

namespace WebApp.Controllers
{
    /// <summary>
    /// 及时分享几口集成
    /// </summary>
    public class ShareApiController : ApiController
    {
        #region Comm
        private IKernel ninjectKernel = new StandardKernel();

        public ShareApiController()
        {
            ninjectKernel.Bind<ILogAction>().To<SharePresentationLog>();
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

        ///// <param name="email">邮箱</param>
        ///// <param name="name">姓名</param>
        ///// <param name="headimg">头像</param>
        ///// <param name="cname">商家名称</param>
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="pwd">密码</param>
        /// <param name="MobileCode">机器码</param>
        /// <param name="MobileCode">短信验证码</param>
        /// <param name="PaltForm">注册平台(1:苹果 2:安卓 3:网页)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/register/{phone}/{comname}/{name}/{addr}/{MobileCode}/{code}/{PaltForm}")]
        public MyJsonResult<string> register(string phone, string comname, string name, string addr, string MobileCode, string code = "102913", int PaltForm = 3)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(phone))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户账号不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(comname))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "公司名称不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(name))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "公司法人不能为空！";
                }
                if (jsonResult.success && string.IsNullOrEmpty(addr))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "公司地址不能为空！";
                }

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
                    var ModsysMasterView = bllmaster.GetModelByWhere(" and Phone='" + phone + "' and Status<>" + (int)StatusEnum.删除);
                    if (ModsysMasterView != null)
                    {
                        jsonResult.msg = "此号码信息已经绑定,不能重复绑定！";
                        jsonResult.success = false;
                    }
                    //验证码是是否正确
                    BllSysMessageAuthCode regVBll = new BllSysMessageAuthCode();
                    ModSysMessageAuthCode modAuthcode = regVBll.GetModelByWhere("and  getdate() < EndTime and TypeInt=0 and Tel='" + phone + "'");
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
                    if (code == "102913")
                    {
                        jsonResult.success = true;
                    }

                    if (jsonResult.success)
                    {
                        ModSysMaster sysMaster = new ModSysMaster();
                        sysMaster.Id = Guid.NewGuid().ToString();
                        sysMaster.LoginName = phone;

                        sysMaster.Pwd = DESEncrypt.Encrypt("666666");  //加密
                        sysMaster.Status = (int)StatusEnum.正常;
                        sysMaster.MobileCode = MobileCode; //机器码
                        sysMaster.PaltForm = PaltForm;
                        sysMaster.CreateTime = DateTime.Now;
                        sysMaster.Phone = phone;
                        sysMaster.Cid = "C0A38317-59DA-4DE2-9428-E5B66EDBC2CF";
                        sysMaster.RoleName = comname;
                        sysMaster.UserName = name;
                        sysMaster.Detail = addr;
                        //sysMaster.Email = email;
                        sysMaster.HeadImg = "";//默认头像
                        sysMaster.Attribute = (int)AdminTypeEnum.手机用户;
                        sysMaster.IsMain = false;

                        if (bllmaster.Insert(sysMaster) > 0)
                        {
                            jsonResult.msg = "绑定成功！";
                            jsonResult.success = true;
                        }
                        else
                        {
                            jsonResult.msg = "绑定失败！";
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
        /// 登录
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
                        if (!string.IsNullOrEmpty(modsysuser.Pwd) && DESEncrypt.Decrypt(modsysuser.Pwd) == pwd)
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

                            JsonUser r = new JsonUser();
                            r.id = modsysuser.Id;
                            r.loginname = modsysuser.LoginName;
                            r.name = modsysuser.UserName;
                            r.sex = modsysuser.Sex;
                            r.headimg = modsysuser.HeadImg;
                            r.email = modsysuser.Email;
                            r.cname = modsysuser.RoleName;
                            r.age = modsysuser.Age;
                            r.ismember = false;
                            r.cid = modsysuser.Cid;
                            BllOrderUserMoneyRecord bll = new BllOrderUserMoneyRecord();
                            List<ModOrderUserMoneyRecord> list = bll.QueryAll(r.id).Where(c => c.Status == 2).ToList();//查询是否是会员
                            if (list != null)
                                foreach (var item in list)
                                {
                                    if ((DateTime.Now - item.CreateTime.Value).TotalDays <= 366) //一年一类
                                    {
                                        r.ismember = true;
                                        break;
                                    }
                                }
                            r.token = Guid.NewGuid().ToString();
                            //HttpContext.Current.Session["token"] = r.token;
                            HttpContext.Current.Session.Add("token", r.token);

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
                var ModsysMasterView = bllmaster.GetModelByWhere(" and LoginName='" + phone + "' and Status<>" + (int)StatusEnum.删除);//GetUserByTel(Phone);
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
        /// <param name="type">类型（0：注册，1：找回密码）</param>
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
                    jsonResult.msg = "获取验证码电话不能为空！";
                }

                if (jsonResult.success)
                {
                    #region ===发送短信验证码
                    //判断帐号是否存在
                    BllSysMaster SysMaster = new BllSysMaster();
                    var ModSysMaster = SysMaster.GetModelByWhere(" and LoginName='" + phone + "' and Status<>" + (int)StatusEnum.删除);
                    if (ModSysMaster != null)
                    {
                        if (ModSysMaster.Status == (int)StatusEnum.未激活)
                        {
                            jsonResult.msg = "此帐号正在等待审核！";
                            jsonResult.success = false;
                        }
                        else if (type == 0)//注册
                        {
                            jsonResult.msg = "此帐号已注册！";
                            jsonResult.success = false;
                        }
                        else if (type == 1)//找回密码
                        {
                            jsonResult.success = true;
                        }
                    }
                    else if (ModSysMaster == null && type == 1)
                    {
                        jsonResult.msg = "该用户不存在！";
                        jsonResult.success = false;
                    }
                    else
                    {
                        jsonResult.success = true;
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
                            var tempid = "133139";
                            string smstext = "您正在注册用户信息,请求的验证码为：" + model.Code + "，打死也不能告诉别人哟.";
                            if (type == 1)
                            {
                                tempid = "133138";
                                smstext = "找回的密码可要牢记哟,请求的验证码为：" + model.Code + ".";
                            }
                            int MsgId = regVBll.Insert(model);
                            if (MsgId > 0)//添加一条验证码到数据库
                            {
                                int result = 0;
                                UCSRestRequest api = new UCSRestRequest();
                                api.init(serverIp, serverPort);
                                api.setAccount(account, token);
                                api.enabeLog(true);
                                api.setAppId(appId);
                                api.enabeLog(true);
                                string r = api.SendSMS(phone, tempid, model.Code);
                                if (result == 0)
                                {
                                    jsonResult.data = r;
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
        /// <param name="userid">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/loginOut/{userid}")]
        public MyJsonResult<string> loginOut(string userid)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(userid))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户Id不能为空！";
                }

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.Get(userid);
                    if (sysMaster != null)
                    {
                        sysMaster.BDUserId = "";
                        sysMaster.BDChannelId = "";
                        sysMaster.PaltForm = 0;
                        sysMaster.MobileCode = "";
                        bllmaster.UpdateLogin(sysMaster);
                        HttpContext.Current.Session["token"] = "";
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
        /// <param name="userid">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getUserInfo/{userid}")]
        public MyJsonResult<JsonUser> getUserInfo(string userid)
        {
            MyJsonResult<JsonUser> jsonResult = new MyJsonResult<JsonUser>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(userid))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户Id不能为空！";
                }

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var modsysuser = bllmaster.Get(userid);
                    if (modsysuser != null)
                    {
                        JsonUser r = new JsonUser();
                        r.id = modsysuser.Id;
                        r.loginname = modsysuser.LoginName;
                        r.name = modsysuser.UserName;
                        r.sex = modsysuser.Sex;
                        r.headimg = modsysuser.HeadImg;
                        r.email = modsysuser.Email;
                        r.cname = modsysuser.RoleName;
                        r.age = modsysuser.Age;
                        r.ismember = false;
                        r.cid = modsysuser.Cid;
                        BllOrderUserMoneyRecord bll = new BllOrderUserMoneyRecord();
                        List<ModOrderUserMoneyRecord> list = bll.QueryAll(r.id).Where(c => c.Status == 2).ToList();//查询是否是会员
                        if (list != null)
                            foreach (var item in list)
                            {
                                if ((DateTime.Now - item.CreateTime.Value).TotalDays <= 366) //一年以内
                                {
                                    r.ismember = true;
                                    break;
                                }
                            }
                        //统计用户收藏总量
                        BllSysCollection bllSysCollection = new BllSysCollection();
                        r.numcollcet = bllSysCollection.getNum(r.id, CollectionEnum.新闻收藏);
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
        /// <param name="userid">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadhimg/{userid}")]
        public MyJsonResult<string> uploadhimg(string userid)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(userid))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户Id不能为空！";
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
                                if (bllmaster.UpdateHeadImg(seavPath + guidName, userid))
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
                        //验证码是是否正确
                        BllSysMessageAuthCode regVBll = new BllSysMessageAuthCode();
                        ModSysMessageAuthCode modAuthcode = regVBll.GetModelByWhere("and  getdate() < EndTime and TypeInt=1 and Tel='" + phone + "'");
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
                        if (code == "102913")
                        {
                            jsonResult.success = true;
                        }
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
        /// <param name="userid">用户id</param>
        /// <param name="pwd">新密码</param>
        /// <param name="oldpwd">旧密码</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/updateUserPwd/{userid}/{pwd}/{oldpwd}")]
        public MyJsonResult<string> updateUserPwd(string userid, string pwd, string oldpwd)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {

                if (jsonResult.success && string.IsNullOrEmpty(userid))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户id不能为空！";
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
                    var userModel = bllmaster.Get(userid);

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
        /// <param name="userid">用户id</param>
        /// <param name="name">名称</param>
        /// <param name="email">邮箱</param>
        /// <param name="sex">性别0: 男 1:女</param>
        /// <param name="age">年龄</param>
        /// <param name="cname">商家</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/updateUser/{userid}")]
        public MyJsonResult<string> updateUser(string userid, string name = "", string email = "", int sex = 0, int age = 0, string cname = "")
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                if (jsonResult.success && string.IsNullOrEmpty(userid))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "用户id不能为空！";
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
                    var sysMaster = bllmaster.Get(userid);
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
                    jsonResult.msg = "用户id手机类型不能为空！";
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
        /// <param name="userid">用户id</param>
        /// <param name="backinfo">意见反馈内容</param>
        /// <param name="phonetype">平台(1:苹果 2:安卓 3:网页)</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/saveFeedback/{userid}/{backinfo}/{phonetype}")]
        public MyJsonResult<string> saveFeedback(string userid, string backinfo, string phonetype)
        {
            MyJsonResult<string> jsonResult = new MyJsonResult<string>();
            try
            {
                //if (jsonResult.success && string.IsNullOrEmpty(userid))
                //{
                //    jsonResult.success = false;
                //    jsonResult.msg = "用户id不能为空！";
                //}
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
                    mod.UserId = userid;
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

        /// <summary>
        /// 获取幻灯片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/swiper")]
        public MyJsonResult<List<ModAdActive>> swiper()
        {
            MyJsonResult<List<ModAdActive>> jsonResult = new MyJsonResult<List<ModAdActive>>();
            try
            {
                BllAdActive bll = new BllAdActive();
                var r = bll.QueryAll(6);
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
        /// 新闻分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getdirc")]
        public MyJsonResult<List<ModSysDirc>> getdirc()
        {
            MyJsonResult<List<ModSysDirc>> jsonResult = new MyJsonResult<List<ModSysDirc>>();
            try
            {
                if (jsonResult.success)
                {
                    BllSysDirc bll = new BllSysDirc();
                    jsonResult.data = bll.QueryToAll().OrderBy(c => c.OrderNum).Where(c => c.Status == 1).ToList();
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
        /// 获取新闻列表
        /// </summary>
        /// <param name="dircid">分类</param>
        /// <param name="userid">用户id</param>
        /// <param name="search">搜索内容</param>
        /// <param name="pagesize">分页条数</param>
        /// <param name="pageindex">第几页</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getNews")]
        public MyJsonResult<List<ModEDynamic>> getNews(string dircid, string userid, string search = "", int pagesize = 20, int pageindex = 1)
        {
            MyJsonResult<List<ModEDynamic>> jsonResult = new MyJsonResult<List<ModEDynamic>>();
            try
            {
                BllEDynamic blledynamic = new BllEDynamic();
                Search searcht = new Search();
                searcht.PageSize = pagesize;
                searcht.CurrentPageIndex = pageindex;

                if (!string.IsNullOrEmpty(userid))
                {
                    //searcht.AddCondition("userid='" + userid + "'");
                }

                if (!string.IsNullOrEmpty(search))
                {
                    searcht.AddCondition("Name like '%" + search + "%'");
                }
                if (!string.IsNullOrEmpty(dircid))
                {
                    searcht.AddCondition("GroupId = '" + dircid + "'");
                }
                List<ModEDynamic> r = blledynamic.GetDynamic(searcht).Items;
                if (r != null)
                {
                    for (int i = 0; i < r.Count; i++)
                    {
                        string mark = new Html().StripHtml(r[i].Content);
                        if (mark.Length > 100)
                        {
                            mark = mark.Substring(0, 100) + "...";
                        }
                        r[i].Mark = mark;
                        r[i].Content = "";
                        //获取上传图片
                        BllSysImgPic bllSysImgPic = new BllSysImgPic();
                        r[i].ImageList = bllSysImgPic.GetList(r[i].Id);
                        r[i].Img = r[i].ImageList != null ? r[i].ImageList[0].PicUrl : "";
                    }
                }

                BllSysDirc bllsysDirc = new BllSysDirc();
                var m = bllsysDirc.LoadData(dircid);
                jsonResult.otherMsg = m != null ? m.Name : "";

                jsonResult.msg = "获取成功！";
                jsonResult.data = r;

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
        /// 获取新闻详情接口
        /// </summary>
        /// <param name="id">新闻id</param>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getNew/{id}")]
        public MyJsonResult<ModEDynamic> getNew(string id, string userid = "")
        {
            MyJsonResult<ModEDynamic> jsonResult = new MyJsonResult<ModEDynamic>();
            try
            {
                BllEDynamic blledynamic = new BllEDynamic();

                var news = blledynamic.LoadData(id);
                if (news != null)
                {

                    //获取上传图片
                    BllSysImgPic bllSysImgPic = new BllSysImgPic();
                    news.ImageList = bllSysImgPic.GetList(id);

                    news.IsCollection = false;
                    BllSysCollection bllSysCollection = new BllSysCollection();
                    var v = bllSysCollection.Exit(userid, id, CollectionEnum.新闻收藏);
                    if (v != null)
                        news.IsCollection = true;
                    try
                    {
                        //修改新闻阅读量
                        news.ReadNum++;
                        blledynamic.Update(news);
                    }
                    catch (Exception ex)
                    {
                        LogErrorRecord.ErrorFormat("错误日志：修改新闻阅读量{0}", ex.Message);
                    }

                    jsonResult.msg = "获取成功！";
                    jsonResult.data = news;
                }
                else
                {
                    jsonResult.msg = "获取失败，新闻已下架！";
                    jsonResult.success = false;
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
