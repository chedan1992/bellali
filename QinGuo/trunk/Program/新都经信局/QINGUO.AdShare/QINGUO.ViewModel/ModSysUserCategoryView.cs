using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.ViewModel
{
   public class ModSysUserCategoryView
    {
        /// <summary>
        /// 用户编号
        /// </summary>		
        private string _userid;
        public string UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 分类编号 (主键,非自增)
        /// </summary>
        /// 字段长度:36
        /// 是否为空:false
        public string Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        /// 字段长度:50
        /// 是否为空:true
        public string Name { get; set; }

        /// <summary>
        /// 父节点编号
        /// </summary>
        /// 字段长度:36
        /// 是否为空:true
        public string ParentId { get; set; }

        /// <summary>
        /// 数据状态(-1: 删除 0:禁用 1:正常)
        /// </summary>
        /// 字段长度:4
        /// 是否为空:true
        public int Status { get; set; }

        /// <summary>
        /// 分类图标
        /// </summary>
        /// 字段长度:100
        /// 是否为空:true
        public string PicUrl { get; set; }
    }
}
