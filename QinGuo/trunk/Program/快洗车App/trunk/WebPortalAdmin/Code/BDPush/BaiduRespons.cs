using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebPortalAdmin.Code
{
    public class BaiduRespons
    {
        public string request_id { get; set; }
        public ResponseParams response_params { get; set; }
    }
    public class ResponseParams
    {
        public int success_amount { get; set; }
        public List<string> msgids { get; set; }
    }
}
