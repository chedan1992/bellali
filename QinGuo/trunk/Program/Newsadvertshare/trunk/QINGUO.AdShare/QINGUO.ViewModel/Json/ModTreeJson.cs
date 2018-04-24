using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    public class ModTreeJson
    {
        public string id { get; set; }

        public string parentId { get; set; }

        public string text { get; set; }

        public string ImgPic { get; set; }

        public List<ModTreeJson> children { get; set; }
    }
}
