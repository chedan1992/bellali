using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using QINGUO.Common;

namespace QINGUO.Model
{
    /// <summary>
    /// 省
    /// </summary>
    [Serializable]
    [Dapper.TableNameAttribute("Sys_HatProvince")]
    [Dapper.PrimaryKeyAttribute("Id", autoIncrement = false)]
    public class ModSysHatProvince
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
        /// 城市列表
        /// </summary>
        private List<ModSysHatcity> _city;
        [Property(IsDataBaseField = false)]
        [Dapper.ResultColumn]
        public List<ModSysHatcity> City
        {
            get { return _city; }
            set { _city = value; }
        }

    }
}
