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
        汽配商管理 = 3,
        维修厂管理 = 4,

        供应商=10,
        维保公司=11,
        消防部门=12,
    }


    /// <summary>
    /// 用户类型(Sys_Master表)
    /// </summary>
    public enum AdminTypeEnum
    {
        菜单配置员 = 0,//系统默认admin,gomrit 账户
        系统管理员 = 1,
        汽配商管理员 = 2,
        维修厂管理员 = 3,
        手机用户 = 4,

        普通用户 =10,
        消防部门管理员=16,
        维保公司管理员=17,
        单位管理员=18,
        供应商管理员=19,
        消防用户=20,
        维保用户=21,
        单位用户=22,
        供应商用户=23,
        普通员工=24
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
