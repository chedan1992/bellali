using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using QINGUO.Common;
using QINGUO.Common;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_User")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysUser
    {

        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        /// 字段长度:50
        /// 是否为空:true
        public string Name { get; set; }

        /// <summary>
        /// 所属商户
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string CId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// 字段长度:80
        /// 是否为空:true
        public string Pwd { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        /// 字段长度:100
        /// 是否为空:true
        public string Nickname { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        /// 字段长度:50
        /// 是否为空:true
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        /// 字段长度:20
        /// 是否为空:true
        public string Tel { get; set; }

        /// <summary>
        /// 0:男 1:女
        /// </summary>
        /// 字段长度:1
        /// 是否为空:true
        public int Sex { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        /// 字段长度:100
        /// 是否为空:true
        public string HeadImg { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        /// 字段长度:3
        /// 是否为空:true
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        /// 字段长度:600
        /// 是否为空:true
        public string Signature { get; set; }

        /// <summary>
        /// -1: 删除 0:禁用 1:正常
        /// </summary>
        /// 字段长度:4
        /// 是否为空:true
        public int Status { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        /// 字段长度:8
        /// 是否为空:true
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        /// 字段长度:4
        /// 是否为空:true
        public int LoginCount { get; set; }

        /// <summary>
        /// 机器码
        /// </summary>
        /// 字段长度:400
        /// 是否为空:true
        public string MobileCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// 字段长度:8
        /// 是否为空:true
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string CreaterId { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public string CityId { get; set; }

        /// <summary>
        /// 是否默认是可见,默认可见
        /// </summary>
        /// 字段长度:1
        /// 是否为空:true
        public bool Visible { get; set; }

        /// <summary>
        /// 百度用户编号
        /// </summary>
        public string BDUserId { get; set; }


        /// <summary>
        /// 百度频道编号
        /// </summary>
        public string BDChannelId { get; set; }

        /// <summary>
        /// 登录平台(1:苹果 2:安卓)
        /// </summary>
        public int PaltForm { get; set; }

        /// <summary>
        /// 职位编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string OrganizaId { get; set; }

        /// <summary>
        /// 是否是汽车司机
        /// </summary>
        public bool IsCarer { get; set; }

        /// <summary>
        ///  用户类型  工程师 = 2,物业端 = 3,电梯公司 = 4,监管部门 = 5,生产厂家 = 6,
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        ///  证件名称
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        ///  证件号
        /// </summary>
        public string CardNum { get; set; }

        /// <summary>
        /// 即将到账金额
        /// </summary>
        [Property(IsDataBaseField = false)]
        [Dapper.Ignore]
        public ModViewSysCompany SysCompany{get;set;}
    }
}
