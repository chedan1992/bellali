using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QINGUO.Model
{
    /// <summary>
    /// 洗汽信息表
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("W_WashCarRecord")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModWashCarRecord
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

        private string _carid;
        /// <summary>
        /// 汽车ID
        /// </summary>
        public string CarId
        {
            get { return _carid; }
            set { _carid = value; }
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
        /// 汽配商id
        /// </summary>		
        private string _createId;
        public string CreateId
        {
            get { return _createId; }
            set { _createId = value; }
        }
    }
}
