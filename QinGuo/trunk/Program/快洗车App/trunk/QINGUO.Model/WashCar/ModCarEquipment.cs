using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 设备站点表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("W_Equipment")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModCarEquipment
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

        private string _name;
        /// <summary>
        /// 设备站点名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 所在位置
        /// </summary>		
        private string _addr;
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
        }

        /// <summary>
        /// 所在位置经度
        /// </summary>		
        private string _complon;
        public string ComPLon
        {
            get { return _complon; }
            set { _complon = value; }
        }
        /// <summary>
        /// 所在位置纬度
        /// </summary>		
        private string _complat;
        public string CompLat
        {
            get { return _complat; }
            set { _complat = value; }
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
