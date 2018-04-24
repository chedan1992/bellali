using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 系统收藏表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Collection")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysCollection
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
        /// 收藏id
        /// </summary>		
        private string _collId;
        public string CollId
        {
            get { return _collId; }
            set { _collId = value; }
        }
        /// <summary>
        /// 类型,自定义（新闻收藏 = 0）
        /// </summary>		
        private CollectionEnum _type;
        public CollectionEnum Type
        {
            get { return _type; }
            set { _type = value; }
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
    }
    /// <summary>
    /// 用户字段
    /// </summary>
    public enum CollectionEnum
    {
        新闻收藏 = 0
    }
}
