using System;


namespace QINGUO.Model
{

    /*
     * 存放角色中访问菜单树的权限
     * 存放角色中访问菜单树的按钮权限
   */
    [Serializable]
    [Dapper.TableNameAttribute("Sys_RoleFun")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysRoleFun
    {
        /// <summary>
        /// 主键
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>		
        public string RoleId { get; set; }
        /// <summary>
        /// 功能编号
        /// </summary>		
        public string FunId { get; set; }
        /// <summary>
        /// 按钮编号
        /// </summary>		
        public string BtnId { get; set; }
        /// <summary>
        /// 按钮组合键(格式:按钮编号+'|'+菜单编号)
        /// </summary>		
        public string UniteId { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>		
        public string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>		
        public string CreaterName { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>		
        public string CId { get; set; }

    }
}
