using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 汽车信息表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("W_Car")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModCar
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
        private string _img;
        /// <summary>
        /// 汽车头图
        /// </summary>
        public string Img
        {
            get { return _img; }
            set { _img = value; }
        }

        private string _phone;
        /// <summary>
        /// 最近维修电话
        /// </summary>
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        private string _licensePlate;
        /// <summary>
        /// 车牌
        /// </summary>
        public string LicensePlate
        {
            get { return _licensePlate; }
            set { _licensePlate = value; }
        }

        private string _type;
        /// <summary>
        /// 汽车类型
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _typeName;
        /// <summary>
        /// 汽车类型
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }


        /// <summary>
        /// 添加时间
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
        private string _createId;
        public string CreateId
        {
            get { return _createId; }
            set { _createId = value; }
        }
        /// <summary>
        /// 状态(0:待审核，1：审核通过，2：审核不通过)
        /// </summary>		
        private StatusEnum _status;
        public StatusEnum Status
        {
            get { return _status; }
            set { _status = value; }
        }

    }
}
