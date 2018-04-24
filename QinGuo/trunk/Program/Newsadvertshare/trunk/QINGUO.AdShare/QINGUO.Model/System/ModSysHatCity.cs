using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using QINGUO.Common;

namespace QINGUO.Model
{
    /// <summary>
    /// 市
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Sys_Hatcity")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysHatcity
    {
        /// <summary>
        /// id
        /// </summary>
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 是否开通
        /// </summary>
        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// 编码
        /// </summary>
        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 省编码
        /// </summary>
        private string _provinceCode;
        public string ProvinceCode
        {
            get { return _provinceCode; }
            set { _provinceCode = value; }
        }

        /// <summary>
        /// 区列表
        /// </summary>
        private List<ModSysHatArea> _area;
        [Property(IsDataBaseField = false)]
        [Dapper.ResultColumn]
        public List<ModSysHatArea> Area
        {
            get { return _area; }
            set { _area = value; }
        }

    }
}
