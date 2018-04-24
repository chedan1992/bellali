using QINGUO.Common;
/*---------------------------------------------------------------- 
// @青果科技. All Rights Reserved. 版权所有。  
// 
// 文件名：动态列表
// 文件功能描述：  
// 
// 创建人： zj
// 创建日期： 2016-2-7
// 
// 修改标识： 
// 修改描述： 
// 
// 修改标识： 
// 修改描述： 
//----------------------------------------------------------------*/
using System;
using System.Collections.Generic;
namespace QINGUO.Model
{

    //E_Elevator
    [Serializable]
    [Dapper.TableNameAttribute("E_Dynamic")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModEDynamic
    {
        /// <summary>
        /// 分类编号
        /// </summary>	
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 新闻标题
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 1:正常,0：删除
        /// </summary>		
        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// 公司id
        /// </summary>		
        private string sysId;
        public string SysId
        {
            get { return sysId; }
            set { sysId = value; }
        }

        /// <summary>
        /// 新闻详情
        /// </summary>		
        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>	
        private DateTime? createTime;
        public DateTime? CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        /// <summary>
        /// 更新时间
        /// </summary>		
        private DateTime? upTime;
        public DateTime? UpTime
        {
            get { return upTime; }
            set { upTime = value; }
        }
        /// <summary>
        /// 更新者
        /// </summary>		
        private string upSysId;
        public string UpSysId
        {
            get { return upSysId; }
            set { upSysId = value; }
        }
        /// <summary>
        /// 描述
        /// </summary>		
        private string mark;
        public string Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        /// <summary>
        /// 阅读数量
        /// </summary>		
        private int readNum;
        public int ReadNum
        {
            get { return readNum; }
            set { readNum = value; }
        }

        /// <summary>
        /// Img
        /// </summary>		
        private string img;
        public string Img
        {
            get { return img; }
            set { img = value; }
        }

        /// <summary>
        /// 创建人编号
        /// </summary>
        private string createrId;
        public string CreaterId
        {
            get { return createrId; }
            set { createrId = value; }
        }
        /// <summary>
        /// 分类id
        /// </summary>
        private string croupId;
        public string GroupId
        {
            get { return croupId; }
            set { croupId = value; }
        }

        /// <summary>
        /// 开始时间
        /// </summary>		
        private DateTime? _activestarttime;
        public DateTime? ActiveStartTime
        {
            get { return _activestarttime; }
            set { _activestarttime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>		
        private DateTime? _activeendtime;
        public DateTime? ActiveEndTime
        {
            get { return _activeendtime; }
            set { _activeendtime = value; }
        }
        /// <summary>
        ///  新闻类型(1:内部新闻 2:外部新闻)
        /// </summary>		
        private int _actiontype;
        public int ActionType
        {
            get { return _actiontype; }
            set { _actiontype = value; }
        }
        /// <summary>
        ///  展示方式(1:无时间限制 2:自动下架)
        /// </summary>		
        private int _showType;
        public int ShowType
        {
            get { return _showType; }
            set { _showType = value; }
        }

        /// <summary>
        /// 是否置顶
        /// </summary>
        private bool isTop;
        public bool IsTop
        {
            get { return isTop; }
            set { isTop = value; }
        }
        /// 省份code
        /// </summary>
        private string provienceId;
        public string ProvienceId
        {
            get { return provienceId; }
            set { provienceId = value; }
        }
        /// <summary>
        /// 市区code
        /// </summary>
        private string cityId;
        public string CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }


        /// <summary>
        /// 作者
        /// </summary>
        private string author;
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        /// <summary>
        /// 模板号
        /// </summary>
        private int template;
        public int Template
        {
            get { return template; }
            set { template = value; }
        }


        /// <summary>
        /// 是否收藏
        /// </summary>
        private bool isCollection;
        [Property(IsDataBaseField = false)]
        [Dapper.ResultColumn]
        public bool IsCollection
        {
            get { return isCollection; }
            set { isCollection = value; }
        }

        /// <summary>
        /// 图片
        /// </summary>
        private List<ModSysImgPic> imageList;
        [Property(IsDataBaseField = false)]
        [Dapper.ResultColumn]
        public List<ModSysImgPic> ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }
         /// <summary>
        /// 排序
        /// </summary>
        private int sort;
        public int Sort
        {
            get { return sort; }
            set { sort = value; }
        }
    }
}