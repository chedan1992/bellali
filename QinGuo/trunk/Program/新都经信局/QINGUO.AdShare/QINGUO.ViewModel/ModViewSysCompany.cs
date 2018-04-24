using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    public class ModViewSysCompany
    {
        /// <summary>
        /// 公司名称简称
        /// </summary>		
        private string _nametitle;
        public string NameTitle
        {
            get { return _nametitle; }
            set { _nametitle = value; }
        }
        /// <summary>
        /// 公司地址
        /// </summary>		
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>		
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
    }
}
