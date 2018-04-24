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
    public class ModJsonResult
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
        /// 当前服务器时间
        /// </summary>
        private string _nowdate = DateTime.Now.ToString();
        public string nowdate
        {
            get { return _nowdate; }
            set { _nowdate = value; }
        }
        /// <summary>
        /// 错误代码
        /// </summary>
        private int _errorcode = 0;
        public int errorCode
        {
            get { return _errorcode; }
            set { _errorcode = value; }
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        private object _data = "";
        public object data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        /// 其他信息
        /// </summary>
        private object _otherMsg = "0";
        public object otherMsg
        {
            get { return _otherMsg; }
            set { _otherMsg = value; }
        }
        public override string ToString()
        {
            return string.Format("{{\"msg\":\"{0}\",\"success\":{1},\"nowdate\":\"{2}\",\"errorcode\":\"{3}\",\"data\":{4},\"otherMsg\":{5}}}", msg.ToLower(), success.ToString().ToLower(), DateTime.Now.ToString(), errorCode, data == "" ? "[]" : data, otherMsg);
        }

       
    }
}
