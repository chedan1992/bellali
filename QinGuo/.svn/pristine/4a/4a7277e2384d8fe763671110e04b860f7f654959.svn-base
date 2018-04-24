using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QINGUO.Common;
using QINGUO.Model;
using QINGUO.Business;
using System.Drawing;
using System.Configuration;
using System.Xml;
using System.Web.Script.Serialization;
using System.Data;
using WebPortalAdmin.Code;
using System.Globalization;
using System.Text;
using System.Threading;

namespace WebPortalAdmin.Controllers
{
    public class HomeController : Controller
    {
        BllSysOperateLog OperateLog = new BllSysOperateLog();

        [HttpPost]
        public ActionResult GetDataContent()
        {
            return Json(null);
        }

        public string key = ConfigurationManager.AppSettings["CompanyId"];

        /// <summary>
        /// 忘记密码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgetPwd()
        {
            ViewData["WebSiteName"] = ConfigurationManager.AppSettings["WebSiteName"];//系统站点名称
            ViewData["copyright"] = "";
            ViewData["Version"] = ConfigurationManager.AppSettings["Version"];//版本号
            var model = new BllSysCompany().LoadData(key);
            if (model != null)
            {
                ViewData["copyright"] = model.Name;
            }
            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string ReturnUrl = "";
            //根据权限,判断跳转路径
            var master = new MasterContext().Master;
            if (master == null)
            {
                ReturnUrl = "/Temple/LoginError";
            }
            else
            {
                switch (master.Attribute)
                { 
                    case 0:
                    case 1://菜单配置员,系统管理员
                        ReturnUrl = "/Home/AdminIndex";
                        break;
                    case 2://单位管理员
                        ReturnUrl = "/Home/CompanyIndex";
                        break;
                    case 3: //消防部门管理员
                    case 4: //维保公司管理员
                    case 5: //供应商管理员
                        ReturnUrl = "/Home/ParentIndex";
                        break;

                    case 10: //单位用户
                    case 11: //消防用户
                    case 12: //维保用户
                    case 13: //供应商用户
                        ReturnUrl = "/Home/UserIndex";
                        break;
                }
            }
            return Redirect(ReturnUrl);
        }

        /// <summary>
        /// 单位管理员 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminIndex()
        {
             var master = new MasterContext().Master;
             if (master == null)
             {
                 return Redirect("/Temple/LoginError");
             }
             else
             {
                 //获取通知公告
                 XMLHealper XmlColl = new XMLHealper(Server.MapPath("~") + "Project\\Template\\notice.xml");
                 string notice = "";
                 foreach (XmlNode Nodes in XmlColl.GetXmlRoot().SelectNodes("tice"))
                 {
                     notice = Nodes.Attributes["value"].InnerText;
                 }
                 ViewData["Notice"] = notice;
                 ViewData["WebSiteName"] = ConfigurationManager.AppSettings["WebSiteName"];//系统站点名称
                 ViewData["Version"] = ConfigurationManager.AppSettings["Version"];
                 ViewData["copyright"] = "";
                 var model = new BllSysCompany().LoadData(key);
                 if (model != null)
                 {
                     ViewData["copyright"] = model.Name;
                 }

                 int UserCount = 0;//系统用户
                 int CompanyCount = 0;//使用单位
                 int EventCount = 0;//设备总量
                 int LostEvent = 0;//过期设备
                 int LostTime = 0;//超期未检

                 DataSet ds = new BllSysAppointed().Total("");
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     UserCount = int.Parse(ds.Tables[0].Rows[0]["UserCount"].ToString());
                     CompanyCount = int.Parse(ds.Tables[0].Rows[0]["CompanyCount"].ToString());
                     EventCount = int.Parse(ds.Tables[0].Rows[0]["EventCount"].ToString());
                     LostEvent = int.Parse(ds.Tables[0].Rows[0]["LostEvent"].ToString());
                     LostTime = int.Parse(ds.Tables[0].Rows[0]["LostTime"].ToString());
                 }
                 //查询广告列表
                 BllEDynamic bllDynamic = new BllEDynamic();
                 List<ModEDynamic> list = bllDynamic.getListAll(9);
                 //查询待办任务
                 List<ModSysFlow> Flowlist = new BllSysFlow().getListAll(7, "");

                 ViewData["UserCount"] = UserCount;
                 ViewData["CompanyCount"] = CompanyCount;
                 ViewData["EventCount"] = EventCount;
                 ViewData["LostEvent"] = LostEvent;
                 ViewData["LostTime"] = LostTime;
                 ViewData["list"] = list;
                 ViewData["Flowlist"] = Flowlist;

                 return View();
             }
        }
        /// <summary>
        /// 单位管理员 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyIndex()
        {
             var master = new MasterContext().Master;
             if (master == null)
             {
                 return Redirect("/Temple/LoginError");
             }
             else
             {
                 //获取通知公告
                 XMLHealper XmlColl = new XMLHealper(Server.MapPath("~") + "Project\\Template\\notice.xml");
                 string notice = "";
                 foreach (XmlNode Nodes in XmlColl.GetXmlRoot().SelectNodes("tice"))
                 {
                     notice = Nodes.Attributes["value"].InnerText;
                 }
                 ViewData["Notice"] = notice;
                 ViewData["WebSiteName"] = ConfigurationManager.AppSettings["WebSiteName"];//系统站点名称
                 ViewData["Version"] = ConfigurationManager.AppSettings["Version"];
                 ViewData["copyright"] = "";
                 var model = new BllSysCompany().LoadData(key);
                 if (model != null)
                 {
                     ViewData["copyright"] = model.Name;
                 }

                 int UserCount = 0;//系统用户
                 int CompanyCount = 0;//使用单位
                 int EventCount = 0;//设备总量
                 int LostEvent = 0;//过期设备
                 int LostTime = 0;//超期未检

                 DataSet ds = new BllSysAppointed().Total(new MasterContext().Master.Cid);
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     UserCount = int.Parse(ds.Tables[0].Rows[0]["UserCount"].ToString());
                     CompanyCount = int.Parse(ds.Tables[0].Rows[0]["CompanyCount"].ToString());
                     EventCount = int.Parse(ds.Tables[0].Rows[0]["EventCount"].ToString());
                     LostEvent = int.Parse(ds.Tables[0].Rows[0]["LostEvent"].ToString());
                     LostTime = int.Parse(ds.Tables[0].Rows[0]["LostTime"].ToString());
                 }
                 //查询广告列表
                 BllEDynamic bllDynamic = new BllEDynamic();
                 List<ModEDynamic> list = bllDynamic.getListAll(9);
                 //查询待办任务
                 List<ModSysFlow> Flowlist = new BllSysFlow().getListAll(7, " and CompanyId='" + new MasterContext().Master.Cid + "'");

                 ViewData["UserCount"] = UserCount;
                 ViewData["CompanyCount"] = CompanyCount;
                 ViewData["EventCount"] = EventCount;
                 ViewData["LostEvent"] = LostEvent;
                 ViewData["LostTime"] = LostTime;
                 ViewData["list"] = list;

                 ViewData["Flowlist"] = Flowlist;

                 return View();
             }
        }

        /// <summary>
        /// 上级单位 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult ParentIndex()
        {
             var master = new MasterContext().Master;
             if (master == null)
             {
                 return Redirect("/Temple/LoginError");
             }
             else
             {
                 //获取通知公告
                 XMLHealper XmlColl = new XMLHealper(Server.MapPath("~") + "Project\\Template\\notice.xml");
                 string notice = "";
                 foreach (XmlNode Nodes in XmlColl.GetXmlRoot().SelectNodes("tice"))
                 {
                     notice = Nodes.Attributes["value"].InnerText;
                 }
                 ViewData["Notice"] = notice;
                 ViewData["WebSiteName"] = ConfigurationManager.AppSettings["WebSiteName"];//系统站点名称
                 ViewData["Version"] = ConfigurationManager.AppSettings["Version"];
                 ViewData["copyright"] = "";
                 var model = new BllSysCompany().LoadData(key);
                 if (model != null)
                 {
                     ViewData["copyright"] = model.Name;
                 }

                 int UserCount = 0;//系统用户
                 int CompanyCount = 0;//使用单位
                 int EventCount = 0;//设备总量
                 int LostEvent = 0;//过期设备
                 int LostTime = 0;//超期未检

                 DataSet ds = new BllSysAppointed().Total(new MasterContext().Master.Cid);
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     UserCount = int.Parse(ds.Tables[0].Rows[0]["UserCount"].ToString());
                     CompanyCount = int.Parse(ds.Tables[0].Rows[0]["CompanyCount"].ToString());
                     EventCount = int.Parse(ds.Tables[0].Rows[0]["EventCount"].ToString());
                     LostEvent = int.Parse(ds.Tables[0].Rows[0]["LostEvent"].ToString());
                     LostTime = int.Parse(ds.Tables[0].Rows[0]["LostTime"].ToString());
                 }
                 //查询广告列表
                 BllEDynamic bllDynamic = new BllEDynamic();
                 List<ModEDynamic> list = bllDynamic.getListAll(9);
                 //查询待办任务
                 List<ModSysFlow> Flowlist = new BllSysFlow().getListAll(7, " and CompanyId='" + new MasterContext().Master.Cid + "'");

                 ViewData["UserCount"] = UserCount;
                 ViewData["CompanyCount"] = CompanyCount;
                 ViewData["EventCount"] = EventCount;
                 ViewData["LostEvent"] = LostEvent;
                 ViewData["LostTime"] = LostTime;
                 ViewData["list"] = list;

                 ViewData["Flowlist"] = Flowlist;

                 return View();
             }
        }

        /// <summary>
        /// 普通用户 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult UserIndex()
        {
              var master = new MasterContext().Master;
              if (master == null)
              {
                  return Redirect( "/Temple/LoginError");
              }
              else
              {
                  XMLHealper XmlColl = new XMLHealper(Server.MapPath("~") + "Project\\Template\\notice.xml");
                  string notice = "";
                  foreach (XmlNode Nodes in XmlColl.GetXmlRoot().SelectNodes("tice"))
                  {
                      notice = Nodes.Attributes["value"].InnerText;
                  }
                  ViewData["Notice"] = notice;
                  ViewData["WebSiteName"] = ConfigurationManager.AppSettings["WebSiteName"];//系统站点名称
                  ViewData["Version"] = ConfigurationManager.AppSettings["Version"];
                  ViewData["copyright"] = "";
                  var model = new BllSysCompany().LoadData(key);
                  if (model != null)
                  {
                      ViewData["copyright"] = model.Name;
                  }

                  int UserCount = 0;//系统用户
                  int CompanyCount = 0;//使用单位
                  int EventCount = 0;//设备总量
                  int LostEvent = 0;//过期设备
                  int LostTime = 0;//超期未检

                  DataSet ds = new BllSysAppointed().Total(new MasterContext().Master.Cid);
                  if (ds.Tables[0].Rows.Count > 0)
                  {
                      UserCount = int.Parse(ds.Tables[0].Rows[0]["UserCount"].ToString());
                      CompanyCount = int.Parse(ds.Tables[0].Rows[0]["CompanyCount"].ToString());
                      EventCount = int.Parse(ds.Tables[0].Rows[0]["EventCount"].ToString());
                      LostEvent = int.Parse(ds.Tables[0].Rows[0]["LostEvent"].ToString());
                      LostTime = int.Parse(ds.Tables[0].Rows[0]["LostTime"].ToString());
                  }
                  //查询广告列表
                  BllEDynamic bllDynamic = new BllEDynamic();
                  List<ModEDynamic> list = bllDynamic.getListAll(9);

                  ViewData["UserCount"] = UserCount;
                  ViewData["CompanyCount"] = CompanyCount;
                  ViewData["EventCount"] = EventCount;
                  ViewData["LostEvent"] = LostEvent;
                  ViewData["LostTime"] = LostTime;
                  ViewData["list"] = list;

                  return View();
              }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MapContent()
        {
            return View();
        }

        /// <summary>
        /// 中间内容页
        /// </summary>
        /// <returns></returns>
        public ActionResult Content()
        {
            //int UserCount = 0;//工程师数量
            //string TakingCashCount ="0";//交易金额
            //string RunCount ="0";//成交金额
            //int CarCount = 0;//订单数量

            //var content = new MasterContext();
            //if (content.Master != null)
            //{
            //    DataSet ds = null;
            //    if (content.Master.Company.Attribute == (int)CompanyType.客户管理)
            //    {
            //        ds = new BllETask().Total(content.Master.Company.Id);
            //    }
            //    else
            //    {
            //        ds = new BllETask().Total("");
            //    }
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        UserCount = int.Parse(ds.Tables[0].Rows[0]["UserCount"].ToString());
            //        TakingCashCount = Convert.ToDouble(ds.Tables[0].Rows[0]["TakingCashCount"].ToString()).ToString("#0.00");
            //        RunCount = Convert.ToDouble(ds.Tables[0].Rows[0]["RunCount"].ToString()).ToString("#0.00");
            //        CarCount = int.Parse(ds.Tables[0].Rows[0]["CarCount"].ToString());
            //    }
            //}

            //ViewData["UserCount"] = UserCount;
            //ViewData["TakingCashCount"] = TakingCashCount;
            //ViewData["RunCount"] = RunCount;
            //ViewData["CarCount"] = CarCount;

            return View();
        }

        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //防止浏览器再次登录
            var content = new MasterContext();
            if (content.Master != null)
            {
                Response.Redirect("/Home/Index");
            }
            ViewData["WebSiteName"] = ConfigurationManager.AppSettings["WebSiteName"];//系统站点名称

            ViewData["copyright"] = "";
            ViewData["Version"] = ConfigurationManager.AppSettings["Version"];
            var model = new BllSysCompany().LoadData("C0A38317-59DA-4DE2-9428-E5B66EDBC2CF");
            if (model != null)
            {
                ViewData["copyright"] = model.Name;
            }
            return View();
        }

        /// <summary>
        /// 注册页
        /// </summary>
        /// <returns></returns>
        public ActionResult Regedit()
        {
            ViewData["WebSiteName"] = ConfigurationManager.AppSettings["WebSiteName"];//系统站点名称
            ViewData["copyright"] = "";
            ViewData["Version"] = ConfigurationManager.AppSettings["Version"];
            var model = new BllSysCompany().LoadData("C0A38317-59DA-4DE2-9428-E5B66EDBC2CF");
            if (model != null)
            {
                ViewData["copyright"] = model.Name;
            }
            return View();
        }
        /// <summary>
        /// 保存用户注册信息
        /// </summary>
        [ValidateInput(false)]
        public void SaveRegedit()
        {
            ModJsonResult json = new ModJsonResult();

            ModSysMaster master = new ModSysMaster();
            ModSysCompany t = new ModSysCompany();

            #region ===获取管理员信息
            string nickName = Request.Params["UserName"];
            string LoginName = Request.Params["LoginName"];
            string Pwd = Request.Params["pwd"];
            master.Pwd = (string.IsNullOrEmpty(Pwd) ? DESEncrypt.Encrypt("666666") : DESEncrypt.Encrypt(Pwd));
            #endregion

            string UserType = Request["UserType"].ToString();//用户类型
            string HidCompanyType = Request["CompanyType"].ToString();//单位类型

            t.Id = Guid.NewGuid().ToString();

            master.Id = Guid.NewGuid().ToString();
            master.Cid = t.Id;
            master.IsSystem = true;
            master.IsMain = true;
            master.CreaterId = "";
            master.UserName = nickName;
            master.LoginName = LoginName;
            master.Phone = LoginName;
            master.Email = "";
            master.Phone = LoginName;
            master.Status = (int)StatusEnum.未激活;

            new BllSysMaster().ClearCache();

            string titleTip = "";
            if (int.Parse(UserType) == 1)//普通用户
            {
                switch (int.Parse(HidCompanyType))
                {
                    case 1: //社会单位
                        master.Attribute = (int)AdminTypeEnum.单位用户;
                        titleTip = "单位用户";
                        break;
                    case 3: //消防单位
                        master.Attribute = (int)AdminTypeEnum.消防用户;
                        titleTip = "消防用户";
                        break;
                    case 4://维保公司
                        master.Attribute = (int)AdminTypeEnum.维保用户;
                        titleTip = "维保用户";
                        break;
                    case 5://器材供应商
                        master.Attribute = (int)AdminTypeEnum.供应商用户;
                        titleTip = "供应商用户";
                        break;
                }

                string CompanyCode = Request["CompanyCode"].ToString();//单位编号
                string DeptCode = Request["DeptCode"].ToString();//部门编号

                master.OrganizaId = DeptCode;
                master.Cid = CompanyCode;//公司编号

                int result = new BllSysMaster().Insert(master);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
                else
                {
                    //保存用户与单位之间的关系
                    ModSysFlow SysFlow = new ModSysFlow();
                    SysFlow.Id = Guid.NewGuid().ToString();
                    SysFlow.Title = titleTip + ":" + nickName + " 注册申请";
                    SysFlow.FlowType = 2;//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核)
                    SysFlow.FlowStatus = 0;
                    SysFlow.Reamrk = "";
                    SysFlow.ApprovalUser = master.Id;
                    SysFlow.ApprovalTime = DateTime.Now;
                    SysFlow.CompanyId = CompanyCode;
                    SysFlow.MasterId = master.Id;
                    int count = new BllSysFlow().Insert(SysFlow);
                    if (count <= 0)
                    {
                        json.success = false;
                        json.msg = "用户同步流程失败,请稍后再操作!";
                        new BllSysMaster().Delete(master.Id);
                    }
                    else
                    {
                        json.success = true;
                    }
                }
            }
            else
            {
                switch (int.Parse(HidCompanyType))
                {
                    case 1: //社会单位
                        master.Attribute = (int)AdminTypeEnum.单位管理员;
                        t.Attribute = (int)CompanyType.单位;
                        titleTip = "社会单位";
                        break;
                    case 3: //消防单位
                        master.Attribute = (int)AdminTypeEnum.消防部门管理员;
                        t.Attribute = (int)CompanyType.消防部门;
                        titleTip = "消防单位";
                        break;
                    case 4://维保公司
                        master.Attribute = (int)AdminTypeEnum.维保公司管理员;
                        t.Attribute = (int)CompanyType.维保公司;
                        titleTip = "维保单位";
                        break;
                    case 5://器材供应商
                        master.Attribute = (int)AdminTypeEnum.供应商管理员;
                        t.Attribute = (int)CompanyType.供应商;
                        titleTip = "供应商";
                        break;
                }
                master.IsMain = true;
                master.OrganizaId = "0";//部门编号
                int result = new BllSysMaster().Insert(master);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = " 保存失败,请稍后再操作!";
                }
                else
                {
                    string x = Request.Params["Bx"];
                    string y = Request.Params["By"];
                    string CompanyName = Request["CompanyName"].ToString();//用户类型
                    string Address = Request["Address"].ToString();//用户类型
                    string LinkUser = Request["LinkUser"].ToString();//用户类型
                    string LinkPhone = Request["LinkPhone"].ToString();//用户类型

                    t.Phone = LinkPhone;
                    t.LinkUser = LinkUser;
                    t.Address = Address;
                    t.Name = CompanyName;
                    t.CompLat = x;
                    t.ComPLon = y;
                    t.Status = (int)StatusEnum.未激活;
                    t.CreaterUserId = "";
                    t.CreateTime = DateTime.Now;
                    t.ProPic = "/UploadFile/CompanyProPic/default_img_company.png";
                    t.MasterId = master.Id;
                    t.CreateCompanyId = "0";
                    t.RegisiTime = DateTime.Now;//公司注册时间

                    result = new BllSysCompany().Insert(t);

                    if (result <= 0)
                    {
                        json.msg = "保存公司失败,请稍后再操作!";
                        new BllSysMaster().Delete(master.Id);
                    }
                    else
                    {
                        //保存用户与单位之间的关系
                        ModSysFlow SysFlow = new ModSysFlow();
                        SysFlow.Id = Guid.NewGuid().ToString();
                        SysFlow.Title = titleTip+":" + CompanyName + "注册申请";
                        SysFlow.FlowType = 1;//(1:单位管理员注册时的审核 2:普通用户注册审核  3:用户单位变更审核)
                        SysFlow.FlowStatus = 0;
                        SysFlow.Reamrk = "";
                        SysFlow.ApprovalUser = master.Id;
                        SysFlow.ApprovalTime = DateTime.Now;
                        SysFlow.CompanyId = t.Id;
                        SysFlow.MasterId = master.Id;
                        int count = new BllSysFlow().Insert(SysFlow);
                        if (count <= 0)
                        {
                            json.msg = "用户同步流程失败,请稍后再操作!";
                            json.success = false;
                            new BllSysMaster().Delete(master.Id);
                            new BllSysCompany().Delete(t.Id);
                        }
                        else
                        {
                            json.success = true;
                        }
                    }
                }
            }
            Response.Write(json.ToString());
            Response.End();
        }



        #region ===验证码生成 CheckCode
        /// <summary>
        /// 获取验证码
        /// </summary>
        public void CheckCode()
        {
            this.CreateCheckCodeImage(GenerateCheckCode());
        }

        /// <summary>
        /// 5位数
        /// </summary>
        /// <returns></returns>
        private string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            System.Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            Session["CheckCode"] = checkCode;
            return checkCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkCode"></param>
        private void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #endregion

        #region  ===登录验证 LoginInfo| SubmitLogin |IPAddressAll

        /// <summary>
        /// 登录验证
        /// </summary> 
        [ValidateInput(false)]
        public void LoginInfo()
        {
            var msg = new ModJsonResult();
            var loginName = Request["loginName"].Trim();//用户名
            var pwd = Request["pwd"].Trim();//密码
            var code = Request["Code"].Trim();//验证码
            string ip = IPAddressAll();
            try
            {
                //if (Session["CheckCode"] == null || Session["CheckCode"].ToString() == "")
                //{
                //    msg.success = false;
                //    msg.msg = "验证码已过期,请刷新!";
                //}
                //else
                //{
                //    var checkCode = Session["CheckCode"].ToString();
                //    if (code.ToLower() != checkCode.ToLower())
                //    {
                //        msg.success = false;
                //        msg.msg = "验证码输入错误!";
                //    }
                //    else
                //    {
                //获取客户端ip

                //获取服务器电脑名：Page.Server.ManchineName
                //获取用户信息：Page.User
                //获取客户端电脑名：Page.Request.UserHostName
                //获取客户端电脑IP：Page.Request.UserHostAddress
                var result = new MasterContext().Login(loginName, pwd, ip);
                if (result == LoginEnum.登录成功)
                {
                    msg.success = true;
                    msg.msg = "登录成功!";
                }
                else
                {
                    if (result == LoginEnum.公司账号输入错误)
                    {
                        msg.msg = "登录失败,公司账号输入错误";
                    }
                    else if (result == LoginEnum.登录失败)
                    {
                        msg.msg = "登录失败,请稍后重新登录";
                    }
                    else if (result == LoginEnum.密码错误)
                    {
                        msg.msg = "密码错误";
                    }
                    else if (result == LoginEnum.公司帐号被禁用)
                    {
                        msg.msg = "公司帐号被禁用";
                    }
                    else if (result == LoginEnum.帐号被禁用)
                    {
                        msg.msg = "此帐号已被禁用";
                    }
                    else if (result == LoginEnum.帐号不存在)
                    {
                        msg.msg = "帐号不存在";
                    }
                    else if (result == LoginEnum.路径错误)
                    {
                        msg.msg = "请在登录页网址尾部加入公司代码参数";
                    }
                    else if (result == LoginEnum.账户未激活)
                    {
                        msg.msg = "此帐号还未审核.";
                    }
                    msg.success = false;
                }
                //    }
                //}
            }
            catch (Exception ex)
            {
                msg.success = false;
                msg.msg = ex.Message;
            }
            Response.Write(msg.ToString());
            Response.End();
        }

        /// <summary>
        /// 用户登录成功后提交
        /// </summary>
        public void SubmitLogin()
        {
            var loginStates = Request["IsRemenberLoginStates"];
            var txtLoginName = "";
            var txtPwd = "";

            if (loginStates == "on")//记住我
            {
                txtLoginName = Request["txtLoginName"];
                txtPwd = Request["txtPwd"];
            }

            var cookie = new HttpCookie("CustomOrderLoginName ", txtLoginName)
            {
                Expires = DateTime.Now.AddDays(10)
            };
            Response.Cookies.Add(cookie);//将cookie写入客户端

            var CustomOrderPwd = new HttpCookie("CustomOrderPwd ", txtPwd)
            {
                Expires = DateTime.Now.AddDays(10)
            };
            Response.Cookies.Add(CustomOrderPwd);//将cookie写入客户端

            var IsLoginStates = new HttpCookie("IsLoginStates ", loginStates)
            {
                Expires = DateTime.Now.AddDays(10)
            };
            Response.Cookies.Add(IsLoginStates);//将cookie写入客户端

            //add Log
            //var master = new MasterContext().Master;
            //new CommonFun().CreateLog(master.Id, master.LoginName, IPAddressAll(), HttpContext.Request.Url.ToString(), "用户登录");

            Response.Redirect("/Home/Index");
        }

        /// <summary>
        /// 获取客户端ip地址
        /// </summary>
        /// <returns></returns>
        public string IPAddressAll()
        {
            string result = String.Empty;
            try
            {
                result = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                {
                    result = Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(result))
                {
                    result = Request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(result))
                {
                    return "127.0.0.1";
                }
            }
            catch
            {
                return "127.0.0.1";
            }
            return result;
        }

        #endregion

        #region ===退出 logOut
        /// <summary>
        /// 退出
        /// </summary>
        public void logOut()
        {
            var msg = new ModJsonResult();
            try
            {
                HttpContext.Session.Clear();
                int count = int.Parse(HttpContext.Application["user_online"].ToString());
                if (count > 1)
                {
                    HttpContext.Application["user_online"] = int.Parse(HttpContext.Application["user_online"].ToString()) - 1;
                }
                new BllSysMaster().LoginOut();
                msg.success = true;
            }
            catch
            {
                msg.success = false;
            }
            Response.Write(msg.ToString());
            Response.End();
        }

        #endregion

        #region ==密码重置 ReSetPwd
        /// <summary>
        /// 密码重置
        /// </summary>
        [AcceptVerbs(HttpVerbs.Post)]
        public void ReSetPwd()
        {
            var json = new ModJsonResult();
            try
            {
                string id = Request["id"];
                string pwd = DESEncrypt.Encrypt("666666");
                int result = new BllSysMaster().ReSetPwd(id, pwd);
                if (result <= 0)
                {
                    json.success = false;
                    json.msg = "重置失败!";
                }
                else
                {
                    json.success = true;
                    json.msg = "重置成功!";
                }
            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = "操作失败!" + ex.Message;
            }
            Response.Write(new JavaScriptSerializer().Serialize(json).ToString());
            Response.End();
        }

        #endregion

        #region --左边导航菜单 GetTree
        /// <summary>
        /// 获取左边导航菜单
        /// </summary>
        [HttpPost]
        public void GetTree()
        {
            string childId = Request["id"];
            var master = new MasterContext().Master;
            if (master != null)
            {
                //获取菜单访问权限
                if (master.IsMain) //超级管理员,不用控权
                {
                    InitAdminTree(childId, master);
                }
                else
                {
                    InitRoleTree(childId, master);
                }
            }
            else
            {
                var json = new ModJsonResult();
                json.success = false;
                json.errorCode = (int)SystemError.用户过期错误;
                Response.Write(new JavaScriptSerializer().Serialize(json).ToString());
                Response.End();
            }
        }


        /// <summary>
        ///菜单树,没有权限的菜单
        /// </summary>
        /// <param name="childId"></param>
        /// <param name="master">登录用户对象</param>
        public void InitAdminTree(string childId, ModSysMaster master)
        {
            string output = "";
            string _where = "1=1";
            childId = (childId == "-1" ? "0" : childId);
            //获取
            try
            {
                //获取菜单访问权限
                int att = master.Attribute;
                _where += " and TypeID=" + att;

                BllSysFun mybase = new BllSysFun();

                string name = (Request["FunName"] == null ? "" : Request["FunName"]).Trim(); //查询过滤条件

                _where += (name == "" ? " and Status=" + (int)StatusEnum.正常 : " and Status=" + (int)StatusEnum.正常 + " and FunName like '%" + name.Trim() + "%'");

                DataSet _mySet = mybase.GetTreeList(_where);

                if (name == "")
                {
                    output = JsonHelper.ToJson(new FunTreeCommon().GetFunTreeNodes(_mySet));
                }
                else
                {
                    output = JsonHelper.ToJson(new FunTreeCommon().GetSearchTreeNodes(_mySet));
                }

            }
            catch
            {
                var json = new ModJsonResult();
                json.success = false;
                json.errorCode = (int)SystemError.正常错误;
                json.msg = "菜单树异常,无法进行操作";
                output = JsonHelper.ToJson(json);
            }
            Response.Write(output);
            Response.End();
        }

        /// <summary>
        /// 菜单树,有权限的菜单
        /// </summary>
        /// <param name="childId"></param>
        /// <param name="master">登录用户对象</param>
        public void InitRoleTree(string childId, ModSysMaster master)
        {
            string output = "";
            string _where = "1=1";
            //获取
            try
            {
                BllSysMaster mybase = new BllSysMaster();

                string name = (Request["name"] == null ? "" : Request["name"]); //查询过滤条件

                _where += (name == "" ? " and Status=" + (int)StatusEnum.正常 : "  and Status=" + (int)StatusEnum.正常 + " and FunName like '%" + name + "%'");

                if (!String.IsNullOrEmpty(master.RoleIdList))
                {
                    DataSet _mySet = mybase.GetAuthByPage(master.RoleIdList, _where);

                    if (_mySet.Tables.Count > 0)
                    {
                        if (name == "")
                        {
                            output = JsonHelper.ToJson(new FunTreeCommon().GetFunTreeNodes(_mySet));
                        }
                        else
                        {
                            output = JsonHelper.ToJson(new FunTreeCommon().GetSearchTreeNodes(_mySet));
                        }
                    }
                    else
                    {
                        var msg = new ModJsonResult();
                        msg.success = false;
                        msg.msg = "您暂未有访问权限";
                        output = JsonHelper.ToJson(msg);
                    }
                }
                else
                {
                    var msg = new ModJsonResult();
                    msg.success = false;
                    msg.msg = "您暂未有访问权限";
                    output = JsonHelper.ToJson(msg);
                }
            }
            catch
            {
                var msg = new ModJsonResult();
                msg.success = false;
                output = JsonHelper.ToJson(msg);
            }
            Response.Write(output);
            Response.End();
        }


        #endregion

        #region ===获取用户信息 GetLoginUser
        /// <summary>
        /// 获取用户信息 
        /// </summary>
        public void GetLoginUser()
        {
            var master = new MasterContext().Master;
            var json = new ModJsonResult();

            if (master != null)
            {
                ModSysMaster userInfo = master;
                //var userId = Request["userId"] == null ? "" : Request["userId"].ToString(CultureInfo.InvariantCulture);
                //if (userId.Length > 0)
                //{
                //    var masterLogic = new BllSysMaster();
                //    userInfo = masterLogic.LoadData(userId);
                //    masterLogic.LoadMasterPower(userInfo);
                //}
                //else
                //{
                //    userInfo = master;
                //}
                json.success = true;
                json.msg = JsonHelper.ToJson(userInfo);
            }
            else
            {
                json.errorCode = (int)SystemError.用户过期错误;
                json.success = false;
                json.msg = "用户信息已过期,请重新登录!";
            }
            Response.Write(new JavaScriptSerializer().Serialize(json).ToString());
            Response.End();
        }

        #endregion

        #region ==密码修改 UpdatePwd
        /// <summary>
        /// 修改密码
        /// </summary>
        [AcceptVerbs(HttpVerbs.Post)]
        public void UpdatePwd()
        {
            var json = new ModJsonResult();
            try
            {
                string oldpwd = Request["oldpwd"];
                string newpwd = Request["newpwd"];
                string newpwd2 = Request["newpwd2"];
                var CurrentMaster = new MasterContext().Master;
                if (DESEncrypt.Decrypt(CurrentMaster.Pwd) == oldpwd.Trim())
                {
                    BllSysMaster bll = new BllSysMaster();
                    if (DESEncrypt.Decrypt(CurrentMaster.Pwd) == newpwd)
                    {
                        json.success = false;
                        json.msg = "修改密码不能和原密码相同!";
                    }
                    else
                    {
                        string pwd = DESEncrypt.Encrypt(newpwd);
                        int result = bll.upPwd(CurrentMaster.Id, pwd);
                        if (result <= 0)
                        {
                            json.success = false;
                            json.msg = "密码修改失败!";
                        }
                        else
                        {
                            json.success = true;
                            json.msg = "修改成功!";
                            CurrentMaster.Pwd = pwd; //重新赋值密码
                        }
                    }
                }
                else
                {
                    json.success = false;
                    json.msg = "原密码输入错误!";
                }

            }
            catch (Exception ex)
            {
                json.success = false;
                json.msg = "操作失败!"+ex.Message;
            }
            Response.Write(new JavaScriptSerializer().Serialize(json).ToString());
            Response.End();
        }

        #endregion

        #region ==帮助
        /// <summary>
        /// 系统使用帮助信息
        /// </summary>
        public void Help()
        {
            ////根据路径调用系统说明文档
            //string str2 = HttpContext.Server.MapPath("/Home/南宁商城后台说明.chm");
            //Process process = new Process();
            //process.StartInfo.FileName = str2;
            //process.Start();
        }

        #endregion
    }
}
