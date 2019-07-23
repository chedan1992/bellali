using System;
namespace QINGUO.Model
{

    [Serializable]
    [Dapper.TableNameAttribute("Sys_Category")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysCategory
    {
        /// <summary>
        /// ���
        /// </summary>		
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// ����
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// ��ʾ˳��
        /// </summary>		
        private string _ordernum;
        public string OrderNum
        {
            get { return _ordernum; }
            set { _ordernum = value; }
        }

        /// <summary>
        /// ���ڵ���
        /// </summary>		
        private string _pid;
        public string ParentCategoryId
        {
            get { return _pid; }
            set { _pid = value; }
        }

        /// <summary>
        /// ����״̬(-1: ɾ�� 0:���� 1:����)
        /// </summary>		
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// �����˱��
        /// </summary>		
        private string _createrid;
        public string CreaterId
        {
            get { return _createrid; }
            set { _createrid = value; }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

        /// <summary>
        /// �ڵ����0:һ����1��������
        /// </summary>		
        private int _depth;
        public int Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        /// <summary>
        /// �ڵ�·��
        /// </summary>		
        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// ����ͼ��
        /// </summary>		
        private string _picUrl;
        public string PicUrl
        {
            get { return _picUrl; }
            set { _picUrl = value; }
        }

        /// <summary>
        /// �Ƿ����ӽڵ�
        /// </summary>		
        private bool _hasChild;
        public bool HasChild
        {
            get { return _hasChild; }
            set { _hasChild = value; }
        }


        /// <summary>
        /// �Ƿ�ϵͳ����(ϵͳ����ķ��಻��ɾ��)
        /// </summary>		
        private bool _isSystem;
        public bool IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }


        /// <summary>
        /// ��������
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }


        /// <summary>
        /// ����������
        /// </summary>		
        private string _createrName;
        public string CreaterName
        {
            get { return _createrName; }
            set { _createrName = value; }
        }

        /// <summary>
        /// ��˾���
        /// </summary>		
        private string _companyId;
        public string CompanyId
        {
            get { return _companyId; }
            set { _companyId = value; }
        }

    }
}