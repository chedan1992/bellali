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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
        }
        #endregion

        #region 操作Active


        //获取活动信息
        [HttpGet("GetActive/{id}")]
        public IActionResult GetActive(int id)
        {
            var r = new RequestModel();
            try
            {
                Active active = db.Actives.Find(id);
                List<ActiveOne> list = db.ActiveOnes.Where(c => c.ActiceId == id).ToList();
                r.data = new { active = active, list = list };
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
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
                        List<ActiveOne> li = db.ActiveOnes.Where<ActiveOne>(c => c.ActiceId == s.Id).ToList();

                        foreach (var item in li)
                        {
                            db.ActiveOnes.Remove(item);
                            db.SaveChanges();
                        }
                    }
                }
                if (r.code > 0 && !string.IsNullOrEmpty(activeone))
                {
                    List<ActiveOne> onelist = JsonConvert.DeserializeObject<List<ActiveOne>>(activeone);
                    foreach (var item in onelist)
                    {
                        item.ActiceId = s.Id;
                        item.CreateTime = DateTime.Now;
                        db.ActiveOnes.Add(item);
                        db.SaveChanges();
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
            return NotFound(r);
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
            return NotFound(r);
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

                foreach (var item in rlist)
                {
                    List<ActiveOne> listone = db.ActiveOnes.Where(c => c.ActiceId == item.Id).ToList();
                    rs.Add(new { active = item, list = listone });
                }
                r.data = rlist;
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
        }


        //获取活动
        [HttpGet("GetCatoryListAll")]
        public IActionResult GetCatoryListAll(int status = 2)
        {
            var r = new RequestModel();
            try
            {

                var d = (from e in db.Catorys
                         where e.Status == status
                         select e);

                List<Catory> rlist = d.ToList();
                r.data = rlist;
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
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
                //新增
                s.CreateTime = DateTime.Now;
                s.PayTime = DateTime.Now;
                s.Payno = DateTime.Now.Ticks.ToString();

                s.Status = 0;
                Active active = db.Actives.Find(s.ActiveId);
                s.Money = active.TotalMoney;
                s.PayMoney = s.Money;

                db.Orders.Add(s);
                if (db.SaveChanges() > 0)
                {
                    active.Count++;
                    db.Actives.Update(active);
                    db.SaveChanges();
                    r.code = 1;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
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
                    List<ActiveOne> list = db.ActiveOnes.Where(c => c.ActiceId == order.ActiveId).ToList();

                    r.data = new { order = order, user = user, active = active, list = list };
                    r.code = 1;
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
        }

        //获取订单信息
        [HttpGet("GetOrderList")]
        public IActionResult GetOrderList(string payno, string name, string phone, int pagesize = 1, int pageno = 1, int status = 0)
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
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
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
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
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
            return NotFound(r);
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
            return NotFound(r);
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
                    r.data = new { src = "http://www.bellali.cn" + rpath };
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
            return NotFound(r);
        }

        #endregion



        //获取订单信息
        [HttpGet("GetToken")]

        public IActionResult GetToken(bool isCache = true)
        {
            var r = new RequestModel();
            try
            {

                //关注公众号 注册或者用户信息
                object token = CacheHelper.Get("tokens");
                string access_token = token != null ? token.ToString() : "";
                if (!string.IsNullOrEmpty(access_token) && isCache)
                {
                    r.data = access_token;
                }
                else
                {
                    StringBuilder userurl = new StringBuilder(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", WxPayConfig.APPID, WxPayConfig.APPSECRET));
                    string userjson = HttpService.Get(userurl.ToString());
                    //{"access_token":"ACCESS_TOKEN","expires_in":7200}
                    Log.WriteLog(userjson);
                    if (!string.IsNullOrEmpty(userjson))
                    {
                        var tokens = JsonConvert.DeserializeObject<ResultToken>(userjson);

                        if (tokens.expires_in == 7200)
                        {
                            CacheHelper.Set("tokens", tokens.access_token);
                            r.data = tokens.access_token;
                        }
                    }
                }
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);

                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
        }


        //获取订单信息
        [HttpGet("GetMenu")]

        public IActionResult GetMenu()
        {
            var r = new RequestModel();
            try
            {
                List<SysMenu> rlist = db.SysMenus.ToList();
                r.data = rlist;
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                Log.WriteLog(e.Message);

                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
        }
        //修改配置信息
        [HttpPost("SaveMenu")]
        public IActionResult SaveMenu(SysMenu s)
        {
            var r = new RequestModel();
            try
            {
                s.CreateTime = DateTime.Now;
                //修改
                if (s.Id > 0)
                {
                    db.SysMenus.Update(s);
                }
                else
                {
                    db.SysMenus.Add(s);
                }
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
            return NotFound(r);
        }

        //删除配置信息
        [HttpPost("DelMenu")]
        public IActionResult DelMenu(int id)
        {
            var r = new RequestModel();
            try
            {
                var m = db.SysMenus.Find(id);
                db.SysMenus.Remove(m);
                if (db.SaveChanges() > 0)
                {
                    r.code = 1;
                }
                else
                {
                    r.msg = "删除失败";
                }
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
        }

        [HttpPost("SaveMenuWX")]
        public IActionResult SaveMenuWX(string json, string access_token)
        {

            var r = new RequestModel();
            try
            {
                StringBuilder userurl = new StringBuilder(string.Format(" https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token));
                r.data = HttpService.Post(json, userurl.ToString(), true, 2000);
                r.code = 1;
                return Ok(r);
            }
            catch (Exception e)
            {
                //打印日志
                r.msg = e.Message;
            }
            return NotFound(r);
        }
    }
}
