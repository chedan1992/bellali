using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Fun")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysFun
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
        /// 类型编号(总公司,分公司,代理商)
        /// </summary>		
        private int _typeid;
        public int TypeId
        {
            get { return _typeid; }
            set { _typeid = value; }
        }
        /// <summary>
        /// 上级编号(对应的是编号)
        /// </summary>		
        private string _parentid;
        public string ParentId
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        /// <summary>
        /// 排序
        /// </summary>		
        private int _funsort;
        public int FunSort
        {
            get { return _funsort; }
            set { _funsort = value; }
        }
        /// <summary>
        /// 功能名称
        /// </summary>		
        private string _funname;
        public string FunName
        {
            get { return _funname; }
            set { _funname = value; }
        }
        /// <summary>
        /// 功能连接地址
        /// </summary>		
        private string _pageurl;
        public string PageUrl
        {
            get { return _pageurl; }
            set { _pageurl = value; }
        }
        /// <summary>
        /// 类名
        /// </summary>		
        private string _classname;
        public string ClassName
        {
            get { return _classname; }
            set { _classname = value; }
        }
        /// <summary>
        /// 状态(-1: 删除 0:禁用 1:正常)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
        {
            get
            {

                if (_createtime.ToShortDateString().IndexOf("0001") > -1)
                    return DateTime.Now;
                else
                    return _createtime;
            }
            set
            {
                    _createtime = value;
            }
        }
        /// <summary>
        /// 栏目图标
        /// </summary>		
        private string _iconcls;
        public string iconCls
        {
            get { return _iconcls; }
            set { _iconcls = value; }
        }
        /// <summary>
        /// 是否是系统定义(超级系统管理员才能看见)
        /// </summary>		
        private bool _issystem;
        public bool IsSystem
        {
            get { return _issystem; }
            set { _issystem = value; }
        }

        /// <summary>
        /// 是否是叶子节点
        /// </summary>		
        private bool _isChild;
        public bool isChild
        {
            get { return _isChild; }
            set { _isChild = value; }
        }

        /// <summary>
        /// 是否需范围控权(权限管理--范围控权选项)
        /// </summary>		
        private bool _isCheckRole;
        public bool isCheckRole
        {
            get { return _isCheckRole; }
            set { _isCheckRole = value; }
        }
    }
}
