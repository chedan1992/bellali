using System;
namespace QINGUO.Model
{

    //Shop_Category
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Category")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysCategory
    {
        /// <summary>
        /// 分类编号
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>		
        public string OrderNum { get; set; }
        /// <summary>
        /// 父节点编号
        /// </summary>		
        public string ParentCategoryId { get; set; }
        /// <summary>
        /// 数据状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// 节点深度0:一级，1：二级，
        /// </summary>		
        public int Depth { get; set; }
        /// <summary>
        /// 节点路径
        /// </summary>		
        public string Path { get; set; }
        /// <summary>
        /// 分类图标
        /// </summary>		
        public string PicUrl { get; set; }
        /// <summary>
        /// 是否有子节点
        /// </summary>		
        public bool HasChild { get; set; }
        /// <summary>
        /// 是否系统分类(系统定义的分类不能删除)
        /// </summary>		
        public bool IsSystem { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 创建人编号
        /// </summary>		
        public string CreaterId { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>		
        public string CreaterName { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public string CompanyId { get; set; }

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