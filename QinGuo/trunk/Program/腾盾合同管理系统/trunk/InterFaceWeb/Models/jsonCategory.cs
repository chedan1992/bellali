using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aspx.Model;

namespace InterFaceWeb
{
    public class jsonCategory
    {

        /// <summary>
        /// id 
        /// </summary>
        public string id { get; set; }

        public string name { get; set; }

        public List<jsonCategory> child { get; set; }
    }
}