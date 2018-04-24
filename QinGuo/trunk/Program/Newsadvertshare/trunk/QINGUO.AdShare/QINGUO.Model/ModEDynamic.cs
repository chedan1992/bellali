using QINGUO.Common;
/*---------------------------------------------------------------- 
// @����Ƽ�. All Rights Reserved. ��Ȩ���С�  
// 
// �ļ�������̬�б�
// �ļ�����������  
// 
// �����ˣ� zj
// �������ڣ� 2016-2-7
// 
// �޸ı�ʶ�� 
// �޸������� 
// 
// �޸ı�ʶ�� 
// �޸������� 
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
        /// ������
        /// </summary>	
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ���ű���
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 1:����,0��ɾ��
        /// </summary>		
        private int status;
        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// ��˾id
        /// </summary>		
        private string sysId;
        public string SysId
        {
            get { return sysId; }
            set { sysId = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>		
        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>	
        private DateTime? createTime;
        public DateTime? CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>		
        private DateTime? upTime;
        public DateTime? UpTime
        {
            get { return upTime; }
            set { upTime = value; }
        }
        /// <summary>
        /// ������
        /// </summary>		
        private string upSysId;
        public string UpSysId
        {
            get { return upSysId; }
            set { upSysId = value; }
        }
        /// <summary>
        /// ����
        /// </summary>		
        private string mark;
        public string Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        /// <summary>
        /// �Ķ�����
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
        /// �����˱��
        /// </summary>
        private string createrId;
        public string CreaterId
        {
            get { return createrId; }
            set { createrId = value; }
        }
        /// <summary>
        /// ����id
        /// </summary>
        private string croupId;
        public string GroupId
        {
            get { return croupId; }
            set { croupId = value; }
        }

        /// <summary>
        /// ��ʼʱ��
        /// </summary>		
        private DateTime? _activestarttime;
        public DateTime? ActiveStartTime
        {
            get { return _activestarttime; }
            set { _activestarttime = value; }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>		
        private DateTime? _activeendtime;
        public DateTime? ActiveEndTime
        {
            get { return _activeendtime; }
            set { _activeendtime = value; }
        }
        /// <summary>
        ///  ��������(1:�ڲ����� 2:�ⲿ����)
        /// </summary>		
        private int _actiontype;
        public int ActionType
        {
            get { return _actiontype; }
            set { _actiontype = value; }
        }
        /// <summary>
        ///  չʾ��ʽ(1:��ʱ������ 2:�Զ��¼�)
        /// </summary>		
        private int _showType;
        public int ShowType
        {
            get { return _showType; }
            set { _showType = value; }
        }

        /// <summary>
        /// �Ƿ��ö�
        /// </summary>
        private bool isTop;
        public bool IsTop
        {
            get { return isTop; }
            set { isTop = value; }
        }
        /// ʡ��code
        /// </summary>
        private string provienceId;
        public string ProvienceId
        {
            get { return provienceId; }
            set { provienceId = value; }
        }
        /// <summary>
        /// ����code
        /// </summary>
        private string cityId;
        public string CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }


        /// <summary>
        /// ����
        /// </summary>
        private string author;
        public string Author
        {
            get { return author; }
            set { author = value; }
        }
        /// <summary>
        /// ģ���
        /// </summary>
        private int template;
        public int Template
        {
            get { return template; }
            set { template = value; }
        }


        /// <summary>
        /// �Ƿ��ղ�
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
        /// ͼƬ
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
        /// ����
        /// </summary>
        private int sort;
        public int Sort
        {
            get { return sort; }
            set { sort = value; }
        }
    }
}