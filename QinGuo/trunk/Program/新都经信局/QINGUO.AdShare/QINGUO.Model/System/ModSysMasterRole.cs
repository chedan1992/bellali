using System;
namespace QINGUO.Model
{

    //该表存放管理员具有的角色信息
    [Serializable]
    [Dapper.TableNameAttribute("Sys_MasterRole")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysMasterRole
    {
        /// <summary>
        /// 主键
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 管理员编号
        /// </summary>		
        public string MasterId { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>		
        public string RoleId { get; set; }

    }
}
