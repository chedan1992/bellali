using System;
namespace QINGUO.Model
{

    //记录用户反馈信息
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Feedback")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysFeedback
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 意见内容
        /// </summary>		
        public string BackInfo { get; set; }
        /// <summary>
        /// 反馈者如果是登陆会员则关联会员
        /// </summary>		
        public string UserId { get; set; }
        /// <summary>
        /// 手机类型1是苹果，2安卓
        /// </summary>		
        public string PhoneType { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>		
        public string Tel { get; set; }
        /// <summary>
        /// 当前状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 是否采纳
        /// </summary>		
        public bool IsAccept { get; set; }

        /// <summary>
        /// 反馈时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>		
        public string CreaterId { get; set; }
    }
}
