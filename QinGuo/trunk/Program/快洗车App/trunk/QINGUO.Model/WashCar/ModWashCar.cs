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
    [Dapper.TableNameAttribute("W_WashCar")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModWashCar
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

        private string _cid;
        /// <summary>
        /// 汽车ID
        /// </summary>
        public string CId
        {
            get { return _cid; }
            set { _cid = value; }
        }

        /// <summary>
        /// 洗车有效期 天数 叠加
        /// </summary>		
        private int _day;
        public int Day
        {
            get { return _day; }
            set { _day = value; }
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
        /// 创建者 汽配商id
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
