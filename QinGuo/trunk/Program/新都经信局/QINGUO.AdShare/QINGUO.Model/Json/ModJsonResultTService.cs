using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace QINGUO.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ModJsonResultTService
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        private bool _success = true;
        public bool success
        {
            get { return _success; }
            set { _success = value; }
        }
        /// <summary>
        /// 提示
        /// </summary>
        private string _msg = "操作成功";
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        private string _data = "";
        public string data
        {
            get { return _data; }
            set { _data = value; }
        }

        private string _timespan = "";
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timespan
        {
            get { return _timespan; }
            set { _timespan = value; }
        }


        public override string ToString()
        {
            return string.Format("{{\"msg\":\"{0}\",\"success\":{1},\"nowdate\":\"{2}\",\"data\":{3}}}", msg.ToLower(), success.ToString().ToLower(), DateTime.Now.ToString(), data == "" ? "[]" : data);
        }
       
    }
}
