using System;

namespace QINGUO.Model
{

    //数据范围权限表
    [Serializable]
    [Dapper.TableNameAttribute("Sys_RoleRangeData")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysRoleRangeData
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 模块树编号
        /// </summary>		
        public string FunId { get; set; }
        /// <summary>
        /// 模块树名称
        /// </summary>		
        public string FunName { get; set; }
        /// <summary>
        /// 部门编号
        /// </summary>		
        public string DeptId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>		
        public string DeptName { get; set; }
        /// <summary>
        /// 数据查看类型(1:查看个人 2:查看全部)
        /// </summary>		
        public int lookPower { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>		
        public string RoleId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>		
        public string CompanyId { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>		
        public string CreaterId { get; set; }        

    }
}
