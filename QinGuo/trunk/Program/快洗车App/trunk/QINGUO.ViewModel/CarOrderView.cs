using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
    /// <summary>
    /// 用户所有评价列表
    /// </summary>
    public class CarOrderView
    {
        /// <summary>
        /// 申请维保id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 维保项目
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 汽车id
        /// </summary>
        public string carid { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string carno { get; set; }

        /// <summary>
        /// 车品牌
        /// </summary>
        public string cartTypeName { get; set; }

        /// <summary>
        /// 汽车备申请备注
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        public string auditorRemarks { get; set; }

        /// <summary>
        /// 审核人id
        /// </summary>
        public string auditorId { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string auditor { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 汽车用户id
        /// </summary>
        public string cUserId { get; set; }

        /// <summary>
        /// 创建者 维修厂id
        /// </summary>
        public int createId { get; set; }

        /// <summary>
        /// 创建者 维修厂id
        /// </summary>
        public string createname { get; set; }

        /// <summary>
        /// TermOfValidity(天)
        /// </summary>
        public int TermOfValidity { get; set; }

        /// <summary>
        /// 维修厂电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? createTime { get; set; }

    }
}
