using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.QuartService
{
    /// <summary>
    /// 短信模板VO对象
    /// </summary>
    [Serializable]
    public class SMSModel
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MobileCode { get; set; }
        /// <summary>
        /// 推送编号主键
        /// </summary>
        public string PushUserId { get; set; }
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string PTitle { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 发送装填
        /// </summary>
        public string SendState { get; set; }
        /// <summary>
        /// 模式1:直接内容，2：订单，3：广告，4，新闻 5，订台
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 短信状态
        /// </summary>
        public string SMSState { get; set; }
        /// <summary>
        /// 关联主键ID
        /// </summary>
        public string RelationID { get; set; }

    }
}
