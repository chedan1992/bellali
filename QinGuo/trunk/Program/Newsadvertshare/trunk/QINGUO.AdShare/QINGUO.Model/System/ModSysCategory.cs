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
        /// ������
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// ��������
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// ��ʾ˳��
        /// </summary>		
        public string OrderNum { get; set; }
        /// <summary>
        /// ���ڵ���
        /// </summary>		
        public string ParentCategoryId { get; set; }
        /// <summary>
        /// ����״̬(-1: ɾ�� 0:���� 1:����)
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// �ڵ����0:һ����1��������
        /// </summary>		
        public int Depth { get; set; }
        /// <summary>
        /// �ڵ�·��
        /// </summary>		
        public string Path { get; set; }
        /// <summary>
        /// ����ͼ��
        /// </summary>		
        public string PicUrl { get; set; }
        /// <summary>
        /// �Ƿ����ӽڵ�
        /// </summary>		
        public bool HasChild { get; set; }
        /// <summary>
        /// �Ƿ�ϵͳ����(ϵͳ����ķ��಻��ɾ��)
        /// </summary>		
        public bool IsSystem { get; set; }
        /// <summary>
        /// ��������
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// �����˱��
        /// </summary>		
        public string CreaterId { get; set; }
        /// <summary>
        /// ����������
        /// </summary>		
        public string CreaterName { get; set; }
        /// <summary>
        /// ��˾���
        /// </summary>
        public string CompanyId { get; set; }

        private DateTime _createTime;
        /// <summary>
        /// ����ʱ��
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