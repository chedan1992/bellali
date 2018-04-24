using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    ///公司类型(Sys_Company表)
    /// </summary>
    public enum CompanyType
    {
        系统公司 = 0,//超级管理员,开发配置员默认公司账户
        单位 = 1,
        部门 = 2,
        消防部门 = 3,
        维保公司 = 4,
        供应商 = 5,

    }


    /// <summary>
    /// 用户类型(Sys_Master表)
    /// </summary>
    public enum AdminTypeEnum
    {
        菜单配置员 = 0,//系统默认admin,gomrit 账户
        系统管理员 = 1,
        单位管理员 = 2,
        消防部门管理员 = 3,
        维保公司管理员 = 4,
        供应商管理员 = 5,
        普通管理员 = 6,//系统配置里添加的管理员
        普通员工 = 7,//User表

        单位用户 = 10,//
        消防用户 = 11,
        维保用户 = 12,
        供应商用户 = 13,
        手机用户 = 15,
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum OperationTypeEnum
    {
        访问 = 0,
        操作 = 1,
        异常 = 2,
    }
}
