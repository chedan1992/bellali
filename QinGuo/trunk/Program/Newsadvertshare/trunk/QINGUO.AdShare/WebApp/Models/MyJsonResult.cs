using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApp
{
    /// <summary>
    /// 数据返回类
    /// </summary>
    public class MyJsonResult<T>
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
        private string _msg = "请求成功";
        public string msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
        /// <summary>
        /// 当前服务器时间
        /// </summary>
        private string _nowdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
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
        private T _data;
        public T data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        /// 其他信息
        /// </summary>
        private string _otherMsg = "0";
        public string otherMsg
        {
            get { return _otherMsg; }
            set { _otherMsg = value; }
        }
    }
}
