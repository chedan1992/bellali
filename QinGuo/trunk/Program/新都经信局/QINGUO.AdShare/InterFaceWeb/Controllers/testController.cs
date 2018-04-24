using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspx.Common;

namespace InterFaceWeb.Controllers
{
    public class testController : Controller
    {
        //
        // GET: /test/

        public ActionResult Index(string Mark)
        {

            List<string> marklist = JsonHelper.serializer.Deserialize<List<string>>(Mark);


            DateTime d1 = new DateTime(2016, 5, 25, 8, 00, 00);
            DateTime d2 = new DateTime(2016, 5, 26, 12, 00, 00);
            TimeSpan d3 = d2.Subtract(d1);
            string write = "";
            write += d3.Days.ToString() + "天";
            write += d3.Hours.ToString() + "小时";
            write += d3.Minutes.ToString() + "分钟";
            write += d3.Seconds.ToString() + "秒";
            ViewBag.write = write;

            string img = "/abc/a/b/c/121212121.png";//缩略图
            string Path = "";

            if (!string.IsNullOrEmpty(img))
            {
                string[] imgs = img.Split(',');
                for (int i = 0; i < imgs.Count(); i++)
                {
                    string str = imgs[i];
                    if (!string.IsNullOrEmpty(str))
                    {
                        string tempstr = str.Substring(0, str.LastIndexOf("/")) + "/BigImg" + str.Substring(str.LastIndexOf("/"), str.Length - str.LastIndexOf("/"));
                        if (i == imgs.Count() - 1)
                        {
                            Path += tempstr;
                        }
                        else
                        {
                            Path += tempstr + ",";
                        }
                    }
                }
            }
            

            return View();
        }

    }
}
