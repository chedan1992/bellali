/*---------------------------------------------------------------- 
// @����Ƽ�. All Rights Reserved. ��Ȩ���С�  
// 
// �ļ�����Ʒ�ƹ���
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
namespace QINGUO.Model
{

    //E_Elevator
    [Serializable]
    [Dapper.TableNameAttribute("Sys_QRCode")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysQRCode
    {
        /// <summary>
        /// ���
        /// </summary>		
        public string Id { get; set; }
        /// <summary>
        /// ����
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// ��������
        /// </summary>		
        public string GroupName { get; set; }
        /// <summary>
        /// ��ά�����
        /// </summary>
        public string QrCode { get; set; }
        /// <summary>
        /// Img
        /// </summary>		
        public string Img { get; set; }
        /// <summary>
        /// Status(0:û��,1����)
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// ��ƽ̨����
        /// </summary>
        public string SysId { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// �����˱��
        /// </summary>
        /// �ֶγ���:36
        /// �Ƿ�Ϊ��:true
        public string CreaterId { get; set; }
    }
}