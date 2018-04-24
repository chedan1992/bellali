using System;
namespace QINGUO.Model
{

    //存储系统角色信息
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Role")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysRole
    {
        /// <summary>
        /// 编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>		
        public int RoleType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>		
        public string Introduction { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>		
        public string CompanyID { get; set; }
        /// <summary>
        /// 排序
        /// </summary>		
        public int RoleSort { get; set; }
        /// <summary>
        /// 状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        public int Status { get; set; }

        /// <summary>
        /// 创建人编号
        /// </summary>		
        public string CreaterId { get; set; }


        private DateTime _createTime;
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime
        {
            get
            {

                if (_createTime.ToShortDateString().IndexOf("0001") > -1)
                    return DateTime.Now;
                else
                    return _createTime;
            }
            set
            {
                    _createTime = value;
            }
        }

    }
}
