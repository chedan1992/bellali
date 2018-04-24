using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterFaceWeb
{
    public class jsonUser
    {
        public string id { get; set; }
        public string loginname { get; set; }
        public string name { get; set; }
        public string headimg { get; set; }
        public string cid { get; set; }
        public int sex { get; set; }
        public int age { get; set; }
        public int msg { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string postName { get; set; }

        public string postLon { get; set; }

        public string postLat { get; set; }

        public int attribute { get; set; }

        public string postPhone { get; set; }

        public int flowmsg { get; set; }
    }
}