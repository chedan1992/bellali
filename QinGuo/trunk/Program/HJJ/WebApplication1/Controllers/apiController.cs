using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Commonlication.Controllers
{
    [Route("api/book/")]
    [EnableCors("AllowAllOrigin")]
    public class ApiController : BaseController
    {
        //获取用户信息
        [HttpGet("GetUser/{id}")]
        public IActionResult GetUser(int id)
        {
            var r = new RequestModel();
            try
            {
                r.data = db.Users.Find(id);
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }


        //获取用户信息
        [HttpGet("GetUserList")]
        public IActionResult GetUserList(string name = "", int pagesize = 1, int pageno = 1)
        {
            var r = new RequestModel();
            try
            {

                var d = (from e in db.Users
                         where e.Status != -100
                         select e);
                if (!String.IsNullOrEmpty(name))
                {
                    d = d.Where(s => s.Name.Contains(name));
                }

                r.data = d.Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取用户信息
        [HttpPost("Login")]
        public IActionResult Login(string loginname, string password)
        {
            var r = new RequestModel();
            try
            {
                r.data = db.Users.FirstOrDefault(c => c.LoginName == loginname && c.Pwd == password);
                r.code = 1;
                if (r.data == null)
                {
                    r.code = 0;
                    r.msg = "用户名或密码错误";
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        #region 操作SysConfig


        //获取配置信息
        [HttpGet("GetSysConfig/{id}")]
        public IActionResult GetSysConfig(int id)
        {
            var r = new RequestModel();
            try
            {
                r.data = db.SysConfigs.Find(id);
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改配置信息
        [HttpPost("SaveSysConfig")]
        public IActionResult Post(SysConfig s)
        {
            var r = new RequestModel();
            try
            {
                s.CreateTime = DateTime.Now;
                //修改
                db.SysConfigs.Update(s);
                if (db.SaveChanges() > 0)
                {
                    r.code = 1;
                }
                else
                {
                    r.msg = "保存失败";
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        #endregion

        #region 操作Book


        //获取书籍信息
        [HttpGet("GetBook/{id}")]
        public IActionResult GetBook(int id)
        {
            var r = new RequestModel();
            try
            {
                r.data = db.Books.Find(id);
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改书籍信息
        [HttpPost("SaveBook")]
        public IActionResult SaveBook(Book s)
        {
            var r = new RequestModel();
            try
            {
                s.CreateTime = DateTime.Now;
                s.Status = 1;
                if (s.Id > 0)
                {
                    //修改
                    db.Books.Update(s);
                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                    else
                    {
                        r.msg = "保存失败";
                    }
                }
                else
                {
                    //新增
                    db.Books.Add(s);
                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                    else
                    {
                        r.msg = "保存失败";
                    }
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改书籍信息
        [HttpPost("DelBooks")]
        public IActionResult DelBooks(string ids)
        {
            var r = new RequestModel();
            try
            {
                string[] idss = ids.Split(',');
                var rint = 0;
                for (int i = 0; i < idss.Length; i++)
                {
                    Book book = db.Books.Find(Convert.ToInt32(idss[i]));
                    book.Status = -1;
                    db.Books.Update(book);
                    if (db.SaveChanges() > 0)
                    {
                        rint++;
                    }
                }
                r.data = rint;
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取书籍
        [HttpGet("GetBookList")]
        public IActionResult GetBookList(string name = "", string author = "", int pagesize = 1, int pageno = 1)
        {
            var r = new RequestModel();
            try
            {

                var d = (from e in db.Books
                         where e.Status == 1
                         select e);
                if (!String.IsNullOrEmpty(name))
                {
                    d = d.Where(s => s.Name.Contains(name));
                }

                if (!String.IsNullOrEmpty(author))
                {
                    d = d.Where(s => s.Author.Contains(author));
                }

                r.data = d.Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }
        #endregion


        #region 操作Voluntary


        //获取志愿者报名信息
        [HttpGet("GetVoluntary/{id}")]
        public IActionResult GetVoluntary(int id)
        {
            var r = new RequestModel();
            try
            {
                r.data = db.Voluntarys.Find(id);
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改志愿者报名信息
        [HttpPost("SaveVoluntary")]
        public IActionResult SaveVoluntary(Voluntary s)
        {
            var r = new RequestModel();
            try
            {
                s.CreateTime = DateTime.Now;
                if (s.Id > 0)
                {
                    //修改
                    db.Voluntarys.Update(s);
                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                    else
                    {
                        r.msg = "保存失败";
                    }
                }
                else
                {
                    //新增
                    db.Voluntarys.Add(s);
                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                    else
                    {
                        r.msg = "保存失败";
                    }
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取志愿者报名
        [HttpGet("GetVoluntaryList")]
        public IActionResult GetVoluntaryList(string name = "", string phone = "", int userid = 0, int pagesize = 1, int pageno = 1)
        {
            var r = new RequestModel();
            try
            {

                var d = (from e in db.Voluntarys
                         select e);
                if (!String.IsNullOrEmpty(name))
                {
                    d = d.Where(s => s.Name.Contains(name));
                }

                if (!String.IsNullOrEmpty(phone))
                {
                    d = d.Where(s => s.Phone.Contains(phone));
                }

                if (userid > 0)
                {
                    d = d.Where(s => s.UserId == userid);
                }

                r.data = d.Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }
        #endregion

        #region 操作Active


        //获取活动信息
        [HttpGet("GetActiveListall")]
        public IActionResult GetActiveListall()
        {
            var r = new RequestModel();
            try
            {
                List<Active> rlist = db.Actives.Where(c => c.Status != -1).ToList();
                r.data = rlist;
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
                Log.WriteLog("GetActive:" + e.Message);
            }
            return Ok(r);
        }

        //获取活动信息
        [HttpGet("GetActive/{id}")]
        public IActionResult GetActive(int id)
        {
            var r = new RequestModel();
            try
            {
                Active active = db.Actives.Find(id);
                r.data = new { active = active };
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
                Log.WriteLog("GetActive:" + e.Message);
            }
            return Ok(r);
        }

        //新增活动信息
        [HttpPost("SaveActive")]
        public IActionResult SaveActive(Active s, string activeone)
        {
            var r = new RequestModel();
            try
            {
                //新增
                s.CreateTime = DateTime.Now;
                if (s.Id == 0)
                {
                    s.Status = 1;
                    db.Actives.Add(s);

                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                }
                else
                {
                    db.Actives.Update(s);
                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                }
                r.code = 1;

                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改活动信息
        [HttpPost("UpdateActiveStatus")]
        public IActionResult UpdateActiveStatus(string ids, int status)
        {
            var r = new RequestModel();
            try
            {
                string[] idss = ids.Split(',');
                var rint = 0;
                for (int i = 0; i < idss.Length; i++)
                {
                    if (!string.IsNullOrEmpty(idss[i]))
                    {
                        Active active = db.Actives.Find(Convert.ToInt32(idss[i]));
                        if (active != null)
                        {
                            active.Status = status;
                            db.Actives.Update(active);
                            if (db.SaveChanges() > 0)
                            {
                                rint++;
                            }
                        }
                    }
                }
                r.code = rint;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取活动
        [HttpGet("GetActiveList")]
        public IActionResult GetActiveList(string title, int pagesize = 1, int pageno = 1, int status = 0)
        {
            var r = new RequestModel();
            try
            {

                var d = (from e in db.Actives
                         where e.Status >= 1
                         select e);
                if (!String.IsNullOrEmpty(title))
                {
                    d = d.Where(s => s.Title.Contains(title));
                }

                List<object> rs = new List<object>();
                List<Active> rlist = d.Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();
                r.data = rlist;
                r.code = 1;
                r.totalCount = d.Count();
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }
        #endregion


        #region 操作Catory
        //新增活动信息
        [HttpPost("SaveCatory")]
        public IActionResult SaveCatory(Catory s)
        {
            var r = new RequestModel();
            try
            {
                //新增
                s.CreateTime = DateTime.Now;
                s.Status = 2;
                if (s.Id == 0)
                {
                    db.Catorys.Add(s);

                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                }
                else
                {
                    db.Catorys.Update(s);
                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                }

                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改活动信息
        [HttpPost("UpdateCatoryStatus")]
        public IActionResult UpdateCatoryStatus(string ids, int status)
        {
            var r = new RequestModel();
            try
            {
                string[] idss = ids.Split(',');
                var rint = 0;
                for (int i = 0; i < idss.Length; i++)
                {
                    if (!string.IsNullOrEmpty(idss[i]))
                    {
                        Catory catory = db.Catorys.Find(Convert.ToInt32(idss[i]));
                        if (catory != null)
                        {
                            catory.Status = status;
                            db.Catorys.Update(catory);
                            if (db.SaveChanges() > 0)
                            {
                                rint++;
                            }
                        }
                    }
                }
                r.code = rint;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取活动
        [HttpGet("GetCatoryList")]
        public IActionResult GetCatoryList(string title, int pagesize = 1, int pageno = 1, int status = 0)
        {
            var r = new RequestModel();
            try
            {

                var d = (from e in db.Catorys
                         where e.Status >= 1
                         select e);
                if (!String.IsNullOrEmpty(title))
                {
                    d = d.Where(s => s.Title.Contains(title));
                }

                List<object> rs = new List<object>();
                List<Catory> rlist = d.Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();

                r.data = rlist;
                r.code = 1;
                r.totalCount = d.Count();
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取活动
        [HttpGet("GetCatoryListAll")]
        public IActionResult GetCatoryListAll(int status = 2, int type = 0)
        {
            var r = new RequestModel();
            try
            {
                var d = (from e in db.Catorys
                         where e.Status == status
                         select e);

                if (type > 0)
                {
                    d = d.Where(c => c.Type == type);
                }

                List<Catory> rlist = d.ToList();
                r.data = rlist;
                r.code = 1;
                r.totalCount = d.Count();
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }
        #endregion


        #region 操作School
        //新增活动信息
        [HttpPost("SaveSchool")]
        public IActionResult SaveSchool(School s)
        {
            var r = new RequestModel();
            try
            {
                //新增
                s.CreateTime = DateTime.Now;
                s.Status = 2;
                if (s.Id == 0)
                {
                    db.Schools.Add(s);

                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                }
                else
                {
                    db.Schools.Update(s);
                    if (db.SaveChanges() > 0)
                    {
                        r.code = 1;
                    }
                }

                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改活动信息
        [HttpPost("UpdateSchoolStatus")]
        public IActionResult UpdateSchoolStatus(string ids, int status)
        {
            var r = new RequestModel();
            try
            {
                string[] idss = ids.Split(',');
                var rint = 0;
                for (int i = 0; i < idss.Length; i++)
                {
                    if (!string.IsNullOrEmpty(idss[i]))
                    {
                        School school = db.Schools.Find(Convert.ToInt32(idss[i]));
                        if (school != null)
                        {
                            school.Status = status;
                            db.Schools.Update(school);
                            if (db.SaveChanges() > 0)
                            {
                                rint++;
                            }
                        }
                    }
                }
                r.code = rint;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取活动
        [HttpGet("GetSchoolList")]
        public IActionResult GetSchoolList(string name, int pagesize = 1, int pageno = 1, int status = 0)
        {
            var r = new RequestModel();
            try
            {

                var d = (from e in db.Schools
                         where e.Status >= 1
                         select e);
                if (!String.IsNullOrEmpty(name))
                {
                    d = d.Where(s => s.Name.Contains(name));
                }

                List<object> rs = new List<object>();
                List<School> rlist = d.Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();

                r.data = rlist;
                r.code = 1;
                r.totalCount = d.Count();
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取活动
        [HttpGet("GetSchoolListAll")]
        public IActionResult GetSchoolListAll(int status = 2)
        {
            var r = new RequestModel();
            try
            {
                var d = (from e in db.Schools
                         where e.Status == status
                         select e);
                List<School> rlist = d.ToList();
                r.data = rlist;
                r.code = 1;
                r.totalCount = d.Count();
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }
        #endregion

        #region 操作Order


        //新增活动报名信息
        [HttpPost("AddOrder")]
        public IActionResult AddOrder(Order s)
        {
            var r = new RequestModel();
            try
            {

                if (db.Orders.FirstOrDefault(c => c.Card == s.Card && c.ActiveId == s.ActiveId && s.Status == 1) != null)
                {
                    r.code = 0;
                    r.msg = "您已经报过名了";
                    return Ok(r);
                }

                var user = db.Users.Find(s.UserId);

                if (user == null)
                {
                    r.code = 0;
                    r.msg = "用户信息错误";
                    return Ok(r);
                }
                //新增
                s.CreateTime = DateTime.Now;
                s.PayTime = DateTime.Now;
                s.Payno = WxPayAPI.WxPayApi.GenerateOutTradeNo();

                s.Status = 0;
                //Active active = db.Actives.Find(s.ActiveId);
                Catory catory = db.Catorys.Find(s.ClassId);

                s.Grade = catory.Title;
                s.Money = catory.Price;
                s.PayMoney = catory.Price;

                if (s.Id > 0)
                {
                    db.Orders.Update(s);
                }
                else
                {
                    db.Orders.Add(s);
                }

                if (db.SaveChanges() > 0)
                {
                    //Log.WriteLog("11111111:SUCCESS");
                    //生成与支付编号
                    unifiedOrderResult = GetUnifiedOrderResult(s.PayMoney, user.LoginName, s.Payno);
                    //Log.WriteLog("11111111:222222222222222222");

                    WxPayData jsApiParam = new WxPayData();
                    jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
                    jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                    jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                    jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
                    jsApiParam.SetValue("signType", "MD5");
                    jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

                    r.data = jsApiParam.ToJson();
                    r.code = 1;
                    r.msg = s.Payno;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }


        //新增活动报名信息
        [HttpPost("AddOrdered")]
        public IActionResult AddOrdered(Order s)
        {
            var r = new RequestModel();
            try
            {

                if (db.Orders.FirstOrDefault(c => c.Card == s.Card && c.ActiveId == s.ActiveId && s.Status == 1) != null)
                {
                    r.code = 0;
                    r.msg = "您已经报过名了";
                    return Ok(r);
                }

                var user = db.Users.Find(s.UserId);

                if (user == null)
                {
                    r.code = 0;
                    r.msg = "用户信息错误";
                    return Ok(r);
                }
                //新增
                s.CreateTime = DateTime.Now;
                s.PayTime = DateTime.Now;
                s.Payno = WxPayAPI.WxPayApi.GenerateOutTradeNo();

                s.Status = 0;
                //Active active = db.Actives.Find(s.ActiveId);
                Catory catory = db.Catorys.Find(s.ClassId);

                s.Grade = catory.Title;
                s.Money = catory.Price;
                s.PayMoney = 0.01M;

                if (s.Id > 0)
                {
                    db.Orders.Update(s);
                }
                else
                {
                    db.Orders.Add(s);
                }

                if (db.SaveChanges() > 0)
                {
                    //Log.WriteLog("11111111:SUCCESS");
                    //生成与支付编号
                    unifiedOrderResult = GetUnifiedOrderResult(s.PayMoney, user.LoginName, s.Payno);
                    //Log.WriteLog("11111111:222222222222222222");

                    WxPayData jsApiParam = new WxPayData();
                    jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
                    jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                    jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                    jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
                    jsApiParam.SetValue("signType", "MD5");
                    jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

                    r.data = jsApiParam.ToJson();
                    r.code = 1;
                    r.msg = s.Payno;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取订单信息
        [HttpGet("GetOrder/{id}")]
        public IActionResult GetOrder(int id)
        {
            var r = new RequestModel();
            try
            {
                Order order = db.Orders.Find(id);
                if (order != null)
                {
                    User user = db.Users.Find(order.UserId);
                    Active active = db.Actives.Find(order.ActiveId);

                    r.data = new { order = order, user = user, active = active };
                    r.code = 1;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取订单信息
        [HttpGet("GetOrderActive/{activeid}")]
        public IActionResult GetOrderActive(int activeid, int userid)
        {
            var r = new RequestModel();
            try
            {
                Order order = db.Orders.FirstOrDefault(c => c.ActiveId == activeid && c.UserId == userid);
                if (order != null)
                {
                    User user = db.Users.Find(order.UserId);
                    Active active = db.Actives.Find(order.ActiveId);
                    r.data = new { order = order, user = user, active = active };
                    r.code = 1;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }


        //获取订单信息
        [HttpGet("GetOrderPayno/{payno}")]
        public IActionResult GetOrderPayno(string payno)
        {
            var r = new RequestModel();
            try
            {
                Order order = db.Orders.FirstOrDefault(c => c.Payno == payno);
                if (order != null)
                {
                    User user = db.Users.Find(order.UserId);
                    Active active = db.Actives.Find(order.ActiveId);
                    r.data = new { order = order, user = user, active = active };
                    r.code = 1;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }


        //获取订单信息
        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList(string payno, string name, string phone, int activeid, int pagesize = 1, int pageno = 1, int status = 1)
        {
            var r = new RequestModel();
            try
            {
                var d = (from e in db.Orders
                         where e.Status >= 0
                         select e);
                if (!string.IsNullOrEmpty(payno))
                {
                    d = d.Where(s => s.Payno.Contains(payno));
                }
                if (!string.IsNullOrEmpty(name))
                {
                    d = d.Where(s => s.RealName.Contains(name));
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    d = d.Where(s => s.Phone.Contains(phone));
                }

                if (activeid > 0)
                {
                    d = d.Where(s => s.ActiveId == activeid);
                }

                d = d.Where(c => c.Status == status);

                List<object> rs = new List<object>();
                List<Order> rlist = d.OrderByDescending(c => c.PayTime).Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();

                foreach (var item in rlist)
                {
                    User user = db.Users.Find(item.UserId);
                    Active active = db.Actives.Find(item.ActiveId);
                    rs.Add(new { order = item, user = user, active = active });
                }
                r.data = rs;
                r.code = 1;
                r.totalCount = d.Count();
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //获取订单信息
        [HttpGet("OutputExcel")]
        public IActionResult OutputExcel(string payno, string name, string phone, int activeid, int pagesize = 15, int pageno = 1, int status = 1)
        {
            var r = new RequestModel();
            try
            {
                var d = (from e in db.Orders
                         where e.Status >= 0
                         select e);
                if (!string.IsNullOrEmpty(payno))
                {
                    d = d.Where(s => s.Payno.Contains(payno));
                }
                if (!string.IsNullOrEmpty(name))
                {
                    d = d.Where(s => s.RealName.Contains(name));
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    d = d.Where(s => s.Phone.Contains(phone));
                }

                if (activeid > 0)
                {
                    d = d.Where(s => s.ActiveId == activeid);
                }
                d = d.Where(c => c.Status == status);
                List<Order> rlist = d.OrderByDescending(c=>c.PayTime).Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();

                List<string> title = new List<string>();
                title.Add("订单号");
                title.Add("姓名");
                title.Add("性别");
                title.Add("电话");
                title.Add("国际");
                title.Add("名族");
                title.Add("就读学校");
                title.Add("身份证号");
                title.Add("校区");
                title.Add("级别");
                title.Add("金额");
                title.Add("寸照");
                title.Add("报名时间");

                r.code = 1;
                return File(Common.OutputExcel(rlist, title, _hostingEnvironment.ContentRootPath), "application/ms-excel", "list.xls");
            }
            catch (Exception e)
            {
                r.msg = e.Message;
                Log.WriteLog("OutputExcel：" + e.Message);
            }
            return Ok(r);
        }


        //获取订单信息
        [HttpGet("GetMyActive")]
        public IActionResult GetMyActive(int userid, int pagesize = 1, int pageno = 1, int status = 0)
        {
            var r = new RequestModel();
            try
            {
                var d = (from e in db.Orders
                         where e.Status >= 0
                         select e);
                if (userid > 0)
                {
                    d = d.Where(s => s.UserId == userid);
                }

                List<object> rs = new List<object>();
                List<Order> rlist = d.Skip(pagesize * (pageno - 1)).Take(pagesize).ToList();

                foreach (var item in rlist)
                {
                    User user = db.Users.Find(item.UserId);
                    Active active = db.Actives.Find(item.ActiveId);
                    rs.Add(new { order = item, user = user, active = active });
                }
                r.data = rs;
                r.code = 1;
                r.totalCount = d.Count();
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改订单信息
        [HttpPost("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(string ids, int status)
        {
            var r = new RequestModel();
            try
            {
                string[] idss = ids.Split(',');
                var rint = 0;
                for (int i = 0; i < idss.Length; i++)
                {
                    if (!string.IsNullOrEmpty(idss[i]))
                    {
                        Order order = db.Orders.Find(Convert.ToInt32(idss[i]));
                        if (order != null)
                        {
                            order.Status = status;
                            db.Orders.Update(order);
                            if (db.SaveChanges() > 0)
                            {
                                rint++;
                            }
                        }
                    }
                }
                r.code = rint;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        //修改订单信息
        [HttpPost("UpdateOrderSucc")]
        public IActionResult UpdateOrderSucc(string payno, int status)
        {
            var r = new RequestModel();
            try
            {
                Order order = db.Orders.First(c => c.Payno == payno);
                if (order != null && order.Status == 0)
                {
                    order.Status = status;
                    order.PayTime = new DateTime();
                    db.Orders.Update(order);
                    if (db.SaveChanges() > 0)
                    {
                        var active = db.Actives.Find(order.ActiveId);
                        active.Count++;
                        db.Actives.Update(active);
                        db.SaveChanges();
                        r.code = 1;
                        return Ok(r);
                    }
                }
                else
                {
                    Log.WriteLog("UpdateOrderSucc：null");
                }
                r.code = 0;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
                Log.WriteLog("UpdateOrderSucc：" + e.Message);
            }
            return Ok(r);
        }
        #endregion

        #region Comm

        //上传 文件
        [HttpPost("Up")]
        public IActionResult Up()
        {
            var r = new RequestModel();
            try
            {
                List<string> rpath = new List<string>();
                long size = 0;
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    string fileExt = file.FileName.Split('.')[1]; //文件扩展名，不含“.”
                    string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名

                    var fileDir = Path.Combine("", "Files\\Image");
                    if (!Directory.Exists(fileDir))
                    {
                        Directory.CreateDirectory(fileDir);
                    }
                    string filePath = fileDir + $@"\{newFileName}";
                    size += file.Length;
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                        rpath.Add("/Files/Image/" + newFileName);
                    }
                }
                r.data = rpath;
                if (rpath.Count > 0)
                {
                    r.msg = "上传成功！";
                    r.code = 1;
                }
                else
                {
                    r.code = 0;
                    r.msg = "上传失败！";
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }


        //上传 文件
        [HttpPost("UpEdit")]
        public IActionResult UpEdit()
        {
            var r = new RequestModel();
            try
            {
                string rpath = "";
                long size = 0;
                var files = Request.Form.Files;
                foreach (var file in files)
                {
                    string fileExt = file.FileName.Split('.')[1]; //文件扩展名，不含“.”
                    string newFileName = Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名

                    var fileDir = Path.Combine("", "Files\\Image");
                    if (!Directory.Exists(fileDir))
                    {
                        Directory.CreateDirectory(fileDir);
                    }
                    string filePath = fileDir + $@"\{newFileName}";
                    size += file.Length;
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                        rpath = "/Files/Image/" + newFileName;
                    }
                }
                if (!string.IsNullOrEmpty(rpath))
                {
                    r.data = new { src = "http://hjj.bellali.cn" + rpath };
                    r.msg = "上传成功！";
                    r.code = 1;
                }
                else
                {
                    r.code = 0;
                    r.msg = "上传失败！";
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return Ok(r);
        }

        #endregion

        #region 微信 

        /// <summary>
        /// 统一下单接口返回结果
        /// </summary>
        public WxPayAPI.WxPayData unifiedOrderResult { get; set; }

        /**
         * 调用统一下单，获得下单结果
         * @return 统一下单结果
         * @失败时抛异常WxPayException
         */
        public WxPayAPI.WxPayData GetUnifiedOrderResult(decimal total_fee, string openid, string out_trade_no)
        {
            //统一下单
            WxPayAPI.WxPayData data = new WxPayAPI.WxPayData();
            data.SetValue("body", "四川红领巾少儿艺术团-报名活动");
            data.SetValue("attach", "test");
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", Convert.ToInt32(total_fee * 100));
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", "test");
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openid);

            Log.WriteLog("GetUnifiedOrderResult!" + total_fee);

            WxPayAPI.WxPayData result = WxPayAPI.WxPayApi.UnifiedOrder(data);
            if (!result.IsSet("appid") || !result.IsSet("prepay_id") || result.GetValue("prepay_id").ToString() == "")
            {
                Log.WriteLog("UnifiedOrder response error!");
                throw new WxPayException("UnifiedOrder response error!");
            }

            Log.WriteLog("GetUnified77777");
            unifiedOrderResult = result;
            return result;
        }


        public string GetToken()
        {
            try
            {
                StringBuilder userurl = new StringBuilder(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", APPID, APPSECRET));
                string userjson = HttpService.Get(userurl.ToString());

                Log.WriteLog("userjson:" + userjson);
                if (!string.IsNullOrEmpty(userjson))
                {
                    var tokens = JsonConvert.DeserializeObject<ResultToken>(userjson);

                    if (tokens.expires_in == 7200)
                    {
                        CacheHelper.Set("tokens", tokens.access_token, 7200);
                        return tokens.access_token;
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);
                return "";
            }
        }

        [HttpGet("GetSignature")]
        public IActionResult GetSignature(string url)
        {
            var r = new RequestModel();
            try
            {
                string token = CacheHelper.Get("tokens") != null ? CacheHelper.Get("tokens").ToString() : "";
                if (string.IsNullOrEmpty(token))
                {
                    token = GetToken();
                }

                string tic = CacheHelper.Get("ticket") != null ? CacheHelper.Get("ticket").ToString() : "";
                if (string.IsNullOrEmpty(tic) && !string.IsNullOrEmpty(token))
                {
                    StringBuilder userurl = new StringBuilder(string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", token));
                    string data = HttpService.Get(userurl.ToString());

                    if (!string.IsNullOrEmpty(data))
                    {
                        ResultTicket ticket = JsonConvert.DeserializeObject<ResultTicket>(data);
                        if (ticket != null && ticket.expires_in == 7200)
                        {
                            tic = ticket.ticket.ToString();
                            CacheHelper.Set("ticket", tic, 7200);
                        }
                    }
                    else
                    {
                        GetToken();
                    }
                }
                if (!string.IsNullOrEmpty(tic))
                {
                    string timeStamp = WxPayAPI.WxPayApi.GenerateTimeStamp();
                    string nonceStr = WxPayAPI.WxPayApi.GenerateNonceStr();
                    string sing = GetSignature(tic, timeStamp, nonceStr, url);

                    r.data = new
                    {
                        appId = WxPayAPI.WxPayConfig.GetConfig().GetAppID(),
                        timeStamp = timeStamp,
                        nonceStr = nonceStr,
                        signature = sing
                    };
                    r.code = 1;
                }
                else
                {
                    r.code = 0;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                Log.WriteLog("Config：" + e.Message);
                Log.WriteLog("Config：" + e.StackTrace);
                r.msg = e.Message;
            }
            return Ok(r);
        }

        public string GetSignature(string jsapi_ticket, string noncestr, string timestamp, string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("jsapi_ticket=")
                .Append(jsapi_ticket)
                .Append("&")
                .Append("noncestr=")
                .Append(noncestr)
                .Append("&")
                .Append("timestamp=" + timestamp)
                .Append("&");

            sb.Append("url=" + url);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            string sign = BitConverter.ToString(hash).Replace("-", "").ToLower();
            return sign;
        }
        #endregion


        private static IHostingEnvironment _hostingEnvironment;

        public ApiController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }

}
