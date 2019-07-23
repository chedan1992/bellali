using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_MessageAuthCode")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysMessageAuthCode
    {

        /// <summary>
        /// 编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public string Id { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        /// 字段长度:40
        /// 是否为空:true
        public string Tel { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        /// 字段长度:40
        /// 是否为空:true
        public string Code { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// 字段长度:8
        /// 是否为空:true
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// 字段长度:8
        /// 是否为空:true
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string CreaterId { get; set; }

        /// <summary>
        /// 状态(0:未验证 1:已验证 -1:过期)
        /// </summary>
        /// 字段长度:4
        /// 是否为空:false
        public int MsgState { get; set; }

        /// <summary>
        /// 验证码类型类型（0：登录，1：登录，2：找回密码）
        /// </summary>
        /// 字段长度:4
        /// 是否为空:false
        public int TypeInt { get; set; }
    }
}
