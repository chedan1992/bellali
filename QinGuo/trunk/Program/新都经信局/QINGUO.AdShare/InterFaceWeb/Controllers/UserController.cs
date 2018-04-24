using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspx.Business;
using Aspx.Model;
using Aspx.Common;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using InterFaceWeb.Common;
using Dapper;
using System.Text;

namespace InterFaceWeb.Controllers
{
    public class UserController : BaseController
    {

        //登录
        //[HttpGet]
        public JsonResult Login(string loginname, string pwd, string BDChannelId, string BDUserId, string MobileCode, int PaltForm = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：loginname={0}&pwd={1}&type={2}&BDChannelId={3}&BDUserId={4}&PaltForm={5}&MobileCode={6}", loginname, pwd, "", BDChannelId, BDUserId, PaltForm, MobileCode);

                CheckParams(loginname, "用户名不能为空！");
                CheckParams(pwd, "密码不能为空！");

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();

                    ModSysMaster modsysuser = bllmaster.GetModelByWhere("and LoginName='" + loginname + "' and Status<>" + (int)StatusEnum.删除);//登录名称
                    if (modsysuser != null)
                    {
                        if (!string.IsNullOrEmpty(modsysuser.Pwd) && DESEncrypt.Decrypt(modsysuser.Pwd) == pwd)
                        {

                            if (modsysuser.Status == -2)
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "正在审核用户信息！";
                            }
                            else
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

                                    LogErrorRecord.InfoFormat("BDUserId={0}", BDUserId);
                                    LogErrorRecord.InfoFormat("BDChannelId={0}", BDChannelId);
                                    LogErrorRecord.InfoFormat("PaltForm={0}", PaltForm);
                                    LogErrorRecord.InfoFormat("MobileCode={0}", MobileCode);

                                    bllmaster.UpdateLogin(modsysuser);
                                }
                                catch (Exception ex)
                                {
                                    LogErrorRecord.ErrorFormat("其他扩展={0}", ex.Message);
                                }
                                #endregion

                                jsonUser r = new jsonUser();
                                r.id = modsysuser.Id;
                                r.cid = modsysuser.Cid;
                                r.name = modsysuser.UserName;
                                r.sex = modsysuser.Sex;
                                r.headimg = modsysuser.HeadImg;
                                r.age = modsysuser.Age;
                                r.email = modsysuser.Email;
                                /*
                                BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                                r.msg = bllSysPushMessage.MsgCount(r.id);//未读消息数

                                int type = modsysuser.Attribute;
                                if (type == (int)AdminTypeEnum.系统管理员 || type == (int)AdminTypeEnum.单位管理员 || type == (int)AdminTypeEnum.供应商管理员 || type == (int)AdminTypeEnum.维保公司管理员 || type == (int)AdminTypeEnum.消防部门管理员)
                                {
                                    BllSysFlow bllSysFlow = new BllSysFlow();
                                    r.flowmsg = bllSysFlow.flowmsg(modsysuser);//待办消息数
                                }
                                */
                                r.phone = modsysuser.Phone;
                                r.attribute = modsysuser.Attribute;

                                BllSysCompany bllsyscompany = new BllSysCompany();
                                var company = bllsyscompany.LoadData(modsysuser.Cid);
                                if (company != null)
                                {
                                    r.postName = company != null ? company.Name : "";
                                    r.postLon = company.ComPLon;
                                    r.postLat = company.CompLat;
                                    //获取平台联系电话
                                    string key = "C0A38317-59DA-4DE2-9428-E5B66EDBC2CF";
                                    var com = bllsyscompany.LoadData(key);
                                    r.postPhone = com.Phone;
                                }
                                if (company != null && company.Status == (int)StatusEnum.正常)
                                {
                                    jsonResult.data = r;//用户信息
                                    jsonResult.msg = "登录成功！";
                                    jsonResult.success = true;
                                }
                                else if (company != null && company.Status == (int)StatusEnum.未激活)
                                {
                                    jsonResult.msg = "登录失败！单位待审核";
                                    jsonResult.success = false;
                                }
                                else
                                {
                                    jsonResult.msg = "登录失败！单位已注销";
                                    jsonResult.success = false;
                                }
                            }
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //获取消息数量
        //[HttpGet]
        public JsonResult StaticeMsg(string userid, int type)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}", userid);

                CheckParams(userid, "用户id不能为空！");

                if (jsonResult.success)
                {
                    var modsysuser = new BllSysMaster().LoadData(userid);
                    BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                    int msg = bllSysPushMessage.MsgCount(userid);//未读消息数
                    int flowmsg = 0;
                    int types = modsysuser.Attribute;
                    if (types == (int)AdminTypeEnum.系统管理员 || types == (int)AdminTypeEnum.单位管理员 || types == (int)AdminTypeEnum.供应商管理员 || types == (int)AdminTypeEnum.维保公司管理员 || types == (int)AdminTypeEnum.消防部门管理员)
                    {
                        BllSysFlow bllSysFlow = new BllSysFlow();
                        flowmsg = bllSysFlow.flowmsg(modsysuser);//待办消息数
                    }
                    jsonResult.data = new { FlowMsg = flowmsg, Msg = msg };
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //注销
        //[HttpGet]
        public JsonResult LoginOut(string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}", userid);

                CheckParams(userid, "用户id不能为空！");

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.LoadData(userid);
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //获取用户详情
        //[HttpGet]
        public JsonResult GetUserInfo(string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}", userid);

                CheckParams(userid, "用户id不能为空！");

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.LoadData(userid);
                    if (sysMaster != null)
                    {

                        jsonUser r = new jsonUser();
                        r.id = sysMaster.Id;
                        r.cid = sysMaster.Cid;//字段赋值错误  ducq
                        r.name = sysMaster.UserName;
                        r.sex = sysMaster.Sex;
                        r.headimg = sysMaster.HeadImg;
                        r.age = sysMaster.Age;
                        r.email = sysMaster.Email;
                        r.phone = sysMaster.Phone;
                        /*
                        BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                        r.msg = bllSysPushMessage.MsgCount(r.id);//未读消息数


                        int type = sysMaster.Attribute;
                        if (type == (int)AdminTypeEnum.系统管理员 || type == (int)AdminTypeEnum.单位管理员 || type == (int)AdminTypeEnum.供应商管理员 || type == (int)AdminTypeEnum.维保公司管理员 || type == (int)AdminTypeEnum.消防部门管理员)
                        {
                            BllSysFlow bllSysFlow = new BllSysFlow();
                            r.flowmsg = bllSysFlow.flowmsg(sysMaster);//代办消息数
                        }
                        */
                        r.attribute = sysMaster.Attribute;

                        BllSysCompany bllsyscompany = new BllSysCompany();
                        var company = bllsyscompany.LoadData(sysMaster.Cid);
                        if (company != null)
                        {
                            r.postName = company != null ? company.Name : "";
                            r.postLon = company.ComPLon;
                            r.postLat = company.CompLat;
                            //获取平台联系电话
                            string key = "C0A38317-59DA-4DE2-9428-E5B66EDBC2CF";
                            var com = bllsyscompany.LoadData(key);
                            r.postPhone = com.Phone;
                        }
                        jsonResult.data = r;// JsonHelper.ToJson(r);//用户信息
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //上传头像
        //[HttpGet]
        public JsonResult Uploadhimg(string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}", userid);

                CheckParams(userid, "用户id不能为空！");

                if (jsonResult.success)
                {
                    HttpFileCollectionBase files = Request.Files;//获取文件

                    string seavPath = ConfigurationManager.AppSettings["HeadImg"];//文件保存路劲
                    string BigPath = Server.MapPath("~" + seavPath + "BigImg/");//大图片
                    string tmpPath = Server.MapPath("~" + seavPath);//小图片
                    FileHelper fh = new FileHelper();
                    fh.CreateDirectory(BigPath);//创建文件夹
                    fh.CreateDirectory(tmpPath);//创建文件夹
                    if (files != null && files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //忘记密码，邮件找回
        //[HttpGet]
        public JsonResult ForgetPwd(string account, string email)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&email={1}", account, email);

                CheckParams(account, "账号不能为空！");
                Regex regex = new Regex("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");

                if (!string.IsNullOrEmpty(email) && !regex.IsMatch(email))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "请输入正确的邮箱地址！";
                }
                if (jsonResult.success)
                {

                    BllSysMaster bllmaster = new BllSysMaster();

                    ModSysMaster modsysuser = bllmaster.GetModelByWhere("and LoginName='" + account + "' and Status<>" + (int)StatusEnum.删除);//登录名称
                    if (modsysuser != null)
                    {
                        if (string.IsNullOrEmpty(email))
                        {
                            email = modsysuser.Email;
                        }
                        CheckParams(email, "邮箱不能为空！");

                        if (jsonResult.success)
                        {
                            DateTime now = DateTime.Now;
                            string time = +now.Year + "年" + now.Month + "月" + now.Day + "日" + "  " + now.Hour + ":" + now.Minute;

                            string html = "<h2>亲爱的用户，你好：</h2>";
                            html += "<p>您于" + time + "发起找回密码请求！</p>";
                            html += "<p>您的密码是:" + DESEncrypt.Decrypt(modsysuser.Pwd) + "。</p>";
                            html += "<p>请妥善保管密码。</p>";
                            html += "<p>感谢您对消防App的大力支持。</p>";

                            bool r = SendEmail.SendHtml("15928877394@163.com", "消防App", email, "消防App密码找回", html, "smtp.163.com", "15928877394@163.com", "qgxflwlxwhjwqfzc");
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //修改性别
        //[HttpGet]
        public JsonResult UpdateUserSex(string userid, string sex)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&sex={1}", userid, sex);

                CheckParams(userid, "用户id不能为空！");
                CheckParams(sex, "用户性别不能为空！");

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.LoadData(userid);
                    sysMaster.Sex = Convert.ToInt32(sex);

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
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //修改姓名
        //[HttpGet]
        public JsonResult UpdateUserName(string userid, string name)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&sex={1}", userid, name);

                CheckParams(userid, "用户id不能为空！");
                CheckParams(name, "用户昵称不能为空！");

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.LoadData(userid);
                    sysMaster.UserName = name;

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
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //修改密码
        //[HttpGet]
        public JsonResult UpdateUserPwd(string userid, string pwd, string oldpwd)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&sex={1}", userid, pwd);

                CheckParams(userid, "用户id不能为空！");
                CheckParams(pwd, "用户密码不能为空！");

                if (pwd.Trim().Length < 6 || pwd.Trim().Length > 20)
                {
                    CheckParams("", "密码长度必须6至20个字符之间！");
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //修改
        //[HttpGet]
        public JsonResult UpdateUser(string userid, string name, string sex, string email, string phone)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&name={1}&sex={2}&email={3}&phone={4}", userid, name, sex, email, phone);

                CheckParams(userid, "用户id不能为空！");
                //CheckParams(name, "用户昵称不能为空！");
                //CheckParams(sex, "性别不能为空！");
                //CheckParams(email, "邮箱不能为空！");
                //CheckParams(phone, "联系不能为空！");

                if (jsonResult.success)
                {
                    BllSysMaster bllmaster = new BllSysMaster();
                    var sysMaster = bllmaster.Get(userid);
                    if (!string.IsNullOrEmpty(name))
                        sysMaster.UserName = name;
                    if (!string.IsNullOrEmpty(email))
                        sysMaster.Email = email;
                    if (!string.IsNullOrEmpty(phone))
                        sysMaster.Phone = phone;
                    if (!string.IsNullOrEmpty(sex))
                        sysMaster.Sex = Convert.ToInt32(sex);

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
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //查询单位
        //[HttpGet]
        public JsonResult GetPost(int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：type={0}", type);
                BllSysCompany bllsyscompany = new BllSysCompany();

                List<ModSysCompany> list = bllsyscompany.GetListByAttribute(type);

                List<ModSysCompany> listP = bllsyscompany.GetListByAttribute((int)CompanyType.部门);

                List<jsonComapny> r = new List<jsonComapny>();
                foreach (var item in list)
                {
                    jsonComapny j = new jsonComapny();
                    j.id = item.Id;
                    j.name = item.Name;
                    j.address = item.Address;
                    j.child = new List<jsonComapny>();
                    foreach (var p in listP.Where(c => c.CreateCompanyId == item.Id))
                    {

                        jsonComapny jm = new jsonComapny();
                        jm.id = p.Id;
                        jm.name = p.Name;
                        jm.address = p.Address;
                        j.child.Add(jm);
                    }
                    r.Add(j);
                }

                jsonResult.data = r;
                jsonResult.msg = "获取成功！";
                jsonResult.success = true;
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //查询单位部门
        //[HttpGet]
        public JsonResult GetDepartments(string cid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：cid={0}", cid);

                CheckParams(cid, "单位id不能为空！");

                BllSysCompany bllsyscompany = new BllSysCompany();
                List<ModSysCompany> list = bllsyscompany.GetDepartments(cid);

                List<jsonComapny> r = new List<jsonComapny>();
                foreach (var item in list)
                {
                    jsonComapny j = new jsonComapny();
                    j.id = item.Id;
                    j.name = item.Name;
                    j.address = item.Address;
                    r.Add(j);
                }

                jsonResult.data = r;
                jsonResult.msg = "获取成功！";
                jsonResult.success = true;
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);

        }

        //新增单位部门
        //[HttpGet]
        public JsonResult AddDepartments(string userid, string cid, string cname, int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：cid={0}&cname={1}&userid={2}&type={3}", cid, cname, userid, type);

                CheckParams(cid, "单位id不能为空！");
                CheckParams(userid, "用户id不能为空！");
                CheckParams(cname, "部门名称不能为空！");

                if (type != (int)AdminTypeEnum.单位管理员)
                {
                    CheckParams("", "对不起,您无操作权限！");
                }

                BllSysCompany bllsyscompany = new BllSysCompany();
                List<ModSysCompany> list = bllsyscompany.GetDepartments(cid);
                if (list.Exists(c => c.Name == cname))
                {
                    CheckParams(cname, "部门名称不能从复！");
                }

                if (jsonResult.success)
                {
                    ModSysCompany sysCompany = new ModSysCompany();

                    sysCompany.Id = Guid.NewGuid().ToString();

                    sysCompany.CreateTime = DateTime.Now;
                    sysCompany.CreaterUserId = userid;
                    sysCompany.MasterId = userid;
                    sysCompany.CreateCompanyId = cid;
                    sysCompany.Name = cname;
                    sysCompany.Attribute = (int)CompanyType.部门;
                    sysCompany.Status = (int)StatusEnum.正常;

                    if (bllsyscompany.Insert(sysCompany) > 0)
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //修改单位部门
        //[HttpGet]
        public JsonResult UpDepartments(string departmentsid, string cid, string cname, int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：departmentsid={0}&cname={1}&type={2}", departmentsid, cname, type);

                CheckParams(cid, "单位id不能为空！");
                CheckParams(departmentsid, "部门id不能为空！");
                CheckParams(cname, "部门名称不能为空！");

                if (type != (int)AdminTypeEnum.单位管理员)
                {
                    CheckParams("", "对不起,您无操作权限！");
                }

                BllSysCompany bllsyscompany = new BllSysCompany();
                List<ModSysCompany> list = bllsyscompany.GetDepartments(cid);
                if (list.Exists(c => c.Name == cname))
                {
                    CheckParams(cname, "部门名称不能从复！");
                }


                if (jsonResult.success)
                {
                    var sysCompany = bllsyscompany.LoadData(departmentsid);
                    sysCompany.Name = cname;
                    if (bllsyscompany.Update(sysCompany) > 0)
                    {
                        jsonResult.msg = "修改成功！";
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }


        //删除单位部门
        //[HttpGet]
        public JsonResult DeDepartments(string departmentsid, string userid, string cid, int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：departmentsid={0}&userid={1}&type={2}&cid={3}", departmentsid, userid, type, cid);

                CheckParams(departmentsid, "部门id不能为空！");
                CheckParams(userid, "用户id不能为空！");
                CheckParams(cid, "单位id不能为空！");

                if (type != (int)AdminTypeEnum.单位管理员)
                {
                    CheckParams("", "对不起,您无操作权限！");
                }

                BllSysMaster bllmaster = new BllSysMaster();
                //查询单位员工
                List<ModSysMaster> rmaster = bllmaster.GetCIdByList(cid);
                if (rmaster.Exists(c => c.OrganizaId == departmentsid))
                {
                    CheckParams("", "部门下还有员工不能删除！");
                }
                if (jsonResult.success)
                {

                    BllSysCompany bllsyscompany = new BllSysCompany();

                    if (bllsyscompany.DeleteStatus(departmentsid) > 0)
                    {
                        jsonResult.msg = "删除成功！";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "删除失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }


        //注册
        //[HttpGet]
        public JsonResult Register(string phone, string pwd, string MobileCode, string email, string name, string headimg, string cid, string organizaId, string cname, string postLon, string postLat, string address, string ccontact, string cphone, int sex = 0, int ctype = 0, int type = 0, int PaltForm = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：pwd={0}&phone={1}&type={2}&MobileCode={3}&PaltForm={4}&email={5}&sex={6}&headimg={7}&cid={8}&cname={9}", pwd, phone, type, MobileCode, PaltForm, email, sex, headimg, cid, cname);

                CheckParams(phone, "用户账号不能为空！");
                CheckParams(pwd, "用户密码不能为空！");

                if (pwd.Trim().Length < 6 || pwd.Trim().Length > 20)
                {
                    CheckParams("", "密码长度必须6至20个字符之间！");
                }

                Regex regex = new Regex("^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
                if (!string.IsNullOrEmpty(email) && !regex.IsMatch(email))
                {
                    jsonResult.success = false;
                    jsonResult.msg = "请输入正确的邮箱地址！";
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
                    ModSysMaster sysMaster = new ModSysMaster();
                    sysMaster.Id = Guid.NewGuid().ToString();
                    sysMaster.LoginName = phone;
                    sysMaster.Pwd = DESEncrypt.Encrypt(pwd);  //加密
                    sysMaster.Attribute = type;//AdminTypeEnum.工程师;
                    sysMaster.Status = (int)StatusEnum.未激活;
                    sysMaster.MobileCode = MobileCode; //机器码
                    sysMaster.PaltForm = PaltForm;
                    sysMaster.CreateTime = DateTime.Now;

                    sysMaster.UserName = name;
                    sysMaster.Email = email;
                    sysMaster.Sex = sex;
                    sysMaster.HeadImg = headimg;//默认头像
                    sysMaster.Cid = cid;

                    if (type == (int)AdminTypeEnum.单位用户)
                    {
                        sysMaster.OrganizaId = organizaId;
                        CheckParams(organizaId, "请选择单位部门！");
                    }

                    sysMaster.IsMain = true;
                    sysMaster.IsSystem = true;
                    int FlowType = 2;
                    string Title = ((AdminTypeEnum)type).ToString();

                    BllSysCompany bllsyscompany = new BllSysCompany();

                    if (jsonResult.success && sysMaster.Attribute == (int)AdminTypeEnum.单位管理员 || sysMaster.Attribute == (int)AdminTypeEnum.供应商管理员 || sysMaster.Attribute == (int)AdminTypeEnum.维保公司管理员 || sysMaster.Attribute == (int)AdminTypeEnum.消防部门管理员)
                    {
                        CheckParams(cname, "单位名称不能为空！");
                        if (jsonResult.success)
                        {
                            //验证单位名是否重复
                            List<ModSysCompany> list = bllsyscompany.GetListByAttribute(ctype);
                            if (list.Exists(c => c.Name == cname))
                            {
                                jsonResult.success = false;
                                jsonResult.msg = "该单位名称已注册！";
                            }
                            if (jsonResult.success)
                            {
                                ModSysCompany sysCompany = new ModSysCompany();
                                sysCompany.Id = Guid.NewGuid().ToString();

                                sysCompany.CreateTime = DateTime.Now;
                                sysCompany.CreaterUserId = sysMaster.Id;
                                sysCompany.MasterId = sysMaster.Id;
                                sysCompany.CreateCompanyId = "0";
                                sysCompany.Name = cname;
                                sysCompany.ComPLon = postLon;
                                sysCompany.CompLat = postLat;
                                sysCompany.Address = address;
                                sysCompany.Phone = cphone;
                                sysCompany.LinkUser = ccontact;
                                sysCompany.Attribute = ctype;
                                sysCompany.Status = (int)StatusEnum.未激活;

                                sysMaster.Cid = sysCompany.Id;
                                FlowType = 1;
                                Title = ((CompanyType)ctype).ToString();

                                if (bllsyscompany.Insert(sysCompany) <= 0)
                                {
                                    jsonResult.msg = "新增单位失败！";
                                    jsonResult.success = false;
                                }
                                else
                                {
                                    jsonResult.msg = "注册成功，等待审核！";
                                    jsonResult.success = true;
                                }
                            }
                        }
                    }
                    if (jsonResult.success)
                    {
                        if (bllmaster.Insert(sysMaster) > 0)
                        {
                            //保存用户与单位之间的关系
                            ModSysFlow SysFlow = new ModSysFlow();
                            SysFlow.Id = Guid.NewGuid().ToString();
                            SysFlow.Title = Title + ":" + cname + "注册申请";
                            SysFlow.FlowType = FlowType;//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核)
                            SysFlow.FlowStatus = 0;
                            SysFlow.Reamrk = "";
                            SysFlow.ApprovalUser = sysMaster.Id;
                            SysFlow.ApprovalTime = DateTime.Now;
                            SysFlow.CompanyId = sysMaster.Cid;
                            SysFlow.MasterId = sysMaster.Id;
                            int count = new BllSysFlow().Insert(SysFlow);
                            if (count <= 0)
                            {
                                bllmaster.Delete(sysMaster.Id);
                                if (FlowType == 1)
                                {
                                    bllsyscompany.Delete(sysMaster.Cid);
                                }
                                jsonResult.msg = "用户同步流程失败,请稍后再操作!";
                                jsonResult.success = false;
                            }
                            else
                            {
                                jsonResult.msg = "注册成功，等待审核！";
                                jsonResult.success = true;
                            }
                        }
                        else
                        {
                            if (FlowType == 1)
                            {
                                bllsyscompany.Delete(sysMaster.Cid);
                            }
                            jsonResult.msg = "注册失败！";
                            jsonResult.success = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //注册前手机验证是否注册
        //[HttpGet]
        public JsonResult RegisterVali(string phone)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：phone={0}", phone);

                CheckParams(phone, "用户账号不能为空！");

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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //验证码
        //[HttpGet]
        public JsonResult SendCode(string phone, int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：type={0}&phone={1}", type, phone);

                CheckParams(phone, "获取验证码电话不能为空！");

                if (jsonResult.success)
                {

                    int openerType = 0;

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

                            string smstext = "您正在注册工程师用户信息,请求的验证码为：" + model.Code + "，打死也不能告诉别人哟.";
                            if (type == 1)
                            {
                                smstext = "找回的密码可要牢记哟,请求的验证码为：" + model.Code + ".";
                            }
                            else if (openerType == 2)
                            {
                                smstext = "取现操作要细心,请求的验证码为：" + model.Code + ".";
                            }
                            int MsgId = regVBll.Insert(model);
                            if (MsgId > 0)//添加一条验证码到数据库
                            {
                                cn.b2m.eucp.hprpt2.SDKService msm = new cn.b2m.eucp.hprpt2.SDKService();

                                string sn = ConfigurationManager.AppSettings["sn"];
                                string pwd = ConfigurationManager.AppSettings["pwd"];

                                int result = msm.sendSMS(sn.Trim().ToString(),
                                            pwd.Trim().ToString(),
                                            "",
                                            phone.Split(new char[] { ',' }),
                                            smstext,
                                            "",
                                            "GBK",
                                            3,
                                            Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                                if (result == 0)
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
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //我的消息列表
        //[HttpGet]
        public JsonResult MyMessage(string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}", userid);
                CheckParams(userid, "用户id不能为空！");

                if (jsonResult.success)
                {
                    BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                    Search search = GetSearch();
                    search.AddCondition("UserId='" + userid + "'");

                    BllSysAppointed bllSysAppointed = new BllSysAppointed();
                    Page<ModSysPushMessage> r = bllSysPushMessage.GetMyMessageSearch(search);
                    jsonResult.data = r.Items;
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //我的消息列表
        //[HttpGet]
        public JsonResult UpMyMessage(string msgid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：msgid={0}", msgid);
                CheckParams(msgid, "消息id不能为空！");

                if (jsonResult.success)
                {

                    //修改消息读状态
                    BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                    if (bllSysPushMessage.UpdateMsgId(msgid))
                    {
                        jsonResult.msg = "修改消息状态成功！";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "修改消息状态失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //删除我的消息
        //[HttpGet]
        public JsonResult DeMyMessage(string msgid, string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：msgid={0}&userid={1}", msgid, userid);
                CheckParams(msgid, "消息id不能为空！");

                if (jsonResult.success)
                {

                    //修改消息读状态
                    BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                    if (bllSysPushMessage.Delete(msgid) > 0)
                    {
                        jsonResult.msg = "删除消息成功！";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "删除消息失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }


        //删除我的所有消息
        //[HttpGet]
        public JsonResult DeAllMyMessage(string userid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}", userid);
                CheckParams(userid, "用户id不能为空！");

                if (jsonResult.success)
                {

                    //修改消息读状态
                    BllSysPushMessage bllSysPushMessage = new BllSysPushMessage();
                    if (bllSysPushMessage.DeleteAllStatus(userid) > 0)
                    {
                        jsonResult.msg = "删除消息成功！";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "删除消息失败！";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //单位详情
        //[HttpGet]
        public JsonResult GetComapny(string cid)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：cid={0}", cid);
                CheckParams(cid, "用户id不能为空！");

                if (jsonResult.success)
                {
                    BllSysCompany bllsyscompany = new BllSysCompany();
                    var company = bllsyscompany.LoadData(cid);
                    jsonResult.data = new
                    {
                        Id = company.Id,
                        Name = company.Name,
                        NameTitle = company.NameTitle,
                        Address = company.Address,
                        Phone = company.Phone,
                        Email = company.Email,
                        LinkUser = company.LinkUser,
                        LegalPerson = company.LegalPerson,
                        Introduction = company.Introduction,
                        Code = company.Code,
                        ComPLon = company.ComPLon,
                        CompLat = company.CompLat,
                        LicenseNumber = company.LicenseNumber
                    };
                    jsonResult.msg = "获取成功！";
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //我的单位列表
        //[HttpGet]
        public JsonResult GetComapnyList(string userid, string cid, string search, int type = 0, int companytype = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&cid={1}&type={2}&search={3}&companytype={4}", userid, cid, type, search, companytype);
                CheckParams(userid, "用户id不能为空！");
                CheckParams(cid, "单位id不能为空！");

                if (jsonResult.success)
                {
                    BllSysCompany bllsyscompany = new BllSysCompany();

                    Search searchtemp = GetSearch();

                    if (!string.IsNullOrEmpty(search))//模糊查询
                    {
                        searchtemp.AddCondition("Name like '%" + search + "%'");
                    }

                    BllSysCompanyCognate bllSysCompanyCognate = new BllSysCompanyCognate();
                    switch (companytype)
                    {
                        case (int)CompanyType.单位:
                            #region 单位查询
                            searchtemp.AddCondition("Attribute=" + (int)CompanyType.单位);
                            switch (type)
                            {
                                case (int)AdminTypeEnum.单位管理员:
                                case (int)AdminTypeEnum.单位用户:
                                    searchtemp.AddCondition("id='" + cid + "'");
                                    break;
                                case (int)AdminTypeEnum.供应商管理员:
                                case (int)AdminTypeEnum.供应商用户:
                                case (int)AdminTypeEnum.维保公司管理员:
                                case (int)AdminTypeEnum.维保用户:
                                case (int)AdminTypeEnum.消防部门管理员:
                                case (int)AdminTypeEnum.消防用户:
                                    string sql = bllSysCompanyCognate.GetInCompanyResult(cid, type);//查询消防部门，维保单位，供应商下的单位列表
                                    if (!string.IsNullOrEmpty(sql))
                                    {
                                        searchtemp.AddCondition("id in (" + sql + ")");
                                    }
                                    else
                                    {
                                        jsonResult.success = false;
                                        jsonResult.msg = "获取成功！";
                                    }
                                    break;
                                default:
                                    jsonResult.success = false;
                                    jsonResult.msg = "获取成功！";
                                    break;
                            }
                            #endregion
                            break;
                        case (int)CompanyType.供应商:
                        case (int)CompanyType.维保公司:
                        case (int)CompanyType.消防部门:
                            #region 消防部门查询
                            searchtemp.AddCondition("Attribute=" + companytype);
                            switch (type)
                            {
                                case (int)AdminTypeEnum.单位管理员:
                                case (int)AdminTypeEnum.单位用户:
                                    string sql4 = bllSysCompanyCognate.GetInEmployerIdResult(cid, type);//单位id查询对应的消防部门，维保单位，供应商列表
                                    if (!string.IsNullOrEmpty(sql4))
                                    {
                                        searchtemp.AddCondition("id in (" + sql4 + ")");
                                    }
                                    else
                                    {
                                        jsonResult.success = false;
                                        jsonResult.msg = "获取成功！";
                                    }
                                    break;
                                default:
                                    break;
                            }
                            #endregion
                            break;
                        default:
                            break;
                    }



                    if (jsonResult.success)
                    {
                        var r = bllsyscompany.SearchDataList(searchtemp);
                        List<object> rl = new List<object>();
                        foreach (var company in r.Items)
                        {
                            rl.Add(new
                            {
                                Id = company.Id,
                                Name = company.Name,
                                NameTitle = company.NameTitle,
                                Address = company.Address,
                                Phone = company.Phone,
                                Email = company.Email,
                                LinkUser = company.LinkUser,
                                LegalPerson = company.LegalPerson,
                                Introduction = company.Introduction,
                                Code = company.Code,
                                ComPLon = company.ComPLon,
                                CompLat = company.CompLat,
                                LicenseNumber = company.LicenseNumber
                            });
                        }
                        jsonResult.data = rl;
                        jsonResult.msg = "获取成功！";
                    }
                    else
                    {
                        jsonResult.success = true;
                        jsonResult.data = new List<object>();
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //我的上级列表
        //[HttpGet]
        public JsonResult GetChiefComapnyList(string userid, string cid, string search, int type = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&cid={1}&type={2}&search={3}", userid, cid, type, search);
                CheckParams(userid, "用户id不能为空！");
                CheckParams(cid, "单位id不能为空！");

                if (jsonResult.success)
                {
                    BllSysCompany bllsyscompany = new BllSysCompany();

                    Search searchtemp = GetSearch();
                    if (!string.IsNullOrEmpty(search))//模糊查询
                    {
                        searchtemp.AddCondition("Name like '%" + search + "%'");
                    }
                    switch (type)
                    {
                        case (int)AdminTypeEnum.单位管理员:
                        case (int)AdminTypeEnum.单位用户:
                            BllSysCompanyCognate bllSysCompanyCognate = new BllSysCompanyCognate();
                            string sql = bllSysCompanyCognate.GetInEmployerIdResult(cid, type);
                            if (!string.IsNullOrEmpty(sql))
                            {
                                searchtemp.AddCondition("id in (" + sql + ")");

                                var r = bllsyscompany.SearchDataList(searchtemp);
                                List<object> rl = new List<object>();
                                foreach (var company in r.Items)
                                {
                                    rl.Add(new
                                    {
                                        Id = company.Id,
                                        Name = company.Name,
                                        NameTitle = company.NameTitle,
                                        Address = company.Address,
                                        Phone = company.Phone,
                                        Email = company.Email,
                                        LinkUser = company.LinkUser,
                                        LegalPerson = company.LegalPerson,
                                        Introduction = company.Introduction,
                                        Code = company.Code,
                                        ComPLon = company.ComPLon,
                                        CompLat = company.CompLat,
                                        LicenseNumber = company.LicenseNumber
                                    });
                                }
                                jsonResult.data = rl;
                                jsonResult.msg = "获取成功！";
                            }
                            else
                            {
                                jsonResult.data = new List<object>();
                                jsonResult.msg = "获取成功！";
                            }
                            break;
                        case (int)AdminTypeEnum.供应商管理员:
                        case (int)AdminTypeEnum.供应商用户:
                        case (int)AdminTypeEnum.维保公司管理员:
                        case (int)AdminTypeEnum.维保用户:
                        case (int)AdminTypeEnum.消防部门管理员:
                        case (int)AdminTypeEnum.消防用户:
                            jsonResult.data = new List<object>();
                            jsonResult.msg = "获取成功！";
                            break;
                        default:
                            jsonResult.data = new List<object>();
                            jsonResult.msg = "获取成功！";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //我的审核列表
        //[HttpGet]
        public JsonResult GetFlowList(string userid, string cid, int type = 0, int FlowStatus = -2, int FlowType = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&cid={1}&type={2}&FlowStatus={3}", userid, cid, type, FlowStatus);
                CheckParams(userid, "用户id不能为空！");
                CheckParams(cid, "单位id不能为空！");

                if (jsonResult.success)
                {
                    BllSysFlow bllSysFlow = new BllSysFlow();
                    Search searchtemp = GetSearch();

                    //系统管理员 查看所有
                    if (type == (int)AdminTypeEnum.单位管理员 || type == (int)AdminTypeEnum.供应商管理员 || type == (int)AdminTypeEnum.维保公司管理员 || type == (int)AdminTypeEnum.消防部门管理员)
                    {
                        searchtemp.AddCondition("CompanyId='" + cid + "'");
                    }

                    /*-1：代办任务
                       0：已办任务
                       1：我发起的
                    */
                    switch (FlowStatus)
                    {
                        case -1:
                            if (type == (int)AdminTypeEnum.系统管理员 || type == (int)AdminTypeEnum.单位管理员 || type == (int)AdminTypeEnum.供应商管理员 || type == (int)AdminTypeEnum.维保公司管理员 || type == (int)AdminTypeEnum.消防部门管理员)
                            {
                                searchtemp.AddCondition("FlowStatus=0");
                            }
                            else
                            {
                                searchtemp.AddCondition("1=2");
                            }
                            break;
                        case 0:
                            //searchtemp.AddCondition("(FlowStatus=-1 or FlowStatus=1)");
                            searchtemp.AddCondition("AuditUser='" + userid + "'");
                            break;
                        case 1:
                            searchtemp.AddCondition("ApprovalUser='" + userid + "'");
                            break;
                        default:
                            break;
                    }

                    //if (FlowStatus >= -1)//审批状态(-1:未通过 0:待审核 1:已通过)
                    //{
                    //    searchtemp.AddCondition("FlowStatus=" + FlowStatus);
                    //}

                    if (FlowType > 0)//审批类型(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核 4:设备责任人变更)
                    {
                        searchtemp.AddCondition("FlowType=" + FlowType);
                    }

                    var page = bllSysFlow.GetFlowList(searchtemp);
                    jsonResult.data = page.Items;
                    jsonResult.msg = "获取成功！";
                }
                else
                {
                    jsonResult.data = new List<object>();
                    jsonResult.msg = "获取成功！";
                    jsonResult.success = true;
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }

        //审核
        //[HttpGet]
        public JsonResult FlowWaitWork(string userid, string cid, string reamrk, string flowId, int isResult = 0)
        {
            try
            {
                LogErrorRecord.InfoFormat("参数请求url:{0}", Request.Url);
                LogErrorRecord.InfoFormat("参数参数：userid={0}&cid={1}&reamrk={2}&flowId={3}&isResult={4}", userid, cid, reamrk, flowId, isResult);
                CheckParams(userid, "用户id不能为空！");
                CheckParams(cid, "单位id不能为空！");
                CheckParams(flowId, "流程id不能为空！");

                if (jsonResult.success)
                {
                    BllSysFlow bllsyscompany = new BllSysFlow();

                    if (bllsyscompany.WaitWork(userid, cid, reamrk, flowId, isResult))
                    {
                        jsonResult.msg = "操作成功!";
                    }
                    else
                    {
                        jsonResult.success = false;
                        jsonResult.msg = "操作失败!";
                    }
                }
            }
            catch (Exception ex)
            {
                LogErrorRecord.ErrorFormat("错误日志：{0},请求url:{1}", ex.Message, Request.Url);
                jsonResult.success = false;
                jsonResult.errorCode = -1;
                jsonResult.msg = "常异:" + ex.Message;
            }
            return Json(jsonResult, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
