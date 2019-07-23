using System;
namespace QINGUO.Model
{

    [Serializable]
    [Dapper.TableNameAttribute("Sys_Category")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysCategory
    {
        /// <summary>
        /// 编号
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 显示顺序
        /// </summary>		
        private string _ordernum;
        public string OrderNum
        {
            get { return _ordernum; }
            set { _ordernum = value; }
        }

        /// <summary>
        /// 父节点编号
        /// </summary>		
        private string _pid;
        public string ParentCategoryId
        {
            get { return _pid; }
            set { _pid = value; }
        }

        /// <summary>
        /// 数据状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 创建人编号
        /// </summary>		
        private string _createrid;
        public string CreaterId
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

        /// <summary>
        /// 节点深度0:一级，1：二级，
        /// </summary>		
        private int _depth;
        public int Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        /// <summary>
        /// 节点路径
        /// </summary>		
        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// 分类图标
        /// </summary>		
        private string _picUrl;
        public string PicUrl
        {
            get { return _picUrl; }
            set { _picUrl = value; }
        }

        /// <summary>
        /// 是否有子节点
        /// </summary>		
        private bool _hasChild;
        public bool HasChild
        {
            get { return _hasChild; }
            set { _hasChild = value; }
        }


        /// <summary>
        /// 是否系统分类(系统定义的分类不能删除)
        /// </summary>		
        private bool _isSystem;
        public bool IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }


        /// <summary>
        /// 分类描述
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }


        /// <summary>
        /// 创建人名称
        /// </summary>		
        private string _createrName;
        public string CreaterName
        {
            get { return _createrName; }
            set { _createrName = value; }
        }

        /// <summary>
        /// 公司编号
        /// </summary>		
        private string _companyId;
        public string CompanyId
        {
            get { return _companyId; }
            set { _companyId = value; }
        }

    }
}