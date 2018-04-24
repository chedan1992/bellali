using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
   public class ModPushMsgCompany
    {
        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public string Id { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 商户第二名称
        /// </summary>		
        private string _nametitle;
        public string NameTitle
        {
            get { return _nametitle; }
            set { _nametitle = value; }
        }        



        /// <summary>
        /// 用户编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public string UserId { get; set; }
        /// <summary>
        /// 机器码
        /// </summary>
        /// 字段长度:400
        /// 是否为空:true
        public string MobileCode { get; set; }

    }
}
