using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Master")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public partial class ModSysMaster
    {

        /// <summary>
        /// 编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public string Id { get; set; }

        /// <summary>
        /// 邮箱手机号
        /// </summary>
        /// 字段长度:100
        /// 是否为空:true
        public string LoginName { get; set; }

        /// <summary>
        /// 昵称或者小区名称或者公司名称
        /// </summary>
        /// 字段长度:50
        /// 是否为空:true
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// 字段长度:60
        /// 是否为空:true
        public string Pwd { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        /// 字段长度:1
        /// 是否为空:true
        public bool IsMain { get; set; }

        /// <summary>
        /// Company表 id
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string Cid { get; set; }

        /// <summary>
        /// 状态(-1: 删除 0:禁用 1:正常)
        /// </summary>
        /// 字段长度:4
        /// 是否为空:true
        public int Status { get; set; }

        /// <summary>
        /// 性别 (0: 男 1:女)
        /// </summary>
        /// 字段长度:4
        /// 是否为空:true
        public int Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        /// 字段长度:64
        /// 是否为空:true
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        /// 字段长度:20
        /// 是否为空:true
        public string Phone { get; set; }
        /// <summary>
        /// 支付宝
        /// </summary>
        public string Alipay { get; set; }

        /// <summary>
        /// 上次登陆ip
        /// </summary>
        /// 字段长度:30
        /// 是否为空:true
        public string LoginIp { get; set; }

        /// <summary>
        /// 上次登陆时间
        /// </summary>
        /// 字段长度:8
        /// 是否为空:true
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// 是否是系统定义(系统定义对象不用控权
        /// </summary>
        /// 字段长度:1
        /// 是否为空:true
        public bool IsSystem { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        /// 字段长度:4
        /// 是否为空:true
        public int LoginNum { get; set; }

        /// <summary>
        /// 职位编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string OrganizaId { get; set; }

        /// <summary>
        /// 角色名称/商家
        /// </summary>
        /// 字段长度:16
        /// 是否为空:true
        public string RoleName { get; set; }

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
        /// 头像
        /// </summary>
        /// 字段长度:100
        /// 是否为空:true
        public string HeadImg { get; set; }

        /// <summary>
        /// 机器码
        /// </summary>
        /// 字段长度:400
        /// 是否为空:true
        public string MobileCode { get; set; }

        /// <summary>
        /// 百度用户编号
        /// </summary>
        public string BDUserId { get; set; }


        /// <summary>
        /// 百度频道编号
        /// </summary>
        public string BDChannelId { get; set; }

        /// <summary>
        /// 登录平台(1:苹果 2:安卓 3:网页)
        /// </summary>
        public int PaltForm { get; set; }

        /// <summary>
        /// 管理员类型(0:系统超级管理员 1:普通管理员 2:普通用户)
        /// </summary>
        public int Attribute { get; set; }

        /// <summary>
        /// 操作证号
        /// </summary>
        public string OperateNum { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 用户证件号码
        /// </summary>
        public string CardNum { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Loe { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Lae { get; set; }

        /// <summary>
        /// Money
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        
    }
}
