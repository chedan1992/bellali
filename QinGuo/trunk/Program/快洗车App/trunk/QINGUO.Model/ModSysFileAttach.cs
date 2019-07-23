using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_FileAttach")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysFileAttach
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
        /// 文件原名称
        /// </summary>		
        private string _nameold;
        public string NameOld
        {
            get { return _nameold; }
            set { _nameold = value; }
        }
        /// <summary>
        /// 文件现名称
        /// </summary>		
        private string _namenew;
        public string NameNew
        {
            get { return _namenew; }
            set { _namenew = value; }
        }
        /// <summary>
        /// 文件大小
        /// </summary>		
        private string _filesize;
        public string FileSize
        {
            get { return _filesize; }
            set { _filesize = value; }
        }
        /// <summary>
        /// 文件保存路径
        /// </summary>		
        private string _filepath;
        public string FilePath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        /// <summary>
        /// 文件类型
        /// </summary>		
        private string _filetype;
        public string FileType
        {
            get { return _filetype; }
            set { _filetype = value; }
        }
        /// <summary>
        /// 文件后缀名
        /// </summary>		
        private string _extension;
        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }
        /// <summary>
        /// 阅读量
        /// </summary>		
        private int _readnum;
        public int ReadNum
        {
            get { return _readnum; }
            set { _readnum = value; }
        }
        /// <summary>
        /// 下载量
        /// </summary>		
        private int _downnum;
        public int DownNum
        {
            get { return _downnum; }
            set { _downnum = value; }
        }
        /// <summary>
        /// 是否分享
        /// </summary>		
        private bool _isshare;
        public bool IsShare
        {
            get { return _isshare; }
            set { _isshare = value; }
        }
        /// <summary>
        /// 公司编号
        /// </summary>		
        private string _Cid;
        public string Cid
        {
            get { return _Cid; }
            set { _Cid = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime? _createtime;
        public DateTime? CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
       
        /// <summary>
        /// 创建者
        /// </summary>		
        private string _createrid;
        public string CreaterId
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
        /// <summary>
        /// -1: 删除 0:禁用 1:正常
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 模块COde
        /// </summary>		
        private string _modelcode;
        public string ModelCode
        {
            get { return _modelcode; }
            set { _modelcode = value; }
        }
        /// <summary>
        /// 分类ID
        /// </summary>		
        private string _typeid;
        public string TypeId
        {
            get { return _typeid; }
            set { _typeid = value; }
        }
        /// <summary>
        /// 关联主键ID
        /// </summary>		
        private string _keyid;
        public string KeyId
        {
            get { return _keyid; }
            set { _keyid = value; }
        }        
    }
}
