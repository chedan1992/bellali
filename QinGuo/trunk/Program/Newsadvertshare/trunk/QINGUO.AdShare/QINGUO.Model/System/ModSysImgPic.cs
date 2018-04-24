using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    [Serializable]
    [Dapper.TableNameAttribute("Sys_ImgPic")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysImgPic
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
        /// 第三方编号
        /// </summary>		
        private string _otherkeyid;
        public string OtherKeyId
        {
            get { return _otherkeyid; }
            set { _otherkeyid = value; }
        }
        /// <summary>
        /// 图片名称
        /// </summary>		
        private string _picname;
        public string PicName
        {
            get { return _picname; }
            set { _picname = value; }
        }
        /// <summary>
        /// 图片地址
        /// </summary>		
        private string _picurl;
        public string PicUrl
        {
            get { return _picurl; }
            set { _picurl = value; }
        }
        /// <summary>
        /// 图片大小
        /// </summary>		
        private string _imglength;
        public string ImgLength
        {
            get { return _imglength; }
            set { _imglength = value; }
        }
        /// <summary>
        /// 图片类型
        /// </summary>		
        private string _pictype;
        public string PicType
        {
            get { return _pictype; }
            set { _pictype = value; }
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
        /// 创建人编号
        /// </summary>		
        private string _createrid;
        public string CreaterId
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
        /// <summary>
        /// Sort
        /// </summary>		
        private int _sort;
        public int Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }        
    }
}
